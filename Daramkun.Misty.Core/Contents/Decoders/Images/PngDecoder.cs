using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Decoders.Images
{
	[FileFormat ( "png" )]
	public class PngDecoder : IDecoder<ImageInfo>
	{
		public bool Decode ( Stream stream, out ImageInfo to, params object [] args )
		{
			try
			{
				Hjg.Pngcs.PngReader pngReader = new Hjg.Pngcs.PngReader ( stream );
				Hjg.Pngcs.ImageInfo imgInfo = pngReader.ImgInfo;

				to = new ImageInfo ( imgInfo.Cols, imgInfo.Rows, 1, stream, pngReader,
					( ImageInfo imageInfo, object raw, int frame, Color? colorKey ) =>
					{
						Hjg.Pngcs.PngReader reader = raw as Hjg.Pngcs.PngReader;
						MemoryStream s = new MemoryStream ();
						for ( int i = 0; i < imageInfo.Height; ++i )
						{
							Hjg.Pngcs.ImageLine column = reader.ReadRowByte ( i );
							byte [] data = column.GetScanlineByte ();
							s.Write ( data, 0, data.Length );
						}

						byte [] pixels = s.ToArray ();
						Color [] colors = new Color [ imageInfo.Width * imageInfo.Height ];
						for ( int i = 0, index = 0; i < pixels.Length; i += reader.ImgInfo.BytesPixel, ++index )
						{
							if ( reader.ImgInfo.BytesPixel == 3 )
								colors [ index ] = new Color ( pixels [ i + 0 ], pixels [ i + 1 ], pixels [ i + 2 ] );
							else if ( reader.ImgInfo.BytesPixel == 1 )
								colors [ index ] = new Color ( pixels [ i ], pixels [ i ], pixels [ i ], 255 );
							else
								colors [ index ] = new Color ( pixels [ i + 0 ], pixels [ i + 1 ], pixels [ i + 2 ], pixels [ i + 3 ] );
						}
						return colors;
					} );
			}
			catch { to = new ImageInfo (); return false; }

			return true;
		}
	}
}
