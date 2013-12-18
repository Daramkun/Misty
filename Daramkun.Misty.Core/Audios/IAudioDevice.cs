using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents;

namespace Daramkun.Misty.Audios
{
	public interface IAudioDevice : IDisposable
	{
		object Handle { get; }

		void Update ();

		IAudioBuffer CreateAudioBuffer ( AudioInfo audioInfo );
	}
}
