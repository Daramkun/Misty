using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Log;
using Daramkun.Misty.Platforms;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace Daramkun.Misty.Audios
{
	class AudioDevice : StandardDispose, IAudioDevice
	{
		//AudioContext audioContext;
		IntPtr _device;
		OpenTK.ContextHandle _context;

		List<IAudioBuffer> audioList = new List<IAudioBuffer> ();

		public object Handle { get { return /*audioContext*/_context; } }

		public AudioDevice ( IWindow window )
		{
			try
			{
				//audioContext = new AudioContext ();
				//audioContext.MakeCurrent ();
				_device = Alc.OpenDevice ( string.Empty );
				int [] attribute = new int [ 0 ];
				_context = Alc.CreateContext ( _device, attribute );
				Alc.MakeContextCurrent ( _context );
			}
			catch ( Exception e )
			{
				throw new PlatformNotSupportedException ( string.Format (
					"Audio device is not available or OpenAL library is not exist: {0}",
					e ) );
			}
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				//audioContext.Dispose ();
				//audioContext = null;
				Alc.DestroyContext ( _context );
				Alc.CloseDevice ( _device );
			}
			base.Dispose ( isDisposing );
		}

		public void Update ()
		{
			if ( audioList.Count > 0 )
				foreach ( IAudioBuffer audio in audioList.ToArray () )
					audio.Update ();
		}

		public IAudioBuffer CreateAudioBuffer ( AudioInfo audioInfo )
		{
			IAudioBuffer buffer = new AudioBuffer ( this, audioInfo );
			audioList.Add ( buffer );
			return buffer;
		}
	}
}
