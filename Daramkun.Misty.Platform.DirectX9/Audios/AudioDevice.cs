using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Daramkun.Misty.Common;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Audios
{
	public class AudioDevice : StandardDispose, IAudioDevice
	{
		SharpDX.DirectSound.DirectSound dSound;
		SharpDX.DirectSound.PrimarySoundBuffer soundBuffer;
		internal SharpDX.DirectSound.DirectSoundCapture dCapture;
		List<IAudioBuffer> audioList;

		public object Handle { get { return dSound; } }

		public AudioDevice ( IWindow window )
		{
			dSound = new SharpDX.DirectSound.DirectSound ();
			dSound.SetCooperativeLevel ( ( window.Handle as Form ).Handle, SharpDX.DirectSound.CooperativeLevel.Normal );

			soundBuffer = new SharpDX.DirectSound.PrimarySoundBuffer ( dSound, new SharpDX.DirectSound.SoundBufferDescription ()
			{
				Flags = SharpDX.DirectSound.BufferFlags.PrimaryBuffer,
			} );

			soundBuffer.Play ( 0, SharpDX.DirectSound.PlayFlags.Looping );

			dCapture = new SharpDX.DirectSound.DirectSoundCapture ();

			audioList = new List<IAudioBuffer> ();
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				dCapture.Dispose ();
				soundBuffer.Dispose ();
				dSound.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		public void Update ()
		{
			if ( audioList.Count > 0 )
				foreach ( IAudioBuffer audio in audioList.ToArray () )
					if(audio.Update ())
						audio.Stop ();
		}

		public IAudioBuffer CreateAudioBuffer ( Contents.AudioInfo audioInfo )
		{
			IAudioBuffer buffer = null;
			Core.Dispatch ( () =>
			{
				buffer = new AudioBuffer ( this, audioInfo );
				audioList.Add ( buffer );
			} );
			return buffer;
		}
	}
}
