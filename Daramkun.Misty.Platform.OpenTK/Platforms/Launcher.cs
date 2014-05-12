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
using Daramkun.Misty.Mathematics.Transforms;
using System.IO;
using System.Reflection;

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

		public bool IsSupportPlatform { get { if ( PlatformInformation.PlatformType != PlatformType.Unknown ) return true; return false; } }
		public float SupportWeight { get { return (PlatformInformation.PlatformType == PlatformType.WindowsNT) ? 0.5f : ((PlatformInformation.PlatformType == PlatformType.Unknown) ? 0 : 1.0f); } }

		public bool Initialize ( bool audioIncluded = true )
		{
			if ( PlatformInformation.PlatformType != PlatformType.WindowsNT &&
				!File.Exists ( "OpenTK.dll.config" ) )
			{
				string contents = null;
				using ( StreamReader r = new StreamReader (
					Assembly.Load ( "Daramkun.Misty.Platform.OpenTK" ).GetManifestResourceStream ( "Daramkun.Misty.Resources.OpenTK.OpenTK.dll.config" )
				) )
					contents = r.ReadToEnd ();
				File.WriteAllText ( "OpenTK.dll.config", contents );
			}

			try
			{
				Core.SetWindow ( new Window () );
				Core.SetGraphicsDevice ( new GraphicsDevice ( Core.Window ) );
			}
			catch ( Exception e ) { Logger.Write ( LogLevel.Level5, e.Message ); return false; }

			try { if ( audioIncluded ) Core.SetAudioDevice ( new AudioDevice ( Core.Window ) ); }
			catch ( Exception e ) { Logger.Write ( LogLevel.Level5, e.Message ); }
			
			Core.Inputs.Add<KeyboardState> ( new Keyboard ( Core.Window ) );
			Core.Inputs.Add<MouseState> ( new Mouse ( Core.Window ) );

			//CommonTransform.HandDirection = HandDirection.RightHand;

			return true;
		}

		public void Dispose ()
		{

		}

		public override string ToString () { return "OpenGL/AL Platform Abstraction Framework"; }
	}
}
