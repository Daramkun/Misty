using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using NVorbis;

namespace Daramkun.Misty.Contents.Decoders.Audios
{
	[FileFormat ( "ogg", "oga" )]
	public class OggVorbisDecoder : IDecoder<AudioInfo>
	{
		public bool Decode ( Stream stream, out AudioInfo to, params object [] args )
		{
			try
			{
				VorbisReader reader = new VorbisReader ( stream, false );
				to = new AudioInfo ( reader.Channels, reader.SampleRate, 2, reader.TotalTime, stream, new object []
				{
					reader,
					reader.TotalTime,
					new float [ reader.SampleRate / 2 ],
					new byte [ reader.SampleRate ]
				}, ( AudioInfo audioInfo, object raw, TimeSpan? timeSpan ) =>
				{
					VorbisReader reader2 = ( VorbisReader ) ( object ) ( ( ( object [] ) ( object ) raw ) [ 0 ] );
					TimeSpan totalTime = ( TimeSpan ) ( object ) ( ( ( object [] ) ( object ) raw ) [ 1 ] );
	
					if ( timeSpan != null )
						reader.DecodedTime = timeSpan.Value;

					if ( reader.DecodedTime >= totalTime )
						return null;

					float [] buffer = ( float [] ) ( object ) ( ( ( object [] ) ( object ) raw ) [ 2 ] );
					byte [] returnBuffer = ( byte [] ) ( object ) ( ( ( object [] ) ( object ) raw ) [ 3 ] );
					int count = reader.ReadSamples ( buffer, 0, buffer.Length );

					CastBuffer ( buffer, returnBuffer, count );
					return returnBuffer;
				} );
				return true;
			}
			catch { to = new AudioInfo (); return false; }
		}

		private void CastBuffer ( float [] inBuffer, byte [] outBuffer, int count )
		{
			for ( int i = 0; i < count; i++ )
			{
				var temp = ( int ) ( 32767f * inBuffer [ i ] );
				if ( temp > short.MaxValue ) temp = short.MaxValue;
				else if ( temp < short.MinValue ) temp = short.MinValue;
				outBuffer [ i * 2 + 0 ] = ( byte ) ( ( ( ( short ) temp ) >> 0 ) & 0xff );
				outBuffer [ i * 2 + 1 ] = ( byte ) ( ( ( ( short ) temp ) >> 8 ) & 0xff );
			}
		}
	}
}
