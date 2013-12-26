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
		List<int> bufferIds;

		TimeSpan lastPosition;
		bool isPlaying = false;

		AudioInfo audioInfo;
		ALFormat alFormat;

		public object Handle { get { return sourceId; } }
		public bool IsPlaying { get { return AL.GetSourceState ( sourceId ) == ALSourceState.Playing; } }
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
		public TimeSpan Position
		{
			get
			{
				float seconds;
				AL.GetSource ( sourceId, ALSourcef.SecOffset, out seconds );
				return TimeSpan.FromSeconds ( seconds );
			}
			set
			{
				if ( value > Duration ) throw new ArgumentException ();
				AL.Source ( sourceId, ALSourcef.SecOffset, ( float ) value.TotalSeconds );
			}
		}
		public TimeSpan Duration { get { return audioInfo.Duration; } }

		public AudioBuffer ( IAudioDevice audioDevice, AudioInfo audioInfo )
		{
			bufferIds = new List<int> ();
			sourceId = AL.GenSource ();
			this.audioInfo = audioInfo;
			alFormat = ( ( audioInfo.AudioChannel == 2 ) ?
				( ( audioInfo.BitPerSamples == 2 ) ? ALFormat.Stereo16 : ALFormat.Stereo8 ) :
				( ( audioInfo.BitPerSamples == 2 ) ? ALFormat.Mono16 : ALFormat.Mono8 ) );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				AL.DeleteSource ( sourceId );
				sourceId = 0;
				foreach ( int bufferId in bufferIds )
					if ( bufferId != 0 )
						AL.DeleteBuffer ( bufferId );
				bufferIds.Clear ();
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
			byte [] data = audioInfo.GetSamples ();
			if ( data == null )
			{
				if ( isPlaying && !IsPlaying )
				{
					if ( BufferEnded != null )
					{
						CancelEventArgs cancelEvent = new CancelEventArgs ();
						BufferEnded ( this, cancelEvent );
						if ( cancelEvent.Cancel ) return true;
						else return false;
					}
					else return true;
				}
				else return true;
			}

			BufferData ( data );

			if ( isPlaying && !IsPlaying )
			{
				Play ();
				Position = lastPosition;
			}
			lastPosition = Position;
			return false;
		}

		public void BufferData ( byte [] data )
		{
			int bufferId = AL.GenBuffer ();
			AL.BufferData ( bufferId, alFormat, data, data.Length, audioInfo.SampleRate );
			AL.SourceQueueBuffer ( sourceId, bufferId );
			bufferIds.Add ( bufferId );
		}

		public event EventHandler<CancelEventArgs> BufferEnded;
	}
}
