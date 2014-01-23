using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Platforms.GameLoopers
{
	public class PlainGameLooper : IGameLooper
	{
		public void Run ( Action updateLoop, Action drawLoop, ref bool isRunningMode )
		{
			while ( Core.Window.IsAlive && isRunningMode )
			{
				updateLoop ();
				drawLoop ();
			}
		}

		public override string ToString () { return "Plain Game Looper (Linear Singlethreaded)"; }
	}
}
