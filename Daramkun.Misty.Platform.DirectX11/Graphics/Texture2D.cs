using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	class Texture2D : StandardDispose, ITexture2D
	{
		SharpDX.Direct3D11.Texture2D texture;

		public int Width { get { return ( int ) Size.X; } }
		public int Height { get { return ( int ) Size.Y; } }

		public Vector2 Size { get; private set; }

		public Color [] Buffer
		{
			get
			{
				SharpDX.DataStream stream;
				SharpDX.DataBox box = texture.Device.ImmediateContext.MapSubresource ( texture, 0, 0, SharpDX.Direct3D11.MapMode.Read, 
					SharpDX.Direct3D11.MapFlags.None, out stream );
				Color [] colours = new Color [ stream.Length / 4 ];
				for ( int y = 0; y < Height; ++y )
					for ( int x = 0; x < Width; ++x )
					{
						stream.Position = ( y * Width + x ) * 4;
						colours [ y * Width + x ] = new Color ( stream.Read<SharpDX.Color> ().ToBgra (), true );
					}
				texture.Device.ImmediateContext.UnmapSubresource ( texture, 0 );
				return colours;
			}
			set
			{
				SharpDX.DataStream stream;
				SharpDX.DataBox box = texture.Device.ImmediateContext.MapSubresource ( texture, 0, 0, SharpDX.Direct3D11.MapMode.Read,
					SharpDX.Direct3D11.MapFlags.None, out stream );
				SharpDX.Color [] colours = new SharpDX.Color [ stream.Length / 4 ];
				for ( int y = 0; y < Height; ++y )
					for ( int x = 0; x < Width; ++x )
						colours [ y * Width + x ] = new SharpDX.Color ( value [ y * Width + x ].ARGBValue );
				stream.WriteRange<SharpDX.Color> ( colours );
				texture.Device.ImmediateContext.UnmapSubresource ( texture, 0 );
			}
		}

		public object Handle { get { return texture; } }

		public Texture2D ( IGraphicsDevice graphicsDevice, int width, int height, int mipmapLevel = 1 )
		{
			if ( width == 0 ) width = 1;
			if ( height == 0 ) height = 1;
			Size = new Vector2 ( width, height );
			texture = new SharpDX.Direct3D11.Texture2D ( graphicsDevice.Handle as SharpDX.Direct3D11.Device, new SharpDX.Direct3D11.Texture2DDescription ()
			{
				Width = width,
				Height = height,
				Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
				MipLevels = mipmapLevel,
				CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.Read | SharpDX.Direct3D11.CpuAccessFlags.Write,
				Usage = SharpDX.Direct3D11.ResourceUsage.Default,
				BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
				ArraySize = 1,
			} );
		}

		public Texture2D ( IGraphicsDevice graphicsDevice, ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
			: this ( graphicsDevice, imageInfo.Width, imageInfo.Height )
		{
			Buffer = imageInfo.GetPixels ( colorKey );
		}
	}
}
