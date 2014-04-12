using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	public partial class GraphicsDevice : StandardDispose, IGraphicsDevice
	{
		private void BeginVertexDeclaration ( IVertexBuffer buffer, IVertexDeclaration decl )
		{
			GL.BindBuffer ( BufferTarget.ArrayBuffer, ( int ) buffer.Handle );

			int i = 0, offset = 0;
			foreach ( VertexElement element in decl )
			{
				int size = ElementSizeToRealSize ( element.Size );
				GL.EnableVertexAttribArray ( i );
				GL.VertexAttribPointer ( i, size / 4, ElementSizeToRealType ( element.Size ),
					false, buffer.VertexTypeSize, offset );
				++i;
				offset += size;
			}
		}

		private void BeginVertexDeclaration<T> ( T [] vertices, IVertexDeclaration decl ) where T : struct
		{
			int stride = Marshal.SizeOf ( typeof ( T ) );
			GL.VertexPointer<T> ( vertices.Length, VertexPointerType.Float, stride, vertices );

			int i = 0;
			foreach ( VertexElement element in decl )
			{
				int size = ElementSizeToRealSize ( element.Size );
				GL.EnableVertexAttribArray ( i );
				GL.VertexAttribPointer ( i, size / 4, ElementSizeToRealType ( element.Size ),
					false, stride, vertices );
				++i;
			}
		}

		private void EndVertexDeclaration ( IVertexDeclaration decl )
		{
			for ( int i = 0; i < decl.Length; ++i )
				GL.DisableVertexAttribArray ( i );

			GL.BindBuffer ( BufferTarget.ArrayBuffer, 0 );
		}

		private int GetCountFromPrimitiveType ( PrimitiveType primitiveType, int primitiveCount )
		{
			switch ( primitiveType )
			{
				case PrimitiveType.PointList: return primitiveCount;
				case PrimitiveType.LineList: return primitiveCount * 2;
				case PrimitiveType.LineStrip: return primitiveCount + 1;
				case PrimitiveType.TriangleList: return primitiveCount * 3;
				case PrimitiveType.TriangleStrip: return primitiveCount + 2;
				case PrimitiveType.TriangleFan: return primitiveCount + 2;
				default: return 0;
			}
		}

		private VertexAttribPointerType ElementSizeToRealType ( ElementSize elementSize )
		{
			switch ( elementSize )
			{
				/*case ElementSize.Byte1:
				case ElementSize.Byte2:
				case ElementSize.Byte3:
				case ElementSize.Byte4: return VertexAttribPointerType.Byte;

				case ElementSize.Short1:
				case ElementSize.Short2:
				case ElementSize.Short3:
				case ElementSize.Short4: return VertexAttribPointerType.Short;

				case ElementSize.Integer1:
				case ElementSize.Integer2:
				case ElementSize.Integer3:
				case ElementSize.Integer4: return VertexAttribPointerType.Int;*/

				case ElementSize.Float1:
				case ElementSize.Float2:
				case ElementSize.Float3:
				case ElementSize.Float4: return VertexAttribPointerType.Float;

				default: throw new ArgumentException ();
			}
		}

		private int ElementSizeToRealSize ( ElementSize elementSize )
		{
			switch ( elementSize )
			{
				/*case ElementSize.Byte1: return 1;

				case ElementSize.Byte2:
				case ElementSize.Short1: return 2;

				case ElementSize.Byte3: return 3;

				case ElementSize.Byte4:
				case ElementSize.Short2:
				case ElementSize.Integer1:*/
				case ElementSize.Float1: return 4;

				//case ElementSize.Short3: return 6;

				/*case ElementSize.Short4:
				case ElementSize.Integer2:*/
				case ElementSize.Float2: return 8;

				//case ElementSize.Integer3:
				case ElementSize.Float3: return 12;

				//case ElementSize.Integer4:
				case ElementSize.Float4: return 16;

				default: throw new ArgumentException ();
			}
		}

		private FillMode OriginalToMistyValue ( OpenTK.Graphics.OpenGL.PolygonMode mode )
		{
			switch ( mode )
			{
				case OpenTK.Graphics.OpenGL.PolygonMode.Line: return FillMode.Wireframe;
				case OpenTK.Graphics.OpenGL.PolygonMode.Fill: return FillMode.Solid;
				default: throw new ArgumentException ();
			}
		}

		private OpenTK.Graphics.OpenGL.PolygonMode MistyValueToOriginal ( FillMode mode )
		{
			switch ( mode )
			{
				case Graphics.FillMode.Wireframe: return OpenTK.Graphics.OpenGL.PolygonMode.Line;
				case Graphics.FillMode.Solid: return OpenTK.Graphics.OpenGL.PolygonMode.Fill;
				default: throw new ArgumentException ();
			}
		}

		private BlendParameter OriginalToMistyValue ( OpenTK.Graphics.OpenGL.BlendingFactorSrc factor )
		{
			switch ( factor )
			{
				case OpenTK.Graphics.OpenGL.BlendingFactorSrc.Zero: return BlendParameter.Zero;
				case OpenTK.Graphics.OpenGL.BlendingFactorSrc.One: return BlendParameter.One;
				case OpenTK.Graphics.OpenGL.BlendingFactorSrc.SrcAlpha: return BlendParameter.SourceAlpha;
				case OpenTK.Graphics.OpenGL.BlendingFactorSrc.OneMinusSrcAlpha: return BlendParameter.InvertSourceAlpha;
				case OpenTK.Graphics.OpenGL.BlendingFactorSrc.DstColor: return BlendParameter.DestinationColor;
				case OpenTK.Graphics.OpenGL.BlendingFactorSrc.DstAlpha: return BlendParameter.DestinationAlpha;
				case OpenTK.Graphics.OpenGL.BlendingFactorSrc.OneMinusDstColor: return BlendParameter.InvertDestinationColor;
				case OpenTK.Graphics.OpenGL.BlendingFactorSrc.OneMinusDstAlpha: return BlendParameter.InvertDestinationAlpha;
				default: throw new ArgumentException ();
			}
		}

		private BlendParameter OriginalToMistyValue ( OpenTK.Graphics.OpenGL.BlendingFactorDest factor )
		{
			switch ( factor )
			{
				case OpenTK.Graphics.OpenGL.BlendingFactorDest.Zero: return BlendParameter.Zero;
				case OpenTK.Graphics.OpenGL.BlendingFactorDest.One: return BlendParameter.One;
				case OpenTK.Graphics.OpenGL.BlendingFactorDest.SrcAlpha: return BlendParameter.SourceAlpha;
				case OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha: return BlendParameter.InvertSourceAlpha;
				//case OpenTK.Graphics.OpenGL.BlendingFactorDest.DstColor: return BlendParameter.DestinationColor;
				case OpenTK.Graphics.OpenGL.BlendingFactorDest.DstAlpha: return BlendParameter.DestinationAlpha;
				//case OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusDstColor: return BlendParameter.InvertDestinationColor;
				case OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusDstAlpha: return BlendParameter.InvertDestinationAlpha;
				default: throw new ArgumentException ();
			}
		}

		private StencilFunction OriginalToMistyValue ( OpenTK.Graphics.OpenGL.StencilFunction func )
		{
			switch ( func )
			{
				case OpenTK.Graphics.OpenGL.StencilFunction.Always: return StencilFunction.Always;
				case OpenTK.Graphics.OpenGL.StencilFunction.Equal: return StencilFunction.Equal;
				case OpenTK.Graphics.OpenGL.StencilFunction.Gequal: return StencilFunction.GreaterEqual;
				case OpenTK.Graphics.OpenGL.StencilFunction.Greater: return StencilFunction.Greater;
				case OpenTK.Graphics.OpenGL.StencilFunction.Lequal: return StencilFunction.LessEqual;
				case OpenTK.Graphics.OpenGL.StencilFunction.Less: return StencilFunction.Less;
				case OpenTK.Graphics.OpenGL.StencilFunction.Never: return StencilFunction.Never;
				case OpenTK.Graphics.OpenGL.StencilFunction.Notequal: return StencilFunction.NotEqual;

				default: throw new ArgumentException ();
			}
		}

		private StencilOperator OriginalToMistyValue ( OpenTK.Graphics.OpenGL.StencilOp op )
		{
			switch ( op )
			{
				case StencilOp.Zero: return StencilOperator.Zero;
				case StencilOp.Keep: return StencilOperator.Keep;
				case StencilOp.Invert: return StencilOperator.Invert;
				case StencilOp.Replace: return StencilOperator.Replace;
				case StencilOp.Incr: return StencilOperator.Increase;
				case StencilOp.IncrWrap: return StencilOperator.IncreaseWrap;
				case StencilOp.Decr: return StencilOperator.Decrease;
				case StencilOp.DecrWrap: return StencilOperator.DecreaseWrap;

				default: throw new ArgumentException ();
			}
		}

		private OpenTK.Graphics.OpenGL.BlendingFactorSrc MistyValueToOriginal ( BlendParameter param )
		{
			switch ( param )
			{
				case BlendParameter.Zero: return OpenTK.Graphics.OpenGL.BlendingFactorSrc.Zero;
				case BlendParameter.One: return OpenTK.Graphics.OpenGL.BlendingFactorSrc.One;
				case BlendParameter.SourceAlpha: return OpenTK.Graphics.OpenGL.BlendingFactorSrc.SrcAlpha;
				case BlendParameter.InvertSourceAlpha: return OpenTK.Graphics.OpenGL.BlendingFactorSrc.OneMinusSrcAlpha;
				case BlendParameter.DestinationColor: return OpenTK.Graphics.OpenGL.BlendingFactorSrc.DstColor;
				case BlendParameter.DestinationAlpha: return OpenTK.Graphics.OpenGL.BlendingFactorSrc.DstAlpha;
				case BlendParameter.InvertDestinationColor: return OpenTK.Graphics.OpenGL.BlendingFactorSrc.OneMinusDstColor;
				case BlendParameter.InvertDestinationAlpha: return OpenTK.Graphics.OpenGL.BlendingFactorSrc.OneMinusDstAlpha;
				default: throw new ArgumentException ();
			}
		}

		private BlendOperator OriginalToMistyValue ( OpenTK.Graphics.OpenGL.BlendEquationMode mode )
		{
			switch ( mode )
			{
				case OpenTK.Graphics.OpenGL.BlendEquationMode.Min: return BlendOperator.Minimum;
				case OpenTK.Graphics.OpenGL.BlendEquationMode.Max: return BlendOperator.Maximum;
				case OpenTK.Graphics.OpenGL.BlendEquationMode.FuncAdd: return BlendOperator.Add;
				case OpenTK.Graphics.OpenGL.BlendEquationMode.FuncSubtract: return BlendOperator.Subtract;
				case OpenTK.Graphics.OpenGL.BlendEquationMode.FuncReverseSubtract: return BlendOperator.ReverseSubtract;
				default: throw new ArgumentException ();
			}
		}

		private OpenTK.Graphics.OpenGL.BlendEquationMode MistyValueToOriginal ( BlendOperator op )
		{
			switch ( op )
			{
				case BlendOperator.Minimum: return OpenTK.Graphics.OpenGL.BlendEquationMode.Min;
				case BlendOperator.Maximum: return OpenTK.Graphics.OpenGL.BlendEquationMode.Max;
				case BlendOperator.Add: return OpenTK.Graphics.OpenGL.BlendEquationMode.FuncAdd;
				case BlendOperator.Subtract: return OpenTK.Graphics.OpenGL.BlendEquationMode.FuncSubtract;
				case BlendOperator.ReverseSubtract: return OpenTK.Graphics.OpenGL.BlendEquationMode.FuncReverseSubtract;
				default: throw new ArgumentException ();
			}
		}

		private OpenTK.Graphics.OpenGL.StencilOp MistyValueToOriginal ( StencilOperator op )
		{
			switch ( op )
			{
				case StencilOperator.Zero: return OpenTK.Graphics.OpenGL.StencilOp.Zero;
				case StencilOperator.Keep: return OpenTK.Graphics.OpenGL.StencilOp.Keep;
				case StencilOperator.Replace: return OpenTK.Graphics.OpenGL.StencilOp.Replace;
				case StencilOperator.Invert: return OpenTK.Graphics.OpenGL.StencilOp.Invert;
				case StencilOperator.Increase: return OpenTK.Graphics.OpenGL.StencilOp.Incr;
				case StencilOperator.Decrease: return OpenTK.Graphics.OpenGL.StencilOp.Decr;
				case StencilOperator.IncreaseWrap: return OpenTK.Graphics.OpenGL.StencilOp.IncrWrap;
				case StencilOperator.DecreaseWrap: return OpenTK.Graphics.OpenGL.StencilOp.DecrWrap;
				default: throw new ArgumentException ();
			}
		}

		private OpenTK.Graphics.OpenGL.StencilFunction MistyValueToOriginal ( StencilFunction func )
		{
			switch ( func )
			{
				case StencilFunction.Never: return OpenTK.Graphics.OpenGL.StencilFunction.Never;
				case StencilFunction.Always: return OpenTK.Graphics.OpenGL.StencilFunction.Always;
				case StencilFunction.Equal: return OpenTK.Graphics.OpenGL.StencilFunction.Equal;
				case StencilFunction.NotEqual: return OpenTK.Graphics.OpenGL.StencilFunction.Notequal;
				case StencilFunction.Greater: return OpenTK.Graphics.OpenGL.StencilFunction.Greater;
				case StencilFunction.Less: return OpenTK.Graphics.OpenGL.StencilFunction.Less;
				case StencilFunction.GreaterEqual: return OpenTK.Graphics.OpenGL.StencilFunction.Gequal;
				case StencilFunction.LessEqual: return OpenTK.Graphics.OpenGL.StencilFunction.Lequal;
				default: throw new ArgumentException ();
			}
		}

		private PrimitiveType OriginalToMistyValue ( OpenTK.Graphics.OpenGL.BeginMode mode )
		{
			switch ( mode )
			{
				case OpenTK.Graphics.OpenGL.BeginMode.Points: return PrimitiveType.PointList;
				case OpenTK.Graphics.OpenGL.BeginMode.Lines: return PrimitiveType.LineList;
				case OpenTK.Graphics.OpenGL.BeginMode.LineStrip: return PrimitiveType.LineStrip;
				case OpenTK.Graphics.OpenGL.BeginMode.Triangles: return PrimitiveType.TriangleList;
				case OpenTK.Graphics.OpenGL.BeginMode.TriangleStrip: return PrimitiveType.TriangleStrip;
				case OpenTK.Graphics.OpenGL.BeginMode.TriangleFan: return PrimitiveType.TriangleFan;
				default: throw new ArgumentException ();
			}
		}

		private OpenTK.Graphics.OpenGL.PrimitiveType MistyValueToOriginal ( PrimitiveType type )
		{
			switch ( type )
			{
				case PrimitiveType.PointList: return OpenTK.Graphics.OpenGL.PrimitiveType.Points;
				case PrimitiveType.LineList: return OpenTK.Graphics.OpenGL.PrimitiveType.Lines;
				case PrimitiveType.LineStrip: return OpenTK.Graphics.OpenGL.PrimitiveType.LineStrip;
				case PrimitiveType.TriangleList: return OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
				case PrimitiveType.TriangleStrip: return OpenTK.Graphics.OpenGL.PrimitiveType.TriangleStrip;
				case PrimitiveType.TriangleFan: return OpenTK.Graphics.OpenGL.PrimitiveType.TriangleFan;
				default: throw new ArgumentException ();
			}
		}

		private OpenTK.Graphics.OpenGL.ClearBufferMask MistyValueToOriginal ( ClearBuffer buffer )
		{
			OpenTK.Graphics.OpenGL.ClearBufferMask bufferMask = ( OpenTK.Graphics.OpenGL.ClearBufferMask ) 0;
			if ( ( buffer & ClearBuffer.ColorBuffer ) != 0 ) bufferMask |= OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit;
			if ( ( buffer & ClearBuffer.DepthBuffer ) != 0 ) bufferMask |= OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit;
			if ( ( buffer & ClearBuffer.StencilBuffer ) != 0 ) bufferMask |= OpenTK.Graphics.OpenGL.ClearBufferMask.StencilBufferBit;
			return bufferMask;
		}
	}
}
