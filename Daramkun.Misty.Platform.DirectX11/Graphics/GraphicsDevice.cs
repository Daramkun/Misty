using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Graphics
{
	public partial class GraphicsDevice : StandardDispose, IGraphicsDevice
	{
		SharpDX.DXGI.SwapChain dxgiSwapChain;
		SharpDX.Direct3D11.Device d3dDevice;

		SharpDX.Direct3D11.RenderTargetView renderTarget;
		SharpDX.Direct3D11.DepthStencilView depthStencil;
		SharpDX.Direct3D11.Texture2D depthStencilBuffer;

		public object Handle { get { return d3dDevice; } }

		public IGraphicsDeviceInformation Information
		{
			get { throw new NotImplementedException (); }
		}

		public IRenderBuffer BackBuffer { get; private set; }
		public IRenderBuffer CurrentRenderBuffer { get; private set; }
		public IGraphicsContext ImmediateContext { get; private set; }

		public bool IsMultisampleEnabled
		{
			get { return false; }
			set { throw new NotImplementedException (); }
		}

		public bool IsFullscreen
		{
			get { return dxgiSwapChain.IsFullScreen; }
			set { dxgiSwapChain.IsFullScreen = value; }
		}

		public ScreenResolution FullscreenResolution
		{
			get
			{
				return new ScreenResolution () {
					ScreenSize = new Vector2 ( dxgiSwapChain.Description.ModeDescription.Width, dxgiSwapChain.Description.ModeDescription.Height ),
					RefreshRate = dxgiSwapChain.Description.ModeDescription.RefreshRate.Numerator
				};
			}
			set
			{
				SharpDX.DXGI.ModeDescription mode = new SharpDX.DXGI.ModeDescription (
						( int ) value.ScreenSize.X, ( int ) value.ScreenSize.Y, new SharpDX.DXGI.Rational ( ( int ) value.RefreshRate, 1 ),
						SharpDX.DXGI.Format.R8G8B8A8_UNorm
				);
				dxgiSwapChain.ResizeTarget (
					ref mode
				);
			}
		}

		public bool VerticalSyncMode { get; set; }

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
			MakeDepthStencil ( 800, 600 );
		}

		private void MakeDepthStencil ( int width, int height )
		{
			SharpDX.Direct3D11.Texture2D tempDsBuffer = this.depthStencilBuffer;
			SharpDX.Direct3D11.DepthStencilView tempDs = this.depthStencil;

			SharpDX.Direct3D11.Texture2D depthStencilBuffer = new SharpDX.Direct3D11.Texture2D ( d3dDevice, new SharpDX.Direct3D11.Texture2DDescription ()
			{
				Width = width,
				Height = height,
				MipLevels = 1,
				ArraySize = 1,
				Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
				BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
				SampleDescription = new SharpDX.DXGI.SampleDescription ( 1, 0 ),
				Usage = SharpDX.Direct3D11.ResourceUsage.Default,
			} );
			SharpDX.Direct3D11.DepthStencilView depthStencil = new SharpDX.Direct3D11.DepthStencilView ( d3dDevice, depthStencilBuffer );

			d3dContext.OutputMerger.SetRenderTargets ( depthStencil, renderTarget );

			tempDs.Dispose ();
			tempDsBuffer.Dispose ();
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

		public void SwapBuffer ()
		{
			dxgiSwapChain.Present ( VerticalSyncMode ? 1 : 0, SharpDX.DXGI.PresentFlags.None );
		}

		public void ResizeBackBuffer ( int width, int height )
		{
			dxgiSwapChain.ResizeBuffers ( 1, width, height, SharpDX.DXGI.Format.R8G8B8A8_UNorm, SharpDX.DXGI.SwapChainFlags.None );
			MakeDepthStencil ( width, height );
			if ( BackbufferResized != null )
				BackbufferResized ( this, null );
		}

		public IRenderBuffer CreateRenderBuffer ( int width, int height )
		{
			return new RenderBuffer ( this, width, height );
		}

		public ITexture1D CreateTexture1D ( int width, int mipmapLevel = 1 ) { throw new NotImplementedException (); }
		public ITexture1D CreateTexture1D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 ) { throw new NotImplementedException (); }
		public ITexture2D CreateTexture2D ( int width, int height, int mipmapLevel = 1 )
		{ return new Texture2D ( this, width, height, mipmapLevel ); }
		public ITexture2D CreateTexture2D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{ return new Texture2D ( this, imageInfo, colorKey, mipmapLevel ); }
		public ITexture3D CreateTexture3D ( int width, int height, int depth, int mipmapLevel = 1 ) { throw new NotImplementedException (); }
		public ITexture3D CreateTexture3D ( ImageInfo [] imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{ throw new NotImplementedException (); }

		public IVertexDeclaration CreateVertexDeclaration ( params VertexElement [] elements ) { return new VertexDeclaration ( this, elements ); }

		public IVertexBuffer CreateVertexBuffer ( Type vertexType, int length ) { return new VertexBuffer ( this, vertexType, length ); }
		public IVertexBuffer CreateVertexBuffer<T> ( T [] vertices ) where T : struct
		{
			IVertexBuffer buffer = new VertexBuffer ( this, typeof ( T ), vertices.Length );
			buffer.SetBufferDatas<T> ( vertices );
			return buffer;
		}

		public IIndexBuffer CreateIndexBuffer ( Type indexType, int length, bool is16bit = false ) { return new IndexBuffer ( this, indexType, length, is16bit ); }
		public IIndexBuffer CreateIndexBuffer<T> ( T [] indices, bool is16bit = false ) where T : struct
		{
			IIndexBuffer buffer = new IndexBuffer ( this, typeof ( T ), indices.Length, is16bit );
			buffer.SetBufferDatas<T> ( indices );
			return buffer;
		}

		public IShader CreateShader ( ShaderType shaderType, string shader ) { return new Shader ( this, shaderType, shader ); }

		public IEffect CreateEffect ( IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName )
		{
			return new Effect ( this, vertexShader, pixelShader, geometryShader, attribName );
		}
		public IEffect CreateEffect ( Stream xmlStream )
		{
			TextReader reader = new StreamReader ( xmlStream );
			XmlDocument doc = new XmlDocument ();
			doc.LoadXml ( reader.ReadToEnd () );
			return CreateEffect ( doc );
		}
		public IEffect CreateEffect ( XmlDocument xmlDoc ) { return new Effect ( this, xmlDoc ); }

#pragma warning disable
		public event EventHandler DeviceLost;
		public event EventHandler BackbufferResized;
	}
}
