using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	partial class GraphicsContext : StandardDispose, IGraphicsContext
	{
		WeakReference d3dDevice;

		public Thread Owner { get; private set; }
		public IGraphicsDevice GraphicsDevice { get; private set; }
		public IRenderBuffer CurrentRenderBuffer { get; private set; }

		public CullMode CullMode
		{
			get { return ConvertCullMode ( ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.CullMode ) ); }
			set { ( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.CullMode, ChangeCullMode ( value ) ); }
		}

		public FillMode FillMode
		{
			get { return ConvertFillMode ( ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.FillMode ) ); }
			set { ( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.FillMode, ChangeFillMode ( value ) ); }
		}

		public BlendState BlendState
		{
			get
			{
				if ( ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.AlphaBlendEnable ) != 0 )
				{
					int op = ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.BlendOperation );
					int sb = ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.SourceBlend );
					int db = ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.DestinationBlend );
					return new BlendState ( DeconvertBlendOp ( op ), DeconvertBlendParam ( sb ), DeconvertBlendParam ( db ) );
				}
				else { return null; }
			}
			set
			{
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.AlphaBlendEnable, value != null );
				if ( value == null ) return;
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.BlendOperation, ConvertBlendOp ( value.Operator ) );
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.SourceBlend, ConvertBlendParam ( value.SourceParameter ) );
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.DestinationBlend, ConvertBlendParam ( value.DestinationParameter ) );
			}
		}

		public DepthStencil DepthStencil
		{
			get
			{
				return new DepthStencil ()
				{
					StencilState = ( ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.StencilEnable ) != 0 ) ? new StencilState (
						DeconvertStencilFunc ( ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.StencilFunc ) ),
						( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.StencilRef ),
						( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.StencilMask ),
						DeconvertStencilOp ( ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.StencilZFail ) ),
						DeconvertStencilOp ( ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.StencilFail ) ),
						DeconvertStencilOp ( ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.StencilPass ) )
						) : null,
					DepthEnable = ( d3dDevice.Target as SharpDX.Direct3D9.Device ).GetRenderState ( SharpDX.Direct3D9.RenderState.ZWriteEnable ) != 0 ? true : false
				};
			}
			set
			{
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.ZWriteEnable, value.DepthEnable );
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.StencilEnable, value.StencilState != null );
				if ( value.StencilState == null ) return;
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.StencilFunc, ConvertStencilFunc ( value.StencilState.Function ) );
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.StencilRef, value.StencilState.Reference );
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.StencilMask, value.StencilState.Mask );
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.StencilZFail, ConvertStencilOp ( value.StencilState.ZFail ) );
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.StencilFail, ConvertStencilOp ( value.StencilState.Fail ) );
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetRenderState ( SharpDX.Direct3D9.RenderState.StencilPass, ConvertStencilOp ( value.StencilState.Pass ) );
			}
		}

		public Viewport Viewport
		{
			get
			{
				SharpDX.Viewport viewPort = ( d3dDevice.Target as SharpDX.Direct3D9.Device ).Viewport;
				return new Viewport () { X = viewPort.X, Y = viewPort.Y, Width = viewPort.Width, Height = viewPort.Height };
			}
			set { ( d3dDevice.Target as SharpDX.Direct3D9.Device ).Viewport = new SharpDX.Viewport ( value.X, value.Y, value.Width, value.Height ); }
		}

		public GraphicsContext ( IGraphicsDevice graphicsDevice )
		{
			GraphicsDevice = graphicsDevice;
			d3dDevice = new WeakReference ( GraphicsDevice.Handle );
			CullMode = CullMode.ClockWise;
		}

		public void BeginScene ( IRenderBuffer renderBuffer = null )
		{
			Owner = Thread.CurrentThread;
			if ( d3dDevice == null ) return;
			CurrentRenderBuffer = renderBuffer;
			if ( renderBuffer == null || renderBuffer == GraphicsDevice.BackBuffer )
			{
				CurrentRenderBuffer = GraphicsDevice.BackBuffer;
				( d3dDevice.Target as SharpDX.Direct3D9.Device ).BeginScene ();
			}
			else
			{
				SharpDX.Direct3D9.Surface surface = ( renderBuffer.Handle as SharpDX.Direct3D9.Texture ).GetSurfaceLevel ( 0 );
				( renderBuffer as RenderBuffer ).rts.BeginScene ( surface, new SharpDX.Viewport ( 0, 0, renderBuffer.Width, renderBuffer.Height ) );
			}
		}

		public void EndScene ()
		{
			Owner = null;
			if ( d3dDevice == null ) return;
			if ( CurrentRenderBuffer != GraphicsDevice.BackBuffer )
				( CurrentRenderBuffer as RenderBuffer ).rts.EndScene ( SharpDX.Direct3D9.Filter.Default );
			else ( d3dDevice.Target as SharpDX.Direct3D9.Device ).EndScene ();
			CurrentRenderBuffer = GraphicsDevice.BackBuffer;
		}

		public void Clear ( ClearBuffer clearBuffer, Color color, float depth = 1, int stencil = 0 )
		{
			if ( d3dDevice == null ) return;
			( d3dDevice.Target as SharpDX.Direct3D9.Device ).Clear ( ChangeClearBuffer ( clearBuffer ), ChangeColor ( color ), depth, stencil );
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, int startVertex, int primitiveCount )
		{
			( d3dDevice.Target as SharpDX.Direct3D9.Device ).VertexDeclaration = vertexDeclaration.Handle as SharpDX.Direct3D9.VertexDeclaration;
			( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetStreamSource ( 0, vertexBuffer.Handle as SharpDX.Direct3D9.VertexBuffer, 0, vertexBuffer.VertexTypeSize );
			( d3dDevice.Target as SharpDX.Direct3D9.Device ).DrawPrimitives ( ConvertPrimitiveType ( primitiveType ), startVertex, primitiveCount );
		}

		public void Draw ( PrimitiveType primitiveType, IVertexBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, IIndexBuffer indexBuffer,
			int startIndex, int primitiveCount )
		{
			( d3dDevice.Target as SharpDX.Direct3D9.Device ).VertexDeclaration = vertexDeclaration.Handle as SharpDX.Direct3D9.VertexDeclaration;
			( d3dDevice.Target as SharpDX.Direct3D9.Device ).SetStreamSource ( 0, vertexBuffer.Handle as SharpDX.Direct3D9.VertexBuffer, 0, vertexBuffer.VertexTypeSize );
			( d3dDevice.Target as SharpDX.Direct3D9.Device ).Indices = indexBuffer.Handle as SharpDX.Direct3D9.IndexBuffer;
			( d3dDevice.Target as SharpDX.Direct3D9.Device ).DrawIndexedPrimitive ( ConvertPrimitiveType ( primitiveType ), 0, 0, vertexBuffer.Length,
				startIndex, primitiveCount );
		}
	}
}
