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
		IWindow window;
		SharpDX.Direct3D9.Direct3D d3d;
		SharpDX.Direct3D9.Device d3dDevice;
		internal SharpDX.Direct3D9.PresentParameters d3dpp;

		public object Handle { get { return d3dDevice; } }

		public IGraphicsDeviceInformation Information { get; private set; }
		public IRenderBuffer BackBuffer { get; private set; }
		public IRenderBuffer CurrentRenderBuffer { get; private set; }
		public IGraphicsContext ImmediateContext { get; private set; }

		public bool IsMultisampleEnabled
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
			get { return new ScreenResolution () { ScreenSize = new Vector2 ( d3dpp.BackBufferWidth, d3dpp.BackBufferHeight ), RefreshRate = d3dpp.FullScreenRefreshRateInHz }; }
			set
			{
				d3dpp.BackBufferWidth = ( int ) value.ScreenSize.X;
				d3dpp.BackBufferHeight = ( int ) value.ScreenSize.Y;
				d3dpp.FullScreenRefreshRateInHz = ( int ) value.RefreshRate;
				d3dDevice.Reset ( d3dpp );
			}
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
			this.window = window;

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

			ImmediateContext = new GraphicsContext ( this );
			//window.Resize += ( object sender, EventArgs e ) => { ResizeBackBuffer ( ( int ) window.ClientSize.X, ( int ) window.ClientSize.Y ); };
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

		public void SwapBuffer ()
		{
			if ( d3dDevice == null ) return;
			try
			{
				d3dDevice.Present ();
			}
			catch { d3dDevice.Reset ( d3dpp ); if ( DeviceLost != null )DeviceLost ( this, EventArgs.Empty ); }
		}

		public void ResizeBackBuffer ( int width, int height )
		{
			d3dpp.BackBufferWidth = width;
			d3dpp.BackBufferHeight = height;
			d3dDevice.Reset ( d3dpp );

			( window.Handle as System.Windows.Forms.Form ).ClientSize = new System.Drawing.Size ( width, height );
			( window.Handle as System.Windows.Forms.Form ).Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 2 - ( window.Handle as System.Windows.Forms.Form ).Width / 2;
			( window.Handle as System.Windows.Forms.Form ).Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 2 - ( window.Handle as System.Windows.Forms.Form ).Height / 2;
			
			if ( BackbufferResized != null )
				BackbufferResized ( this, null );
		}

		public event EventHandler DeviceLost;
		public event EventHandler BackbufferResized;
	}
}
