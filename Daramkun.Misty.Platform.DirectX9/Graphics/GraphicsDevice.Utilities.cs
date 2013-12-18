using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	public partial class GraphicsDevice : StandardDispose, IGraphicsDevice
	{
		private int ChangeCullMode ( Graphics.CullingMode value )
		{
			switch ( value )
			{
				case Graphics.CullingMode.None: return ( int ) SharpDX.Direct3D9.Cull.None;
				case Graphics.CullingMode.ClockWise: return ( int ) SharpDX.Direct3D9.Cull.Clockwise;
				case Graphics.CullingMode.CounterClockWise: return ( int ) SharpDX.Direct3D9.Cull.Counterclockwise;
				default: throw new ArgumentException ();
			}
		}

		private Graphics.CullingMode ConvertCullMode ( int p )
		{
			switch ( ( SharpDX.Direct3D9.Cull ) p )
			{
				case SharpDX.Direct3D9.Cull.None: return Graphics.CullingMode.None;
				case SharpDX.Direct3D9.Cull.Clockwise: return Graphics.CullingMode.ClockWise;
				case SharpDX.Direct3D9.Cull.Counterclockwise: return Graphics.CullingMode.CounterClockWise;
				default: throw new ArgumentException ();
			}
		}

		private int ChangeFillMode ( Graphics.FillMode value )
		{
			switch ( value )
			{
				case Graphics.FillMode.Point: return ( int ) SharpDX.Direct3D9.FillMode.Point;
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
				case SharpDX.Direct3D9.FillMode.Point: return Graphics.FillMode.Point;
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
	}
}
