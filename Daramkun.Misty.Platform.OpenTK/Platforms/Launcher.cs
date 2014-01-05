using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Audios;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Inputs.Devices;
using Daramkun.Misty.Inputs.States;
using Daramkun.Misty.Log;

namespace Daramkun.Misty.Platforms
{
	public sealed class Launcher : ILauncher
	{
		public PlatformInformation PlatformInformation
		{
			get
			{
				OperatingSystem os = Environment.OSVersion;

				return new PlatformInformation ()
				{
					PlatformType = ( os.Platform == PlatformID.Win32NT ) ? PlatformType.WindowsNT :
								( os.Platform == PlatformID.Unix ) ? PlatformType.Unix :
								( os.Platform == PlatformID.MacOSX ) ? PlatformType.OSX :
								PlatformType.Unknown,
					PlatformVersion = os.Version,

					UserName = Environment.UserName,
					MachineUniqueIdentifier = NetworkInterface.GetAllNetworkInterfaces () [ 0 ].GetPhysicalAddress ().ToString (),
				};
			}
		}

		public bool Initialize ( bool audioIncluded = true )
		{
			try
			{
				Core.SetWindow ( new Window () );
				Core.SetGraphicsDevice ( new GraphicsDevice ( Core.Window ) );
				if ( audioIncluded ) Core.SetAudioDevice ( new AudioDevice ( Core.Window ) );

				Core.Inputs.Add<KeyboardState> ( new Keyboard ( Core.Window ) );
				Core.Inputs.Add<MouseState> ( new Mouse ( Core.Window ) );
			}
			catch ( Exception e ) { Logger.Write ( LogLevel.Level5, e.Message ); return false; }
			return true;
		}

		public void Dispose ()
		{

		}

		public override string ToString () { return "OpenGL/OpenAL Platform Abstraction Framework"; }
	}
}
