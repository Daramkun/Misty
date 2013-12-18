using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Inputs.States;

namespace Daramkun.Misty.Inputs.Devices
{
	public class BaseGamePad : StandardDispose, IInputDevice<GamePadState>
	{
		public virtual bool IsSupport { get { return false; } }
		public virtual bool IsConnected
		{
			get
			{
				foreach ( PlayerIndex pi in new [] { PlayerIndex.Player1, PlayerIndex.Player2, PlayerIndex.Player3, PlayerIndex.Player4 } )
					if ( IsConnectedPlayer ( pi ) ) return true;
				return false;
			}
		}
		public virtual bool IsMultiPlayerable { get { return false; } }
		public virtual bool IsVibrationSupport { get { return false; } }

		public virtual GamePadState GetState ( PlayerIndex index = PlayerIndex.Player1 ) { return new GamePadState (); }
		public virtual void Vibrate ( float leftSpeed, float rightSpeed, PlayerIndex index = PlayerIndex.Player1 ) { }
		public virtual bool IsConnectedPlayer ( PlayerIndex index = PlayerIndex.Player1 ) { return false; }
	}
}
