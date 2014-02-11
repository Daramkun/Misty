using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;
using Hjg.Pngcs.Chunks;

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
						int [] rgb = new int [ 3 ];
						for ( int i = 0, index = 0; i < pixels.Length; i += reader.ImgInfo.BytesPixel, ++index )
						{
							Color color = new Color ();
							if ( reader.ImgInfo.BytesPixel == 4 )
							{
								if ( imgInfo.Indexed )
								{
									PngChunkPLTE pallete = pngReader.GetChunksList ().GetById ( "PLTE" ) [ 0 ] as PngChunkPLTE;
									pallete.GetEntryRgb ( pixels [ i ], rgb );
									color = new Color ( rgb [ 0 ] / 255.0f, rgb [ 1 ] / 255.0f, rgb [ 2 ] / 255.0f, 1.0f );
								}
								else
									color = new Color ( pixels [ i + 0 ], pixels [ i + 1 ], pixels [ i + 2 ], pixels [ i + 3 ] );
							}
							else if ( reader.ImgInfo.BytesPixel == 3 )
							{
								if ( imgInfo.Indexed )
								{
									PngChunkPLTE pallete = pngReader.GetChunksList ().GetById ( "PLTE" ) [ 0 ] as PngChunkPLTE;
									pallete.GetEntryRgb ( pixels [ i ], rgb );
									color = new Color ( rgb [ 0 ] / 255.0f, rgb [ 1 ] / 255.0f, rgb [ 2 ] / 255.0f, 1.0f );
								}
								else
									color = new Color ( pixels [ i + 0 ], pixels [ i + 1 ], pixels [ i + 2 ] );
							}
							else if ( reader.ImgInfo.BytesPixel == 1 )
							{
								if ( imgInfo.Greyscale )
									color = new Color ( pixels [ i ], pixels [ i ], pixels [ i ], 255 );
								else if ( imgInfo.Indexed )
								{
									PngChunkPLTE pallete = pngReader.GetChunksList ().GetById ( "PLTE" ) [ 0 ] as PngChunkPLTE;
									pallete.GetEntryRgb ( pixels [ i ], rgb );
									color = new Color ( rgb [ 0 ] / 255.0f, rgb [ 1 ] / 255.0f, rgb [ 2 ] / 255.0f, 1.0f );
								}
							}
							colors [ index ] = ( color == colorKey ) ? Color.Transparent : color;
						}
						return colors;
					} );
			}
			catch { to = null; return false; }

			return true;
		}
	}
}
