using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Audios
{
	public interface IAudioBuffer : IDisposable
	{
		bool IsPlaying { get; }
		bool IsPaused { get; }

		float Volume { get; set; }
		float Balance { get; set; }

		TimeSpan Position { get; set; }
		TimeSpan Duration { get; }

		object Handle { get; }

		void Play ();
		void Stop ();
		void Pause ();

		bool Update ();

		void BufferData ( byte [] data );

		event EventHandler<CancelEventArgs> BufferEnded;
	}
}
