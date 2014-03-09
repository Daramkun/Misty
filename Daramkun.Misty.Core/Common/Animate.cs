using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Common
{
	public class Animate
	{
		TimeSpan lastElapsed;

		public TimeSpan Position { get; private set; }
		public TimeSpan Duration { get; set; }
		public double Objective { get; set; }
		public double Animated { get; private set; }
		public double TotalAnimated { get; private set; }
		public bool IsAnimating { get; private set; }
		public bool IsLoopingMode { get; set; }

		public Animate () { Stop (); }
		public Animate ( TimeSpan duration, double objective ) { Duration = duration; Objective = objective; }

		public void Start () { IsAnimating = true; }
		public void Stop () { IsAnimating = false; Position = new TimeSpan (); Animated = 0; TotalAnimated = 0; }
		public void Pause () { IsAnimating = false; }

		public void Update ( GameTime gameTime )
		{
			if ( !IsAnimating ) return;
			if ( Position >= Duration ) { if ( IsLoopingMode ) { Position -= Duration; TotalAnimated -= Objective; Animated = 0; } else { IsAnimating = false; return; } }

			Position += gameTime.ElapsedGameTime;
			double totalAnimated = TotalAnimated;
			TotalAnimated = Position.TotalMilliseconds / Duration.TotalMilliseconds * Objective;
			Animated = TotalAnimated - totalAnimated;

			if ( Objective <= TotalAnimated )
			{
				TotalAnimated = Objective;
				IsAnimating = false;
			}

			lastElapsed = gameTime.ElapsedGameTime;
		}
	}
}
