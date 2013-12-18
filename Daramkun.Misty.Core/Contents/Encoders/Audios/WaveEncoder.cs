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
			writer.Write ( Encoding.UTF8.GetBytes ( "RIFF" ) );
			writer.Write ( totalLength );
			writer.Write ( Encoding.UTF8.GetBytes ( "WAVE" ) );
			
			writer.Write ( Encoding.UTF8.GetBytes ( "fmt" ) );
			writer.Write ( new byte [] { 0 } );
			writer.Write ( 16 );
			writer.Write ( ( short ) 0 );
			writer.Write ( ( short ) data.AudioChannel );
			writer.Write ( data.SampleRate );
			writer.Write ( data.SampleRate * data.AudioChannel * data.BitsPerSample );
			writer.Write ( ( short ) 0 );
			writer.Write ( ( short ) data.BitsPerSample * 8 );

			writer.Write ( Encoding.UTF8.GetBytes ( "data" ) );
			foreach ( byte [] temp in buffer )
				writer.Write ( temp );

			return true;
		}
	}
}
