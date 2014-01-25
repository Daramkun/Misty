using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Daramkun.Misty.Platforms.GameLoopers
{
	public class ParallelGameLooper : IGameLooper
	{
		Thread updateThread;

		public void Run ( Action updateLoop, Action drawLoop, ref bool isRunningMode )
		{
			updateThread = new Thread ( () =>
			{
				while ( Core.Window.IsAlive )
				{
					updateLoop ();
					Thread.Sleep ( 1 );
				}
			} );
			updateThread.Start ();

			try
			{
				Process process = Process.GetCurrentProcess ();
				foreach ( ProcessThread processThread in process.Threads )
					processThread.ProcessorAffinity = process.ProcessorAffinity;
			}
			catch { }

			while ( Core.Window.IsAlive && isRunningMode )
			{
				drawLoop ();
			}

			updateThread.Abort ();
		}

		public override string ToString () { return "Parallel Game Looper (Multithreaded)"; }
	}
}
