using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Contents.Decoders.Audios
{
	[FileFormat ( "ogg", "oga" )]
	public class OggVorbisDecoder : IDecoder<AudioInfo>
	{
		WaveDecoder waveDecoder = new WaveDecoder ();

		public bool Decode ( Stream stream, out AudioInfo to, params object [] args )
		{
			try
			{
				OggDecodeStream decodeStream = new OggDecodeStream ( stream );
				new WaveDecoder ().Decode ( decodeStream, out to );
				return true;
			}
			catch { to = new AudioInfo (); return false; }
		}
	}
}
