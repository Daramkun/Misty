using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BitMiracle.LibJpeg;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Decoders.Images
{
	[FileFormat ( "jpg", "jpeg" )]
	public class JpegDecoder : IDecoder<ImageInfo>
	{
		public bool Decode ( Stream stream, out ImageInfo to, params object [] args )
		{
			try
			{
				JpegImage jpeg = new JpegImage ( stream );
				to = new ImageInfo ( jpeg.Width, jpeg.Height, 1, stream, jpeg, ( ImageInfo imageInfo, object raw, int frame, Color? colorKey ) =>
				{
					JpegImage jpegImage = raw as JpegImage;
					Color [] data = new Color [ imageInfo.Width * imageInfo.Height ];
					int index = 0;

					for ( int i = 0; i < imageInfo.Height; ++i )
					{
						byte [] row = jpegImage.GetRow ( i ).ToBytes ();
						for ( int j = 0; j < row.Length; j += 3 )
						{
							data [ index ] = new Color ( row [ j + 0 ], row [ j + 1 ], row [ j + 2 ] );
							++index;
						}
					}

					return data;
				} );
				return true;
			}
			catch { to = null; return false; }
		}
	}
}
