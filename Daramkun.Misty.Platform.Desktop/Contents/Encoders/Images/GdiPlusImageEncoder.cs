using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.Encoders;

namespace Daramkun.Misty.Contents.Encoders.Images
{
	public enum GdiPlusImageEncodingType
	{
		Png,
		Bmp,
		Jpeg,
		Tiff,
		Gif,
	}

	[FileFormat ( "bmp", "dib", "gif", "jpeg", "jpg", "png", "tiff", "exif", "wmf", "emf" )]
	public class GdiPlusImageEncoder : IEncoder<ImageInfo>
	{
		public bool Encode ( Stream stream, ImageInfo data, params object [] args )
		{
			GdiPlusImageEncodingType type = ( args.Length != 1 ) ? GdiPlusImageEncodingType.Png : ( GdiPlusImageEncodingType ) args [ 0 ];
			Bitmap bitmap = new Bitmap ( data.Width, data.Height, PixelFormat.Format32bppArgb );

			for ( int i = 0; i < data.FrameCount; ++i )
			{
				Daramkun.Misty.Graphics.Color [] colours = data.GetPixels ( null, i );
				bitmap.SelectActiveFrame ( new FrameDimension ( bitmap.FrameDimensionsList [ 0 ] ), i );
				BitmapData bitmapData = bitmap.LockBits ( new Rectangle ( 0, 0, data.Width, data.Height ), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb );
				byte [] buffer = new byte [ colours.Length * 4 ];
				for ( int j = 0, index = 0; j < buffer.Length; j += 4, ++index )
				{
					buffer [ j + 0 ] = colours [ index ].AlphaValue;
					buffer [ j + 1 ] = colours [ index ].RedValue;
					buffer [ j + 2 ] = colours [ index ].GreenValue;
					buffer [ j + 3 ] = colours [ index ].BlueValue;
				}
				Marshal.Copy ( buffer, 0, bitmapData.Scan0, buffer.Length );
				bitmap.UnlockBits ( bitmapData );
			}

			bitmap.Save ( stream, GetImageFormat ( type ) );
			bitmap.Dispose ();

			return true;
		}

		private ImageFormat GetImageFormat ( GdiPlusImageEncodingType type )
		{
			switch ( type )
			{
				case GdiPlusImageEncodingType.Png: return ImageFormat.Png;
				case GdiPlusImageEncodingType.Jpeg: return ImageFormat.Jpeg;
				case GdiPlusImageEncodingType.Gif: return ImageFormat.Gif;
				case GdiPlusImageEncodingType.Bmp: return ImageFormat.Bmp;
				case GdiPlusImageEncodingType.Tiff: return ImageFormat.Tiff;
				default: throw new ArgumentException ();
			}
		}
	}
}
