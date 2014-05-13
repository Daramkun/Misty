using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Daramkun.Misty.Common;
using Daramkun.Misty.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	class Effect : StandardDispose, IEffect
	{
		static readonly Type TypeMatrix4x4 = typeof ( Matrix4x4 );
		static readonly Type TypeVector2 = typeof ( Vector2 );
		static readonly Type TypeVector3 = typeof ( Vector3 );
		static readonly Type TypeVector4 = typeof ( Vector4 );
		static readonly Type TypeFloat = typeof ( float );

		int programId;

		public object Handle { get { return programId; } }
		public IShader VertexShader { get; private set; }
		public IShader PixelShader { get; private set; }
		public IShader GeometryShader { get; private set; }

		public Effect ( IGraphicsDevice graphicsDevice, IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName )
		{
			InitializeEffect ( graphicsDevice, vertexShader, pixelShader, geometryShader, attribName );
		}

		public Effect ( IGraphicsDevice graphicsDevice, Stream stream )
		{
			string attribName;
			foreach ( var v in ShaderXmlParser.Parse ( stream, out attribName ) )
			{
				IShader shader = new Shader ( graphicsDevice, v.Key, v.Value );
				switch ( v.Key )
				{
					case ShaderType.VertexShader: VertexShader = shader; break;
					case ShaderType.PixelShader: PixelShader = shader; break;
					case ShaderType.GeometryShader: GeometryShader = shader; break;
				}
			}

			InitializeEffect ( graphicsDevice, VertexShader, PixelShader, GeometryShader, attribName != null ? attribName.Split ( ',' ) : new string [ 0 ] );
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

		public void Use ( IGraphicsContext graphicsContext )
		{
			if ( graphicsContext.Owner != Thread.CurrentThread ) throw new Exception ( "This thread is not owner of Context." );
			GL.UseProgram ( programId );
		}

		public void SetUniform<T> ( string name, T value ) where T : struct { SetUniform<T> ( name, ref value ); }
		public void SetUniform<T> ( string name, ref T value ) where T : struct
		{
			int uniform = GL.GetUniformLocation ( programId, name );
			Type baseType = typeof ( T );
			if ( baseType == TypeMatrix4x4 ) GL.UniformMatrix4 ( uniform, 1, false, ( ( Matrix4x4 ) ( object ) value ).ToArray () );
			else if ( baseType == TypeVector2 ) { Vector2 v = ( Vector2 ) ( object ) value; GL.Uniform2 ( uniform, v.X, v.Y ); }
			else if ( baseType == TypeVector3 ) { Vector3 v = ( Vector3 ) ( object ) value; GL.Uniform3 ( uniform, v.X, v.Y, v.Z ); }
			else if ( baseType == TypeVector4 ) { Vector4 v = ( Vector4 ) ( object ) value; GL.Uniform4 ( uniform, v.X, v.Y, v.Z, v.W ); }
			else if ( baseType == TypeFloat ) { GL.Uniform1 ( uniform, ( float ) ( object ) value ); }
			else if ( baseType == typeof ( int ) ) { GL.Uniform1 ( uniform, ( int ) ( object ) value ); }
		}

		public void SetUniform ( string name, params int [] value )
		{
			int uniform = GL.GetUniformLocation ( programId, name );
			GL.Uniform1 ( uniform, value.Length, value );
		}

		public void SetUniform ( string name, params float [] value )
		{
			int uniform = GL.GetUniformLocation ( programId, name );
			GL.Uniform1 ( uniform, value.Length, value );
		}
	}
}
