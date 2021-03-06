﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Blockar.Json;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Mathematics.Geometries;

namespace Daramkun.Misty.Contents.Readers
{
	public class SpriteContentLoader : IContentReader
	{
		Texture2DContentLoader textureContentLoader = new Texture2DContentLoader ();

		public Type ContentType { get { return typeof ( Sprite ); } }

		public IEnumerable<string> FileExtensions { get { foreach ( string s in textureContentLoader.FileExtensions ) yield return s; yield return "spr"; yield return "json"; } }

		public bool AutoStreamDispose { get { return true; } }

		public object Read ( Stream stream, ResourceTable resourceTable, params object [] args )
		{
			try
			{
				return new Sprite ( textureContentLoader.Read ( stream, resourceTable, args ) as ITexture2D );
			}
			catch
			{
				JsonContainer data = new JsonContainer ( stream );
				
				Sprite sprite = new Sprite ( null );
				if ( data.Contains ( "colorkey" ) )
				{
					JsonContainer colorKey = data [ "colorkey" ] as JsonContainer;
					sprite.Texture = textureContentLoader.Read ( new MemoryStream ( Convert.FromBase64String ( data [ "image" ] as string ) ),
						resourceTable,
						new Color ( ( byte ) colorKey [ 0 ], ( byte ) colorKey [ 1 ], ( byte ) colorKey [ 2 ], ( byte ) colorKey [ 3 ] ) ) as ITexture2D;
				}
				else
					sprite.Texture = textureContentLoader.Read ( new MemoryStream ( Convert.FromBase64String ( data [ "image" ] as string ) ),
						resourceTable ) as ITexture2D;

				if ( data.Contains ( "cliparea" ) )
				{
					JsonContainer clipArea = data [ "clipArea" ] as JsonContainer;
					if ( clipArea.ContainerType == ContainType.Object )
					{
						sprite.ClippingArea = new Rectangle ( ( int ) clipArea [ "left" ], ( int ) clipArea [ "top" ],
							( int ) clipArea [ "right" ] - ( int ) clipArea [ "left" ], ( int ) clipArea [ "bottom" ] - ( int ) clipArea [ "top" ] );
					}
					else
					{
						sprite.ClippingArea = new Rectangle ( ( int ) clipArea [ 0 ], ( int ) clipArea [ 1 ],
							( int ) clipArea [ 2 ] - ( int ) clipArea [ 0 ], ( int ) clipArea [ 3 ] - ( int ) clipArea [ 1 ] );
					}
				}

				if ( data.Contains ( "overlay" ) )
				{
					JsonContainer overlay = data [ "overlay" ] as JsonContainer;
					sprite.OverlayColor = new Color ( ( byte ) overlay [ 0 ], ( byte ) overlay [ 1 ], ( byte ) overlay [ 2 ], ( byte ) overlay [ 3 ] );
				}

				return sprite;
			}
		}
	}
}
