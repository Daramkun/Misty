using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Inputs.States;

namespace Daramkun.Misty.Inputs.Devices
{
	public class BaseAccelerometer : StandardDispose, IInputDevice<AccelerometerState>
	{
		public virtual bool IsSupport { get { return false; } }
		public virtual bool IsConnected { get { return false; } }
		public virtual bool IsMultiPlayerable { get { return false; } }

		public virtual void Start () { }
		public virtual void Stop () { }

		public virtual AccelerometerState GetState ( PlayerIndex index = PlayerIndex.Player1 ) { return new AccelerometerState (); }
	}
}
