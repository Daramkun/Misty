namespace Hjg.Pngcs.Chunks
{
	using Hjg.Pngcs;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.CompilerServices;

	internal class PngChunkTRNS : PngChunkSingle
	{
		public const String ID = ChunkHelper.tRNS;

		private int gray;
		private int red, green, blue;
		private int [] paletteAlpha;

		public PngChunkTRNS ( ImageInfo info )
			: base ( ID, info )
		{
		}

		public override ChunkOrderingConstraint GetOrderingConstraint ()
		{
			return ChunkOrderingConstraint.AFTER_PLTE_BEFORE_IDAT;
		}

		public override ChunkRaw CreateRawChunk ()
		{
			ChunkRaw c = null;
			if ( ImgInfo.Greyscale )
			{
				c = createEmptyChunk ( 2, true );
				Hjg.Pngcs.PngHelperInternal.WriteInt2tobytes ( gray, c.Data, 0 );
			}
			else if ( ImgInfo.Indexed )
			{
				c = createEmptyChunk ( paletteAlpha.Length, true );
				for ( int n = 0; n < c.Length; n++ )
				{
					c.Data [ n ] = ( byte ) paletteAlpha [ n ];
				}
			}
			else
			{
				c = createEmptyChunk ( 6, true );
				Hjg.Pngcs.PngHelperInternal.WriteInt2tobytes ( red, c.Data, 0 );
				Hjg.Pngcs.PngHelperInternal.WriteInt2tobytes ( green, c.Data, 0 );
				Hjg.Pngcs.PngHelperInternal.WriteInt2tobytes ( blue, c.Data, 0 );
			}
			return c;
		}

		public override void ParseFromRaw ( ChunkRaw c )
		{
			if ( ImgInfo.Greyscale )
			{
				gray = Hjg.Pngcs.PngHelperInternal.ReadInt2fromBytes ( c.Data, 0 );
			}
			else if ( ImgInfo.Indexed )
			{
				int nentries = c.Data.Length;
				paletteAlpha = new int [ nentries ];
				for ( int n = 0; n < nentries; n++ )
				{
					paletteAlpha [ n ] = ( int ) ( c.Data [ n ] & 0xff );
				}
			}
			else
			{
				red = Hjg.Pngcs.PngHelperInternal.ReadInt2fromBytes ( c.Data, 0 );
				green = Hjg.Pngcs.PngHelperInternal.ReadInt2fromBytes ( c.Data, 2 );
				blue = Hjg.Pngcs.PngHelperInternal.ReadInt2fromBytes ( c.Data, 4 );
			}
		}

		public override void CloneDataFromRead ( PngChunk other )
		{
			PngChunkTRNS otherx = ( PngChunkTRNS ) other;
			gray = otherx.gray;
			red = otherx.red;
			green = otherx.red;
			blue = otherx.red;
			if ( otherx.paletteAlpha != null )
			{
				paletteAlpha = new int [ otherx.paletteAlpha.Length ];
				System.Array.Copy ( otherx.paletteAlpha, 0, paletteAlpha, 0, paletteAlpha.Length );
			}
		}

		public void SetRGB ( int r, int g, int b )
		{
			if ( ImgInfo.Greyscale || ImgInfo.Indexed )
				throw new PngjException ( "only rgb or rgba images support this" );
			red = r;
			green = g;
			blue = b;
		}

		public int [] GetRGB ()
		{
			if ( ImgInfo.Greyscale || ImgInfo.Indexed )
				throw new PngjException ( "only rgb or rgba images support this" );
			return new int [] { red, green, blue };
		}

		public void SetGray ( int g )
		{
			if ( !ImgInfo.Greyscale )
				throw new PngjException ( "only grayscale images support this" );
			gray = g;
		}

		public int GetGray ()
		{
			if ( !ImgInfo.Greyscale )
				throw new PngjException ( "only grayscale images support this" );
			return gray;
		}

		public void SetPalletteAlpha ( int [] palAlpha )
		{
			if ( !ImgInfo.Indexed )
				throw new PngjException ( "only indexed images support this" );
			paletteAlpha = palAlpha;
		}

		public void setIndexEntryAsTransparent ( int palAlphaIndex )
		{
			if ( !ImgInfo.Indexed )
				throw new PngjException ( "only indexed images support this" );
			paletteAlpha = new int [] { palAlphaIndex + 1 };
			for ( int i = 0; i < palAlphaIndex; i++ )
				paletteAlpha [ i ] = 255;
			paletteAlpha [ palAlphaIndex ] = 0;
		}

		public int [] GetPalletteAlpha ()
		{
			if ( !ImgInfo.Indexed )
				throw new PngjException ( "only indexed images support this" );
			return paletteAlpha;
		}
	}
}
