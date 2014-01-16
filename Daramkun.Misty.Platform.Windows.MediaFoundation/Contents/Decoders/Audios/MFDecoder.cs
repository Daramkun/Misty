using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.MediaFoundation;

namespace Daramkun.Misty.Contents.Decoders.Audios
{
	public class MFDecoder : IDecoder<AudioInfo>
	{
		public bool Decode ( Stream stream, out AudioInfo to, params object [] args )
		{
			try
			{
				AudioDecoder dec = new AudioDecoder ( stream );
				to = new AudioInfo ( dec.WaveFormat.Channels, dec.WaveFormat.SampleRate, dec.WaveFormat.BitsPerSample,
					dec.Duration, stream, new object [] { dec, null }, ( AudioInfo audioInfo, object raws, TimeSpan? timeSpan ) =>
				{
					object [] datas = (object [] )(object)raws;
					if ( timeSpan != null ) datas [ 1 ] = ( datas [ 0 ] as AudioDecoder ).GetSamples ( timeSpan.Value ).GetEnumerator ();
					if ( datas [ 1 ] == null ) datas [ 1 ] = ( datas [ 0 ] as AudioDecoder ).GetSamples ().GetEnumerator ();
					if ( !( datas [ 1 ] as IEnumerator<SharpDX.DataPointer> ).MoveNext () ) return null;
					SharpDX.DataPointer pt = ( datas [ 1 ] as IEnumerator<SharpDX.DataPointer> ).Current;
					SharpDX.DataBuffer dataBuffer = new SharpDX.DataBuffer ( pt.Pointer, pt.Size );
					byte [] data = dataBuffer.GetRange<byte> ( 0, pt.Size );
					dataBuffer.Dispose ();
					return data;
				} );
				return true;
			}
			catch { to = null; return false; }
		}
	}
}
