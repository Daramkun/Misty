using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Inputs.States
{
	public struct KeyboardState
	{
		public Key [] PressedKeys { get; private set; }

		public KeyboardState ( params Key [] keys )
			: this ()
		{
			PressedKeys = new Key [ keys.Length ];
			for ( int i = 0; i < keys.Length; i++ )
				PressedKeys [ i ] = keys [ i ];
		}

		public bool IsKeyDown ( Key key )
		{
			if ( PressedKeys == null )
				return false;
			return PressedKeys.Contains ( key );
		}

		public bool IsKeyUp ( Key key )
		{
			if ( PressedKeys == null )
				return false;
			return !PressedKeys.Contains ( key );
		}
	}
}
