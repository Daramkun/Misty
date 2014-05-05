using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	partial class GraphicsContext : StandardDispose, IGraphicsContext
	{
		SharpDX.Direct3D11.DeviceContext d3dContext;

		internal Shader currentVertexShader;

		public Thread Owner { get; private set; }
		public IGraphicsDevice GraphicsDevice { get; private set; }

		public IRenderBuffer CurrentRenderBuffer
		{
			get { throw new NotImplementedException (); }
		}

		public CullMode CullMode
		{
			get { return ConvertFromCullMode ( d3dContext.Rasterizer.State.Description.CullMode ); }
			set
			{
				SharpDX.Direct3D11.RasterizerStateDescription desc = d3dContext.Rasterizer.State.Description;
				desc.CullMode = ConvertToCullMode ( value );
				SharpDX.Direct3D11.RasterizerState tempState = d3dContext.Rasterizer.State;
				d3dContext.Rasterizer.State = new SharpDX.Direct3D11.RasterizerState ( GraphicsDevice.Handle as SharpDX.Direct3D11.Device, desc );
				tempState.Dispose ();
			}
		}

		public FillMode FillMode
		{
			get { return ConvertFromFillMode ( d3dContext.Rasterizer.State.Description.FillMode ); }
			set
			{
				SharpDX.Direct3D11.RasterizerStateDescription desc = d3dContext.Rasterizer.State.Description;
				desc.FillMode = ConvertToFillMode ( value );
				d3dContext.Rasterizer.State.Dispose ();
				d3dContext.Rasterizer.State = new SharpDX.Direct3D11.RasterizerState ( GraphicsDevice.Handle as SharpDX.Direct3D11.Device, desc );
			}
		}

		public BlendState BlendState
		{
			get
			{
				throw new NotImplementedException ();
			}
			set
			{
				throw new NotImplementedException ();
			}
		}

		public DepthStencil DepthStencil
		{
			get
			{
				throw new NotImplementedException ();
			}
			set
			{
				d3dContext.OutputMerger.DepthStencilState = new SharpDX.Direct3D11.DepthStencilState ( GraphicsDevice.Handle as SharpDX.Direct3D11.Device,
					new SharpDX.Direct3D11.DepthStencilStateDescription ()
					{

					} );
			}
		}

		public Viewport Viewport
		{
			get
			{
				SharpDX.ViewportF viewport = d3dContext.Rasterizer.GetViewports () [ 0 ];
				return new Viewport ( ( int ) viewport.X, ( int ) viewport.Y, ( int ) viewport.Width, ( int ) viewport.Height );
			}
			set
			{
				d3dContext.Rasterizer.SetViewport ( new SharpDX.ViewportF ( value.X, value.Y, value.Width, value.Height ) );
			}
		}

		public void BeginScene ( IRenderBuffer renderBuffer = null )
		{
			if ( renderBuffer == null || renderBuffer == GraphicsDevice.BackBuffer )
			{

			}
			else
			{

			}
		}

		public void EndScene ()
		{

		}

		public void Clear ( ClearBuffer clearBuffer, Color color, float depth = 0, int stencil = 0 )
		{
			if ( clearBuffer.HasFlag ( ClearBuffer.ColorBuffer ) )
				d3dContext.ClearRenderTargetView ( renderTarget, ConvertColor ( color ) );
			if ( clearBuffer.HasFlag ( ClearBuffer.DepthBuffer ) || clearBuffer.HasFlag ( ClearBuffer.StencilBuffer ) )
				d3dContext.ClearDepthStencilView ( depthStencil, ConvertDepthStencilClearMethod ( clearBuffer ), depth, ( byte ) stencil );
		}

		public void Draw ( PrimitiveType primitiveType, IBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount )
		{
			( vertexDeclaration as VertexDeclaration ).GenerateInputLayout ( GraphicsDevice, currentVertexShader );
			d3dContext.InputAssembler.InputLayout = vertexDeclaration.Handle as SharpDX.Direct3D11.InputLayout;
			d3dContext.InputAssembler.SetVertexBuffers ( 0,
				new SharpDX.Direct3D11.VertexBufferBinding ( vertexBuffer.Handle as SharpDX.Direct3D11.Buffer, vertexBuffer.RecordTypeSize, 0 )
			);
			d3dContext.InputAssembler.PrimitiveTopology = ConvertToPrimitiveTopology ( primitiveType );
			d3dContext.Draw ( GetPrimitiveCount ( primitiveType, primitiveCount ), startVertex );
		}

		public void Draw ( PrimitiveType primitiveType, IBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, IBuffer indexBuffer, int startIndex, int primitiveCount )
		{
			( vertexDeclaration as VertexDeclaration ).GenerateInputLayout ( GraphicsDevice, currentVertexShader );
			d3dContext.InputAssembler.InputLayout = vertexDeclaration.Handle as SharpDX.Direct3D11.InputLayout;
			d3dContext.InputAssembler.SetVertexBuffers ( 0,
				new SharpDX.Direct3D11.VertexBufferBinding ( vertexBuffer.Handle as SharpDX.Direct3D11.Buffer, vertexBuffer.RecordTypeSize, 0 )
			);
			d3dContext.InputAssembler.SetIndexBuffer ( indexBuffer.Handle as SharpDX.Direct3D11.Buffer,
				indexBuffer.BufferType.HasFlag ( BufferType.Index16 ) ? SharpDX.DXGI.Format.R16_SInt : SharpDX.DXGI.Format.R32_SInt, 0 );
			d3dContext.InputAssembler.PrimitiveTopology = ConvertToPrimitiveTopology ( primitiveType );
			d3dContext.DrawIndexed ( GetPrimitiveCount ( primitiveType, primitiveCount ), startIndex, 0 );
		}
	}
}
