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
	class GraphicsDeviceInformation : IGraphicsDeviceInformation
	{
		SharpDX.Direct3D9.Direct3D d3d;
		SharpDX.Direct3D9.Capabilities d3dCaps;

		public BaseRenderer BaseRenderer { get { return BaseRenderer.DirectX; } }
		public Version RendererVersion { get { return new Version ( 9, 0 ); } }
		public Version ShaderVersion { get { return new Version ( 3, 0 ); } }

		public ScreenResolution [] AvailableScreenResolution
		{
			get
			{
				List<ScreenResolution> sizes = new List<ScreenResolution> ();
				int count = d3d.GetAdapterModeCount ( 0, SharpDX.Direct3D9.Format.X8R8G8B8 );
				for ( int i = 0; i < count; i++ )
				{
					SharpDX.Direct3D9.DisplayMode mode = d3d.EnumAdapterModes ( 0, SharpDX.Direct3D9.Format.X8R8G8B8, i );
					sizes.Add ( new ScreenResolution ( new Vector2 ( mode.Width, mode.Height ), mode.RefreshRate ) );
				}
				return sizes.ToArray ();
			}
		}

		public int MaximumAnisotropicLevel { get { return d3dCaps.MaxAnisotropy; } }

		public bool IsSupportTexture1D { get { return false; } }
		public bool IsSupportTexture3D { get { return false; } }
		public bool IsSupportGeometryShader { get { return false; } }

		public GraphicsDeviceInformation ( SharpDX.Direct3D9.Direct3D d3d )
		{
			this.d3d = d3d;
			d3dCaps = d3d.GetDeviceCaps ( 0, SharpDX.Direct3D9.DeviceType.Hardware );
		}
	}

	class BackBufferRenderBuffer : IRenderBuffer
	{
		GraphicsDevice graphicsDevice;

		public int Width { get { return graphicsDevice.d3dpp.BackBufferWidth; } }
		public int Height { get { return graphicsDevice.d3dpp.BackBufferHeight; } }
		public Vector2 Size { get { return new Vector2 ( Width, Height ); } }
		public object Handle { get { return ( graphicsDevice.Handle as SharpDX.Direct3D9.Device ).GetBackBuffer ( 0, 0 ); } }
		public Color [] Buffer
		{
			get
			{
				SharpDX.Direct3D9.Surface texture = Handle as SharpDX.Direct3D9.Surface;
				SharpDX.DataRectangle dr = texture.LockRectangle ( new SharpDX.Rectangle ( 0, 0, Width, Height ), SharpDX.Direct3D9.LockFlags.None );
				SharpDX.DataStream stream = new SharpDX.DataStream ( dr.DataPointer, Width * Height * 4, true, false );
				Color [] colours = new Color [ stream.Length / 4 ];
				for ( int i = 0; i < colours.Length; ++i )
					colours [ i ] = new Color ( stream.Read<SharpDX.Color> ().ToBgra (), true );
				texture.UnlockRectangle ();
				return colours;
			}
			set
			{
				SharpDX.Direct3D9.Surface texture = Handle as SharpDX.Direct3D9.Surface;
				SharpDX.DataRectangle dr = texture.LockRectangle ( new SharpDX.Rectangle ( 0, 0, Width, Height ), SharpDX.Direct3D9.LockFlags.None );
				SharpDX.DataStream stream = new SharpDX.DataStream ( dr.DataPointer, Width * Height * 4, false, true );
				SharpDX.Color [] colours = new SharpDX.Color [ stream.Length / 4 ];
				for ( int i = 0; i < value.Length; ++i )
					colours [ i ] = new SharpDX.Color ( value [ i ].ARGBValue );
				stream.WriteRange<SharpDX.Color> ( colours );
				texture.UnlockRectangle ();
			}
		}

		public BackBufferRenderBuffer ( GraphicsDevice graphicsDevice ) { this.graphicsDevice = graphicsDevice; }
		public void Dispose () { }
	}

	public partial class GraphicsDevice : StandardDispose, IGraphicsDevice
	{
		SharpDX.Direct3D9.Direct3D d3d;
		SharpDX.Direct3D9.Device d3dDevice;
		internal SharpDX.Direct3D9.PresentParameters d3dpp;

		public object Handle { get { return d3dDevice; } }

		public IGraphicsDeviceInformation Information { get; private set; }
		public IRenderBuffer BackBuffer { get; private set; }
		public IRenderBuffer CurrentRenderBuffer { get; private set; }

		public CullingMode CullMode
		{
			get { return ConvertCullMode ( d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.CullMode ) ); }
			set { d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.CullMode, ChangeCullMode ( value ) ); }
		}

		public FillMode FillMode
		{
			get { return ConvertFillMode ( d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.FillMode ) ); }
			set { d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.FillMode, ChangeFillMode ( value ) ); }
		}

		public bool IsZWriteEnable
		{
			get { return d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.ZWriteEnable ) != 0 ? true : false; }
			set { d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.ZWriteEnable, value ); }
		}

		public bool BlendState
		{
			get { return d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.AlphaBlendEnable ) != 0 ? true : false; }
			set { d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.AlphaBlendEnable, value ); }
		}

		public bool StencilState
		{
			get { return d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.StencilEnable ) != 0 ? true : false; }
			set { d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.StencilEnable, value ); }
		}

		public bool IsMultisampleRendering
		{
			get
			{
				return d3dpp.MultiSampleType != SharpDX.Direct3D9.MultisampleType.None;
			}
			set
			{
				if ( value ) d3dpp.MultiSampleType = SharpDX.Direct3D9.MultisampleType.TwoSamples;
				else d3dpp.MultiSampleType = SharpDX.Direct3D9.MultisampleType.None;
				d3dDevice.Reset ( d3dpp );
			}
		}

		public bool IsFullscreen
		{
			get { return !d3dpp.Windowed; }
			set { d3dpp.Windowed = !value; d3dDevice.Reset ( d3dpp ); }
		}

		public ScreenResolution FullscreenResolution
		{
			get { return new ScreenResolution ( new Vector2 ( d3dpp.BackBufferWidth, d3dpp.BackBufferHeight ), d3dpp.FullScreenRefreshRateInHz ); }
			set
			{
				d3dpp.BackBufferWidth = ( int ) value.ScreenSize.X;
				d3dpp.BackBufferHeight = ( int ) value.ScreenSize.Y;
				d3dpp.FullScreenRefreshRateInHz = ( int ) value.RefreshRate;
				d3dDevice.Reset ( d3dpp );
			}
		}

		public BlendOperation BlendOperation
		{
			get
			{
				int op = d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.BlendOperation );
				int sb = d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.SourceBlend );
				int db = d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.DestinationBlend );
				return new BlendOperation ( DeconvertBlendOp ( op ), DeconvertBlendParam ( sb ), DeconvertBlendParam ( db ) );
			}
			set
			{
				d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.BlendOperation, ConvertBlendOp ( value.Operator ) );
				d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.SourceBlend, ConvertBlendParam ( value.SourceParameter ) );
				d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.DestinationBlend, ConvertBlendParam ( value.DestinationParameter ) );
			}
		}

		public StencilOperation StencilOperation
		{
			get
			{
				return new StencilOperation (
					DeconvertStencilFunc ( d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.StencilFunc ) ),
					d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.StencilRef ),
					d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.StencilMask ),
					DeconvertStencilOp ( d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.StencilZFail ) ),
					DeconvertStencilOp ( d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.StencilFail ) ),
					DeconvertStencilOp ( d3dDevice.GetRenderState ( SharpDX.Direct3D9.RenderState.StencilPass ) )
				);
			}
			set
			{
				d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.StencilFunc, ConvertStencilFunc ( value.Function ) );
				d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.StencilRef, value.Reference );
				d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.StencilMask, value.Mask );
				d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.StencilZFail, ConvertStencilOp ( value.ZFail ) );
				d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.StencilFail, ConvertStencilOp ( value.Fail ) );
				d3dDevice.SetRenderState ( SharpDX.Direct3D9.RenderState.StencilPass, ConvertStencilOp ( value.Pass ) );
			}
		}

		public Viewport Viewport
		{
			get
			{
				SharpDX.Viewport viewPort = d3dDevice.Viewport;
				return new Viewport () { X = viewPort.X, Y = viewPort.Y, Width = viewPort.Width, Height = viewPort.Height };
			}
			set { d3dDevice.Viewport = new SharpDX.Viewport ( value.X, value.Y, value.Width, value.Height ); }
		}

		public bool VerticalSyncMode
		{
			get { return d3dpp.PresentationInterval == SharpDX.Direct3D9.PresentInterval.Default; }
			set
			{
				d3dpp.PresentationInterval = ( value ) ? SharpDX.Direct3D9.PresentInterval.Default : SharpDX.Direct3D9.PresentInterval.Immediate;
				d3dDevice.Reset ( d3dpp );
			}
		}

		public GraphicsDevice ( IWindow window )
		{
			d3d = new SharpDX.Direct3D9.Direct3D ();

			IntPtr handle = ( window.Handle as System.Windows.Forms.Form ).Handle;

			d3dpp = new SharpDX.Direct3D9.PresentParameters ( 800, 600, SharpDX.Direct3D9.Format.A8R8G8B8,
					1, SharpDX.Direct3D9.MultisampleType.None, 0, SharpDX.Direct3D9.SwapEffect.Discard,
					handle, true, true, SharpDX.Direct3D9.Format.D24S8, SharpDX.Direct3D9.PresentFlags.None,
					0, SharpDX.Direct3D9.PresentInterval.Immediate );

			try
			{
				d3dDevice = new SharpDX.Direct3D9.Device ( d3d, 0, SharpDX.Direct3D9.DeviceType.Hardware,
						handle, SharpDX.Direct3D9.CreateFlags.HardwareVertexProcessing,
						d3dpp );
			}
			catch
			{
				d3dDevice = new SharpDX.Direct3D9.Device ( d3d, 0, SharpDX.Direct3D9.DeviceType.Hardware,
						handle, SharpDX.Direct3D9.CreateFlags.SoftwareVertexProcessing,
						d3dpp );
			}

			Information = new GraphicsDeviceInformation ( d3d );
			BackBuffer = new BackBufferRenderBuffer ( this );

			CullMode = CullingMode.CounterClockWise;

			window.Resize += ( object sender, EventArgs e ) => { ResizeBackBuffer ( ( int ) window.ClientSize.X, ( int ) window.ClientSize.Y ); };
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				SharpDX.Direct3D9.Device device = d3dDevice;
				d3dDevice = null;
				device.Dispose ();
				d3d.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		public void BeginScene ( IRenderBuffer renderBuffer = null )
		{
			if ( d3dDevice == null ) return;
			CurrentRenderBuffer = renderBuffer;
			if ( renderBuffer == null || renderBuffer == BackBuffer ) { CurrentRenderBuffer = BackBuffer; d3dDevice.BeginScene (); }
			else
			{
				SharpDX.Direct3D9.Surface surface = ( renderBuffer.Handle as SharpDX.Direct3D9.Texture ).GetSurfaceLevel ( 0 );
				( renderBuffer as RenderBuffer ).rts.BeginScene ( surface, new SharpDX.Viewport ( 0, 0, renderBuffer.Width, renderBuffer.Height ) );
			}
		}

		public void EndScene ()
		{
			if ( d3dDevice == null ) return;
			if ( CurrentRenderBuffer != BackBuffer ) ( CurrentRenderBuffer as RenderBuffer ).rts.EndScene ( SharpDX.Direct3D9.Filter.Default );
			else d3dDevice.EndScene ();
			CurrentRenderBuffer = BackBuffer;
		}

		public void Clear ( ClearBuffer clearBuffer, Color color, float depth = 1, int stencil = 0 )
		{
			if ( d3dDevice == null ) return;
			d3dDevice.Clear ( ChangeClearBuffer ( clearBuffer ), ChangeColor ( color ), depth, stencil );
		}

		public void SwapBuffer ()
		{
			if ( d3dDevice == null ) return;
			try
			{
				d3dDevice.Present ();
			}
			catch { d3dDevice.Reset ( d3dpp ); if ( DeviceLost != null )DeviceLost ( this, EventArgs.Empty ); }
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount )
		{
			if ( d3dDevice == null ) return;
			d3dDevice.VertexDeclaration = vertexDeclaration.Handle as SharpDX.Direct3D9.VertexDeclaration;
			d3dDevice.SetStreamSource ( 0, vertexBuffer.Handle as SharpDX.Direct3D9.VertexBuffer, 0, vertexBuffer.VertexTypeSize );
			d3dDevice.DrawPrimitives ( ConvertPrimitiveType ( primitiveType ), startVertex, primitiveCount );
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, IIndexBuffer indexBuffer, 
			int startIndex, int primitiveCount )
		{
			if ( d3dDevice == null ) return;
			d3dDevice.VertexDeclaration = vertexDeclaration.Handle as SharpDX.Direct3D9.VertexDeclaration;
			d3dDevice.SetStreamSource ( 0, vertexBuffer.Handle as SharpDX.Direct3D9.VertexBuffer, 0, vertexBuffer.VertexTypeSize );
			d3dDevice.Indices = indexBuffer.Handle as SharpDX.Direct3D9.IndexBuffer;
			d3dDevice.DrawIndexedPrimitive ( ConvertPrimitiveType ( primitiveType ), 0, 0, vertexBuffer.Length,
				startIndex, primitiveCount );
		}

		public void ResizeBackBuffer ( int width, int height )
		{
			d3dpp.BackBufferWidth = width;
			d3dpp.BackBufferHeight = height;
			d3dDevice.Reset ( d3dpp );
		}

		public IRenderBuffer CreateRenderBuffer ( int width, int height )
		{
			return new RenderBuffer ( this, width, height );
		}

		public ITexture2D CreateTexture2D ( int width, int height, int mipmapLevel = 1 ) { return new Texture2D ( this, width, height, mipmapLevel ); }
		public ITexture2D CreateTexture2D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{ return new Texture2D ( this, imageInfo, colorKey, mipmapLevel ); }

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

		public IEffect CreateEffect ( IShader vertexShader, IShader pixelShader, IShader geometryShader = null )
		{
			return new Effect ( this, vertexShader, pixelShader, geometryShader );
		}
		public IEffect CreateEffect ( Stream xmlStream )
		{
			TextReader reader = new StreamReader ( xmlStream );
			XmlDocument doc = new XmlDocument ();
			doc.LoadXml ( reader.ReadToEnd () );
			return CreateEffect ( doc );
		}
		public IEffect CreateEffect ( XmlDocument xmlDoc ) { return new Effect ( this, xmlDoc ); }

		public event EventHandler DeviceLost;
	}
}
