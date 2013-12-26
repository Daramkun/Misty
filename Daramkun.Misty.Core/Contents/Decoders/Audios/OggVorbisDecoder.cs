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
				to = new AudioInfo ( reader.Channels, reader.SampleRate, 2, reader.TotalTime, stream, reader,
					( AudioInfo audioInfo, object raw, TimeSpan? timeSpan ) =>
					{
						if ( timeSpan != null )
							reader.DecodedTime = timeSpan.Value;

						if ( reader.DecodedTime >= reader.TotalTime )
							return null;

						if ( reader.StreamIndex - 1 >= reader.StreamCount )
							return null;

						float [] buffer = new float [ audioInfo.SampleRate / 2 ];
						byte [] returnBuffer = new byte [ audioInfo.SampleRate ];
						int count = reader.ReadSamples ( buffer, 0, buffer.Length );

						CastBuffer ( buffer, returnBuffer, count );
						return returnBuffer;
					} );
				return true;
			}
			catch { to = new AudioInfo (); return false; }
		}

		private void CastBuffer ( float [] inBuffer, short [] outBuffer, int length )
		{
			for ( int i = 0; i < length; i++ )
			{
				var temp = ( int ) ( 32767f * inBuffer [ i ] );
				if ( temp > short.MaxValue ) temp = short.MaxValue;
				else if ( temp < short.MinValue ) temp = short.MinValue;
				outBuffer [ i ] = ( short ) temp;
			}
		}

		private void CastBuffer ( float [] inBuffer, byte [] outBuffer, int count )
		{
			short [] tempBuffer = new short [ count ];
			CastBuffer ( inBuffer, tempBuffer, count );

			MemoryStream stream = new MemoryStream ();
			BinaryWriter writer = new BinaryWriter ( stream );
			foreach ( short s in tempBuffer )
				writer.Write ( s );
			byte [] data = stream.ToArray ();
			stream.Dispose ();
			data.CopyTo ( outBuffer, 0 );
		}
	}
}
