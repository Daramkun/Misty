using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Contents
{
	public struct AudioInfo
	{
		public bool IsSettingCompleted { get; private set; }

		public int AudioChannel { get; private set; }
		public int SampleRate { get; private set; }
		public int BitsPerSample { get; private set; }
		public TimeSpan Duration { get; private set; }
		public Stream AudioStream { get; private set; }

		object RawSamples;
		Func<AudioInfo, object, TimeSpan?, byte []> GetSampleFunc;

		public AudioInfo ( int channel, int sampleRate, int bitPerSamples, TimeSpan duration, Stream audioStream,
			object rawSamples, Func<AudioInfo, object, TimeSpan?, byte []> func )
			: this ()
		{
			AudioChannel = channel;
			SampleRate = sampleRate;
			BitsPerSample = bitPerSamples;
			Duration = duration;
			AudioStream = audioStream;
			RawSamples = rawSamples;
			GetSampleFunc = func;

			IsSettingCompleted = true;
		}

		public byte [] GetSamples ( TimeSpan? timeSpan = null )
		{
			return GetSampleFunc ( this, RawSamples, timeSpan );
		}

		public override string ToString ()
		{
			return string.Format ( "{{Duration: {0}, SampleRate: {1}}}", Duration, SampleRate );
		}
	}
}
