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
		InputAssembler inputAssembler;

		internal Shader currentVertexShader;

		public Thread Owner { get; private set; }
		public IGraphicsDevice GraphicsDevice { get; private set; }
		public object Handle { get { return d3dContext; } }

		public IRenderBuffer CurrentRenderBuffer { get; private set; }

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

		public InputAssembler InputAssembler
		{
			get { return inputAssembler; }
			set
			{
				inputAssembler = value;
				d3dContext.InputAssembler.InputLayout = value.VertexDeclaration.Handle as SharpDX.Direct3D11.InputLayout;
				d3dContext.InputAssembler.SetVertexBuffers ( 0,
					new SharpDX.Direct3D11.VertexBufferBinding ( value.VertexBuffer.Handle as SharpDX.Direct3D11.Buffer, value.VertexBuffer.RecordTypeSize, 0 )
				);
				d3dContext.InputAssembler.SetIndexBuffer ( value.IndexBuffer.Handle as SharpDX.Direct3D11.Buffer,
					value.IndexBuffer.BufferType.HasFlag ( BufferType.Index16 ) ? SharpDX.DXGI.Format.R16_SInt : SharpDX.DXGI.Format.R32_SInt, 0 );
				d3dContext.InputAssembler.PrimitiveTopology = ConvertToPrimitiveTopology ( value.PrimitiveType );
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

		public void SetSampler(int slot, SamplerState sampler)
		{

		}

		internal GraphicsContext ( IGraphicsDevice graphicsDevice, bool isImmediate )
		{
			GraphicsDevice = graphicsDevice;
			d3dContext = isImmediate ? ( graphicsDevice.Handle as SharpDX.Direct3D11.Device ).ImmediateContext :
				new SharpDX.Direct3D11.DeviceContext ( graphicsDevice.Handle as SharpDX.Direct3D11.Device );
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
				d3dContext.ClearRenderTargetView ( ( GraphicsDevice.BackBuffer as BackBuffer ).renderTarget, ConvertColor ( color ) );
			if ( clearBuffer.HasFlag ( ClearBuffer.DepthBuffer ) || clearBuffer.HasFlag ( ClearBuffer.StencilBuffer ) )
				d3dContext.ClearDepthStencilView ( ( GraphicsDevice.BackBuffer as BackBuffer ).depthStencil, ConvertDepthStencilClearMethod ( clearBuffer ), depth, ( byte ) stencil );
		}

		public void Draw ( int startIndex, int primitiveCount )
		{
			( inputAssembler.VertexDeclaration as VertexDeclaration ).GenerateInputLayout ( GraphicsDevice, currentVertexShader );
			if ( inputAssembler.IndexBuffer == null )
				d3dContext.Draw ( GetPrimitiveCount ( inputAssembler.PrimitiveType, primitiveCount ), startIndex );
			else
				d3dContext.DrawIndexed ( GetPrimitiveCount ( inputAssembler.PrimitiveType, primitiveCount ), startIndex, 0 );
		}
	}
}
