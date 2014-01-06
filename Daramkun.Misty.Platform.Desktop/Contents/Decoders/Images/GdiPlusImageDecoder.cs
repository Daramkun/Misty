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
using Daramkun.Misty.Contents.Decoders;

namespace Daramkun.Misty.Contents.Decoders.Images
{
	[FileFormat ( "bmp", "dib", "gif", "jpeg", "jpg", "png", "tiff", "exif", "wmf", "emf" )]
	public class GdiPlusImageDecoder : IDecoder<ImageInfo>
	{
		public bool Decode ( Stream stream, out ImageInfo to, params object [] args )
		{
			Bitmap bitmap = new Bitmap ( stream );
			to = new ImageInfo ( bitmap.Width, bitmap.Height, bitmap.GetFrameCount ( new FrameDimension ( bitmap.FrameDimensionsList [ 0 ] ) ),
				stream, bitmap, ( ImageInfo imageInfo, object raws, int f, Daramkun.Misty.Graphics.Color? colorKey ) =>
			{
				bitmap.SelectActiveFrame ( new FrameDimension ( bitmap.FrameDimensionsList [ 0 ] ), f );
				BitmapData bitmapData = bitmap.LockBits ( new Rectangle ( 0, 0, bitmap.Width, bitmap.Height ), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );
				byte [] temp = new byte [ bitmap.Width * bitmap.Height * 4 ];
				Marshal.Copy ( bitmapData.Scan0, temp, 0, bitmap.Width * bitmap.Height * 4 );
				bitmap.UnlockBits ( bitmapData );
				bitmap.Dispose ();
				Daramkun.Misty.Graphics.Color [] colours = new Daramkun.Misty.Graphics.Color [ bitmap.Width * bitmap.Height ];
				for ( int i = 0, index = 0; i < temp.Length; i += 4, ++index )
				{
					colours [ index ] = new Graphics.Color ( temp [ i + 1 ], temp [ i + 2 ], temp [ i + 3 ], temp [ i + 0 ] );
				}
				return colours;
			} );
			return true;
		}
	}
}
