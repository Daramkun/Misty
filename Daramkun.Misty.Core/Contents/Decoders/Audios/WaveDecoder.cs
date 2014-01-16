using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Contents.Decoders.Audios
{
	[FileFormat ( "wav" )]
	public class WaveDecoder : IDecoder<AudioInfo>
	{
		private class SampleInfo
		{
			public int StartPoint { get; set; }
			public int DataSize { get; set; }
			public int Offset { get; set; }
			public int ByteRate { get; set; }
			public BinaryReader Reader { get; set; }
		}

		public bool Decode ( Stream stream, out AudioInfo to, params object [] args )
		{
			BinaryReader br = new BinaryReader ( stream );
			int chunkSize;
			if ( !ReadRIFFHeader ( br, out chunkSize ) ) { to = null; return false; }
			int channel, sampleRate, bitPerSamples, byteRate, dataSize;
			SampleInfo s;
			if ( !ReadfmtHeader ( br, out channel, out sampleRate, out bitPerSamples, out byteRate ) ) { to = null; return false; }
			if ( !ReadWaveChunk ( br, out s, out dataSize ) ) { to = null; return false; }

			s.ByteRate = byteRate;

			to = new AudioInfo ( channel, sampleRate, bitPerSamples, TimeSpan.FromSeconds ( dataSize / ( float ) byteRate ), stream,
				s, ( AudioInfo audioInfo, object sample, TimeSpan? timeSpan ) =>
				{
					SampleInfo sampleInfo = sample as SampleInfo;

					if ( timeSpan != null )
						audioInfo.AudioStream.Position = sampleInfo.StartPoint + ( int ) ( timeSpan.Value.TotalSeconds * sampleInfo.ByteRate );

					if ( ( int ) sampleInfo.DataSize <= ( int ) sampleInfo.Offset )
					{
						return null;
					}
					byte [] data = sampleInfo.Reader.ReadBytes ( audioInfo.SampleRate );
					sampleInfo.Offset += audioInfo.SampleRate;
					return data;
				} );

			return true;
		}

		private bool ReadRIFFHeader ( BinaryReader br, out int fileSize )
		{
			string riffSignature = Encoding.UTF8.GetString ( br.ReadBytes ( 4 ), 0, 4 );
			if ( riffSignature == "WAVE" ) { fileSize = 0; return true; }

			fileSize = br.ReadInt32 ();
			string waveSignature = Encoding.UTF8.GetString ( br.ReadBytes ( 4 ), 0, 4 );

			if ( riffSignature != "RIFF" || waveSignature != "WAVE" )
				return false;

			return true;
		}

		private bool ReadfmtHeader ( BinaryReader br, out int channel, out int sampleRate, out int bitPerSamples, out int byteRate )
		{
			string fmtSignature = Encoding.UTF8.GetString ( br.ReadBytes ( 4 ), 0, 4 ).Trim ();
			if ( fmtSignature != "fmt" ) { channel = sampleRate = bitPerSamples = byteRate = 0; return false; }

			int chunkSize = br.ReadInt32 ();
			int audioFormat = br.ReadInt16 ();
			if ( audioFormat != 1 ) { channel = sampleRate = bitPerSamples = byteRate = 0; return false; }
			channel = br.ReadInt16 ();
			sampleRate = br.ReadInt32 ();
			byteRate = br.ReadInt32 ();

			int blockAlign = br.ReadInt16 ();
			bitPerSamples = br.ReadInt16 () / 8;

			if ( chunkSize != 16 )
			{
				int extraSize = br.ReadInt16 ();
				br.ReadBytes ( extraSize );
			}

			return true;
		}

		private bool ReadWaveChunk ( BinaryReader br, out SampleInfo sampleInfo, out int dataSize )
		{
			string fmtSignature = Encoding.UTF8.GetString ( br.ReadBytes ( 4 ), 0, 4 );
			if ( fmtSignature != "data" )
			{
				if ( fmtSignature == "fact" )
				{
					br.ReadInt32 ();
					br.ReadInt32 ();
					return ReadWaveChunk ( br, out sampleInfo, out dataSize );
				}
				else
				{
					dataSize = 0;
					sampleInfo = new SampleInfo ();
					return false;
				}
			}

			dataSize = br.ReadInt32 ();
			sampleInfo = new SampleInfo ()
			{
				DataSize = dataSize,
				Offset = 0,
				Reader = br,
				StartPoint = ( int ) br.BaseStream.Position
			};

			return true;
		}
	}
}
