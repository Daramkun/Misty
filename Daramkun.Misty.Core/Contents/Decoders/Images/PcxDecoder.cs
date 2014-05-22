using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Decoders.Images
{
	[FileFormat ( "pcx" )]
	public class PcxDecoder : IDecoder<ImageInfo>
	{
		struct PcxHeader
		{
			public BinaryReader reader;
			public int imgWidth;
			public int imgHeight;
			public int imgBpp;
			public UInt16 xmin, ymin, xmax, ymax;
		}

		public bool Decode ( Stream stream, out ImageInfo to, params object [] args )
		{
			PcxHeader pcxData;
			try
			{
				if ( !LoadHeader ( stream, out pcxData ) )
				{
					to = null;
					return false;
				}
				else
					to = new ImageInfo ( pcxData.imgWidth, pcxData.imgHeight, 1, stream, pcxData, ( imageInfo, rawData, frame, colorKey ) =>
					{
						return LoadData ( stream, pcxData );
					} );
			}
			catch { to = null; return false; }
			return true;
		}

		private static bool LoadHeader ( Stream stream, out PcxHeader header )
		{
			header.reader = new BinaryReader ( stream );

			byte tempByte = ( byte ) stream.ReadByte ();
			if ( tempByte != 10 )
				throw new Exception ( "This is not a valid PCX file." );

			tempByte = ( byte ) stream.ReadByte ();
			if ( tempByte != 5 )
				throw new Exception ( "Only Version-5 PCX files are supported." );

			tempByte = ( byte ) stream.ReadByte ();
			if ( tempByte != 1 )
				throw new Exception ( "Invalid PCX compression type." );

			header.imgBpp = stream.ReadByte ();
			if ( header.imgBpp != 8 && header.imgBpp != 4 && header.imgBpp != 2 && header.imgBpp != 1 )
				throw new Exception ( "Only 8, 4, 2, and 1-bit PCX samples are supported." );

			header.xmin = LittleEndian ( header.reader.ReadUInt16 () );
			header.ymin = LittleEndian ( header.reader.ReadUInt16 () );
			header.xmax = LittleEndian ( header.reader.ReadUInt16 () );
			header.ymax = LittleEndian ( header.reader.ReadUInt16 () );

			header.imgWidth = header.xmax - header.xmin + 1;
			header.imgHeight = header.ymax - header.ymin + 1;

			if ( ( header.imgWidth < 1 ) || ( header.imgHeight < 1 ) || ( header.imgWidth > 32767 ) || ( header.imgHeight > 32767 ) )
				throw new Exception ( "This PCX file appears to have invalid dimensions." );

			LittleEndian ( header.reader.ReadUInt16 () ); //hdpi
			LittleEndian ( header.reader.ReadUInt16 () ); //vdpi

			return true;
		}

		private static Color [] LoadData(Stream stream, PcxHeader header)
		{
			byte [] colorPalette = new byte [ 48 ];
			stream.Read ( colorPalette, 0, 48 );

			stream.ReadByte ();

			int numPlanes = stream.ReadByte ();
			int bytesPerLine = LittleEndian ( header.reader.ReadUInt16 () );
			if ( bytesPerLine == 0 ) bytesPerLine = header.xmax - header.xmin + 1;

			if ( header.imgBpp == 8 && numPlanes == 1 )
			{
				colorPalette = new byte [ 768 ];
				stream.Seek ( -768, SeekOrigin.End );
				stream.Read ( colorPalette, 0, 768 );
			}

			//fix color palette if it's a 1-bit image, and there's no palette information
			if ( header.imgBpp == 1 )
			{
				if ( ( colorPalette [ 0 ] == colorPalette [ 3 ] ) && ( colorPalette [ 1 ] == colorPalette [ 4 ] ) && ( colorPalette [ 2 ] == colorPalette [ 5 ] ) )
				{
					colorPalette [ 0 ] = colorPalette [ 1 ] = colorPalette [ 2 ] = 0;
					colorPalette [ 3 ] = colorPalette [ 4 ] = colorPalette [ 5 ] = 0xFF;
				}
			}

			Color [] bmpData = new Color [ ( header.imgWidth + 1 ) * header.imgHeight ];
			stream.Seek ( 128, SeekOrigin.Begin );
			int x = 0, y = 0, i, j;

			RleReader rleReader = new RleReader ( stream );

			try
			{
				if ( header.imgBpp == 1 )
				{
					int b, p;
					byte val;
					byte [] scanline = new byte [ bytesPerLine ];
					byte [] realscanline = new byte [ bytesPerLine * 8 ];

					for ( y = 0; y < header.imgHeight; y++ )
					{
						//add together all the planes...
						Array.Clear ( realscanline, 0, realscanline.Length );
						for ( p = 0; p < numPlanes; p++ )
						{
							x = 0;
							for ( i = 0; i < bytesPerLine; i++ )
							{
								scanline [ i ] = ( byte ) rleReader.ReadByte ();

								for ( b = 7; b >= 0; b-- )
								{
									if ( ( scanline [ i ] & ( 1 << b ) ) != 0 ) val = 1; else val = 0;
									realscanline [ x ] |= ( byte ) ( val << p );
									x++;
								}
							}
						}

						for ( x = 0; x < header.imgWidth; x++ )
						{
							i = realscanline [ x ];
							bmpData [ ( y * header.imgWidth + x ) ] = new Color ( colorPalette [ i * 3 ], colorPalette [ i * 3 + 1 ], colorPalette [ i * 3 + 2 ] );
						}
					}
				}
				else
				{
					if ( numPlanes == 1 )
					{
						if ( header.imgBpp == 8 )
						{
							byte [] scanline = new byte [ bytesPerLine ];
							for ( y = 0; y < header.imgHeight; y++ )
							{
								for ( i = 0; i < bytesPerLine; i++ )
									scanline [ i ] = ( byte ) rleReader.ReadByte ();

								for ( x = 0; x < header.imgWidth; x++ )
								{
									i = scanline [ x ];
									bmpData [ ( y * header.imgWidth + x ) ] = new Color ( colorPalette [ i * 3 ], colorPalette [ i * 3 + 1 ], colorPalette [ i * 3 + 2 ] );
								}
							}
						}
						else if ( header.imgBpp == 4 )
						{
							byte [] scanline = new byte [ bytesPerLine ];
							for ( y = 0; y < header.imgHeight; y++ )
							{
								for ( i = 0; i < bytesPerLine; i++ )
									scanline [ i ] = ( byte ) rleReader.ReadByte ();

								for ( x = 0; x < header.imgWidth; x++ )
								{
									i = scanline [ x / 2 ];
									bmpData [ ( y * header.imgWidth + x ) ] = new Color ( colorPalette [ ( ( i >> 4 ) & 0xF ) * 3 ],
										colorPalette [ ( ( i >> 4 ) & 0xF ) * 3 + 1 ], colorPalette [ ( ( i >> 4 ) & 0xF ) * 3 + 2 ] );
									x++;
									bmpData [ ( y * header.imgWidth + x ) ] = new Color ( colorPalette [ ( i & 0xF ) * 3 ],
										colorPalette [ ( i & 0xF ) * 3 + 1 ], colorPalette [ ( i & 0xF ) * 3 + 2 ] );
								}
							}
						}
						else if ( header.imgBpp == 2 )
						{
							byte [] scanline = new byte [ bytesPerLine ];
							for ( y = 0; y < header.imgHeight; y++ )
							{
								for ( i = 0; i < bytesPerLine; i++ )
									scanline [ i ] = ( byte ) rleReader.ReadByte ();

								for ( x = 0; x < header.imgWidth; x++ )
								{
									i = scanline [ x / 4 ];
									bmpData [ ( y * header.imgWidth + x ) ] = new Color ( colorPalette [ ( ( i >> 6 ) & 0x3 ) * 3 ], 
										colorPalette [ ( ( i >> 6 ) & 0x3 ) * 3 + 1 ], colorPalette [ ( ( i >> 6 ) & 0x3 ) * 3 + 2 ] );
									x++;
									bmpData [ ( y * header.imgWidth + x ) ] = new Color ( colorPalette [ ( ( i >> 4 ) & 0x3 ) * 3 ],
										colorPalette [ ( ( i >> 4 ) & 0x3 ) * 3 + 1 ], colorPalette [ ( ( i >> 4 ) & 0x3 ) * 3 + 2 ] );
									x++;
									bmpData [ ( y * header.imgWidth + x ) ] = new Color ( colorPalette [ ( ( i >> 2 ) & 0x3 ) * 3 ],
										colorPalette [ ( ( i >> 2 ) & 0x3 ) * 3 + 1 ], colorPalette [ ( ( i >> 2 ) & 0x3 ) * 3 + 2 ] );
									x++;
									bmpData [ ( y * header.imgWidth + x ) ] = new Color ( colorPalette [ ( i & 0x3 ) * 3 ],
										colorPalette [ ( i & 0x3 ) * 3 + 1 ], colorPalette [ ( i & 0x3 ) * 3 + 2 ] );
								}
							}
						}
					}
					else if ( numPlanes == 3 )
					{
						byte [] scanlineR = new byte [ bytesPerLine ];
						byte [] scanlineG = new byte [ bytesPerLine ];
						byte [] scanlineB = new byte [ bytesPerLine ];
						int bytePtr = 0;

						for ( y = 0; y < header.imgHeight; y++ )
						{
							for ( i = 0; i < bytesPerLine; i++ )
								scanlineR [ i ] = ( byte ) rleReader.ReadByte ();
							for ( i = 0; i < bytesPerLine; i++ )
								scanlineG [ i ] = ( byte ) rleReader.ReadByte ();
							for ( i = 0; i < bytesPerLine; i++ )
								scanlineB [ i ] = ( byte ) rleReader.ReadByte ();

							for ( int n = 0; n < header.imgWidth; n++ )
							{
								bmpData [ bytePtr ] = new Color ( scanlineR [ n ], scanlineG [ n ], scanlineB [ n ] );
								bytePtr++;
							}
						}
					}

				}//bpp

			}
			catch ( Exception e )
			{
				//give a partial image in case of unexpected end-of-file

				System.Diagnostics.Debug.WriteLine ( "Error while processing PCX file: " + e.Message );
			}

			return bmpData;
		}

		private class RleReader
		{
			private int currentByte = 0;
			private int runLength = 0, runIndex = 0;
			private Stream stream;

			public RleReader ( Stream stream )
			{
				this.stream = stream;
			}

			public int ReadByte ()
			{
				if ( runLength > 0 )
				{
					runIndex++;
					if ( runIndex == ( runLength - 1 ) )
						runLength = 0;
				}
				else
				{
					currentByte = stream.ReadByte ();
					if ( currentByte > 191 )
					{
						runLength = currentByte - 192;
						currentByte = stream.ReadByte ();
						if ( runLength == 1 )
							runLength = 0;
						runIndex = 0;
					}
				}
				return currentByte;
			}
		}

		private static UInt16 LittleEndian ( UInt16 val )
		{
			if ( BitConverter.IsLittleEndian ) return val;
			return conv_endian ( val );
		}
		private static UInt32 LittleEndian ( UInt32 val )
		{
			if ( BitConverter.IsLittleEndian ) return val;
			return conv_endian ( val );
		}

		private static UInt16 conv_endian ( UInt16 val )
		{
			UInt16 temp;
			temp = ( UInt16 ) ( val << 8 ); temp &= 0xFF00; temp |= ( UInt16 ) ( ( val >> 8 ) & 0xFF );
			return temp;
		}
		private static UInt32 conv_endian ( UInt32 val )
		{
			UInt32 temp = ( val & 0x000000FF ) << 24;
			temp |= ( val & 0x0000FF00 ) << 8;
			temp |= ( val & 0x00FF0000 ) >> 8;
			temp |= ( val & 0xFF000000 ) >> 24;
			return ( temp );
		}
	}
}
