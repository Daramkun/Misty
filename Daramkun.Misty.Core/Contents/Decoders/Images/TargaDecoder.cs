using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Decoders.Images
{
	[FileFormat ( "tga", "tpic" )]
	public class TargaDecoder : IDecoder<ImageInfo>
	{
		struct TargaData
		{
			public BinaryReader reader;
			public byte idFieldLength;
			public byte colorMap;
			public byte imageType;
			public UInt16 colorMapOffset;
			public UInt16 colorsUsed;
			public byte bitsPerColorMap;
			public UInt16 xCoord;
			public UInt16 yCoord;
			public UInt16 imgWidth;
			public UInt16 imgHeight;
			public byte bitsPerPixel;
			public byte imgFlags;
		}

		public bool Decode ( Stream stream, out ImageInfo to, params object [] args )
		{
			TargaData targaData;
			try
			{
				if ( !LoadHeader ( stream, out targaData ) )
				{
					to = null;
					return false;
				}
				else
					to = new ImageInfo ( targaData.imgWidth, targaData.imgHeight, 1, stream, targaData, ( imageInfo, rawData, frame, colorKey ) =>
					{
						return LoadData ( stream, targaData );
					} );
			}
			catch { to = null; return false; }
			return true;
		}

		static bool LoadHeader ( Stream stream, out TargaData targaData )
		{
			targaData.reader = new BinaryReader ( stream );

			targaData.idFieldLength = ( byte ) stream.ReadByte ();
			targaData.colorMap = ( byte ) stream.ReadByte ();
			targaData.imageType = ( byte ) stream.ReadByte ();
			targaData.colorMapOffset = LittleEndian ( targaData.reader.ReadUInt16 () );
			targaData.colorsUsed = LittleEndian ( targaData.reader.ReadUInt16 () );
			targaData.bitsPerColorMap = ( byte ) stream.ReadByte ();
			targaData.xCoord = LittleEndian ( targaData.reader.ReadUInt16 () );
			targaData.yCoord = LittleEndian ( targaData.reader.ReadUInt16 () );
			targaData.imgWidth = LittleEndian ( targaData.reader.ReadUInt16 () );
			targaData.imgHeight = LittleEndian ( targaData.reader.ReadUInt16 () );
			targaData.bitsPerPixel = ( byte ) stream.ReadByte ();
			targaData.imgFlags = ( byte ) stream.ReadByte ();

			if ( targaData.colorMap > 1 )
				return false;

			if ( targaData.idFieldLength > 0 )
			{
				byte [] idBytes = new byte [ targaData.idFieldLength ];
				stream.Read ( idBytes, 0, targaData.idFieldLength );
				string idStr = System.Text.Encoding.UTF8.GetString ( idBytes, 0, idBytes.Length );

				//do something with the ID string...
				System.Diagnostics.Debug.WriteLine ( "Targa image ID: " + idStr );
			}

			return true;
		}

		private Color [] LoadData ( Stream stream, TargaData targaData )
		{
			BinaryReader reader = new BinaryReader ( stream );

			UInt32 [] palette = null;
			byte [] scanline = null;

			//image types:
			//0 - No Image Data Available
			//1 - Uncompressed Color Image
			//2 - Uncompressed RGB Image
			//3 - Uncompressed Black & White Image
			//9 - Compressed Color Image
			//10 - Compressed RGB Image
			//11 - Compressed Black & White Image

			if ( ( targaData.imageType > 11 ) || ( ( targaData.imageType > 3 ) && ( targaData.imageType < 9 ) ) )
			{
				throw new Exception ( "This image type (" + targaData.imageType + ") is not supported." );
			}
			else if ( targaData.bitsPerPixel != 8 && targaData.bitsPerPixel != 15 && targaData.bitsPerPixel != 16 && targaData.bitsPerPixel != 24 && targaData.bitsPerPixel != 32 )
			{
				throw new Exception ( "Number of bits per pixel (" + targaData.bitsPerPixel + ") is not supported." );
			}
			if ( targaData.colorMap > 0 )
			{
				if ( targaData.bitsPerColorMap != 15 && targaData.bitsPerColorMap != 16 && targaData.bitsPerColorMap != 24 && targaData.bitsPerColorMap != 32 )
				{
					throw new Exception ( "Number of bits per color map (" + targaData.bitsPerPixel + ") is not supported." );
				}
			}

			Color [] bmpData = new Color [ targaData.imgWidth * targaData.imgHeight ];

			try
			{
				if ( targaData.colorMap > 0 )
				{
					int paletteEntries = targaData.colorMapOffset + targaData.colorsUsed;
					palette = new UInt32 [ paletteEntries ];

					if ( targaData.bitsPerColorMap == 24 )
					{
						for ( int i = targaData.colorMapOffset; i < paletteEntries; i++ )
						{
							palette [ i ] = 0xFF000000;
							palette [ i ] |= ( UInt32 ) ( stream.ReadByte () << 16 );
							palette [ i ] |= ( UInt32 ) ( stream.ReadByte () << 8 );
							palette [ i ] |= ( UInt32 ) ( stream.ReadByte () );
						}
					}
					else if ( targaData.bitsPerColorMap == 32 )
					{
						for ( int i = targaData.colorMapOffset; i < paletteEntries; i++ )
						{
							palette [ i ] = 0xFF000000;
							palette [ i ] |= ( UInt32 ) ( stream.ReadByte () << 16 );
							palette [ i ] |= ( UInt32 ) ( stream.ReadByte () << 8 );
							palette [ i ] |= ( UInt32 ) ( stream.ReadByte () );
							palette [ i ] |= ( UInt32 ) ( stream.ReadByte () << 24 );
						}
					}
					else if ( ( targaData.bitsPerColorMap == 15 ) || ( targaData.bitsPerColorMap == 16 ) )
					{
						int hi, lo;
						for ( int i = targaData.colorMapOffset; i < paletteEntries; i++ )
						{
							hi = stream.ReadByte ();
							lo = stream.ReadByte ();
							palette [ i ] = 0xFF000000;
							palette [ i ] |= ( UInt32 ) ( ( hi & 0x1F ) << 3 ) << 16;
							palette [ i ] |= ( UInt32 ) ( ( ( ( lo & 0x3 ) << 3 ) + ( ( hi & 0xE0 ) >> 5 ) ) << 3 ) << 8;
							palette [ i ] |= ( UInt32 ) ( ( ( lo & 0x7F ) >> 2 ) << 3 );
						}
					}
				}

				if ( targaData.imageType == 1 || targaData.imageType == 2 || targaData.imageType == 3 )
				{
					scanline = new byte [ targaData.imgWidth * ( targaData.bitsPerPixel / 8 ) ];
					for ( int y = targaData.imgHeight - 1; y >= 0; y-- )
					{
						switch ( targaData.bitsPerPixel )
						{
							case 8:
								stream.Read ( scanline, 0, scanline.Length );
								if ( targaData.imageType == 1 )
								{
									for ( int x = 0; x < targaData.imgWidth; x++ )
									{
										bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( ( byte ) ( ( palette [ scanline [ x ] ] ) & 0XFF ),
											( byte ) ( ( palette [ scanline [ x ] ] >> 8 ) & 0XFF ), ( byte ) ( ( palette [ scanline [ x ] ] >> 16 ) & 0XFF ), 0xFF );
									}
								}
								else if ( targaData.imageType == 3 )
								{
									for ( int x = 0; x < targaData.imgWidth; x++ )
									{
										bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( scanline [ x ], scanline [ x ], scanline [ x ], 0xFF );
									}
								}
								break;
							case 15:
							case 16:
								int hi, lo;
								for ( int x = 0; x < targaData.imgWidth; x++ )
								{
									hi = stream.ReadByte ();
									lo = stream.ReadByte ();

									bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( ( byte ) ( ( ( lo & 0x7F ) >> 2 ) << 3 ),
										( byte ) ( ( ( ( lo & 0x3 ) << 3 ) + ( ( hi & 0xE0 ) >> 5 ) ) << 3 ), ( byte ) ( ( hi & 0x1F ) << 3 ), 0xFF );
								}
								break;
							case 24:
								stream.Read ( scanline, 0, scanline.Length );
								for ( int x = 0; x < targaData.imgWidth; x++ )
								{
									bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( scanline [ x * 3 + 2 ], scanline [ x * 3 + 1 ], scanline [ x * 3 ], 0xFF );
								}
								break;
							case 32:
								stream.Read ( scanline, 0, scanline.Length );
								for ( int x = 0; x < targaData.imgWidth; x++ )
								{
									bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( scanline [ x * 4 + 2 ], scanline [ x * 4 + 1 ], scanline [ x * 4 ], scanline [ x * 4 + 3 ] );
								}
								break;
						}
					}

				}
				else if ( targaData.imageType == 9 || targaData.imageType == 10 || targaData.imageType == 11 )
				{
					int y = targaData.imgHeight - 1, x = 0, i;
					int bytesPerPixel = targaData.bitsPerPixel / 8;
					scanline = new byte [ targaData.imgWidth * 4 ];

					while ( y >= 0 && stream.Position < stream.Length )
					{
						i = stream.ReadByte ();
						if ( i < 128 )
						{
							i++;
							switch ( targaData.bitsPerPixel )
							{
								case 8:
									stream.Read ( scanline, 0, i * bytesPerPixel );
									if ( targaData.imageType == 9 )
									{
										for ( int j = 0; j < i; j++ )
										{
											bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( ( byte ) ( ( palette [ scanline [ j ] ] ) & 0XFF ),
												( byte ) ( ( palette [ scanline [ j ] ] >> 8 ) & 0XFF ), ( byte ) ( ( palette [ scanline [ j ] ] >> 16 ) & 0XFF ), 0xFF );
											x++;
											if ( x >= targaData.imgWidth ) { x = 0; y--; }
										}
									}
									else if ( targaData.imageType == 11 )
									{
										for ( int j = 0; j < i; j++ )
										{
											bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( scanline [ j ], scanline [ j ], scanline [ j ], 0xFF );
											x++;
											if ( x >= targaData.imgWidth ) { x = 0; y--; }
										}
									}
									break;
								case 15:
								case 16:
									int hi, lo;
									for ( int j = 0; j < i; j++ )
									{
										hi = stream.ReadByte ();
										lo = stream.ReadByte ();

										bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( ( byte ) ( ( ( lo & 0x7F ) >> 2 ) << 3 ),
											( byte ) ( ( ( ( lo & 0x3 ) << 3 ) + ( ( hi & 0xE0 ) >> 5 ) ) << 3 ), ( byte ) ( ( hi & 0x1F ) << 3 ), 0xFF );
										x++;
										if ( x >= targaData.imgWidth ) { x = 0; y--; }
									}
									break;
								case 24:
									stream.Read ( scanline, 0, i * bytesPerPixel );
									for ( int j = 0; j < i; j++ )
									{
										bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( scanline [ j * 3 + 2 ], scanline [ j * 3 + 1 ], scanline [ j * 3 ], 0xFF );
										x++;
										if ( x >= targaData.imgWidth ) { x = 0; y--; }
									}
									break;
								case 32:
									stream.Read ( scanline, 0, i * bytesPerPixel );
									for ( int j = 0; j < i; j++ )
									{
										bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( scanline [ j * 4 + 2 ], scanline [ j * 4 + 1 ], scanline [ j * 4 ], scanline [ j * 4 + 3 ] );
										x++;
										if ( x >= targaData.imgWidth ) { x = 0; y--; }
									}
									break;
							}
						}
						else
						{
							i = ( i & 0x7F ) + 1;
							int r, g, b, a;

							switch ( targaData.bitsPerPixel )
							{
								case 8:
									int p = stream.ReadByte ();
									if ( targaData.imageType == 9 )
									{
										for ( int j = 0; j < i; j++ )
										{
											bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( ( byte ) ( ( palette [ p ] ) & 0XFF ),
												( byte ) ( ( palette [ p ] >> 8 ) & 0XFF ), ( byte ) ( ( palette [ p ] >> 16 ) & 0XFF ), 0xFF );
											x++;
											if ( x >= targaData.imgWidth ) { x = 0; y--; }
										}
									}
									else if ( targaData.imageType == 11 )
									{
										for ( int j = 0; j < i; j++ )
										{
											bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( ( byte ) p, ( byte ) p, ( byte ) p, 0xFF );
											x++;
											if ( x >= targaData.imgWidth ) { x = 0; y--; }
										}
									}
									break;
								case 15:
								case 16:
									int hi = stream.ReadByte ();
									int lo = stream.ReadByte ();
									for ( int j = 0; j < i; j++ )
									{
										bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( ( byte ) ( ( ( lo & 0x7F ) >> 2 ) << 3 ),
											( byte ) ( ( ( ( lo & 0x3 ) << 3 ) + ( ( hi & 0xE0 ) >> 5 ) ) << 3 ), ( byte ) ( ( hi & 0x1F ) << 3 ), 0xFF );
										x++;
										if ( x >= targaData.imgWidth ) { x = 0; y--; }
									}
									break;
								case 24:
									b = stream.ReadByte ();
									g = stream.ReadByte ();
									r = stream.ReadByte ();
									for ( int j = 0; j < i; j++ )
									{
										bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( ( byte ) r, ( byte ) g, ( byte ) b, 0xFF );
										x++;
										if ( x >= targaData.imgWidth ) { x = 0; y--; }
									}
									break;
								case 32:
									b = stream.ReadByte ();
									g = stream.ReadByte ();
									r = stream.ReadByte ();
									a = stream.ReadByte ();
									for ( int j = 0; j < i; j++ )
									{
										bmpData [ ( y * targaData.imgWidth + x ) ] = new Color ( ( byte ) r, ( byte ) g, ( byte ) b, ( byte ) a );
										x++;
										if ( x >= targaData.imgWidth ) { x = 0; y--; }
									}
									break;
							}
						}

					}
				}

			}
			catch ( Exception e )
			{
				//give a partial image in case of unexpected end-of-file

				System.Diagnostics.Debug.WriteLine ( "Error while processing TGA file: " + e.Message );
			}

			return bmpData;
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
