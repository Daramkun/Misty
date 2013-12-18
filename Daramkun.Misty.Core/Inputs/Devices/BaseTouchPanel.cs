using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Inputs.States;

namespace Daramkun.Misty.Inputs.Devices
{
	public class BaseTouchPanel : StandardDispose, IInputDevice<TouchState>
	{
		public virtual bool IsSupport { get { return false; } }
		public virtual bool IsConnected { get { return false; } }
		public virtual bool IsMultiPlayerable { get { return false; } }
		public virtual int MaximumTouchCount { get { return 0; } }

		public virtual TouchState GetState ( PlayerIndex index = PlayerIndex.Player1 ) { return new TouchState (); }
	}
}
