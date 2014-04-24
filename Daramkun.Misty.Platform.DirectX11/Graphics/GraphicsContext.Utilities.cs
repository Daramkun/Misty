using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	partial class GraphicsContext
	{
		private CullMode ConvertFromCullMode ( SharpDX.Direct3D11.CullMode cullMode )
		{
			switch ( cullMode )
			{
				case SharpDX.Direct3D11.CullMode.Back: return CullMode.CounterClockWise;
				case SharpDX.Direct3D11.CullMode.Front: return CullMode.ClockWise;
				case SharpDX.Direct3D11.CullMode.None: return CullMode.None;
				default: throw new ArgumentException ();
			}
		}

		private SharpDX.Direct3D11.CullMode ConvertToCullMode ( CullMode value )
		{
			switch ( value )
			{
				case CullMode.None: return SharpDX.Direct3D11.CullMode.None;
				case CullMode.ClockWise: return SharpDX.Direct3D11.CullMode.Front;
				case CullMode.CounterClockWise: return SharpDX.Direct3D11.CullMode.Back;
				default: throw new ArgumentException ();
			}
		}

		private Graphics.FillMode ConvertFromFillMode ( SharpDX.Direct3D11.FillMode fillMode )
		{
			switch ( fillMode )
			{
				case SharpDX.Direct3D11.FillMode.Solid: return Graphics.FillMode.Solid;
				case SharpDX.Direct3D11.FillMode.Wireframe: return Graphics.FillMode.Wireframe;
				default: throw new ArgumentException ();
			}
		}

		private SharpDX.Direct3D11.FillMode ConvertToFillMode ( Graphics.FillMode value )
		{
			switch ( value )
			{
				case Graphics.FillMode.Solid: return SharpDX.Direct3D11.FillMode.Solid;
				case Graphics.FillMode.Wireframe: return SharpDX.Direct3D11.FillMode.Wireframe;
				default: throw new ArgumentException ();
			}
		}

		private SharpDX.Color4 ConvertColor ( Color color )
		{
			return new SharpDX.Color4 ( color.RedScalar, color.GreenScalar, color.BlueScalar, color.AlphaScalar );
		}

		private SharpDX.Direct3D11.DepthStencilClearFlags ConvertDepthStencilClearMethod ( ClearBuffer clearBuffer )
		{
			return ( clearBuffer.HasFlag ( ClearBuffer.DepthBuffer ) ? SharpDX.Direct3D11.DepthStencilClearFlags.Depth : 0 ) |
				( clearBuffer.HasFlag ( ClearBuffer.StencilBuffer ) ? SharpDX.Direct3D11.DepthStencilClearFlags.Stencil : 0 );
		}

		private SharpDX.Direct3D.PrimitiveTopology ConvertToPrimitiveTopology ( PrimitiveType primitiveType )
		{
			switch ( primitiveType )
			{
				case PrimitiveType.PointList: return SharpDX.Direct3D.PrimitiveTopology.PointList;
				case PrimitiveType.LineList: return SharpDX.Direct3D.PrimitiveTopology.LineList;
				case PrimitiveType.LineStrip: return SharpDX.Direct3D.PrimitiveTopology.LineStrip;
				case PrimitiveType.TriangleList: return SharpDX.Direct3D.PrimitiveTopology.TriangleList;
				case PrimitiveType.TriangleStrip: return SharpDX.Direct3D.PrimitiveTopology.TriangleStrip;
				default: throw new ArgumentException ();
			}
		}

		private int GetPrimitiveCount ( PrimitiveType primitiveType, int primitiveCount )
		{
			switch ( primitiveType )
			{
				case PrimitiveType.PointList: return primitiveCount;
				case PrimitiveType.LineList: return primitiveCount * 2;
				case PrimitiveType.LineStrip: return primitiveCount + 1;
				case PrimitiveType.TriangleList: return primitiveCount * 3;
				case PrimitiveType.TriangleStrip: return primitiveCount + 2;
				case PrimitiveType.TriangleFan: return primitiveCount + 2;
				default: throw new ArgumentException ();
			}
		}
	}
}
