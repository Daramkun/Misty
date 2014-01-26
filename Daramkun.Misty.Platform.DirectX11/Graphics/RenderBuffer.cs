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
		internal SharpDX.Direct3D11.RenderTargetView targetView;
		internal SharpDX.Direct3D11.DepthStencilView depthStencil;
		internal SharpDX.Direct3D11.Texture2D depthStencilBuffer;
		internal SharpDX.Direct3D11.ShaderResourceView shaderView;

		public RenderBuffer ( IGraphicsDevice graphicsDevice, int width, int height )
			: base ( graphicsDevice, width, height, 1, true )
		{
			targetView = new SharpDX.Direct3D11.RenderTargetView ( graphicsDevice.Handle as SharpDX.Direct3D11.Device,
				Handle as SharpDX.Direct3D11.Texture2D, new SharpDX.Direct3D11.RenderTargetViewDescription ()
				{
					Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2D,
					Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
					Texture2D = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DResource () { MipSlice = 0 }
				}
			);
			depthStencilBuffer = new SharpDX.Direct3D11.Texture2D ( graphicsDevice.Handle as SharpDX.Direct3D11.Device,
				new SharpDX.Direct3D11.Texture2DDescription ()
				{
					ArraySize = 1,
					Width = width,
					Height = height,
					Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
					MipLevels = 0,
					BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
				}
			);
			depthStencil = new SharpDX.Direct3D11.DepthStencilView ( graphicsDevice.Handle as SharpDX.Direct3D11.Device,
				depthStencilBuffer, new SharpDX.Direct3D11.DepthStencilViewDescription ()
				{
					Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D,
					Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
					Texture2D = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture2DResource () { MipSlice = 0 }
				}
			);
			shaderView = new SharpDX.Direct3D11.ShaderResourceView ( graphicsDevice.Handle as SharpDX.Direct3D11.Device,
				Handle as SharpDX.Direct3D11.Texture2D, new SharpDX.Direct3D11.ShaderResourceViewDescription ()
				{
					Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
					Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2D,
					Texture2D = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture2DResource () { MipLevels = 1, MostDetailedMip = 0 }
				}
			);
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				depthStencil.Dispose ();
				depthStencilBuffer.Dispose ();
				targetView.Dispose ();
			}
			base.Dispose ( isDisposing );
		}
	}
}
