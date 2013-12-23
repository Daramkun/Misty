using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Daramkun.Misty.Common;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Graphics
{
	public partial class GraphicsDevice : StandardDispose, IGraphicsDevice
	{
		Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice;

		public object Handle { get { return graphicsDevice; } }

		public IGraphicsDeviceInformation Information
		{
			get { throw new NotImplementedException (); }
		}

		public IRenderBuffer BackBuffer
		{
			get { throw new NotImplementedException (); }
		}

		public CullingMode CullMode
		{
			get { return ConvertNativeValue ( graphicsDevice.RasterizerState.CullMode ); }
			set { graphicsDevice.RasterizerState.CullMode = ConvertMistyValue ( value ); }
		}

		public FillMode FillMode
		{
			get { return ConvertNativeValue ( graphicsDevice.RasterizerState.FillMode ); }
			set { graphicsDevice.RasterizerState.FillMode = ConvertMistyValue ( value ); }
		}

		public bool IsZWriteEnable
		{
			get { return graphicsDevice.DepthStencilState.DepthBufferWriteEnable; }
			set { graphicsDevice.DepthStencilState.DepthBufferWriteEnable = value; }
		}

		public bool BlendState
		{
			get { return graphicsDevice.BlendState != null; }
			set { graphicsDevice.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend; }
		}

		public bool StencilState
		{
			get { return graphicsDevice.DepthStencilState.StencilEnable; }
			set { graphicsDevice.DepthStencilState.StencilEnable = value; }
		}

		public bool IsMultisampleRendering { get { return false; } set { } }
		public bool IsFullscreen { get { return false; } set { } }
		public ScreenResolution FullscreenResolution { get { return new ScreenResolution (); } set { } }

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
				Microsoft.Xna.Framework.Graphics.Viewport viewport = graphicsDevice.Viewport;
				return new Viewport ( viewport.X, viewport.Y, viewport.Width, viewport.Height );
			}
			set { graphicsDevice.Viewport = new Microsoft.Xna.Framework.Graphics.Viewport ( value.X, value.Y, value.Width, value.Height ); }
		}

		public bool VerticalSyncMode { get { return false; } set { } }

		public GraphicsDevice ( IWindow window )
		{
			graphicsDevice = System.Windows.Graphics.GraphicsDeviceManager.Current.GraphicsDevice;
		}

		public void BeginScene ( IRenderBuffer renderBuffer = null )
		{
			
			throw new NotImplementedException ();
		}

		public void EndScene ()
		{
			throw new NotImplementedException ();
		}

		public void Clear ( ClearBuffer clearBuffer, Misty.Graphics.Color color, float depth = 1, int stencil = 0 )
		{
			throw new NotImplementedException ();
		}

		public void SwapBuffer () { }

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount )
		{
			throw new NotImplementedException ();
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, IIndexBuffer indexBuffer, int startIndex, int primitiveCount )
		{
			throw new NotImplementedException ();
		}

		public void ResizeBackBuffer ( int width, int height ) { }

		public IRenderBuffer CreateRenderBuffer ( int width, int height )
		{
			throw new NotImplementedException ();
		}

		public ITexture2D CreateTexture2D ( int width, int height, int mipmapLevel = 1 )
		{
			throw new NotImplementedException ();
		}

		public ITexture2D CreateTexture2D ( Contents.ImageInfo imageInfo, Misty.Graphics.Color? colorKey = null, int mipmapLevel = 1 )
		{
			throw new NotImplementedException ();
		}

		public IVertexDeclaration CreateVertexDeclaration ( params VertexElement [] elements )
		{
			throw new NotImplementedException ();
		}

		public IVertexBuffer CreateVertexBuffer ( Type vertexType, int length )
		{
			throw new NotImplementedException ();
		}

		public IVertexBuffer CreateVertexBuffer<T> ( T [] vertices ) where T : struct
		{
			throw new NotImplementedException ();
		}

		public IIndexBuffer CreateIndexBuffer ( Type indexType, int length, bool is16bit = false )
		{
			throw new NotImplementedException ();
		}

		public IIndexBuffer CreateIndexBuffer<T> ( T [] indices, bool is16bit = false ) where T : struct
		{
			throw new NotImplementedException ();
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
	}
}
