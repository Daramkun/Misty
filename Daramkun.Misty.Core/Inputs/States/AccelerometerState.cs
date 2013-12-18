using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Inputs.States
{
	public struct AccelerometerState
	{
		public Vector3 Acceleration { get; private set; }

		public AccelerometerState ( Vector3 accel )
			: this ()
		{
			Acceleration = accel;
		}
	}
}
