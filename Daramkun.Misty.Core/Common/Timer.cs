using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Common
{
	public struct Timer
	{
		TimeSpan delay;
		TimeSpan lastTimeSpan;

		public TimeSpan Delay { get { return delay; } set { delay = value; } }
		public TimeSpan DeltaTime { get { return lastTimeSpan - delay; } }
		public bool Check { get { return delay <= lastTimeSpan; } }

		public Timer ( TimeSpan delay )
		{
			this.delay = delay;
			this.lastTimeSpan = new TimeSpan ();
		}

		public void Update ( GameTime gameTime )
		{
			Update ( gameTime.ElapsedGameTime );
		}

		public void Update ( TimeSpan gameTime )
		{
			lastTimeSpan += gameTime;
		}

		public void Reset ()
		{
			lastTimeSpan -= delay;
		}

		public void Clear ()
		{
			lastTimeSpan = new TimeSpan ();
		}
	}
}
