using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents;

namespace Daramkun.Misty.Graphics.Spirit.Fonts
{
	public class TrueTypeFont : Font
	{
		TrueTypeSharp.TrueTypeFont trueType;
		Dictionary<char, ITexture2D> readedImage = new Dictionary<char, ITexture2D> ();
		List<char> noneList = new List<char> ();

		float fontSize;
		int fontSizeOfPixel;

		public TrueTypeFont ( Stream trueTypeFontStream, int fontSizeOfPixel )
		{
			trueType = new TrueTypeSharp.TrueTypeFont ( trueTypeFontStream );
			this.fontSizeOfPixel = fontSizeOfPixel;
			fontSize = trueType.GetScaleForPixelHeight ( fontSizeOfPixel - fontSizeOfPixel / 4 );
		}

		protected override void Dispose ( bool isDisposing )
		{
			base.Dispose ( isDisposing );
		}

		protected override ITexture2D this [ char ch ]
		{
			get
			{
				if ( noneList.Contains ( ch ) ) return null;
				if ( readedImage.ContainsKey ( ch ) ) return readedImage [ ch ];

				if ( ch == ' ' )
				{
					Color [] buffer = new Color [ fontSizeOfPixel * ( fontSizeOfPixel / 3 ) ];
					ImageInfo imageInfo = new ImageInfo ( fontSizeOfPixel / 3, fontSizeOfPixel, 1, null, buffer, ( ImageInfo i, object rawData, int ii, Color? colorKey ) =>
					{
						return rawData as Color [];
					} );
					ITexture2D texture = Core.GraphicsDevice.CreateTexture2D ( imageInfo );
					readedImage.Add ( ch, texture );
					return texture;
				}
				else
				{
					int width, height, xOffset, yOffset;
					byte [] data = trueType.GetCodepointBitmap ( ch, fontSize, fontSize, out width, out height, out xOffset, out yOffset );

					if ( data == null ) { noneList.Add ( ch ); return null; }

					int extendedHeight = Math.Abs ( yOffset ) * ( width );
					Color [] buffer = new Color [ ( width ) * fontSizeOfPixel ];
					for ( int x = 0; x < width; x++ )
					{
						for ( int y = 0; y < height; y++ )
						{
							int dataIndex = ( y * width ) + x;
							int index = ( ( y + ( fontSizeOfPixel / 3 * 2 ) + yOffset ) * ( width ) ) + x;
							if ( index >= buffer.Length || index < 0 ) continue;
							buffer [ index ] = data [ dataIndex ] > 0 ? new Color ( 255, 255, 255, data [ dataIndex ] ) : new Color ( 0, 0, 0, 0 );
						}
					}

					ImageInfo imageInfo = new ImageInfo ( width, fontSizeOfPixel, 1, null, buffer, ( ImageInfo i, object rawData, int ii, Color? colorKey ) =>
					{
						return rawData as Color [];
					} );
					ITexture2D texture = Core.GraphicsDevice.CreateTexture2D ( imageInfo );
					readedImage.Add ( ch, texture );
					return texture;
				}
			}
		}
	}
}
