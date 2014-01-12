using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Daramkun.Misty.Common;
using Daramkun.Misty.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	class Effect : StandardDispose, IEffect
	{
		int programId;

		public object Handle { get { return programId; } }
		public IShader VertexShader { get; private set; }
		public IShader PixelShader { get; private set; }
		public IShader GeometryShader { get; private set; }

		public Effect ( IGraphicsDevice graphicsDevice, IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName )
		{
			InitializeEffect ( graphicsDevice, vertexShader, pixelShader, geometryShader, attribName );
		}

		public Effect ( IGraphicsDevice graphicsDevice, XmlDocument xmlDoc, params string [] attribName )
		{
			foreach ( XmlNode lang in xmlDoc.ChildNodes [ 1 ].ChildNodes )
			{
				if ( lang.Name != "language" ) throw new ArgumentException ();
				if ( lang.Attributes [ "type" ].Value != "glsl" ) continue;
				if ( ( lang.Attributes [ "version" ] != null && new Version ( lang.Attributes [ "version" ].Value ) <= Core.GraphicsDevice.Information.ShaderVersion )
					|| lang.Attributes [ "version" ] == null )
				{
					foreach ( XmlNode node in lang.ChildNodes )
					{
						if ( node.Name == "shader" )
						{
							switch ( node.Attributes [ "type" ].Value )
							{
								case "vertex":
									if ( VertexShader != null ) break;
									VertexShader = new Shader ( graphicsDevice, ShaderType.VertexShader, node.ChildNodes [ 0 ].Value );
									break;
								case "pixel":
									if ( PixelShader != null ) break;
									PixelShader = new Shader ( graphicsDevice, ShaderType.PixelShader, node.ChildNodes [ 0 ].Value );
									break;
								case "geometry":
									if ( GeometryShader != null ) break;
									GeometryShader = new Shader ( graphicsDevice, ShaderType.GeometryShader, node.ChildNodes [ 0 ].Value );
									break;
							}
						}
					}
				}
			}

			InitializeEffect ( graphicsDevice, VertexShader, PixelShader, GeometryShader, attribName );
		}

		private void InitializeEffect ( IGraphicsDevice graphicsDevice, IShader vertexShader, IShader pixelShader, IShader geometryShader, params string [] attribName )
		{
			VertexShader = vertexShader;
			PixelShader = pixelShader;
			GeometryShader = geometryShader;

			programId = GL.CreateProgram ();

			int effectState;

			foreach ( IShader shader in new IShader [] { vertexShader, pixelShader, geometryShader } )
			{
				if ( shader == null ) continue;
				GL.AttachShader ( programId, ( int ) shader.Handle );
				GL.GetProgram ( programId, GetProgramParameterName.AttachedShaders, out effectState );
				if ( effectState == 0 )
					throw new ArgumentException ();
			}

			int count = 0;
			foreach ( string attr in attribName )
				GL.BindAttribLocation ( programId, count++, attr );

			GL.LinkProgram ( programId );
			GL.GetProgram ( programId, GetProgramParameterName.LinkStatus, out effectState );
			if ( effectState == 0 )
				throw new ArgumentException ();
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				foreach ( IShader shader in new IShader [] { VertexShader, PixelShader, GeometryShader } )
				{
					if ( shader == null ) continue;
					GL.DetachShader ( programId, ( int ) shader.Handle );
					shader.Dispose ();
				}
				GL.DeleteProgram ( programId );
				programId = 0;
			}
			base.Dispose ( isDisposing );
		}

		public void Begin () { GL.UseProgram ( programId ); }
		public void End () { GL.UseProgram ( 0 ); }

		public void SetUniform<T> ( string name, T value ) where T : struct { SetUniform<T> ( name, ref value ); }
		public void SetUniform<T> ( string name, ref T value ) where T : struct
		{
			GL.UseProgram ( programId );
			int uniform = GL.GetUniformLocation ( programId, name );
			Type baseType = typeof ( T );
			if ( baseType == typeof ( int ) ) { GL.Uniform1 ( uniform, ( int ) ( object ) value ); }
			else if ( baseType == typeof ( float ) ) { GL.Uniform1 ( uniform, ( float ) ( object ) value ); }
			else if ( baseType == typeof ( Vector2 ) ) { Vector2 v = ( Vector2 ) ( object ) value; GL.Uniform2 ( uniform, v.X, v.Y ); }
			else if ( baseType == typeof ( Vector3 ) ) { Vector3 v = ( Vector3 ) ( object ) value; GL.Uniform3 ( uniform, v.X, v.Y, v.Z ); }
			else if ( baseType == typeof ( Vector4 ) ) { Vector4 v = ( Vector4 ) ( object ) value; GL.Uniform4 ( uniform, v.X, v.Y, v.Z, v.W ); }
			else if ( baseType == typeof ( Matrix4x4 ) )
			{
				 Matrix4x4 v = ( Matrix4x4 ) ( object ) value;
				 GL.UniformMatrix4 ( uniform, 1, false, v.ToArray () );
			}
		}

		public void SetUniform ( string name, params int [] value )
		{
			GL.UseProgram ( programId );
			int uniform = GL.GetUniformLocation ( programId, name );
			GL.Uniform1 ( uniform, value.Length, value );
		}

		public void SetUniform ( string name, params float [] value )
		{
			GL.UseProgram ( programId );
			int uniform = GL.GetUniformLocation ( programId, name );
			GL.Uniform1 ( uniform, value.Length, value );
		}

		public void SetTextures ( params TextureArgument [] args )
		{
			GL.UseProgram ( programId );
			for ( int i = 0; i < args.Length; i++ )
			{
				GL.ActiveTexture ( TextureUnit.Texture0 + i );
				GL.BindTexture ( TextureTarget.Texture2D, ( int ) args [ i ].Texture.Handle );

				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, GetFilter ( args [ i ].Filter ) );
				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, GetFilter ( args [ i ].Filter ) );

				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureWrapS, GetAddressing ( args [ i ].Addressing ) );
				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureWrapT, GetAddressing ( args [ i ].Addressing ) );

				GL.TexParameter ( TextureTarget.Texture2D, ( TextureParameterName ) ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, args [ i ].AnisotropicLevel );

				int uniform = GL.GetUniformLocation ( programId, args [ i ].Uniform );
				GL.Uniform1 ( uniform, i );
			}
		}

		private int GetAddressing ( TextureAddressing textureAddressing )
		{
			switch ( textureAddressing )
			{
				case TextureAddressing.Wrap: return ( int ) TextureWrapMode.Repeat;
				case TextureAddressing.Mirror: return ( int ) TextureWrapMode.MirroredRepeat;
				case TextureAddressing.Clamp: return ( int ) TextureWrapMode.Clamp;
				default: throw new ArgumentException ();
			}
		}

		private int GetFilter ( TextureFilter textureFilter )
		{
			switch ( textureFilter )
			{
				case TextureFilter.Nearest: return ( int ) TextureMinFilter.Nearest;
				case TextureFilter.Linear: return ( int ) TextureMinFilter.Linear;
				case TextureFilter.Anisotropic: return ( int ) TextureMinFilter.LinearMipmapLinear;
				default: throw new ArgumentException ();
			}
		}
	}
}
