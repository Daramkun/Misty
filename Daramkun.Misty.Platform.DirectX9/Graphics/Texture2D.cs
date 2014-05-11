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
		SharpDX.Direct3D9.Texture texture;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public object Handle { get { return texture; } }

		public Color [] Buffer
		{
			get
			{
				SharpDX.DataRectangle dr = texture.LockRectangle ( 0, SharpDX.Direct3D9.LockFlags.ReadOnly );
				SharpDX.DataStream stream = new SharpDX.DataStream ( dr.DataPointer, ( dr.Pitch / 4 ) * Height * 4, true, false );
				Color [] colours = new Color [ stream.Length / 4 ];
				for ( int y = 0; y < Height; ++y )
					for ( int x = 0; x < Width; ++x )
					{
						stream.Position = ( y * ( dr.Pitch / 4 ) + x ) * 4;
						colours [ y * Width + x ] = new Color ( stream.Read<SharpDX.Color> ().ToBgra (), true );
					}
				texture.UnlockRectangle ( 0 );
				stream.Dispose ();
				return colours;
			}
			set
			{
				if ( value.Length == 0 ) return;
				SharpDX.DataRectangle dr = texture.LockRectangle ( 0, SharpDX.Direct3D9.LockFlags.None );
				SharpDX.DataStream stream = new SharpDX.DataStream ( dr.DataPointer, ( dr.Pitch / 4 ) * Height * 4, false, true );
				SharpDX.Color [] colours = new SharpDX.Color [ stream.Length / 4 ];
				for ( int y = 0; y < Height; ++y )
					for ( int x = 0; x < Width; ++x )
						colours [ y * ( dr.Pitch / 4 ) + x ] = new SharpDX.Color ( value [ y * Width + x ].ARGBValue );
				stream.WriteRange<SharpDX.Color> ( colours );
				stream.Dispose ();
				texture.UnlockRectangle ( 0 );
			}
		}

		protected Texture2D ( IGraphicsDevice graphicsDevice, int width, int height, SharpDX.Direct3D9.Usage usage, int mipmapLevel = 1 )
		{
			if ( width == 0 ) width = 1;
			if ( height == 0 ) height = 1;
			texture = new SharpDX.Direct3D9.Texture ( graphicsDevice.Handle as SharpDX.Direct3D9.Device, width, height,
				mipmapLevel, usage, SharpDX.Direct3D9.Format.A8R8G8B8,
				usage.HasFlag ( SharpDX.Direct3D9.Usage.RenderTarget ) ? SharpDX.Direct3D9.Pool.Default : SharpDX.Direct3D9.Pool.Managed );
			texture.FilterTexture ( 0, SharpDX.Direct3D9.Filter.Point );
			Width = width;
			Height = height;
		}

		public Texture2D ( IGraphicsDevice graphicsDevice, int width, int height, int mipmapLevel = 1 )
			: this ( graphicsDevice, width, height, SharpDX.Direct3D9.Usage.AutoGenerateMipMap, 1 ) { }

		public Texture2D ( IGraphicsDevice graphicsDevice, ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
			: this ( graphicsDevice, imageInfo.Width, imageInfo.Height )
		{
			Buffer = imageInfo.GetPixels ( colorKey );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				texture.Dispose ();
			}
			base.Dispose ( isDisposing );
		}
	}
}
