using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Daramkun.Misty.Common;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	partial class GraphicsContext : StandardDispose, IGraphicsContext
	{
		InputAssembler inputAssembler;
		OpenTK.Graphics.OpenGL.PrimitiveType currentPrimitiveType;
		OpenTK.Graphics.OpenGL.DrawElementsType elementsType;

		public Thread Owner { get; private set; }
		public IGraphicsDevice GraphicsDevice { get; private set; }
		public IRenderBuffer CurrentRenderBuffer { get; private set; }

		public CullMode CullMode
		{
			get
			{
				bool cullFace;
				GL.GetBoolean ( GetPName.CullFace, out cullFace );
				if ( !cullFace ) return CullMode.None;
				int frontFace;
				GL.GetInteger ( GetPName.FrontFace, out frontFace );
				switch ( ( FrontFaceDirection ) frontFace )
				{
					case FrontFaceDirection.Ccw: return CullMode.ClockWise;
					case FrontFaceDirection.Cw: return CullMode.CounterClockWise;
					default: throw new ArgumentException ();
				}
			}
			set
			{
				if ( value == CullMode.None ) GL.Disable ( EnableCap.CullFace );
				else
				{
					GL.Enable ( EnableCap.CullFace );
					GL.FrontFace ( ( value == CullMode.ClockWise ) ? FrontFaceDirection.Ccw : FrontFaceDirection.Cw );
				}
			}
		}

		public FillMode FillMode
		{
			get { int fillMode; GL.GetInteger ( GetPName.PolygonMode, out fillMode ); return OriginalToMistyValue ( ( PolygonMode ) fillMode ); }
			set { GL.PolygonMode ( MaterialFace.FrontAndBack, MistyValueToOriginal ( value ) ); }
		}

		public BlendState BlendState
		{
			get
			{
				if ( GL.IsEnabled ( EnableCap.Blend ) )
				{
					int op, dest, src;
					GL.GetInteger ( GetPName.Blend, out op ); GL.GetInteger ( GetPName.BlendDst, out dest ); GL.GetInteger ( GetPName.BlendSrc, out src );
					return new Graphics.BlendState (
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

		public DepthStencil DepthStencil
		{
			get
			{
				DepthStencil ds = new DepthStencil ();
				ds.DepthEnable = GL.IsEnabled ( EnableCap.DepthTest );
				if ( GL.IsEnabled ( EnableCap.StencilTest ) )
				{
					int func, mask, fail, zfail, pass, reference;
					GL.GetInteger ( GetPName.StencilFunc, out func ); GL.GetInteger ( GetPName.StencilValueMask, out mask );
					GL.GetInteger ( GetPName.StencilFail, out fail ); GL.GetInteger ( GetPName.StencilPassDepthFail, out zfail );
					GL.GetInteger ( GetPName.StencilPassDepthPass, out pass ); GL.GetInteger ( GetPName.StencilRef, out reference );
					ds.StencilState = new StencilState ( OriginalToMistyValue ( ( OpenTK.Graphics.OpenGL.StencilFunction ) func ), reference, mask,
						OriginalToMistyValue ( ( OpenTK.Graphics.OpenGL.StencilOp ) zfail ), OriginalToMistyValue ( ( OpenTK.Graphics.OpenGL.StencilOp ) fail ),
						OriginalToMistyValue ( ( OpenTK.Graphics.OpenGL.StencilOp ) pass ) );
				}
				else { ds.StencilState = null; }
				return ds;
			}
			set
			{
				if ( value.DepthEnable ) GL.Enable ( EnableCap.DepthTest ); else GL.Disable ( EnableCap.DepthTest );
				if ( value.StencilState != null ) GL.Enable ( EnableCap.StencilTest ); else GL.Disable ( EnableCap.StencilTest );
				if ( value.StencilState != null )
				{
					GL.StencilFunc ( MistyValueToOriginal ( value.StencilState.Function ), value.StencilState.Reference, value.StencilState.Mask );
					GL.StencilOp ( MistyValueToOriginal ( value.StencilState.Fail ), MistyValueToOriginal ( value.StencilState.ZFail ),
						MistyValueToOriginal ( value.StencilState.Pass ) );
				}
			}
		}

		public InputAssembler InputAssembler
		{
			get { return inputAssembler; }
			set
			{
				if ( inputAssembler.VertexDeclaration != null )
					EndVertexDeclaration ( inputAssembler.VertexDeclaration );
				inputAssembler = value;
				currentPrimitiveType = MistyValueToOriginal ( inputAssembler.PrimitiveType );
				BeginVertexDeclaration ( inputAssembler.VertexBuffer, inputAssembler.VertexDeclaration );
				if ( inputAssembler.IndexBuffer != null )
				{
					GL.BindBuffer ( BufferTarget.ElementArrayBuffer, ( int ) inputAssembler.IndexBuffer.Handle );
					elementsType = ( inputAssembler.IndexBuffer.BufferType.HasFlag ( BufferType.Index16 ) ? DrawElementsType.UnsignedShort : DrawElementsType.UnsignedInt );
				}
				else
					GL.BindBuffer ( BufferTarget.ElementArrayBuffer, 0 );
			}
		}

		public Viewport Viewport
		{
			get { int [] viewport = new int [ 4 ]; GL.GetInteger ( GetPName.Viewport, viewport ); return new Viewport ( viewport ); }
			set { GL.Viewport ( value.X, value.Y, value.Width, value.Height ); }
		}

		public GraphicsContext ( IGraphicsDevice graphicsDevice )
		{
			GraphicsDevice = graphicsDevice;
			CullMode = CullMode.ClockWise;
		}

		public void BeginScene ( IRenderBuffer renderBuffer = null )
		{
			Owner = Thread.CurrentThread;
			if ( renderBuffer != null && renderBuffer != GraphicsDevice.BackBuffer )
			{
				GL.BindTexture ( TextureTarget.Texture2D, 0 );
				GL.Enable ( EnableCap.Texture2D );
				GL.BindFramebuffer ( FramebufferTarget.Framebuffer, ( renderBuffer as RenderBuffer ).frameBuffer );
				GL.Viewport ( 0, 0, renderBuffer.Width, renderBuffer.Height );
				CurrentRenderBuffer = renderBuffer;
			}
			else
			{
				CurrentRenderBuffer = GraphicsDevice.BackBuffer;
				GL.BindFramebuffer ( FramebufferTarget.Framebuffer, 0 );
				GL.Viewport ( 0, 0, GraphicsDevice.BackBuffer.Width, GraphicsDevice.BackBuffer.Height );
			}
		}

		public void EndScene ()
		{
			Owner = null;
			GL.BindFramebuffer ( FramebufferTarget.Framebuffer, 0 );
			CurrentRenderBuffer = GraphicsDevice.BackBuffer;
		}

		public void Clear ( ClearBuffer clearBuffer, Color color, float depth = 1, int stencil = 0 )
		{
			GL.ClearColor ( color.RedScalar, color.GreenScalar, color.BlueScalar, color.AlphaScalar );
			GL.ClearDepth ( depth );
			GL.ClearStencil ( stencil );
			GL.Clear ( MistyValueToOriginal ( clearBuffer ) );
		}

		public void Draw ( int startIndex, int primitiveCount )
		{
			if ( InputAssembler.IndexBuffer == null )
				GL.DrawArrays ( currentPrimitiveType, startIndex, primitiveCount * GetCountFromPrimitiveType ( inputAssembler.PrimitiveType, primitiveCount ) );
			else
				GL.DrawElements ( currentPrimitiveType, GetCountFromPrimitiveType ( inputAssembler.PrimitiveType, primitiveCount ),
					elementsType, startIndex );
		}
	}
}
