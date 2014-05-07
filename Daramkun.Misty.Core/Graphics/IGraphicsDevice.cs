using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	public enum BaseRenderer : byte { Unknown, DirectX, MonoGame, OpenGL, OpenGLES, }

	public enum CullMode : byte { None, ClockWise, CounterClockWise, }
	public enum FillMode : byte { Wireframe, Solid, }
	public enum PrimitiveType : byte { PointList, LineList, LineStrip, TriangleList, TriangleStrip, TriangleFan, }
	[Flags] public enum ClearBuffer : byte { ColorBuffer = 1 << 0, DepthBuffer = 1 << 1, StencilBuffer = 1 << 2, AllBuffer = ColorBuffer | DepthBuffer | StencilBuffer, }

	public struct ScreenResolution
	{
		public Vector2 ScreenSize;
		public float RefreshRate;
		public override string ToString () { return string.Format ( "Screen Size: {0}, Refresh Rate: {1}", ScreenSize, RefreshRate ); }
	}

	public struct Viewport
	{
		public int X, Y, Width, Height;
		public Viewport ( int x, int y, int width, int height ) : this () { X = x; Y = y; Width = width; Height = height; }
		public Viewport ( int [] viewport ) : this () { X = viewport [ 0 ]; Y = viewport [ 1 ]; Width = viewport [ 2 ]; Height = viewport [ 3 ]; }
		public override string ToString () { return string.Format ( "{{X:{0}, Y:{1}, Width:{2}, Height:{3}}}", X, Y, Width, Height ); }
	}

	public interface IGraphicsDeviceInformation
	{
		BaseRenderer BaseRenderer { get; }
		Version RendererVersion { get; }
		Version ShaderVersion { get; }
		string DeviceVendor { get; }

		ScreenResolution [] AvailableScreenResolutions { get; }
		int MaximumAnisotropicLevel { get; }

		bool IsSupportTexture1D { get; }
		bool IsSupportTexture3D { get; }
		bool IsSupportGeometryShader { get; }
		bool IsSupportMultiContext { get; }
	}

	public interface IGraphicsDevice : IDisposable
	{
		object Handle { get; }
		IGraphicsDeviceInformation Information { get; }

		IRenderBuffer BackBuffer { get; }

		bool IsMultisampleEnabled { get; set; }
		bool IsFullscreen { get; set; }
		ScreenResolution FullscreenResolution { get; set; }
		void ResizeBackBuffer ( int width, int height );

		void SwapBuffer ();

		bool VerticalSyncMode { get; set; }

		IGraphicsContext ImmediateContext { get; }

		IGraphicsContext CreateDeferredContext ();
		IRenderBuffer CreateRenderBuffer ( int width, int height );
		ITexture1D CreateTexture1D ( int width, int mipmapLevel = 1 );
		ITexture1D CreateTexture1D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 );
		ITexture2D CreateTexture2D ( int width, int height, int mipmapLevel = 1 );
		ITexture2D CreateTexture2D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 );
		ITexture3D CreateTexture3D ( int width, int height, int depth, int mipmapLevel = 1 );
		ITexture3D CreateTexture3D ( ImageInfo [] imageInfo, Color? colorKey = null, int mipmapLevel = 1 );
		IVertexDeclaration CreateVertexDeclaration ( params VertexElement [] elements );
		IBuffer CreateBuffer ( BufferType bufferType, Type vertexType, int length );
		IBuffer CreateBuffer<T> ( BufferType bufferType, T [] vertices ) where T : struct;
		IShader CreateShader ( ShaderType shaderType, string shader );
		IEffect CreateEffect ( IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName );
		IEffect CreateEffect ( Stream stream );

		event EventHandler DeviceLost;
		event EventHandler BackbufferResized;
	}
}
