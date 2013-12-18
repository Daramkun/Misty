using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Inputs.States;

namespace Daramkun.Misty.Inputs.Devices
{
	public class KeyboardEventArgs : EventArgs
	{
		public Key KeyCode { get; set; }

		public KeyboardEventArgs ( Key key )
		{
			KeyCode = key;
		}
	}

	public class BaseKeyboard : StandardDispose, IInputDevice<KeyboardState>
	{
		public virtual bool IsSupport { get { return false; } }
		public virtual bool IsConnected { get { return false; } }
		public virtual bool IsMultiPlayerable { get { return false; } }

		public event EventHandler<KeyboardEventArgs> KeyDown, KeyUp;

		protected void RunKeyDown ( KeyboardEventArgs e ) { if ( KeyDown != null ) KeyDown ( this, e ); }
		protected void RunKeyUp ( KeyboardEventArgs e ) { if ( KeyUp != null ) KeyUp ( this, e ); }

		public virtual KeyboardState GetState ( PlayerIndex index = PlayerIndex.Player1 ) { return new KeyboardState (); }
	}
}
