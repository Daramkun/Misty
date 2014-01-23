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
			BackBuffer = new BackBuffer ( this );

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

		public event EventHandler DeviceLost;
	}
}
