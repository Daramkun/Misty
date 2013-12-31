using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Graphics
{
	public partial class GraphicsDevice : StandardDispose, IGraphicsDevice
	{
		SharpDX.DXGI.SwapChain dxgiSwapChain;
		SharpDX.Direct3D11.Device d3dDevice;
		SharpDX.Direct3D11.DeviceContext d3dContext;

		SharpDX.Direct3D11.RenderTargetView renderTarget;
		SharpDX.Direct3D11.DepthStencilView depthStencil;
		SharpDX.Direct3D11.Texture2D depthStencilBuffer;

		internal Shader currentVertexShader;

		public object Handle { get { return d3dDevice; } }

		public IGraphicsDeviceInformation Information
		{
			get { throw new NotImplementedException (); }
		}

		public IRenderBuffer BackBuffer
		{
			get { throw new NotImplementedException (); }
		}
		public IRenderBuffer CurrentRenderBuffer { get; private set; }

		public CullingMode CullMode
		{
			get { return ConvertFromCullMode ( d3dContext.Rasterizer.State.Description.CullMode ); }
			set
			{
				throw new NotImplementedException ();
			}
		}

		public FillMode FillMode
		{
			get { return ConvertFromFillMode ( d3dContext.Rasterizer.State.Description.FillMode ); }
			set
			{
				throw new NotImplementedException ();
			}
		}

		public bool IsZWriteEnable
		{
			get { return d3dContext.Rasterizer.State.Description.IsDepthClipEnabled; }
			set
			{
				throw new NotImplementedException ();
			}
		}

		public bool BlendState
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

		public bool StencilState
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

		public bool IsMultisampleRendering
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

		public bool IsFullscreen
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

		public ScreenResolution FullscreenResolution
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

		public BlendOperation BlendOperation
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

		public StencilOperation StencilOperation
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

		public bool VerticalSyncMode
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

		public GraphicsDevice ( IWindow window )
		{
			SharpDX.Direct3D11.Device.CreateWithSwapChain ( SharpDX.Direct3D.DriverType.Hardware, SharpDX.Direct3D11.DeviceCreationFlags.None,
				new SharpDX.DXGI.SwapChainDescription ()
				{
					BufferCount = 1,
					Flags = SharpDX.DXGI.SwapChainFlags.None,
					IsWindowed = true,
					ModeDescription = new SharpDX.DXGI.ModeDescription ( 800, 600, new SharpDX.DXGI.Rational ( 1, 60 ), SharpDX.DXGI.Format.R8G8B8A8_UNorm ),
					OutputHandle = ( window.Handle as System.Windows.Forms.Form ).Handle,
					SampleDescription = new SharpDX.DXGI.SampleDescription ( 1, 0 ),
					SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
					Usage = SharpDX.DXGI.Usage.RenderTargetOutput
				},
				out d3dDevice, out dxgiSwapChain );
			d3dContext = d3dDevice.ImmediateContext;

			var backBuffer = dxgiSwapChain.GetBackBuffer<SharpDX.Direct3D11.Resource> ( 0 );
			renderTarget = new SharpDX.Direct3D11.RenderTargetView ( d3dDevice, backBuffer );
			depthStencilBuffer = new SharpDX.Direct3D11.Texture2D ( d3dDevice, new SharpDX.Direct3D11.Texture2DDescription ()
			{
				Width = 800,
				Height = 600,
				MipLevels = 1,
				ArraySize = 1,
				Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt
			} );
			depthStencil = new SharpDX.Direct3D11.DepthStencilView ( d3dDevice, depthStencilBuffer );

			d3dContext.OutputMerger.SetRenderTargets ( depthStencil, renderTarget );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				depthStencil.Dispose ();
				depthStencilBuffer.Dispose ();
				renderTarget.Dispose ();
				d3dDevice.Dispose ();
				dxgiSwapChain.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		public void BeginScene ( IRenderBuffer renderBuffer = null )
		{
			throw new NotImplementedException ();
		}

		public void EndScene ()
		{
			throw new NotImplementedException ();
		}

		public void Clear ( ClearBuffer clearBuffer, Color color, float depth = 0, int stencil = 0 )
		{
			if(clearBuffer.HasFlag(ClearBuffer.ColorBuffer))
			d3dContext.ClearRenderTargetView ( renderTarget, ConvertColor ( color ) );
			d3dContext.ClearDepthStencilView ( depthStencil, ConvertDepthStencilClearMethod ( clearBuffer ), depth, ( byte ) stencil );
		}

		public void SwapBuffer ()
		{
			dxgiSwapChain.Present ( 1, SharpDX.DXGI.PresentFlags.None );
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount )
		{
			( vertexDeclaration as VertexDeclaration ).GenerateInputLayout ( this, currentVertexShader );
			d3dContext.InputAssembler.InputLayout = vertexDeclaration.Handle as SharpDX.Direct3D11.InputLayout;
			d3dContext.InputAssembler.SetVertexBuffers ( 0, 
				new SharpDX.Direct3D11.VertexBufferBinding ( vertexBuffer.Handle as SharpDX.Direct3D11.Buffer, vertexBuffer.VertexTypeSize, 0 )
			);
			d3dContext.InputAssembler.PrimitiveTopology = ConvertToPrimitiveTopology ( primitiveType );
			d3dContext.Draw ( primitiveCount * GetPrimitiveCount ( primitiveType ), startVertex );
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, IIndexBuffer indexBuffer, int startIndex, int primitiveCount )
		{
			( vertexDeclaration as VertexDeclaration ).GenerateInputLayout ( this, currentVertexShader );
			d3dContext.InputAssembler.InputLayout = vertexDeclaration.Handle as SharpDX.Direct3D11.InputLayout;
			d3dContext.InputAssembler.SetVertexBuffers ( 0,
				new SharpDX.Direct3D11.VertexBufferBinding ( vertexBuffer.Handle as SharpDX.Direct3D11.Buffer, vertexBuffer.VertexTypeSize, 0 )
			);
			d3dContext.InputAssembler.SetIndexBuffer ( indexBuffer.Handle as SharpDX.Direct3D11.Buffer,
				indexBuffer.Is16bitIndex ? SharpDX.DXGI.Format.R16_SInt : SharpDX.DXGI.Format.R32_SInt, 0 );
			d3dContext.InputAssembler.PrimitiveTopology = ConvertToPrimitiveTopology ( primitiveType );
			d3dContext.DrawIndexed ( primitiveCount * GetPrimitiveCount ( primitiveType ), startIndex, 0 );
		}

		public void ResizeBackBuffer ( int width, int height )
		{
			dxgiSwapChain.ResizeBuffers ( 1, width, height, SharpDX.DXGI.Format.R8G8B8A8_UNorm, SharpDX.DXGI.SwapChainFlags.None );
		}

		public IRenderBuffer CreateRenderBuffer ( int width, int height )
		{
			throw new NotImplementedException ();
		}

		public ITexture2D CreateTexture2D ( int width, int height, int mipmapLevel = 1 ) { return new Texture2D ( this, width, height, mipmapLevel ); }
		public ITexture2D CreateTexture2D ( Contents.ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{
			return new Texture2D ( this, imageInfo, colorKey, mipmapLevel );
		}

		public IVertexDeclaration CreateVertexDeclaration ( params VertexElement [] elements )
		{
			throw new NotImplementedException ();
		}

		public IVertexBuffer CreateVertexBuffer ( Type vertexType, int length ) { return new VertexBuffer ( this, vertexType, length ); }
		public IVertexBuffer CreateVertexBuffer<T> ( T [] vertices ) where T : struct
		{
			VertexBuffer b = new VertexBuffer ( this, typeof ( T ), vertices.Length );
			b.SetBufferDatas<T> ( vertices );
			return b;
		}

		public IIndexBuffer CreateIndexBuffer ( Type indexType, int length, bool is16bit = false ) { return new IndexBuffer ( this, indexType, length, is16bit ); }
		public IIndexBuffer CreateIndexBuffer<T> ( T [] indices, bool is16bit = false ) where T : struct
		{
			IndexBuffer b = new IndexBuffer ( this, typeof ( T ), indices.Length, is16bit );
			b.SetBufferDatas<T> ( indices );
			return b;
		}

		public IShader CreateShader ( ShaderType shaderType, string shader )
		{
			throw new NotImplementedException ();
		}

		public IEffect CreateEffect ( IShader vertexShader, IShader pixelShader, IShader geometryShader = null )
		{
			throw new NotImplementedException ();
		}

		public IEffect CreateEffect ( System.IO.Stream stream )
		{
			throw new NotImplementedException ();
		}

#pragma warning disable
		public event EventHandler DeviceLost;
	}
}
