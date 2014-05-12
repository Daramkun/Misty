using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	partial class GraphicsContext
	{
		private int ChangeCullMode ( Graphics.CullMode value )
		{
			switch ( value )
			{
				case Graphics.CullMode.None: return ( int ) SharpDX.Direct3D9.Cull.None;
				case Graphics.CullMode.ClockWise: return ( int ) SharpDX.Direct3D9.Cull.Clockwise;
				case Graphics.CullMode.CounterClockWise: return ( int ) SharpDX.Direct3D9.Cull.Counterclockwise;
				default: throw new ArgumentException ();
			}
		}

		private Graphics.CullMode ConvertCullMode ( int p )
		{
			switch ( ( SharpDX.Direct3D9.Cull ) p )
			{
				case SharpDX.Direct3D9.Cull.None: return Graphics.CullMode.None;
				case SharpDX.Direct3D9.Cull.Clockwise: return Graphics.CullMode.ClockWise;
				case SharpDX.Direct3D9.Cull.Counterclockwise: return Graphics.CullMode.CounterClockWise;
				default: throw new ArgumentException ();
			}
		}

		private int ChangeFillMode ( Graphics.FillMode value )
		{
			switch ( value )
			{
				case Graphics.FillMode.Wireframe: return ( int ) SharpDX.Direct3D9.FillMode.Wireframe;
				case Graphics.FillMode.Solid: return ( int ) SharpDX.Direct3D9.FillMode.Solid;
				default: throw new ArgumentException ();
			}
		}

		private Graphics.FillMode ConvertFillMode ( int p )
		{
			switch ( ( SharpDX.Direct3D9.FillMode ) p )
			{
				case SharpDX.Direct3D9.FillMode.Solid: return Graphics.FillMode.Solid;
				case SharpDX.Direct3D9.FillMode.Wireframe: return Graphics.FillMode.Wireframe;
				default: throw new ArgumentException ();
			}
		}

		private BlendParameter DeconvertBlendParam ( int blendParameter )
		{
			switch ( ( SharpDX.Direct3D9.Blend ) blendParameter )
			{
				case SharpDX.Direct3D9.Blend.Zero: return BlendParameter.Zero;
				case SharpDX.Direct3D9.Blend.One: return BlendParameter.One;
				case SharpDX.Direct3D9.Blend.SourceColor: return BlendParameter.SourceColor;
				case SharpDX.Direct3D9.Blend.SourceAlpha: return BlendParameter.SourceAlpha;
				case SharpDX.Direct3D9.Blend.DestinationColor: return BlendParameter.DestinationColor;
				case SharpDX.Direct3D9.Blend.DestinationAlpha: return BlendParameter.DestinationAlpha;
				case SharpDX.Direct3D9.Blend.InverseSourceColor: return BlendParameter.InvertSourceColor;
				case SharpDX.Direct3D9.Blend.InverseSourceAlpha: return BlendParameter.InvertSourceAlpha;
				case SharpDX.Direct3D9.Blend.InverseDestinationColor: return BlendParameter.InvertDestinationColor;
				case SharpDX.Direct3D9.Blend.InverseDestinationAlpha: return BlendParameter.InvertDestinationAlpha;
				default: throw new ArgumentException ();
			}
		}

		private BlendOperator DeconvertBlendOp ( int op )
		{
			switch ( ( SharpDX.Direct3D9.BlendOperation ) op )
			{
				case SharpDX.Direct3D9.BlendOperation.Add: return BlendOperator.Add;
				case SharpDX.Direct3D9.BlendOperation.Subtract: return BlendOperator.Subtract;
				case SharpDX.Direct3D9.BlendOperation.ReverseSubtract: return BlendOperator.ReverseSubtract;
				case SharpDX.Direct3D9.BlendOperation.Maximum: return BlendOperator.Maximum;
				case SharpDX.Direct3D9.BlendOperation.Minimum: return BlendOperator.Minimum;
				default: throw new ArgumentException ();
			}
		}

		private StencilOperator DeconvertStencilOp ( int op )
		{
			switch ( ( SharpDX.Direct3D9.StencilOperation ) op )
			{
				case SharpDX.Direct3D9.StencilOperation.Zero: return StencilOperator.Zero;
				case SharpDX.Direct3D9.StencilOperation.Replace: return StencilOperator.Replace;
				case SharpDX.Direct3D9.StencilOperation.Keep: return StencilOperator.Keep;
				case SharpDX.Direct3D9.StencilOperation.Invert: return StencilOperator.Invert;
				case SharpDX.Direct3D9.StencilOperation.Increment: return StencilOperator.Increase;
				case SharpDX.Direct3D9.StencilOperation.Decrement: return StencilOperator.Decrease;
				case SharpDX.Direct3D9.StencilOperation.IncrementSaturate: return StencilOperator.IncreaseWrap;
				case SharpDX.Direct3D9.StencilOperation.DecrementSaturate: return StencilOperator.DecreaseWrap;
				default: throw new ArgumentException ();
			}
		}

		private StencilFunction DeconvertStencilFunc ( int func )
		{
			switch ( ( SharpDX.Direct3D9.Compare ) func )
			{
				case SharpDX.Direct3D9.Compare.Always: return StencilFunction.Always;
				case SharpDX.Direct3D9.Compare.Never: return StencilFunction.Never;
				case SharpDX.Direct3D9.Compare.Equal: return StencilFunction.Equal;
				case SharpDX.Direct3D9.Compare.NotEqual: return StencilFunction.NotEqual;
				case SharpDX.Direct3D9.Compare.Greater: return StencilFunction.Greater;
				case SharpDX.Direct3D9.Compare.GreaterEqual: return StencilFunction.GreaterEqual;
				case SharpDX.Direct3D9.Compare.Less: return StencilFunction.Less;
				case SharpDX.Direct3D9.Compare.LessEqual: return StencilFunction.LessEqual;
				default: throw new ArgumentException ();
			}
		}

		private int ConvertStencilOp ( StencilOperator op )
		{
			switch ( op )
			{
				case StencilOperator.Zero: return ( int ) SharpDX.Direct3D9.StencilOperation.Zero;
				case StencilOperator.Keep: return ( int ) SharpDX.Direct3D9.StencilOperation.Keep;
				case StencilOperator.Replace: return ( int ) SharpDX.Direct3D9.StencilOperation.Replace;
				case StencilOperator.Invert: return ( int ) SharpDX.Direct3D9.StencilOperation.Invert;
				case StencilOperator.Increase: return ( int ) SharpDX.Direct3D9.StencilOperation.Increment;
				case StencilOperator.Decrease: return ( int ) SharpDX.Direct3D9.StencilOperation.Decrement;
				case StencilOperator.IncreaseWrap: return ( int ) SharpDX.Direct3D9.StencilOperation.IncrementSaturate;
				case StencilOperator.DecreaseWrap: return ( int ) SharpDX.Direct3D9.StencilOperation.DecrementSaturate;
				default: throw new ArgumentException ();
			}
		}

		private int ConvertStencilFunc ( StencilFunction func )
		{
			switch ( func )
			{
				case StencilFunction.Always: return ( int ) SharpDX.Direct3D9.Compare.Always;
				case StencilFunction.Never: return ( int ) SharpDX.Direct3D9.Compare.Never;
				case StencilFunction.Equal: return ( int ) SharpDX.Direct3D9.Compare.Equal;
				case StencilFunction.NotEqual: return ( int ) SharpDX.Direct3D9.Compare.NotEqual;
				case StencilFunction.Greater: return ( int ) SharpDX.Direct3D9.Compare.Greater;
				case StencilFunction.GreaterEqual: return ( int ) SharpDX.Direct3D9.Compare.GreaterEqual;
				case StencilFunction.Less: return ( int ) SharpDX.Direct3D9.Compare.Less;
				case StencilFunction.LessEqual: return ( int ) SharpDX.Direct3D9.Compare.LessEqual;
				default: throw new ArgumentException ();
			}
		}

		private int ConvertBlendParam ( BlendParameter blendParameter )
		{
			switch ( blendParameter )
			{
				case BlendParameter.Zero: return ( int ) SharpDX.Direct3D9.Blend.Zero;
				case BlendParameter.One: return ( int ) SharpDX.Direct3D9.Blend.One;
				case BlendParameter.SourceColor: return ( int ) SharpDX.Direct3D9.Blend.SourceColor;
				case BlendParameter.SourceAlpha: return ( int ) SharpDX.Direct3D9.Blend.SourceAlpha;
				case BlendParameter.DestinationColor: return ( int ) SharpDX.Direct3D9.Blend.DestinationColor;
				case BlendParameter.DestinationAlpha: return ( int ) SharpDX.Direct3D9.Blend.DestinationAlpha;
				case BlendParameter.InvertSourceColor: return ( int ) SharpDX.Direct3D9.Blend.InverseSourceColor;
				case BlendParameter.InvertSourceAlpha: return ( int ) SharpDX.Direct3D9.Blend.InverseSourceAlpha;
				case BlendParameter.InvertDestinationColor: return ( int ) SharpDX.Direct3D9.Blend.InverseDestinationColor;
				case BlendParameter.InvertDestinationAlpha: return ( int ) SharpDX.Direct3D9.Blend.InverseDestinationAlpha;
				default: throw new ArgumentException ();
			}
		}

		private int ConvertBlendOp ( BlendOperator blendOperator )
		{
			switch ( blendOperator )
			{
				case BlendOperator.Add: return ( int ) SharpDX.Direct3D9.BlendOperation.Add;
				case BlendOperator.Minimum: return ( int ) SharpDX.Direct3D9.BlendOperation.Minimum;
				case BlendOperator.Maximum: return ( int ) SharpDX.Direct3D9.BlendOperation.Maximum;
				case BlendOperator.Subtract: return ( int ) SharpDX.Direct3D9.BlendOperation.Subtract;
				case BlendOperator.ReverseSubtract: return ( int ) SharpDX.Direct3D9.BlendOperation.ReverseSubtract;
				default: throw new ArgumentException ();
			}
		}

		private SharpDX.Direct3D9.ClearFlags ChangeClearBuffer ( ClearBuffer clearBuffer )
		{
			SharpDX.Direct3D9.ClearFlags clearFlags = SharpDX.Direct3D9.ClearFlags.None;
			if ( clearBuffer.HasFlag ( ClearBuffer.ColorBuffer ) ) clearFlags |= SharpDX.Direct3D9.ClearFlags.Target;
			if ( clearBuffer.HasFlag ( ClearBuffer.DepthBuffer ) ) clearFlags |= SharpDX.Direct3D9.ClearFlags.ZBuffer;
			if ( clearBuffer.HasFlag ( ClearBuffer.StencilBuffer ) ) clearFlags |= SharpDX.Direct3D9.ClearFlags.Stencil;
			return clearFlags;
		}

		private SharpDX.ColorBGRA ChangeColor ( Color color )
		{
			return new SharpDX.ColorBGRA ( new SharpDX.Vector3 ( color.RedScalar, color.GreenScalar, color.BlueScalar ), color.AlphaScalar );
		}

		private SharpDX.Direct3D9.PrimitiveType ConvertPrimitiveType ( PrimitiveType primitiveType )
		{
			switch ( primitiveType )
			{
				case PrimitiveType.PointList: return SharpDX.Direct3D9.PrimitiveType.PointList;
				case PrimitiveType.LineList: return SharpDX.Direct3D9.PrimitiveType.LineList;
				case PrimitiveType.LineStrip: return SharpDX.Direct3D9.PrimitiveType.LineStrip;
				case PrimitiveType.TriangleList: return SharpDX.Direct3D9.PrimitiveType.TriangleList;
				case PrimitiveType.TriangleStrip: return SharpDX.Direct3D9.PrimitiveType.TriangleStrip;
				case PrimitiveType.TriangleFan: return SharpDX.Direct3D9.PrimitiveType.TriangleFan;
				default: throw new ArgumentException ();
			}
		}

		private int GetPrimitiveUnit ( PrimitiveType primitiveType )
		{
			switch ( primitiveType )
			{
				case PrimitiveType.PointList: return 1;
				case PrimitiveType.LineList:
				case PrimitiveType.LineStrip: return 2;
				case PrimitiveType.TriangleList:
				case PrimitiveType.TriangleStrip:
				case PrimitiveType.TriangleFan: return 3;
				default: throw new ArgumentException ();
			}
		}

		private int ChangeFilter ( TextureFilter textureFilter )
		{
			switch ( textureFilter )
			{
				case TextureFilter.Nearest: return ( int ) SharpDX.Direct3D9.TextureFilter.Point;
				case TextureFilter.Linear: return ( int ) SharpDX.Direct3D9.TextureFilter.Linear;
				case TextureFilter.Anisotropic: return ( int ) SharpDX.Direct3D9.TextureFilter.Anisotropic;
				default: throw new ArgumentException ();
			}
		}

		private int ChangeAddress ( TextureAddressing textureAddressing )
		{
			switch ( textureAddressing )
			{
				case TextureAddressing.Wrap: return ( int ) SharpDX.Direct3D9.TextureAddress.Wrap;
				case TextureAddressing.Mirror: return ( int ) SharpDX.Direct3D9.TextureAddress.Mirror;
				case TextureAddressing.Clamp: return ( int ) SharpDX.Direct3D9.TextureAddress.Clamp;
				default: throw new ArgumentException ();
			}
		}
	}
}
