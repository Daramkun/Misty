using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Contents.Encoders.Audios
{
	[FileFormat ( "wav" )]
	public class WaveEncoder : IEncoder<AudioInfo>
	{
		public bool Encode ( Stream stream, AudioInfo data, params object [] args )
		{
			List<byte []> buffer = new List<byte []> ();
			int totalLength = 0;
			while ( true )
			{
				byte[] temp = data.GetSamples ();
				if ( temp == null ) break;
				buffer.Add ( temp );
				totalLength += temp.Length;
			}

			BinaryWriter writer = new BinaryWriter ( stream );
			writer.Write ( Encoding.UTF8.GetBytes ( "RIFF" ), 0, 4 );
			writer.Write ( totalLength + 36 );
			writer.Write ( Encoding.UTF8.GetBytes ( "WAVE" ), 0, 4 );
			
			writer.Write ( Encoding.UTF8.GetBytes ( "fmt " ), 0, 4 );
			writer.Write ( 16 );
			writer.Write ( ( short ) 0 );
			writer.Write ( ( short ) data.AudioChannel );
			writer.Write ( data.SampleRate );
			writer.Write ( data.SampleRate * data.AudioChannel * data.BitsPerSample );
			writer.Write ( ( short ) 0 );
			writer.Write ( ( short ) data.BitsPerSample );

			writer.Write ( Encoding.UTF8.GetBytes ( "data" ) );
			writer.Write ( totalLength );
			foreach ( byte [] temp in buffer )
				writer.Write ( temp );

			return true;
		}
	}
}
