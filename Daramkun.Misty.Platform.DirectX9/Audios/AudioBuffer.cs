using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;

namespace Daramkun.Misty.Audios
{
	class AudioBuffer : StandardDispose, IAudioBuffer
	{
		SharpDX.DirectSound.SecondarySoundBuffer soundBuffer;
		AudioInfo audioInfo;

		internal bool isPlaying = false;

		CancelEventArgs cancelEvent = new CancelEventArgs ();

		public object Handle { get { return soundBuffer; } }
		public TimeSpan Duration { get { return audioInfo.Duration; } }
		internal bool IsInnerPlaying { get { return soundBuffer.Status == 1; } }
		public bool IsPlaying { get { return IsInnerPlaying && isPlaying; } }
		public bool IsPaused { get { return soundBuffer.Status == 0; } }

		public float Volume
		{
			get { return ( 10000 + soundBuffer.Volume ) / 10000.0f; }
			set
			{
				int v = ( ( int ) ( ( value - 1 ) * 10000.0f ) );
				if ( v > 0 ) v = 0;
				else if ( v < -10000 ) v = -10000;
				soundBuffer.Volume = v;
			}
		}

		public float Balance
		{
			get { return soundBuffer.Pan / 10000.0f; }
			set { soundBuffer.Pan = ( int ) ( value * 10000.0f ); }
		}

		/*public TimeSpan Position
		{
			get
			{
				int playCursor, writeCursor;
				soundBuffer.GetCurrentPosition ( out playCursor, out writeCursor );
				return TimeSpan.FromSeconds ( playCursor / audioInfo.ByteRate );
			}
			set
			{
				soundBuffer.CurrentPosition = ( int ) ( value.TotalSeconds * audioInfo.ByteRate );
			}
		}*/
		public TimeSpan Position { get; set; }
		
		public AudioBuffer ( IAudioDevice audioDevice, AudioInfo audioInfo )
		{
			this.audioInfo = audioInfo;

			SharpDX.DirectSound.SoundBufferDescription bufferDesc = new SharpDX.DirectSound.SoundBufferDescription ()
			{
				Flags = SharpDX.DirectSound.BufferFlags.ControlVolume | SharpDX.DirectSound.BufferFlags.ControlPan |
				SharpDX.DirectSound.BufferFlags.ControlPositionNotify | SharpDX.DirectSound.BufferFlags.StickyFocus |
				SharpDX.DirectSound.BufferFlags.Software | SharpDX.DirectSound.BufferFlags.GetCurrentPosition2 |
				SharpDX.DirectSound.BufferFlags.ControlFrequency | SharpDX.DirectSound.BufferFlags.GlobalFocus,
				Format = new SharpDX.Multimedia.WaveFormat ( audioInfo.SampleRate, audioInfo.BitsPerSample, audioInfo.AudioChannel ),
				BufferBytes = audioInfo.SampleRate
			};
			soundBuffer = new SharpDX.DirectSound.SecondarySoundBuffer ( audioDevice.Handle as SharpDX.DirectSound.DirectSound,
				bufferDesc );
		}

		public void Play ()
		{
			soundBuffer.Play ( 0, SharpDX.DirectSound.PlayFlags.None );
			isPlaying = true;
		}

		public void Stop ()
		{
			soundBuffer.Stop ();
			isPlaying = false;
			Position = new TimeSpan ();
		}

		public void Pause ()
		{
			isPlaying = false;
			soundBuffer.Stop ();
		}

		public bool Update ()
		{
			if ( soundBuffer == null ) return true;
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
			SharpDX.DataStream secondPart;
			SharpDX.DataStream stream = soundBuffer.Lock ( 0, data.Length, SharpDX.DirectSound.LockFlags.EntireBuffer, out secondPart );
			stream.WriteRange<byte> ( data, 0, Math.Min ( data.Length, ( int ) stream.Length ) );
			soundBuffer.Unlock ( stream, secondPart );
		}

		public event EventHandler<System.ComponentModel.CancelEventArgs> BufferEnded;
	}
}
