using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Encoders.Images
{
	[FileFormat ( "png" )]
	public class PngEncoder : IEncoder<ImageInfo>
	{
		public bool Encode ( Stream stream, ImageInfo data, params object [] args )
		{
			try
			{
				Color [] pixels = data.GetPixels ();
				Hjg.Pngcs.ImageInfo imageInfo = new Hjg.Pngcs.ImageInfo ( data.Width, data.Height, 32, true );
				Hjg.Pngcs.PngWriter writer = new Hjg.Pngcs.PngWriter ( stream, imageInfo );
				for ( int y = 0; y < data.Height; ++y )
				{
					int [] row = new int [ data.Width ];
					for ( int x = 0; x < data.Width; ++x )
						row [ x ] = pixels [ ( y * data.Width ) + x ].ColorValue;
					writer.WriteRow ( row, y );
				}
				writer.End ();
			}
			catch { return false; }
			return true;
		}
	}
}
