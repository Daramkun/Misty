using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Nodes
{
	public class TimerScheduler : Node
	{
		Dictionary<Timer, Action<Timer>> timers;

		public TimerScheduler ()
		{
			timers = new Dictionary<Timer, Action<Timer>> ();
		}

		public void AddTimer ( Timer timer, Action<Timer> action )
		{
			timers.Add ( timer, action );
		}

		public void RemoveTimer ( Timer timer )
		{
			timers.Remove ( timer );
		}

		public override void Update ( TimeSpan gameTime )
		{
			List<Timer> removeTimers = new List<Timer> ();
			foreach ( KeyValuePair<Timer, Action<Timer>> pair in timers.ToArray () )
			{
				pair.Key.Update ( gameTime );
				if ( pair.Key.Check )
				{
					pair.Key.Reset ();
					pair.Value ( pair.Key );
					removeTimers.Add ( pair.Key );
				}
			}
			foreach ( Timer timer in removeTimers )
				timers.Remove ( timer );
			base.Update ( gameTime );
		}
	}
}
