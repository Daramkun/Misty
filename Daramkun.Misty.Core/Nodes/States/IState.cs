using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Nodes.States
{
	public interface IState
	{
		void Execute ( StateMachine stateMachine );
	}
}
