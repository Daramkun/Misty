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

		public CullingMode CullMode
		{
			get
			{
				bool cullFace;
				GL.GetBoolean ( GetPName.CullFace, out cullFace );
				if ( !cullFace ) return CullingMode.None;
				int frontFace;
				GL.GetInteger ( GetPName.FrontFace, out frontFace );
				switch ( ( FrontFaceDirection ) frontFace )
				{
					case FrontFaceDirection.Ccw: return CullingMode.ClockWise;
					case FrontFaceDirection.Cw: return CullingMode.CounterClockWise;
					default: throw new ArgumentException ();
				}
			}
			set
			{
				if ( value == CullingMode.None ) GL.Disable ( EnableCap.CullFace );
				else
				{
					GL.Enable ( EnableCap.CullFace );
					GL.FrontFace ( ( value == CullingMode.ClockWise ) ? FrontFaceDirection.Ccw : FrontFaceDirection.Cw );
				}
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
				if ( GL.IsEnabled ( EnableCap.Blend ) )
				{
					int op, dest, src;
					GL.GetInteger ( GetPName.Blend, out op ); GL.GetInteger ( GetPName.BlendDst, out dest ); GL.GetInteger ( GetPName.BlendSrc, out src );
					return new Graphics.BlendOperation (
						OriginalToMistyValue ( ( BlendEquationMode ) op ),
						OriginalToMistyValue ( ( BlendingFactorSrc ) src ),
						OriginalToMistyValue ( ( BlendingFactorDest ) dest )
					);
				}
				else { return null; }
			}
			set
			{
				if ( value != null ) GL.Enable ( EnableCap.Blend ); else GL.Disable ( EnableCap.Blend );
				if ( value == null ) return;
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
				if ( GL.IsEnabled ( EnableCap.StencilTest ) )
				{
					int func, mask, fail, zfail, pass, reference;
					GL.GetInteger ( GetPName.StencilFunc, out func ); GL.GetInteger ( GetPName.StencilValueMask, out mask );
					GL.GetInteger ( GetPName.StencilFail, out fail ); GL.GetInteger ( GetPName.StencilPassDepthFail, out zfail );
					GL.GetInteger ( GetPName.StencilPassDepthPass, out pass ); GL.GetInteger ( GetPName.StencilRef, out reference );
					return new StencilOperation ( OriginalToMistyValue ( ( OpenTK.Graphics.OpenGL.StencilFunction ) func ), reference, mask,
						OriginalToMistyValue ( ( OpenTK.Graphics.OpenGL.StencilOp ) zfail ), OriginalToMistyValue ( ( OpenTK.Graphics.OpenGL.StencilOp ) fail ),
						OriginalToMistyValue ( ( OpenTK.Graphics.OpenGL.StencilOp ) pass ) );
				}
				else { return null; }
			}
			set
			{
				if ( value != null ) GL.Enable ( EnableCap.StencilTest ); else GL.Disable ( EnableCap.StencilTest );
				if ( value == null ) return;
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

			CullMode = CullingMode.ClockWise;

			if ( deviceInfo.RendererVersion.Major < 2 )
				throw new PlatformNotSupportedException ();

			OpenTK.Graphics.GraphicsContext.ShareContexts = true;
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				window = null;
			}
			base.Dispose ( isDisposing );
		}

		public void BeginScene ( IRenderBuffer renderBuffer = null )
		{
			if ( renderBuffer != null && renderBuffer != BackBuffer )
			{
				GL.BindTexture ( TextureTarget.Texture2D, 0 );
				GL.Enable ( EnableCap.Texture2D );
				GL.BindFramebuffer ( FramebufferTarget.Framebuffer, ( renderBuffer as RenderBuffer ).frameBuffer );
				GL.Viewport ( 0, 0, renderBuffer.Width, renderBuffer.Height );
				CurrentRenderBuffer = renderBuffer;
			}
			else
			{
				CurrentRenderBuffer = BackBuffer;
				GL.BindFramebuffer ( FramebufferTarget.Framebuffer, 0 );
				GL.Viewport ( 0, 0, BackBuffer.Width, BackBuffer.Height );
			}
		}

		public void EndScene ()
		{
			GL.BindFramebuffer ( FramebufferTarget.Framebuffer, 0 );
			CurrentRenderBuffer = BackBuffer;
		}

		public void Clear ( ClearBuffer clearBuffer, Color color, float depth = 1, int stencil = 0 )
		{
			GL.ClearColor ( color.RedScalar, color.GreenScalar, color.BlueScalar, color.AlphaScalar );
			GL.ClearDepth ( depth );
			GL.ClearStencil ( stencil );
			GL.Clear ( MistyValueToOriginal ( clearBuffer ) );
		}

		public void SwapBuffer () { window.SwapBuffers (); }

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount )
		{
			BeginVertexDeclaration ( vertexBuffer, vertexDeclaration );
			GL.DrawArrays ( MistyValueToOriginal ( primitiveType ), startVertex, primitiveCount * GetCountFromPrimitiveType ( primitiveType, primitiveCount ) );
			EndVertexDeclaration ( vertexDeclaration );
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, IIndexBuffer indexBuffer, int startIndex, int primitiveCount )
		{
			BeginVertexDeclaration ( vertexBuffer, vertexDeclaration );
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, ( int ) indexBuffer.Handle );
			GL.DrawElements ( MistyValueToOriginal ( primitiveType ), GetCountFromPrimitiveType ( primitiveType, primitiveCount ),
				( !indexBuffer.Is16bitIndex ? DrawElementsType.UnsignedInt : DrawElementsType.UnsignedShort ), startIndex );
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, 0 );
			EndVertexDeclaration ( vertexDeclaration );
		}

		public void DrawUP<T> ( PrimitiveType primitiveType, T [] vertices, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount ) where T : struct
		{
			BeginVertexDeclaration ( vertices, vertexDeclaration );
			GL.DrawArrays ( MistyValueToOriginal ( primitiveType ), startVertex, primitiveCount * GetCountFromPrimitiveType ( primitiveType, primitiveCount ) );
			EndVertexDeclaration ( vertexDeclaration );
		}

		public void DrawUP<T1, T2> ( PrimitiveType primitiveType, T1 [] vertices, IVertexDeclaration vertexDeclaration, T2 [] indices, int startIndex, int primitiveCount )
			where T1 : struct
			where T2 : struct
		{
			BeginVertexDeclaration ( vertices, vertexDeclaration );
			GL.IndexPointer<T2> ( IndexPointerType.Int, Marshal.SizeOf ( typeof ( T2 ) ), indices );
			GL.DrawElements ( MistyValueToOriginal ( primitiveType ), GetCountFromPrimitiveType ( primitiveType, primitiveCount ),
				DrawElementsType.UnsignedInt, startIndex );
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, 0 );
			EndVertexDeclaration ( vertexDeclaration );
		}

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
