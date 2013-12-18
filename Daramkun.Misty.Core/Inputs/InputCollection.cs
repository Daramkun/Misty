using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Inputs.Devices;
using Daramkun.Misty.Inputs.States;

namespace Daramkun.Misty.Inputs
{
	public sealed class InputCollection : IDisposable
	{
		Dictionary<Type, IInputDevice> inputDevices = new Dictionary<Type, IInputDevice> ();

		public void Add<T> ( IInputDevice<T> device )
		{
			if ( inputDevices.ContainsKey ( typeof ( T ) ) )
				inputDevices [ typeof ( T ) ] = device;
			else
				inputDevices.Add ( typeof ( T ), device );
		}

		public IInputDevice<T> GetDevice<T> () { return inputDevices [ typeof ( T ) ] as IInputDevice<T>; }

		internal InputCollection ()
		{
			Add<KeyboardState> ( new BaseKeyboard () );
			Add<MouseState> ( new BaseMouse () );
			Add<GamePadState> ( new BaseGamePad () );
			Add<TouchState> ( new BaseTouchPanel () );
			Add<AccelerometerState> ( new BaseAccelerometer () );
		}

		public void Dispose ()
		{
			foreach ( IInputDevice devices in inputDevices.Values )
				devices.Dispose ();
			inputDevices.Clear ();
		}
	}
}
