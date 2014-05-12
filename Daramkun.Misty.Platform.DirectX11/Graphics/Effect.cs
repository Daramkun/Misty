using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class Effect : StandardDispose, IEffect
	{
		IGraphicsDevice graphicsDevice;

		public object Handle { get { return new IShader [] { VertexShader, PixelShader, GeometryShader }; } }

		public IShader VertexShader { get; private set; }
		public IShader PixelShader { get; private set; }
		public IShader GeometryShader { get; private set; }

		public Effect ( IGraphicsDevice graphicsDevice, IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName )
		{
			InitializeEffect ( graphicsDevice, vertexShader, pixelShader, geometryShader );
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

			InitializeEffect ( graphicsDevice, VertexShader, PixelShader, GeometryShader );
		}

		private void InitializeEffect ( IGraphicsDevice graphicsDevice, IShader vertexShader, IShader pixelShader, IShader geometryShader, params string [] attribName )
		{
			this.graphicsDevice = graphicsDevice;

			VertexShader = vertexShader;
			PixelShader = pixelShader;
			GeometryShader = geometryShader;
		}

		public void Use ( IGraphicsContext graphicsContext )
		{
			if ( graphicsContext.Owner != Thread.CurrentThread ) throw new Exception ( "This thread is not owner of Context." );

			var context = graphicsContext.Handle as SharpDX.Direct3D11.DeviceContext;

			( graphicsContext as GraphicsContext ).currentVertexShader = VertexShader as Shader;

			context.VertexShader.Set ( VertexShader.Handle as SharpDX.Direct3D11.VertexShader );
			context.PixelShader.Set ( PixelShader.Handle as SharpDX.Direct3D11.PixelShader );
			if ( GeometryShader != null )
				context.GeometryShader.Set ( GeometryShader.Handle as SharpDX.Direct3D11.GeometryShader );
			
		}

		public void SetUniform<T> ( string name, T value ) where T : struct
		{
			
			throw new NotImplementedException ();
		}

		public void SetUniform<T> ( string name, ref T value ) where T : struct
		{

			throw new NotImplementedException ();
		}

		public void SetUniform ( string name, params int [] value )
		{
			throw new NotImplementedException ();
		}

		public void SetUniform ( string name, params float [] value )
		{
			throw new NotImplementedException ();
		}

		public void SetTextures ( params TextureArgument [] args )
		{
			throw new NotImplementedException ();
		}
	}
}
