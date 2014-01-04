using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Effect ( IGraphicsDevice graphicsDevice, IShader vertexShader, IShader pixelShader, IShader geometryShader = null )
		{
			this.graphicsDevice = graphicsDevice;

			VertexShader = vertexShader;
			PixelShader = pixelShader;
			GeometryShader = geometryShader;
		}

		public void Begin ()
		{
			( graphicsDevice as GraphicsDevice ).currentVertexShader = VertexShader as Shader;
			( graphicsDevice.Handle as SharpDX.Direct3D11.Device ).ImmediateContext.VertexShader.Set ( VertexShader.Handle as SharpDX.Direct3D11.VertexShader );
			( graphicsDevice.Handle as SharpDX.Direct3D11.Device ).ImmediateContext.PixelShader.Set ( PixelShader.Handle as SharpDX.Direct3D11.PixelShader );
			if ( GeometryShader != null )
				( graphicsDevice.Handle as SharpDX.Direct3D11.Device ).ImmediateContext.GeometryShader.
					Set ( GeometryShader.Handle as SharpDX.Direct3D11.GeometryShader );
		}

		public void End ()
		{
			( graphicsDevice as GraphicsDevice ).currentVertexShader = null;
			( graphicsDevice.Handle as SharpDX.Direct3D11.Device ).ImmediateContext.VertexShader.Set ( null );
			( graphicsDevice.Handle as SharpDX.Direct3D11.Device ).ImmediateContext.PixelShader.Set ( null );
			( graphicsDevice.Handle as SharpDX.Direct3D11.Device ).ImmediateContext.GeometryShader.Set ( null );
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
