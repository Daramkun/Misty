using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Audios
{
	public class AudioDevice : StandardDispose, IAudioDevice
	{
		SharpDX.XAudio2.XAudio2 audio;

		public object Handle { get { return audio; } }

		public AudioDevice ()
		{
			audio = new SharpDX.XAudio2.XAudio2 ();
			audio.StartEngine ();
		}

		public void Update ()
		{
			throw new NotImplementedException ();
		}

		public IAudioBuffer CreateAudioBuffer ( Contents.AudioInfo audioInfo )
		{
			throw new NotImplementedException ();
		}
	}
}
