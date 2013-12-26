using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Daramkun.Misty.Common;
using Daramkun.Misty.Inputs.States;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Inputs.Devices
{
	public class Keyboard : BaseKeyboard
	{
		IWindow window;
		private List<Key> pressedKeys;
		Key [] nullKeys = new Key [ 0 ];
		KeyboardEventArgs eventArgs = new KeyboardEventArgs ( Key.Unknown );

		public override bool IsSupport { get { return true; } }
		public override bool IsConnected { get { return true; } }
		public override bool IsMultiPlayerable { get { return false; } }

		public Keyboard ( IWindow window )
		{
			this.window = window;
			pressedKeys = new List<Key> ();

			Form w = window.Handle as Form;
			w.KeyDown += KeyDownEvent;
			w.KeyUp += KeyUpEvent;
		}

		private void KeyDownEvent ( object sender, KeyEventArgs e )
		{
			Key key = ConvertKeys ( e.KeyCode );
			if ( key == Key.Unknown ) return;
			if ( pressedKeys.Contains ( key ) ) return;
			pressedKeys.Add ( key );

			eventArgs.KeyCode = key;
			RunKeyDown ( eventArgs );
		}

		private void KeyUpEvent ( object sender, KeyEventArgs e )
		{
			Key key = ConvertKeys ( e.KeyCode );
			if ( key == Key.Unknown ) return;
			if ( pressedKeys.Contains ( key ) ) pressedKeys.Remove ( key );

			eventArgs.KeyCode = key;
			RunKeyUp ( eventArgs );
		}

		public override KeyboardState GetState ( PlayerIndex index = PlayerIndex.Player1 )
		{
			if ( pressedKeys.Count == 0 ) return new KeyboardState ( nullKeys );
			return new KeyboardState ( pressedKeys.ToArray () );
		}

		#region Converter
		private Key ConvertKeys ( Keys keys )
		{
			switch ( keys )
			{
				case Keys.A: return Key.A;
				case Keys.B: return Key.B;
				case Keys.C: return Key.C;
				case Keys.D: return Key.D;
				case Keys.E: return Key.E;
				case Keys.F: return Key.F;
				case Keys.G: return Key.G;
				case Keys.H: return Key.H;
				case Keys.I: return Key.I;
				case Keys.J: return Key.J;
				case Keys.K: return Key.K;
				case Keys.L: return Key.L;
				case Keys.M: return Key.M;
				case Keys.N: return Key.N;
				case Keys.O: return Key.O;
				case Keys.P: return Key.P;
				case Keys.Q: return Key.Q;
				case Keys.R: return Key.R;
				case Keys.S: return Key.S;
				case Keys.T: return Key.T;
				case Keys.U: return Key.U;
				case Keys.V: return Key.V;
				case Keys.W: return Key.W;
				case Keys.X: return Key.X;
				case Keys.Y: return Key.Y;
				case Keys.Z: return Key.Z;

				case Keys.F1: return Key.F1;
				case Keys.F2: return Key.F2;
				case Keys.F3: return Key.F3;
				case Keys.F4: return Key.F4;
				case Keys.F5: return Key.F5;
				case Keys.F6: return Key.F6;
				case Keys.F7: return Key.F7;
				case Keys.F8: return Key.F8;
				case Keys.F9: return Key.F9;
				case Keys.F10: return Key.F10;
				case Keys.F11: return Key.F11;
				case Keys.F12: return Key.F12;

				case Keys.D0: return Key.D0;
				case Keys.D1: return Key.D1;
				case Keys.D2: return Key.D2;
				case Keys.D3: return Key.D3;
				case Keys.D4: return Key.D4;
				case Keys.D5: return Key.D5;
				case Keys.D6: return Key.D6;
				case Keys.D7: return Key.D7;
				case Keys.D8: return Key.D8;
				case Keys.D9: return Key.D9;

				case Keys.Back: return Key.Backspace;
				case Keys.Enter: return Key.Return;
				case Keys.Tab: return Key.Tab;
				case Keys.CapsLock: return Key.Capital;
				case Keys.Escape: return Key.Escape;
				case Keys.Space: return Key.Space;

				/*case Keys.ControlLeft: return Key.LeftControl;
				case Keys.ControlRight: return Key.RightControl;
				case Keys.AltLeft: return Key.LeftAlt;
				case Keys.AltRight: return Key.RightAlt;
				case Keys.ShiftLeft: return Key.LeftShift;
				case Keys.ShiftRight: return Key.RightShift;*/
				case Keys.LWin: return Key.LeftWin;
				case Keys.RWin: return Key.RightWin;

				case Keys.Left: return Key.Left;
				case Keys.Right: return Key.Right;
				case Keys.Up: return Key.Up;
				case Keys.Down: return Key.Down;

				case Keys.Insert: return Key.Insert;
				case Keys.Delete: return Key.Delete;
				case Keys.Home: return Key.Home;
				case Keys.End: return Key.End;
				case Keys.PageUp: return Key.PageUp;
				case Keys.PageDown: return Key.PageDown;

				/*case Keys.BracketLeft: return Key.LeftBracket;
				case Keys.BracketRight: return Key.RightBracket;
				case Keys.BackSlash: return Key.BackSlash;
				case Keys.Comma: return Key.Comma;
				case Keys.Period: return Key.Period;
				case Keys.Minus: return Key.Subtract;
				case Keys.Plus: return Key.Equal;
				case Keys.Semicolon: return Key.Semicolon;
				case Keys.Slash: return Key.Slash;
				case Keys.Tilde: return Key.Grave;
				case Keys.Quote: return Key.Apostrophe;*/
			}

			return Key.Unknown;
		}
		#endregion
	}
}
