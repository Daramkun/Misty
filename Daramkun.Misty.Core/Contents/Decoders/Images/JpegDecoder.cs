using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BitMiracle.LibJpeg;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Decoders.Images
{
	public class JpegDecoder : IDecoder<ImageInfo>
	{
		public bool Decode ( Stream stream, out ImageInfo to, params object [] args )
		{
			try
			{
				JpegImage jpeg = new JpegImage ( stream );
				to = new ImageInfo ( jpeg.Width, jpeg.Height, 1, stream, jpeg, ( ImageInfo imageInfo, object raw, int frame, Color? colorKey ) =>
				{
					
				} );
				return true;
			}
			catch { to = null; return false; }
		}
	}
}
