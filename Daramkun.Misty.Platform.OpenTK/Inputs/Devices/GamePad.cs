using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty.Inputs.Devices
{
	public class GamePad : BaseGamePad
	{
		public GamePad(IWindow window)
		{

		}

		public override bool IsVibrationSupport { get { return true; } }
		
		public override bool IsConnectedPlayer ( PlayerIndex index = PlayerIndex.Player1 )
		{
			return OpenTK.Input.GamePad.GetCapabilities ( ( int ) index ).IsConnected;
		}

		public override States.GamePadState GetState ( PlayerIndex index = PlayerIndex.Player1 )
		{
			OpenTK.Input.GamePadState state = OpenTK.Input.GamePad.GetState ( ( int ) index );

			GamePadButton gamePadButton = GamePadButton.None;

			if ( state.DPad.IsLeft ) gamePadButton |= GamePadButton.DPadLeft;
			if ( state.DPad.IsRight ) gamePadButton |= GamePadButton.DPadRight;
			if ( state.DPad.IsUp ) gamePadButton |= GamePadButton.DPadUp;
			if ( state.DPad.IsDown ) gamePadButton |= GamePadButton.DPadDown;

			if ( state.Buttons.A == OpenTK.Input.ButtonState.Pressed ) gamePadButton |= GamePadButton.A;
			if ( state.Buttons.B  == OpenTK.Input.ButtonState.Pressed ) gamePadButton |= GamePadButton.B;
			if ( state.Buttons.X  == OpenTK.Input.ButtonState.Pressed ) gamePadButton |= GamePadButton.X;
			if ( state.Buttons.Y  == OpenTK.Input.ButtonState.Pressed ) gamePadButton |= GamePadButton.Y;

			if ( state.Buttons.Start  == OpenTK.Input.ButtonState.Pressed ) gamePadButton |= GamePadButton.Start;
			if ( state.Buttons.Back  == OpenTK.Input.ButtonState.Pressed) gamePadButton |= GamePadButton.Back;

			if ( state.Buttons.LeftShoulder == OpenTK.Input.ButtonState.Pressed ) gamePadButton |= GamePadButton.LeftBumper;
			if ( state.Buttons.RightShoulder == OpenTK.Input.ButtonState.Pressed ) gamePadButton |= GamePadButton.RightBumper;

			if ( state.Buttons.LeftStick  == OpenTK.Input.ButtonState.Pressed ) gamePadButton |= GamePadButton.LeftThumbStick;
			if ( state.Buttons.RightStick == OpenTK.Input.ButtonState.Pressed ) gamePadButton |= GamePadButton.RightThumbStick;

			if ( state.Triggers.Left > 0.5f ) gamePadButton |= GamePadButton.LeftTrigger;
			if ( state.Triggers.Right > 0.5f ) gamePadButton |= GamePadButton.RightTrigger;

			return new Daramkun.Misty.Inputs.States.GamePadState (
				new Vector2 ( state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y ),
				new Vector2 ( state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y ),
				state.Triggers.Left / 255.0f,
				state.Triggers.Right / 255.0f,
				gamePadButton
			);
		}
		
		public override void Vibrate ( float leftSpeed, float rightSpeed, PlayerIndex index = PlayerIndex.Player1 )
		{
			OpenTK.Input.GamePad.SetVibration ( ( int ) index, leftSpeed, rightSpeed );
		}
	}
}
