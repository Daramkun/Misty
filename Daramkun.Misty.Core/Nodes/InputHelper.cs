using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Inputs.States;

namespace Daramkun.Misty.Nodes
{
	public class InputHelper : Node
	{
		static InputHelper instance;
		public static InputHelper Instance
		{
			get
			{
				if ( instance == null )
				{
					instance = new InputHelper ();

					if ( Core.Inputs.GetDevice<KeyboardState> ().IsConnected ) IsKeyboardEnabled = true;
					else IsKeyboardEnabled = false;

					if ( Core.Inputs.GetDevice<MouseState> ().IsConnected ) IsMouseEnabled = true;
					else IsMouseEnabled = false;

					if ( Core.Inputs.GetDevice<GamePadState> ().IsConnected ) IsGamePadEnabled = true;
					else IsGamePadEnabled = false;

					if ( Core.Inputs.GetDevice<TouchState> ().IsConnected ) IsTouchEnabled = true;
					else IsTouchEnabled = false;

					if ( Core.Inputs.GetDevice<AccelerometerState> ().IsConnected ) IsAccelerometerEnabled = true;
					else IsAccelerometerEnabled = false;
				}
				return instance;
			}
		}

		public static KeyboardState LastKeyboardState { get; private set; }
		public static KeyboardState CurrentKeyboardState { get; private set; }

		public static MouseState LastMouseState { get; private set; }
		public static MouseState CurrentMouseState { get; private set; }

		public static GamePadState [] LastGamePadState { get; private set; }
		public static GamePadState [] CurrentGamePadState { get; private set; }

		public static TouchState LastTouchState { get; private set; }
		public static TouchState CurrentTouchState { get; private set; }

		public static AccelerometerState LastAccelerometerState { get; private set; }
		public static AccelerometerState CurrentAccelerometerState { get; private set; }

		public static bool IsKeyboardEnabled { get; set; }
		public static bool IsMouseEnabled { get; set; }
		public static bool IsGamePadEnabled { get; set; }
		public static bool IsTouchEnabled { get; set; }
		public static bool IsAccelerometerEnabled { get; set; }

		public static bool IsKeyboardKeyDownRightNow ( Key key )
		{
			return CurrentKeyboardState.IsKeyDown ( key ) &&
				LastKeyboardState.IsKeyUp ( key );
		}

		public static bool IsKeyboardKeyUpRightNow ( Key key )
		{
			return CurrentKeyboardState.IsKeyUp ( key ) &&
				LastKeyboardState.IsKeyDown ( key );
		}

		public static bool IsMouseButtonDownRightNow ( MouseButton button )
		{
			return CurrentMouseState.IsButtonDown ( button ) &&
				LastMouseState.IsButtonUp ( button );
		}

		public static bool IsMouseButtonUpRightNow ( MouseButton button )
		{
			return CurrentMouseState.IsButtonUp ( button ) &&
				LastMouseState.IsButtonDown ( button );
		}

		public static bool IsGamePadButtonDownRightNow ( GamePadButton button, PlayerIndex playerIndex = PlayerIndex.Player1 )
		{
			return CurrentGamePadState [ ( int ) playerIndex ].IsButtonDown ( button ) &&
				LastGamePadState [ ( int ) playerIndex ].IsButtonUp ( button );
		}

		public static bool IsGamePadButtonUpRightNow ( GamePadButton button, PlayerIndex playerIndex = PlayerIndex.Player1 )
		{
			return CurrentGamePadState [ ( int ) playerIndex ].IsButtonUp ( button ) &&
				LastGamePadState [ ( int ) playerIndex ].IsButtonDown ( button );
		}

		static InputHelper ()
		{
			LastGamePadState = new GamePadState [ 4 ];
			CurrentGamePadState = new GamePadState [ 4 ];
		}

		[Obsolete("Please use the Instance property.", false)]
		public static InputHelper CreateInstance () { return Instance; }

		private InputHelper ()
		{
			IsVisible = false;
		}

		public override void Update ( GameTime gameTime )
		{
			if ( Core.Inputs.GetDevice<KeyboardState> ().IsConnected && IsKeyboardEnabled )
			{
				LastKeyboardState = CurrentKeyboardState;
				CurrentKeyboardState = Core.Inputs.GetDevice<KeyboardState> ().GetState ();
			}

			if ( Core.Inputs.GetDevice<MouseState> ().IsConnected && IsMouseEnabled )
			{
				LastMouseState = CurrentMouseState;
				CurrentMouseState = Core.Inputs.GetDevice<MouseState> ().GetState ();
			}

			if ( Core.Inputs.GetDevice<GamePadState> ().IsConnected )
			{
				for ( int i = 0; i < 4; i++ )
				{
					LastGamePadState [ i ] = CurrentGamePadState [ i ];
					CurrentGamePadState [ i ] = Core.Inputs.GetDevice<GamePadState> ().GetState ( ( PlayerIndex ) i );
				}
			}

			if ( Core.Inputs.GetDevice<TouchState> ().IsConnected )
			{
				LastTouchState = CurrentTouchState;
				CurrentTouchState = Core.Inputs.GetDevice<TouchState> ().GetState ();
			}

			if ( Core.Inputs.GetDevice<AccelerometerState> ().IsConnected )
			{
				LastAccelerometerState = CurrentAccelerometerState;
				CurrentAccelerometerState = Core.Inputs.GetDevice<AccelerometerState> ().GetState ();
			}

			base.Update ( gameTime );
		}
	}
}
