using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using OpenTK.Audio.OpenAL;

namespace Daramkun.Misty.Audios
{
	class AudioBuffer : StandardDispose, IAudioBuffer
	{
		int sourceId;
		int bufferId;

		CancelEventArgs cancelEvent = new CancelEventArgs ();

		bool isPlaying = false;

		AudioInfo audioInfo;
		ALFormat alFormat;

		public object Handle { get { return sourceId; } }
		internal bool IsInnerPlaying { get { return AL.GetSourceState ( sourceId ) == ALSourceState.Playing; } }
		public bool IsPlaying { get { return IsInnerPlaying && isPlaying; } }
		public bool IsPaused { get { return AL.GetSourceState ( sourceId ) == ALSourceState.Paused; } }
		public float Volume
		{
			get
			{
				float volume;
				AL.GetSource ( sourceId, ALSourcef.Gain, out volume );
				return volume;
			}
			set
			{
				if ( value < 0f || value > 1f )
					throw new ArgumentException ();
				AL.Source ( sourceId, ALSourcef.Gain, value );
			}
		}
		public float Balance
		{
			get
			{
				float volume;
				AL.GetSource ( sourceId, ALSourcef.Pitch, out volume );
				return volume;
			}
			set
			{
				if ( value < 0f || value > 1f )
					throw new ArgumentException ();
				AL.Source ( sourceId, ALSourcef.Pitch, value );
			}
		}
		public TimeSpan Position { get; set; }
		public TimeSpan Duration { get { return audioInfo.Duration; } }

		public AudioBuffer ( IAudioDevice audioDevice, AudioInfo audioInfo )
		{
			sourceId = AL.GenSource ();
			this.audioInfo = audioInfo;
			alFormat = ( ( audioInfo.AudioChannel == 2 ) ?
				( ( audioInfo.BitsPerSample == 16 ) ? ALFormat.Stereo16 : ALFormat.Stereo8 ) :
				( ( audioInfo.BitsPerSample == 16 ) ? ALFormat.Mono16 : ALFormat.Mono8 ) );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				AL.DeleteSource ( sourceId );
				sourceId = 0;
				AL.DeleteBuffer ( bufferId );
				bufferId = 0;
			}
			base.Dispose ( isDisposing );
		}

		public void Play ()
		{
			isPlaying = true;
			AL.SourcePlay ( sourceId );
		}

		public void Stop ()
		{
			isPlaying = false;
			Position = new TimeSpan ();
			AL.SourceStop ( sourceId );
		}

		public void Pause ()
		{
			isPlaying = false;
			AL.SourcePause ( sourceId );
		}

		public bool Update ()
		{
			if ( sourceId == 0 ) return true;

			if ( isPlaying && !IsInnerPlaying )
			{
				byte [] data = audioInfo.GetSamples ( Position );
				if ( data == null )
				{
					if ( BufferEnded != null )
					{
						BufferEnded ( this, cancelEvent );
						if ( cancelEvent.Cancel ) return false;
						else return true;
					}
					else return true;
				}

				BufferData ( data );

				Play ();
				Position += TimeSpan.FromSeconds ( data.Length / ( double ) audioInfo.ByteRate );
				if ( Position >= Duration )
				{
					Position = Duration;
					return true;
				}
			}
			return false;
		}

		public void BufferData ( byte [] data )
		{
			if ( bufferId == 0 ) bufferId = AL.GenBuffer ();
			else AL.SourceUnqueueBuffer ( sourceId );
			AL.BufferData ( bufferId, alFormat, data, data.Length, audioInfo.SampleRate );
			AL.SourceQueueBuffer ( sourceId, bufferId );
		}

		public event EventHandler<CancelEventArgs> BufferEnded;
	}
}
