using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Nodes.States
{
	public class StateMachine : Node
	{
		IState currentState;

		public void ChangeState ( IState nextState )
		{
			currentState = nextState;
		}

		public override void Update ( TimeSpan gameTime )
		{
			if ( currentState != null )
				currentState.Execute ( this );
		}

		public override void Draw ( TimeSpan gameTime ) { }
	}
}
