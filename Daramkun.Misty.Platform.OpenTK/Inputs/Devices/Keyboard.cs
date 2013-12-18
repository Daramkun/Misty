using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Inputs.States;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Inputs.Devices
{
	public class Keyboard : BaseKeyboard
	{
		OpenTK.GameWindow w;
		List<Key> pressedKeys = new List<Key> ();
		Key [] nullKeys = new Key [ 0 ];
		KeyboardEventArgs eventArgs = new KeyboardEventArgs ( Key.Unknown );

		public override bool IsConnected { get { return true; } }
		public override bool IsSupport { get { return true; } }
		public override bool IsMultiPlayerable { get { return false; } }

		public Keyboard ( IWindow window )
		{
			w = window.Handle as OpenTK.GameWindow;
			w.Keyboard.KeyDown += Keyboard_KeyDown;
			w.Keyboard.KeyUp += Keyboard_KeyUp;
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				w.Keyboard.KeyUp -= Keyboard_KeyUp;
				w.Keyboard.KeyDown -= Keyboard_KeyDown;
			}
			base.Dispose ( isDisposing );
		}

		void Keyboard_KeyDown ( object sender, OpenTK.Input.KeyboardKeyEventArgs e )
		{
			Key key = ConvertKeys ( e.Key );
			if ( key == Key.Unknown ) return;
			if ( pressedKeys.Contains ( key ) ) return;
			pressedKeys.Add ( key );

			eventArgs.KeyCode = key;
			RunKeyDown ( eventArgs );
		}

		void Keyboard_KeyUp ( object sender, OpenTK.Input.KeyboardKeyEventArgs e )
		{
			Key key = ConvertKeys ( e.Key );
			if ( key == Key.Unknown ) return;
			if ( pressedKeys.Contains ( key ) ) pressedKeys.Remove ( key );

			eventArgs.KeyCode = key;
			RunKeyUp ( eventArgs );
		}

		public override States.KeyboardState GetState ( PlayerIndex index = PlayerIndex.Player1 )
		{
			if ( pressedKeys.Count == 0 ) return new KeyboardState ( nullKeys );
			return new KeyboardState ( pressedKeys.ToArray () );
		}

		#region Converter
		private Key ConvertKeys ( OpenTK.Input.Key key )
		{
			switch ( key )
			{
				case OpenTK.Input.Key.A: return Key.A;
				case OpenTK.Input.Key.B: return Key.B;
				case OpenTK.Input.Key.C: return Key.C;
				case OpenTK.Input.Key.D: return Key.D;
				case OpenTK.Input.Key.E: return Key.E;
				case OpenTK.Input.Key.F: return Key.F;
				case OpenTK.Input.Key.G: return Key.G;
				case OpenTK.Input.Key.H: return Key.H;
				case OpenTK.Input.Key.I: return Key.I;
				case OpenTK.Input.Key.J: return Key.J;
				case OpenTK.Input.Key.K: return Key.K;
				case OpenTK.Input.Key.L: return Key.L;
				case OpenTK.Input.Key.M: return Key.M;
				case OpenTK.Input.Key.N: return Key.N;
				case OpenTK.Input.Key.O: return Key.O;
				case OpenTK.Input.Key.P: return Key.P;
				case OpenTK.Input.Key.Q: return Key.Q;
				case OpenTK.Input.Key.R: return Key.R;
				case OpenTK.Input.Key.S: return Key.S;
				case OpenTK.Input.Key.T: return Key.T;
				case OpenTK.Input.Key.U: return Key.U;
				case OpenTK.Input.Key.V: return Key.V;
				case OpenTK.Input.Key.W: return Key.W;
				case OpenTK.Input.Key.X: return Key.X;
				case OpenTK.Input.Key.Y: return Key.Y;
				case OpenTK.Input.Key.Z: return Key.Z;

				case OpenTK.Input.Key.F1: return Key.F1;
				case OpenTK.Input.Key.F2: return Key.F2;
				case OpenTK.Input.Key.F3: return Key.F3;
				case OpenTK.Input.Key.F4: return Key.F4;
				case OpenTK.Input.Key.F5: return Key.F5;
				case OpenTK.Input.Key.F6: return Key.F6;
				case OpenTK.Input.Key.F7: return Key.F7;
				case OpenTK.Input.Key.F8: return Key.F8;
				case OpenTK.Input.Key.F9: return Key.F9;
				case OpenTK.Input.Key.F10: return Key.F10;
				case OpenTK.Input.Key.F11: return Key.F11;
				case OpenTK.Input.Key.F12: return Key.F12;

				case OpenTK.Input.Key.Number0: return Key.D0;
				case OpenTK.Input.Key.Number1: return Key.D1;
				case OpenTK.Input.Key.Number2: return Key.D2;
				case OpenTK.Input.Key.Number3: return Key.D3;
				case OpenTK.Input.Key.Number4: return Key.D4;
				case OpenTK.Input.Key.Number5: return Key.D5;
				case OpenTK.Input.Key.Number6: return Key.D6;
				case OpenTK.Input.Key.Number7: return Key.D7;
				case OpenTK.Input.Key.Number8: return Key.D8;
				case OpenTK.Input.Key.Number9: return Key.D9;

				case OpenTK.Input.Key.Back: return Key.Backspace;
				case OpenTK.Input.Key.Enter: return Key.Return;
				case OpenTK.Input.Key.Tab: return Key.Tab;
				case OpenTK.Input.Key.CapsLock: return Key.Capital;
				case OpenTK.Input.Key.Escape: return Key.Escape;
				case OpenTK.Input.Key.Space: return Key.Space;

				case OpenTK.Input.Key.ControlLeft: return Key.LeftControl;
				case OpenTK.Input.Key.ControlRight: return Key.RightControl;
				case OpenTK.Input.Key.AltLeft: return Key.LeftAlt;
				case OpenTK.Input.Key.AltRight: return Key.RightAlt;
				case OpenTK.Input.Key.ShiftLeft: return Key.LeftShift;
				case OpenTK.Input.Key.ShiftRight: return Key.RightShift;
				case OpenTK.Input.Key.WinLeft: return Key.LeftWin;
				case OpenTK.Input.Key.WinRight: return Key.RightWin;

				case OpenTK.Input.Key.Left: return Key.Left;
				case OpenTK.Input.Key.Right: return Key.Right;
				case OpenTK.Input.Key.Up: return Key.Up;
				case OpenTK.Input.Key.Down: return Key.Down;

				case OpenTK.Input.Key.Insert: return Key.Insert;
				case OpenTK.Input.Key.Delete: return Key.Delete;
				case OpenTK.Input.Key.Home: return Key.Home;
				case OpenTK.Input.Key.End: return Key.End;
				case OpenTK.Input.Key.PageUp: return Key.PageUp;
				case OpenTK.Input.Key.PageDown: return Key.PageDown;

				case OpenTK.Input.Key.BracketLeft: return Key.LeftBracket;
				case OpenTK.Input.Key.BracketRight: return Key.RightBracket;
				case OpenTK.Input.Key.BackSlash: return Key.BackSlash;
				case OpenTK.Input.Key.Comma: return Key.Comma;
				case OpenTK.Input.Key.Period: return Key.Period;
				case OpenTK.Input.Key.Minus: return Key.Subtract;
				case OpenTK.Input.Key.Plus: return Key.Equal;
				case OpenTK.Input.Key.Semicolon: return Key.Semicolon;
				case OpenTK.Input.Key.Slash: return Key.Slash;
				case OpenTK.Input.Key.Tilde: return Key.Grave;
				case OpenTK.Input.Key.Quote: return Key.Apostrophe;
			}

			return Key.Unknown;
		}
		#endregion
	}
}
