﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Blockar.Json;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Readers;

namespace Daramkun.Misty.Graphics.Spirit.Fonts
{
	public class LsfFont : Font
	{
		Dictionary<char, ITexture2D> readedImage = new Dictionary<char, ITexture2D> ();
		List<char> noneList = new List<char> ();
		Texture2DContentLoader imageContentLoader;

		ZipFileSystem fileSystem;

		private char GetChar ( string filename )
		{
			if ( filename [ 0 ] >= '0' && filename [ 0 ] <= '9' )
			{
				return ( char ) int.Parse ( filename.Substring ( 0, filename.LastIndexOf ( '.' ) - 1 ) );
			}
			else return filename [ 0 ];
		}

		public LsfFont ( Stream stream )
		{
			fileSystem = new ZipFileSystem ( stream );
			Stream file = fileSystem.OpenFile ( "info.json" );
			JsonContainer entry = new JsonContainer ( file );
			FontFamily = entry [ "fontfamily" ] as string;
			FontSize = ( int ) entry [ "fontsize" ];

			Texture2DContentLoader.AddDefaultDecoders ();
			imageContentLoader = new Texture2DContentLoader ();
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				foreach ( KeyValuePair<char, ITexture2D> image in readedImage )
					image.Value.Dispose ();
				readedImage.Clear ();
				fileSystem.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		private string IsExistCharFile ( char ch )
		{
			string filename = String.Format ( "{0}.bmp", ( int ) ch );
			if ( fileSystem.Files.Contains ( filename ) )
				return filename;
			filename = String.Format ( "{0}.png", ( int ) ch );
			if ( fileSystem.Files.Contains ( filename ) )
				return filename;
			filename = String.Format ( "{0}.bmp", ch );
			if ( fileSystem.Files.Contains ( filename ) )
				return filename;
			filename = String.Format ( "{0}.png", ch );
			if ( fileSystem.Files.Contains ( filename ) )
				return filename;
			return null;
		}

		protected override ITexture2D this [ char ch ]
		{
			get
			{
				if ( readedImage.ContainsKey ( ch ) )
					return readedImage [ ch ];
				else
				{
					if ( noneList.Contains ( ch ) ) return null;

					string filename = IsExistCharFile ( ch );

					if ( filename == null )
					{
						noneList.Add ( ch );
						return null;
					}

					ITexture2D fontImage = imageContentLoader.Read ( fileSystem.OpenFile ( filename ), null, Color.Magenta ) as ITexture2D;
					readedImage.Add ( ch, fontImage );
					return fontImage;
				}
			}
		}
	}
}
