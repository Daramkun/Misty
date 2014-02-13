using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	public enum BaseRenderer
	{
		Unknown,

		DirectX,
		XNA,
		MonoGame = XNA,
		OpenGL,
		OpenGLES,
	}

	public interface IGraphicsDeviceInformation
	{
		BaseRenderer BaseRenderer { get; }
		Version RendererVersion { get; }
		Version ShaderVersion { get; }
		string DeviceVendor { get; }

		ScreenResolution [] AvailableScreenResolution { get; }
		int MaximumAnisotropicLevel { get; }

		bool IsSupportTexture1D { get; }
		bool IsSupportTexture3D { get; }
		bool IsSupportGeometryShader { get; }
	}

	public enum CullingMode
	{
		None,
		ClockWise,
		CounterClockWise,
	}

	public enum FillMode
	{
		Point,
		Wireframe,
		Solid,
	}

	public enum PrimitiveType
	{
		PointList,
		LineList,
		LineStrip,
		TriangleList,
		TriangleStrip,
		TriangleFan,
	}

	[Flags]
	public enum ClearBuffer
	{
		ColorBuffer = 1 << 0,
		DepthBuffer = 1 << 1,
		StencilBuffer = 1 << 2,
		AllBuffer = ColorBuffer | DepthBuffer | StencilBuffer,
	}

	public interface IGraphicsDevice : IDisposable
	{
		object Handle { get; }
		IGraphicsDeviceInformation Information { get; }

		IRenderBuffer BackBuffer { get; }
		IRenderBuffer CurrentRenderBuffer { get; }

		CullingMode CullMode { get; set; }
		FillMode FillMode { get; set; }

		bool IsZWriteEnable { get; set; }
		bool BlendState { get; set; }
		bool StencilState { get; set; }
		bool IsMultisampleRendering { get; set; }

		bool IsFullscreen { get; set; }
		ScreenResolution FullscreenResolution { get; set; }

		BlendOperation BlendOperation { get; set; }
		StencilOperation StencilOperation { get; set; }

		Viewport Viewport { get; set; }
		bool VerticalSyncMode { get; set; }

		void BeginScene ( IRenderBuffer renderBuffer = null );
		void EndScene ();

		void Clear ( ClearBuffer clearBuffer, Color color, float depth = 1, int stencil = 0 );
		void SwapBuffer ();

		void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount );
		void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, IIndexBuffer indexBuffer, int startIndex, int primitiveCount );
		void DrawUP<T> ( PrimitiveType primitiveType, T [] vertices, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount ) where T : struct;
		void DrawUP<T1, T2> ( PrimitiveType primitiveType, T1 [] vertices, IVertexDeclaration vertexDeclaration, T2 [] indices, int startIndex, int primitiveCount )
			where T1 : struct
			where T2 : struct;

		void ResizeBackBuffer ( int width, int height );

		IRenderBuffer CreateRenderBuffer ( int width, int height );
		ITexture1D CreateTexture1D ( int width, int mipmapLevel = 1 );
		ITexture1D CreateTexture1D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 );
		ITexture2D CreateTexture2D ( int width, int height, int mipmapLevel = 1 );
		ITexture2D CreateTexture2D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 );
		ITexture3D CreateTexture3D ( int width, int height, int depth, int mipmapLevel = 1 );
		ITexture3D CreateTexture3D ( ImageInfo [] imageInfo, Color? colorKey = null, int mipmapLevel = 1 );
		IVertexDeclaration CreateVertexDeclaration ( params VertexElement [] elements );
		IVertexBuffer CreateVertexBuffer ( Type vertexType, int length );
		IVertexBuffer CreateVertexBuffer<T> ( T [] vertices ) where T : struct;
		IIndexBuffer CreateIndexBuffer ( Type indexType, int length, bool is16bit = false );
		IIndexBuffer CreateIndexBuffer<T> ( T [] indices, bool is16bit = false ) where T : struct;
		IShader CreateShader ( ShaderType shaderType, string shader );
		IEffect CreateEffect ( IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName );
		IEffect CreateEffect ( Stream stream );

		event EventHandler DeviceLost;
		event EventHandler BackbufferResized;
	}
}
