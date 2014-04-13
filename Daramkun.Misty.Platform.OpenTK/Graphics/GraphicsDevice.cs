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

		public object Handle { get { return window.Context; } }
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

			OpenTK.Graphics.GraphicsContext.ShareContexts = true;

			ImmediateContext = new GraphicsContext ( this );
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
