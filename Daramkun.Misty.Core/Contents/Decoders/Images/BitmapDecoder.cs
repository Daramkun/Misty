using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Decoders.Images
{
	[FileFormat ( "bmp", "dib" )]
	public class BitmapDecoder : IDecoder<ImageInfo>
	{
		public bool Decode ( Stream stream, out ImageInfo to, params object [] args )
		{
			BinaryReader reader = new BinaryReader ( stream );

			BITMAPFILEHEADER fileHeader;
			BITMAPINFOHEADER infoHeader;

			to = new ImageInfo ();
			if ( !LoadBitmapFileHeader ( reader, out fileHeader ) )
				return false;

			int _padding;
			if ( !LoadBitmapInfoHeader ( reader, out infoHeader, out _padding ) )
				return false;

			stream.Seek ( fileHeader.bfOffBits, SeekOrigin.Begin );

			to = new ImageInfo ( infoHeader.biWidth, infoHeader.biHeight, 1, stream,
				new object [] { _padding, infoHeader.biBitCount, reader.ReadBytes ( ( int ) infoHeader.biSizeImage ) },
				( ImageInfo imageInfo, object raw, int frame, Color? colorKey ) =>
				{
					object [] imageDataData = raw as object [];
					int padding = ( int ) imageDataData [ 0 ];
					ushort bpp = ( ushort ) imageDataData [ 1 ];
					byte [] pixels = imageDataData [ 2 ] as byte [];

					Color [] convPixels = new Color [ imageInfo.Width * imageInfo.Height ];
					int index = 0;
					if ( bpp == 8 )
					{
						for ( int i = 0; i < imageInfo.Height; i++ )
						{
							for ( int j = 0; j < imageInfo.Width; j++ )
							{
								index = ( ( imageInfo.Height - i - 1 ) * ( imageInfo.Width + padding ) + j );
								Color color = new Color ( pixels [ index ], pixels [ index ], pixels [ index ] );
								convPixels [ ( i * imageInfo.Width ) + j ] = ( color == colorKey ) ? Color.Transparent : color;
							}
						}
					}
					else if ( bpp == 24 )
					{
						for ( int i = 0; i < imageInfo.Height; i++ )
						{
							for ( int j = 0; j < imageInfo.Width; j++ )
							{
								index = ( ( imageInfo.Height - i - 1 ) * ( imageInfo.Width * 3 + padding ) + j * 3 );
								Color color = new Color ( pixels [ index + 2 ], pixels [ index + 1 ], pixels [ index + 0 ] );
								convPixels [ ( i * imageInfo.Width ) + j ] = ( color == colorKey ) ? Color.Transparent : color;
							}
						}
					}
					else if ( bpp == 32 )
					{
						for ( int i = 0; i < imageInfo.Height; i++ )
						{
							for ( int j = 0; j < imageInfo.Width; j++ )
							{
								index = ( ( imageInfo.Height - i - 1 ) * ( imageInfo.Width * 4 ) + j * 4 );
								Color color = new Color ( pixels [ index + 2 ], pixels [ index + 1 ], pixels [ index + 0 ], pixels [ index + 3 ] );
								convPixels [ ( i * imageInfo.Width ) + j ] = ( color == colorKey ) ? Color.Transparent : color;
							}
						}
					}

					return convPixels;
				} );

			return true;
		}

		[StructLayout ( LayoutKind.Sequential )]
		internal struct BITMAPFILEHEADER
		{
			public ushort bfType;
			public uint bfSize;
			public ushort bfReserved1;
			public ushort bfReserved2;
			public uint bfOffBits;
		}

		[StructLayout ( LayoutKind.Sequential )]
		internal struct BITMAPINFOHEADER
		{
			public uint biSize;
			public int biWidth;
			public int biHeight;
			public ushort biPlanes;
			public ushort biBitCount;
			public uint biCompression;
			public uint biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public uint biClrUsed;
			public uint biClrImportant;
		}

		private bool LoadBitmapFileHeader ( BinaryReader reader, out BITMAPFILEHEADER fileHeader )
		{
			fileHeader = new BITMAPFILEHEADER ();
			fileHeader.bfType = reader.ReadUInt16 ();
			if ( fileHeader.bfType != 0x4D42 )
				return false;
			fileHeader.bfSize = reader.ReadUInt32 ();
			fileHeader.bfReserved1 = reader.ReadUInt16 ();
			fileHeader.bfReserved2 = reader.ReadUInt16 ();
			// 예약된 공간이 0이 아니면 종료
			if ( fileHeader.bfReserved1 != 0 || fileHeader.bfReserved2 != 0 )
				return false;
			fileHeader.bfOffBits = reader.ReadUInt32 ();

			return true;
		}
		private bool LoadBitmapInfoHeader ( BinaryReader reader,
			out BITMAPINFOHEADER infoHeader, out int padding )
		{
			infoHeader = new BITMAPINFOHEADER ();

			padding = 0;

			infoHeader.biSize = reader.ReadUInt32 ();
			// 정보 구조체 크기가 40이 아니면 V3 비트맵이 아니므로 종료
			if ( infoHeader.biSize != 40 )
				return false;

			infoHeader.biWidth = reader.ReadInt32 ();
			infoHeader.biHeight = reader.ReadInt32 ();
			infoHeader.biPlanes = reader.ReadUInt16 ();
			infoHeader.biBitCount = reader.ReadUInt16 ();
			infoHeader.biCompression = reader.ReadUInt32 ();
			infoHeader.biSizeImage = reader.ReadUInt32 ();
			infoHeader.biXPelsPerMeter = reader.ReadInt32 ();
			infoHeader.biYPelsPerMeter = reader.ReadInt32 ();
			infoHeader.biClrUsed = reader.ReadUInt32 ();
			infoHeader.biClrImportant = reader.ReadUInt32 ();

			// 1장이 아니면 BMP가 아니므로 종료
			if ( infoHeader.biPlanes != 1 )
				return false;
			// 24비트 또는 32비트 비트맵이 아니면 종료
			if ( !( infoHeader.biBitCount == 24 || infoHeader.biBitCount == 32 ) )
				return false;
			// 압축된 데이터일 경우 종료
			if ( infoHeader.biCompression != 0 )
				return false;

			int rowWidth = infoHeader.biWidth * 24 / 8;
			padding = 0;
			while ( rowWidth % 4 != 0 )
			{
				rowWidth++;
				padding++;
			}

			if ( infoHeader.biSizeImage == 0 )
				infoHeader.biSizeImage = ( uint ) ( rowWidth * infoHeader.biHeight * ( ( infoHeader.biBitCount == 24 ) ? 3 : 4 ) );

			return true;
		}
	}
}
