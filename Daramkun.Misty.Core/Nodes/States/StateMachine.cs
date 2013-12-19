using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Nodes.States
{
	public class StateMachine : Node
	{
		IState currentState;

		public void ChangeState ( IState nextState )
		{
			currentState = nextState;
		}

		public override void Update ( GameTime gameTime )
		{
			if ( currentState != null )
				currentState.Execute ( this );
		}

		public override void Draw ( GameTime gameTime ) { }
	}
}
