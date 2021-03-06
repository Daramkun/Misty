﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.Decoders;
using Daramkun.Misty.Contents.Decoders.Images;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Readers
{
	public class Texture2DContentLoader : IContentReader
	{
		public static List<IDecoder<ImageInfo>> Decoders { get; private set; }
		
		static Texture2DContentLoader ()
		{
			Decoders = new List<IDecoder<ImageInfo>> ();
		}

		public static void AddDefaultDecoders ()
		{
			Decoders.Add ( new BitmapDecoder () );
			Decoders.Add ( new PngDecoder () );
			Decoders.Add ( new JpegDecoder () );
			Decoders.Add ( new TargaDecoder () );
			Decoders.Add ( new PcxDecoder () );
		}

		public Type ContentType { get { return typeof ( ITexture2D ); } }
		public IEnumerable<string> FileExtensions
		{
			get
			{
				foreach ( IDecoder<ImageInfo> decoder in Decoders )
					foreach ( object attr in decoder.GetType ().GetCustomAttributes ( typeof ( FileFormatAttribute ), true ) )
						foreach ( string ext in ( attr as FileFormatAttribute ).FileExtension )
							yield return ext;
			}
		}
		public bool AutoStreamDispose { get { return true; } }

		public object Read ( Stream stream, ResourceTable resourceTable, params object [] args )
		{
			bool isLoadComplete = false;
			ImageInfo imageInfo = null;
			foreach ( IDecoder<ImageInfo> decoder in Decoders )
			{
				stream.Position = 0;
				if ( decoder.Decode ( stream, out imageInfo ) )
				{
					isLoadComplete = true;
					break;
				}
			}
			if ( isLoadComplete )
				return Core.GraphicsDevice.CreateTexture2D ( imageInfo, ( args.Length == 1 ) ? new Color? ( ( Color ) args [ 0 ] ) : null );
			else throw new ArgumentException ();
		}
	}
}
