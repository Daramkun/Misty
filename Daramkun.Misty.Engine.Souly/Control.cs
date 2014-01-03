using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Inputs.Devices;
using Daramkun.Misty.Inputs.States;

namespace Daramkun.Misty.Souly
{
	public class Control : Node
	{
		Vector2 position, size;
		KeyboardEventArgs keyEvent;
		MouseButtonEventArgs mouseButtonEvent;
		MouseWheelEventArgs mouseWheelEvent;

		public Vector2 Position { get { return position; } set { position = value; if ( Moving != null ) Moving ( this, null ); } }
		public Vector2 Size { get { return size; } set { size = value; if ( Resizing != null ) Resizing ( this, null ); } }

		public Control ()
		{
			keyEvent = new KeyboardEventArgs ( Key.Unknown );
			mouseButtonEvent = new MouseButtonEventArgs ( new MouseState () );
			mouseWheelEvent = new MouseWheelEventArgs ( 0 );
		}

		public override void Update ( GameTime gameTime )
		{
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			base.Draw ( gameTime );
		}

		public event EventHandler<KeyboardEventArgs> KeyDown, KeyUp;
		public event EventHandler<MouseButtonEventArgs> MouseDown, MouseMove, MouseUp;
		public event EventHandler<MouseWheelEventArgs> MouseWheel;
		public event EventHandler Resizing, Moving;
		public event EventHandler Activated, Deactivated;

		internal void KeyDownCaller ( Key key ) { if ( KeyDown != null ) { keyEvent.KeyCode = key; KeyDown ( this, keyEvent ); } }
		internal void KeyUpCaller ( Key key ) { if ( KeyUp != null ) { keyEvent.KeyCode = key; KeyUp ( this, keyEvent ); } }
		internal void MouseDownCaller ( MouseButton button, Vector2 pos )
		{
			if ( MouseDown != null )
			{
				mouseButtonEvent.MouseState = new MouseState ( pos, 0, button );
				MouseDown ( this, mouseButtonEvent );
			}
		}
		internal void MouseMoveCaller ( MouseButton button, Vector2 pos )
		{
			if ( MouseMove != null )
			{
				mouseButtonEvent.MouseState = new MouseState ( pos, 0, button );
				MouseMove ( this, mouseButtonEvent );
			}
		}
		internal void MouseUpCaller ( MouseButton button, Vector2 pos )
		{
			if ( MouseUp != null )
			{
				mouseButtonEvent.MouseState = new MouseState ( pos, 0, button );
				MouseUp ( this, mouseButtonEvent );
			}
		}
		internal void MouseWheelCaller ( float delta )
		{
			if ( MouseWheel != null )
			{
				mouseWheelEvent.Wheel = delta;
				MouseWheel ( this, mouseWheelEvent );
			}
		}
		internal void ActivatedCaller () { if ( Activated != null ) Activated ( this, null ); }
		internal void DeactivatedCaller () { if ( Deactivated != null ) Deactivated ( this, null ); }
	}
}
