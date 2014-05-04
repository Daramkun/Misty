using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Org.Nflac.Wave.Util;
//using org.nflac.structure;

namespace Daramkun.Misty.Contents.Decoders.Audios
{
	[FileFormat ( "flac", "fla" )]
	public class FlacDecoder : IDecoder<AudioInfo>
	{
		WaveDecoder wavDec = new WaveDecoder ();

		public bool Decode ( Stream stream, out AudioInfo to, params object [] args )
		{
			try
			{
				//FlacFile flac = new FlacFile ( stream );
				//flac.ParseFile ();
				//return wavDec.Decode ( flac.WaveStream, out to, args );
				Org.Nflac.Flac.Integration.FlacDecoder dec = new Org.Nflac.Flac.Integration.FlacDecoder ( stream );
				return wavDec.Decode ( new WaveStream ( dec ), out to, args );
			}
			catch { to = null; return false; }
		}
	}
}
