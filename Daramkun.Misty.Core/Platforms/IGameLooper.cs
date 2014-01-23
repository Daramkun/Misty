using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Platforms
{
	public interface IGameLooper
	{
		void Run ( Action updateLoop, Action drawLoop, ref bool isRunningMode );
	}
}
