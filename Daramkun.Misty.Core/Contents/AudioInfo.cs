﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Contents
{
	public sealed class AudioInfo : IDisposable
	{
		public int AudioChannel { get; private set; }
		public int SampleRate { get; private set; }
		public int BitsPerSample { get; private set; }
		public TimeSpan Duration { get; private set; }
		public Stream AudioStream { get; private set; }
		public int ByteRate { get { return AudioChannel * SampleRate * ( BitsPerSample / 8 ); } }

		object RawSamples;
		Func<AudioInfo, object, TimeSpan?, byte []> GetSampleFunc;

		public AudioInfo ( int channel, int sampleRate, int bitPerSamples, TimeSpan duration, Stream audioStream,
			object rawSamples, Func<AudioInfo, object, TimeSpan?, byte []> func )
		{
			AudioChannel = channel;
			SampleRate = sampleRate;
			BitsPerSample = bitPerSamples;
			Duration = duration;
			AudioStream = audioStream;
			RawSamples = rawSamples;
			GetSampleFunc = func;
		}

		public void Dispose ()
		{
			try { if ( AudioStream != null ) AudioStream.Dispose (); }
			catch { }
		}

		public byte [] GetSamples ( TimeSpan? timeSpan = null )
		{
			return GetSampleFunc ( this, RawSamples, timeSpan );
		}

		public override string ToString ()
		{
			return string.Format ( "{{Duration: {0}, SampleRate: {1}}}", Duration, SampleRate );
		}

		public static float [] ConvertBytesToFloats ( byte [] data )
		{
			List<float> floats = new List<float> ();
			for ( int i = 0; i < data.Length; i += 2 )
				floats [ i / 2 ] = ( ( data [ i + 0 ] << 8 ) + data [ i + 1 ] ) / 32767f;
			return floats.ToArray ();
		}
	}
}
