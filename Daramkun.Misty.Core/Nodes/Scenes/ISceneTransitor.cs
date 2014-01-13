using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Nodes.Scenes
{
	public enum TransitionState
	{
		None,
		Begin,
		Pretransition,
		PretransitionEnd,
		Posttransition,
		End,
	}

	public interface ISceneTransitor
	{
		TransitionState Transitioning ( TransitionState currentState, Node scene, GameTime gameTime );
	}
}
