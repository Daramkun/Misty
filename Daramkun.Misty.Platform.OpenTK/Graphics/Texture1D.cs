using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	public class Texture1D : StandardDispose, ITexture1D
	{
		int texture;

		public int Width { get { return ( int ) Size; } }
		public float Size { get; private set; }

		public Color [] Buffer
		{
			get
			{
				byte [] raws = new byte [ Width * 4 ];
				GL.BindTexture ( TextureTarget.Texture2D, texture );
				GL.GetTexImage ( TextureTarget.Texture2D, 0, PixelFormat.Bgra, PixelType.UnsignedByte, raws );
				Color [] pixels = new Color [ Width ];
				for ( int x = 0; x < Width; ++x )
				{
					int index = x;
					int dataIndex = x * 4;
					byte blue = raws [ dataIndex + 0 ];
					byte green = raws [ dataIndex + 1 ];
					byte red = raws [ dataIndex + 2 ];
					byte alpha = raws [ dataIndex + 3 ];
					pixels [ index ] = new Color ( red, green, blue, alpha );
				}
				return pixels;
			}
			set
			{
				if ( value == null || value.Length == 0 ) return;

				GL.BindTexture ( TextureTarget.Texture1D, texture );

				GL.TexParameter ( TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, ( int ) TextureMagFilter.Nearest );
				GL.TexParameter ( TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, ( int ) TextureMinFilter.Nearest );
				GL.TexParameter ( TextureTarget.Texture1D, TextureParameterName.TextureWrapS, ( int ) TextureWrapMode.Repeat );
				GL.TexParameter ( TextureTarget.Texture1D, TextureParameterName.TextureWrapT, ( int ) TextureWrapMode.Repeat );

				byte [] colorData = new byte [ Width * 4 ];

				for ( int x = 0; x < Width; ++x )
				{
					int index = x;
					int dataIndex = x * 4;
					colorData [ dataIndex + 0 ] = value [ index ].BlueValue;
					colorData [ dataIndex + 1 ] = value [ index ].GreenValue;
					colorData [ dataIndex + 2 ] = value [ index ].RedValue;
					colorData [ dataIndex + 3 ] = value [ index ].AlphaValue;
				}
				GL.TexImage1D<byte> ( TextureTarget.Texture1D, 0, PixelInternalFormat.Rgba8,
					Width, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
					PixelType.UnsignedByte, colorData );

				GL.BindTexture ( TextureTarget.Texture1D, 0 );
			}
		}

		public object Handle { get { return texture; } }

		private void MakeTexture ( int width )
		{
			if ( width == 0 ) width = 1;
			Size = width;
			texture = GL.GenTexture ();
			GL.BindTexture ( TextureTarget.Texture1D, texture );
			GL.TexImage1D ( TextureTarget.Texture1D, 0, PixelInternalFormat.Rgba8, width, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero );
			GL.BindTexture ( TextureTarget.Texture1D, 0 );
		}

		private void GenerateMipmap ( int mipmapLevel )
		{
			GL.BindTexture ( TextureTarget.Texture1D, texture );
			GL.TexParameter ( TextureTarget.Texture1D, TextureParameterName.GenerateMipmap, mipmapLevel );
			GL.GenerateMipmap ( GenerateMipmapTarget.Texture1D );
			GL.BindTexture ( TextureTarget.Texture1D, 0 );
		}

		public Texture1D ( IGraphicsDevice graphicsDevice, int width, int mipmapLevel = 1 )
		{
			MakeTexture ( width );
			GenerateMipmap ( mipmapLevel );
		}

		public Texture1D ( IGraphicsDevice graphicsDevice, ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{
			MakeTexture ( imageInfo.Width );
			Buffer = imageInfo.GetPixels ( colorKey );
			GenerateMipmap ( mipmapLevel );
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
