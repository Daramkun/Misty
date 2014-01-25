using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Daramkun.Misty.Log;

namespace Daramkun.Misty.Common
{
	public class HighResolutionGameTime : GameTime
	{
		System.Diagnostics.Stopwatch stopwatch;

		public HighResolutionGameTime ()
		{
			stopwatch = new Stopwatch ();
			stopwatch.Start ();
		}

		protected override void Update ()
		{
			elapsedTimeSpan = stopwatch.Elapsed;
			totalTimeSpan += elapsedTimeSpan;
			stopwatch.Restart ();
		}
	}
}
