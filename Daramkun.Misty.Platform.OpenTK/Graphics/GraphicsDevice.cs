using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Platforms;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	public partial class GraphicsDevice : StandardDispose, IGraphicsDevice
	{
		OpenTK.GameWindow window;

		IGraphicsDeviceInformation deviceInfo;

		public object Handle { get { return window; } }
		public IGraphicsDeviceInformation Information { get { return deviceInfo; } }

		public IRenderBuffer BackBuffer { get; private set; }
		public IRenderBuffer CurrentRenderBuffer { get; private set; }
		public IGraphicsContext ImmediateContext { get; private set; }

		public bool IsMultisampleEnabled
		{
			get { return GL.IsEnabled ( EnableCap.Multisample ); }
			set { if ( value ) GL.Enable ( EnableCap.Multisample ); else GL.Disable ( EnableCap.Multisample ); }
		}

		public bool IsFullscreen
		{
			get { return window.WindowState == OpenTK.WindowState.Fullscreen; }
			set
			{
				window.WindowState = value ? OpenTK.WindowState.Fullscreen : OpenTK.WindowState.Normal;
				if ( !value ) OpenTK.DisplayDevice.Default.RestoreResolution ();
				else window.ClientSize = new System.Drawing.Size ( OpenTK.DisplayDevice.Default.Width, OpenTK.DisplayDevice.Default.Height );
			}
		}

		public ScreenResolution FullscreenResolution
		{
			get
			{
				OpenTK.DisplayDevice device = OpenTK.DisplayDevice.Default;
				return new ScreenResolution () { ScreenSize = new Vector2 ( device.Width, device.Height ), RefreshRate = device.RefreshRate };
			}
			set
			{
				OpenTK.DisplayDevice.Default.ChangeResolution ( ( int ) value.ScreenSize.X, ( int ) value.ScreenSize.Y, 32, value.RefreshRate );
				window.ClientSize = new System.Drawing.Size ( ( int ) value.ScreenSize.X, ( int ) value.ScreenSize.Y );
			}
		}

		public bool VerticalSyncMode
		{
			get { return window.VSync == OpenTK.VSyncMode.On; }
			set { window.VSync = ( value ) ? OpenTK.VSyncMode.On : OpenTK.VSyncMode.Off; }
		}

		public GraphicsDevice ( IWindow window )
		{
			this.window = window.Handle as OpenTK.GameWindow;

			deviceInfo = new GraphicsDeviceInformation ();
			BackBuffer = new BackBuffer ( this.window );

			if ( deviceInfo.RendererVersion.Major < 2 )
				throw new PlatformNotSupportedException ();

			ImmediateContext = new GraphicsContext ( this, true );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				window = null;
			}
			base.Dispose ( isDisposing );
		}

		public void SwapBuffer () { window.SwapBuffers (); }

		public void ResizeBackBuffer ( int width, int height )
		{
			window.ClientSize = new System.Drawing.Size ( width, height );
			window.X = Screen.PrimaryScreen.WorkingArea.Width / 2 - window.Width / 2;
			window.Y = Screen.PrimaryScreen.WorkingArea.Height / 2 - window.Height / 2;
			if ( BackbufferResized != null )
				BackbufferResized ( this, null );
		}

		public IGraphicsContext CreateDeferredContext () { return new GraphicsContext ( this, false ); }

		public IRenderBuffer CreateRenderBuffer ( int width, int height ) { return new RenderBuffer ( this, width, height ); }

		public ITexture1D CreateTexture1D ( int width, int mipmapLevel = 1 ) { return new Texture1D ( this, width, mipmapLevel ); }
		public ITexture1D CreateTexture1D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{ return new Texture1D ( this, imageInfo, colorKey, mipmapLevel ); }
		public ITexture2D CreateTexture2D ( int width, int height, int mipmapLevel = 1 ) { return new Texture2D ( this, width, height, mipmapLevel ); }
		public ITexture2D CreateTexture2D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{ return new Texture2D ( this, imageInfo, colorKey, mipmapLevel ); }
		public ITexture3D CreateTexture3D ( int width, int height, int depth, int mipmapLevel = 1 ) { return new Texture3D ( this, width, height, depth, mipmapLevel ); }
		public ITexture3D CreateTexture3D ( ImageInfo [] imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{ return new Texture3D ( this, imageInfo, colorKey, mipmapLevel ); }

		public IVertexDeclaration CreateVertexDeclaration ( params VertexElement [] elements ) { return new VertexDeclaration ( this, elements ); }

		public IBuffer CreateBuffer ( BufferType bufferType, Type vertexType, int length ) { return new Buffer ( this, vertexType, length, bufferType ); }
		public IBuffer CreateBuffer<T> ( BufferType bufferType, T [] vertices ) where T : struct
		{
			IBuffer buffer = new Buffer ( this, typeof ( T ), vertices.Length, bufferType );
			buffer.SetBufferDatas<T> ( vertices );
			return buffer;
		}

		public IShader CreateShader ( ShaderType shaderType, string shader ) { return new Shader ( this, shaderType, shader ); }

		public IEffect CreateEffect ( IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName )
		{
			return new Effect ( this, vertexShader, pixelShader, geometryShader, attribName );
		}
		public IEffect CreateEffect ( Stream xmlStream ) { return new Effect ( this, xmlStream ); }

#pragma warning disable
		public event EventHandler DeviceLost;
		public event EventHandler BackbufferResized;
	}
}
