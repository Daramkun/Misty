using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	class Texture2D : StandardDispose, ITexture2D
	{
		int texture;

		public int Width { get { return ( int ) Size.X; } }
		public int Height { get { return ( int ) Size.Y; } }

		public object Handle { get { return texture; } }

		public Vector2 Size { get; private set; }

		public Color [] Buffer
		{
			get
			{
				byte [] raws = new byte [ Width * Height * 4 ];
				GL.BindTexture ( TextureTarget.Texture2D, texture );
				GL.GetTexImage ( TextureTarget.Texture2D, 0, PixelFormat.Bgra, PixelType.UnsignedByte, raws );
				Color [] pixels = new Color [ Width * Height ];
				for ( int y = 0; y < Height; ++y )
				{
					for ( int x = 0; x < Width; ++x )
					{
						int index = x + ( ( Height - y - 1 ) * Width );
						int dataIndex = ( x + ( y * Width ) ) * 4;
						byte blue = raws [ dataIndex + 0 ];
						byte green = raws [ dataIndex + 1 ];
						byte red = raws [ dataIndex + 2 ];
						byte alpha = raws [ dataIndex + 3 ];
						pixels [ index ] = new Color ( red, green, blue, alpha );
					}
				}
				return pixels;
			}
			set
			{
				if ( value == null || value.Length == 0 ) return;

				GL.BindTexture ( TextureTarget.Texture2D, texture );

				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ( int ) TextureMagFilter.Nearest );
				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ( int ) TextureMinFilter.Nearest );
				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ( int ) TextureWrapMode.Repeat );
				GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ( int ) TextureWrapMode.Repeat );

				byte [] colorData = new byte [ Width * Height * 4 ];

				for ( int y = 0; y < Height; ++y )
				{
					for ( int x = 0; x < Width; ++x )
					{
						int index = x + ( ( Height - y - 1 ) * Width );
						int dataIndex = ( x + ( y * Width ) ) * 4;
						colorData [ dataIndex + 0 ] = value [ index ].BlueValue;
						colorData [ dataIndex + 1 ] = value [ index ].GreenValue;
						colorData [ dataIndex + 2 ] = value [ index ].RedValue;
						colorData [ dataIndex + 3 ] = value [ index ].AlphaValue;
					}
				}
				GL.TexImage2D<byte> ( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8,
					Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
					PixelType.UnsignedByte, colorData );

				GL.BindTexture ( TextureTarget.Texture2D, 0 );
			}
		}

		private void MakeTexture ( int width, int height )
		{
			if ( width == 0 ) width = 1;
			if ( height == 0 ) height = 1;
			Size = new Vector2 ( width, height );
			texture = GL.GenTexture ();
			GL.BindTexture ( TextureTarget.Texture2D, texture );
			GL.TexImage2D ( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero );
			GL.BindTexture ( TextureTarget.Texture2D, 0 );
		}

		private void GenerateMipmap ( int width, int height, int mipmapLevel )
		{
			GL.BindTexture ( TextureTarget.Texture2D, texture );
			GL.TexParameter ( TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, mipmapLevel );
			GL.GenerateMipmap ( GenerateMipmapTarget.Texture2D );
			GL.BindTexture ( TextureTarget.Texture2D, 0 );
		}

		public Texture2D ( IGraphicsDevice graphicsDevice, int width, int height, int mipmapLevel = 1 )
		{
			MakeTexture ( width, height );
			GenerateMipmap ( width, height, mipmapLevel );
		}

		public Texture2D ( IGraphicsDevice graphicsDevice, ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{
			MakeTexture ( imageInfo.Width, imageInfo.Height );
			Buffer = imageInfo.GetPixels ( colorKey );
			GenerateMipmap ( imageInfo.Width, imageInfo.Height, mipmapLevel );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				if ( texture == 0 )
					return;
				GL.DeleteTexture ( texture );
				texture = 0;
			}
			base.Dispose ( isDisposing );
		}
	}
}
