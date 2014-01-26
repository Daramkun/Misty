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

namespace Daramkun.Misty.Platforms
{
	public class Launcher : ILauncher
	{
		public PlatformInformation PlatformInformation
		{
			get
			{
				return new PlatformInformation ()
				{
					PlatformType = PlatformType.WindowsNT,
					PlatformVersion = Environment.OSVersion.Version,
					UserName = Environment.UserName,
					MachineUniqueIdentifier = NetworkInterface.GetAllNetworkInterfaces () [ 0 ].GetPhysicalAddress ().ToString ()
				};
			}
		}

		public bool Initialize ( bool audioIncluded = true )
		{
			try
			{
				Core.SetWindow ( new Window () );
				Core.SetGraphicsDevice ( new GraphicsDevice ( Core.Window ) );
			}
			catch ( Exception e ) { Logger.Write ( LogLevel.Level5, e.Message ); return false; }

			//try { if ( audioIncluded ) Core.SetAudioDevice ( new AudioDevice ( Core.Window ) ); }
			//catch ( Exception e ) { Logger.Write ( LogLevel.Level5, e.Message ); }

			Core.Inputs.Add<KeyboardState> ( new Keyboard ( Core.Window ) );
			Core.Inputs.Add<MouseState> ( new Mouse ( Core.Window ) );
			Core.Inputs.Add<GamePadState> ( new GamePad ( Core.Window ) );

			CommonTransform.HandDirection = HandDirection.LeftHand;
			return true;
		}

		public void Dispose ()
		{

		}

		public override string ToString () { return "DirectX11 Platform Abstraction Framework"; }
	}
}
