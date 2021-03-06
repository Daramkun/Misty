﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Audios;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.Decoders;
using Daramkun.Misty.Contents.Decoders.Audios;
using Daramkun.Misty.Contents.Tables;

namespace Daramkun.Misty.Contents.Readers
{
	public class AudioContentLoader : IContentReader
	{
		public static List<IDecoder<AudioInfo>> Decoders { get; private set; }

		static AudioContentLoader ()
		{
			Decoders = new List<IDecoder<AudioInfo>> ();
		}

		public static void AddDefaultDecoders ()
		{
			Decoders.Add ( new WaveDecoder () );
			Decoders.Add ( new OggVorbisDecoder () );
			//Decoders.Add ( new FlacDecoder () );
		}

		public Type ContentType { get { return typeof ( IAudioBuffer ); } }
		public IEnumerable<string> FileExtensions
		{
			get
			{
				foreach ( IDecoder<AudioInfo> decoder in Decoders )
					foreach ( object attr in decoder.GetType ().GetCustomAttributes ( typeof ( FileFormatAttribute ), true ) )
						foreach ( string ext in ( attr as FileFormatAttribute ).FileExtension )
							yield return ext;
			}
		}

		public bool AutoStreamDispose { get { return true; } }

		public object Read ( Stream stream, ResourceTable resourceTable, params object [] args )
		{
			bool isLoadComplete = false;
			AudioInfo audioInfo = null;
			foreach ( IDecoder<AudioInfo> decoder in Decoders )
			{
				try
				{
					stream.Position = 0;
					if ( decoder.Decode ( stream, out audioInfo ) )
					{
						isLoadComplete = true;
						break;
					}
				}
				catch { }
			}
			if ( isLoadComplete )
				return Core.AudioDevice.CreateAudioBuffer ( audioInfo );
			else throw new ArgumentException ();
		}
	}
}
