using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Platforms;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	class GraphicsDeviceInformation : IGraphicsDeviceInformation
	{
		public BaseRenderer BaseRenderer { get { return Graphics.BaseRenderer.OpenGL; } }
		public Version RendererVersion
		{
			get
			{
				string versionString = GL.GetString ( StringName.Version );
				int index = versionString.IndexOf ( ' ' );
				if ( index <= -1 ) index = versionString.IndexOf ( '-' );
				if ( index <= -1 ) index = versionString.Length;
				string [] v = versionString.Substring ( 0, index ).Trim ().Split ( '.' );
				return new Version ( int.Parse ( v [ 0 ] ), int.Parse ( v [ 1 ] ) );
			}
		}
		public ScreenResolution [] AvailableScreenResolution
		{
			get
			{
				List<ScreenResolution> screenSizes = new List<ScreenResolution> ();
				foreach ( OpenTK.DisplayResolution resolution in OpenTK.DisplayDevice.Default.AvailableResolutions )
					screenSizes.Add ( new ScreenResolution ( new Vector2 ( resolution.Width, resolution.Height ), resolution.RefreshRate ) );
				return screenSizes.ToArray ();
			}
		}
		public int MaximumAnisotropicLevel
		{
			get
			{
				int level;
				GL.GetInteger ( ( GetPName ) ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out level );
				return level;
			}
		}
		public bool IsSupportTexture1D { get { return false; } }
		public bool IsSupportTexture3D { get { return false; } }
		public bool IsSupportGeometryShader { get { return RendererVersion >= new Version ( 3, 2 ); } }
		public Version ShaderVersion
		{
			get
			{
				if ( RendererVersion == new Version ( 2, 0 ) ) return new Version ( 1, 1 );
				else if ( RendererVersion == new Version ( 2, 1 ) ) return new Version ( 1, 2 );
				else if ( RendererVersion == new Version ( 3, 0 ) ) return new Version ( 1, 3 );
				else if ( RendererVersion == new Version ( 3, 1 ) ) return new Version ( 1, 4 );
				else if ( RendererVersion == new Version ( 3, 2 ) ) return new Version ( 1, 5 );
				else if ( RendererVersion == new Version ( 3, 3 ) ) return new Version ( 3, 3 );
				else if ( RendererVersion == new Version ( 4, 0 ) ) return new Version ( 4, 0 );
				else if ( RendererVersion == new Version ( 4, 1 ) ) return new Version ( 4, 1 );
				else if ( RendererVersion == new Version ( 4, 2 ) ) return new Version ( 4, 2 );
				else if ( RendererVersion == new Version ( 4, 3 ) ) return new Version ( 4, 3 );
				else if ( RendererVersion == new Version ( 4, 4 ) ) return new Version ( 4, 4 );
				else throw new PlatformNotSupportedException ();
			}
		}
	}

	class BackBuffer : StandardDispose, IRenderBuffer
	{
		OpenTK.GameWindow window;

		public int Width { get { return window.ClientSize.Width; } }
		public int Height { get { return window.ClientSize.Height; } }

		public object Handle { get { return 0; } }

		public Vector2 Size { get { return new Vector2 ( Width, Height ); } }

		public Color [] Buffer
		{
			get
			{
				byte [] raws = new byte [ Width * Height * 4 ];
				GL.BindTexture ( TextureTarget.Texture2D, 0 );
				GL.GetTexImage ( TextureTarget.Texture2D, 0, PixelFormat.Bgra, PixelType.UnsignedByte, raws );
				Color [] pixels = new Color [ Width * Height ];
				for ( int i = 0, index = 0; i < pixels.Length; i += 4 )
				{
					byte blue = raws [ i + 0 ];
					byte green = raws [ i + 1 ];
					byte red = raws [ i + 2 ];
					byte alpha = raws [ i + 3 ];
					pixels [ index++ ] = new Color ( red, green, blue, alpha );
				}
				return pixels;
			}
			set
			{
				GL.BindTexture ( TextureTarget.Texture2D, 0 );

				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ( int ) TextureMagFilter.Nearest );
				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ( int ) TextureMinFilter.Nearest );
				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ( int ) TextureWrapMode.Repeat );
				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ( int ) TextureWrapMode.Repeat );

				byte [] colorData = new byte [ Width * Height * 4 ];

				for ( int i = 0, index = 0; i < value.Length; i++ )
				{
					colorData [ index++ ] = value [ i ].BlueValue;
					colorData [ index++ ] = value [ i ].GreenValue;
					colorData [ index++ ] = value [ i ].RedValue;
					colorData [ index++ ] = value [ i ].AlphaValue;
				}

				GL.TexImage2D<byte> ( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8,
					Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
					PixelType.UnsignedByte, colorData );

				GL.BindTexture ( TextureTarget.Texture2D, 0 );
			}
		}

		public BackBuffer ( OpenTK.GameWindow window )
		{
			this.window = window;
		}
	}

	public partial class GraphicsDevice : StandardDispose, IGraphicsDevice
	{
		OpenTK.GameWindow window;

		IGraphicsDeviceInformation deviceInfo;

		public object Handle { get { return window.Context; } }
		public IGraphicsDeviceInformation Information { get { return deviceInfo; } }

		public IRenderBuffer BackBuffer { get; private set; }

		public CullingMode CullMode
		{
			get
			{
				bool cullFace;
				GL.GetBoolean ( GetPName.CullFace, out cullFace );
				if ( cullFace ) return CullingMode.None;
				int frontFace;
				GL.GetInteger ( GetPName.FrontFace, out frontFace );
				switch ( ( FrontFaceDirection ) frontFace )
				{
					case FrontFaceDirection.Cw: return CullingMode.ClockWise;
					case FrontFaceDirection.Ccw: return CullingMode.CounterClockWise;
					default: throw new ArgumentException ();
				}
			}
			set
			{
				if ( value == CullingMode.None ) GL.Disable ( EnableCap.CullFace );
				else GL.Enable ( EnableCap.CullFace );
				GL.FrontFace ( ( value == CullingMode.ClockWise ) ? FrontFaceDirection.Cw : FrontFaceDirection.Ccw );
			}
		}

		public FillMode FillMode
		{
			get { int fillMode; GL.GetInteger ( GetPName.PolygonMode, out fillMode ); return OriginalToMistyValue ( ( PolygonMode ) fillMode ); }
			set { GL.PolygonMode ( MaterialFace.FrontAndBack, MistyValueToOriginal ( value ) ); }
		}

		public bool IsZWriteEnable
		{
			get { return GL.IsEnabled ( EnableCap.DepthTest ); }
			set { if ( value ) GL.Enable ( EnableCap.DepthTest ); else GL.Disable ( EnableCap.DepthTest ); }
		}

		public bool IsMultisampleRendering
		{
			get { return GL.IsEnabled ( EnableCap.Multisample ); }
			set { if ( value ) GL.Enable ( EnableCap.Multisample ); else GL.Disable ( EnableCap.Multisample ); }
		}

		public bool BlendState
		{
			get { return GL.IsEnabled ( EnableCap.Blend ); }
			set { if ( value ) GL.Enable ( EnableCap.Blend ); else GL.Disable ( EnableCap.Blend ); }
		}

		public bool StencilState
		{
			get { return GL.IsEnabled ( EnableCap.StencilTest ); }
			set { if ( value ) GL.Enable ( EnableCap.StencilTest ); else GL.Disable ( EnableCap.StencilTest ); }
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
				return new ScreenResolution ( new Vector2 ( device.Width, device.Height ), device.RefreshRate );
			}
			set
			{
				OpenTK.DisplayDevice.Default.ChangeResolution ( ( int ) value.ScreenSize.X, ( int ) value.ScreenSize.Y, 32, value.RefreshRate );
				window.ClientSize = new System.Drawing.Size ( ( int ) value.ScreenSize.X, ( int ) value.ScreenSize.Y );
			}
		}

		public BlendOperation BlendOperation
		{
			get
			{
				int op, dest, src;
				GL.GetInteger ( GetPName.Blend, out op ); GL.GetInteger ( GetPName.BlendDst, out dest ); GL.GetInteger ( GetPName.BlendSrc, out src );
				return new Graphics.BlendOperation (
					OriginalToMistyValue ( ( BlendEquationMode ) op ),
					OriginalToMistyValue ( ( BlendingFactorSrc ) src ),
					OriginalToMistyValue ( ( BlendingFactorDest ) dest )
				);
			}
			set
			{
				GL.BlendFunc (
					MistyValueToOriginal ( value.SourceParameter ),
					( BlendingFactorDest ) MistyValueToOriginal ( value.DestinationParameter )
				);
				GL.BlendEquation ( MistyValueToOriginal ( value.Operator ) );
			}
		}

		public StencilOperation StencilOperation
		{
			get
			{
				int func, mask, fail, zfail, pass, reference;
				GL.GetInteger ( GetPName.StencilFunc, out func ); GL.GetInteger ( GetPName.StencilValueMask, out mask );
				GL.GetInteger ( GetPName.StencilFail, out fail ); GL.GetInteger ( GetPName.StencilPassDepthFail, out zfail );
				GL.GetInteger ( GetPName.StencilPassDepthPass, out pass ); GL.GetInteger ( GetPName.StencilRef, out reference );
				return new StencilOperation ( ( StencilFunction ) func, reference, mask, ( StencilOperator ) zfail, ( StencilOperator ) fail, ( StencilOperator ) pass );
			}
			set
			{
				GL.StencilFunc ( MistyValueToOriginal ( value.Function ), value.Reference, value.Mask );
				GL.StencilOp ( MistyValueToOriginal ( value.Fail ), MistyValueToOriginal ( value.ZFail ),
					MistyValueToOriginal ( value.Pass ) );
			}
		}

		public Viewport Viewport
		{
			get { int [] viewport = new int [ 4 ]; GL.GetInteger ( GetPName.Viewport, viewport ); return new Viewport ( viewport ); }
			set { GL.Viewport ( value.X, value.Y, value.Width, value.Height ); }
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
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{

			}
			base.Dispose ( isDisposing );
		}

		public void BeginScene ( IRenderBuffer renderBuffer = null )
		{
			if ( renderBuffer != null && renderBuffer != BackBuffer )
			{
				GL.BindFramebuffer ( FramebufferTarget.Framebuffer, ( renderBuffer as RenderBuffer ).frameBuffer );
				GL.BindFramebuffer ( FramebufferTarget.DrawFramebuffer, ( renderBuffer as RenderBuffer ).frameBuffer );
				GL.BindFramebuffer ( FramebufferTarget.ReadFramebuffer, ( renderBuffer as RenderBuffer ).frameBuffer );
			}
		}

		public void EndScene ()
		{
			GL.BindFramebuffer ( FramebufferTarget.Framebuffer, 0 );
			GL.BindFramebuffer ( FramebufferTarget.DrawFramebuffer, 0 );
			GL.BindFramebuffer ( FramebufferTarget.ReadFramebuffer, 0 );
		}

		public void Clear ( ClearBuffer clearBuffer, Color color, float depth = 1, int stencil = 0 )
		{
			GL.ClearColor ( color.RedScalar, color.GreenScalar, color.BlueScalar, color.AlphaScalar );
			GL.ClearDepth ( depth );
			GL.ClearStencil ( stencil );
			GL.Clear ( MistyValueToOriginal ( clearBuffer ) );
		}

		public void SwapBuffer ()
		{
			window.SwapBuffers ();
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount )
		{
			BeginVertexDeclaration ( vertexBuffer, vertexDeclaration );
			GL.DrawArrays ( MistyValueToOriginal ( primitiveType ), startVertex, primitiveCount * GetCountFromPrimitiveType ( primitiveType ) );
			EndVertexDeclaration ( vertexDeclaration );
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, IIndexBuffer indexBuffer, int startIndex, int primitiveCount )
		{
			BeginVertexDeclaration ( vertexBuffer, vertexDeclaration );
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, ( int ) indexBuffer.Handle );
			GL.DrawElements ( MistyValueToOriginal ( primitiveType ), primitiveCount * GetCountFromPrimitiveType ( primitiveType ),
				indexBuffer.Is16bitIndex ? DrawElementsType.UnsignedShort : DrawElementsType.UnsignedInt, startIndex );
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, 0 );
			EndVertexDeclaration ( vertexDeclaration );
		}

		public void ResizeBackBuffer ( int width, int height )
		{
			window.ClientSize = new System.Drawing.Size ( width, height );
		}

		public IRenderBuffer CreateRenderBuffer ( int width, int height ) { return new RenderBuffer ( this, width, height ); }

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

#pragma warning disable
		public event EventHandler DeviceLost;
	}
}
