using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Nodes.Scenes.Transitors
{
	public class DirectTransitor : ISceneTransitor
	{
		public TransitionState Transitioning ( TransitionState currentState, Node scene, GameTime gameTime )
		{
			if ( currentState == TransitionState.Begin )
				return TransitionState.PretransitionEnd;
			else if ( currentState == TransitionState.PretransitionEnd )
				return TransitionState.End;
			else throw new ArgumentException ();
		}
	}
}
