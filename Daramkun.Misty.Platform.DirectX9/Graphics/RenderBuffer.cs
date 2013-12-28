using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class RenderBuffer : Texture2D, IRenderBuffer
	{
		internal SharpDX.Direct3D9.RenderToSurface rts;

		public RenderBuffer ( IGraphicsDevice graphicsDevice, int width, int height )
			: base ( graphicsDevice, width, height, SharpDX.Direct3D9.Usage.RenderTarget )
		{
			if ( width == 0 ) width = 1;
			if ( height == 0 ) height = 1;
			SharpDX.Direct3D9.Surface surface = ( Handle as SharpDX.Direct3D9.Texture ).GetSurfaceLevel ( 0 );
			rts = new SharpDX.Direct3D9.RenderToSurface ( graphicsDevice.Handle as SharpDX.Direct3D9.Device,
				surface.Description.Width, surface.Description.Height, surface.Description.Format, true, SharpDX.Direct3D9.Format.D24S8 );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				rts.Dispose ();
			}
			base.Dispose ( isDisposing );
		}
	}
}
