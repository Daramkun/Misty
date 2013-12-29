using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daramkun.Misty.Graphics
{
	public partial class GraphicsDevice
	{
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

		private int GetPrimitiveCount ( PrimitiveType primitiveType )
		{
			switch ( primitiveType )
			{
				case PrimitiveType.PointList: return 1;
				case PrimitiveType.LineList: case PrimitiveType.LineStrip: return 2;
				case PrimitiveType.TriangleList: case PrimitiveType.TriangleStrip: return 3;
				default: throw new ArgumentException ();
			}
		}
	}
}
