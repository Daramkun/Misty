using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Common
{
	public class GameTime
	{
		int lastTickCount;
		protected TimeSpan totalTimeSpan;
		protected TimeSpan elapsedTimeSpan;

		public TimeSpan TotalGameTime { get { return totalTimeSpan; } }
		public TimeSpan ElapsedGameTime { get { return elapsedTimeSpan; } }

		public GameTime ()
		{
			lastTickCount = Environment.TickCount;
			totalTimeSpan = new TimeSpan ();
			elapsedTimeSpan = new TimeSpan ();
		}

		protected internal virtual void Update ()
		{
			int nowTickCount = Environment.TickCount;
			elapsedTimeSpan = TimeSpan.FromMilliseconds ( nowTickCount - lastTickCount );
			totalTimeSpan += elapsedTimeSpan;
			lastTickCount = nowTickCount;
		}

		public virtual void Reset ()
		{
			elapsedTimeSpan = new TimeSpan ();
		}
	}
}
