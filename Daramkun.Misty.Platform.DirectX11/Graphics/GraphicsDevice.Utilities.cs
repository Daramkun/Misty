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
	}
}
