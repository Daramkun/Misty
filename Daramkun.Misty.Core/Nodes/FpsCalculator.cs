using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Nodes
{
	public class FpsCalculator : Node
	{
		TimeSpan updateSum, drawSum;
		int updateFrame, drawFrame;

		double updateFps, drawFps;

		public float UpdateFPS { get { return ( float ) updateFps; } }
		public float DrawFPS { get { return ( float ) drawFps; } }

		public FpsCalculator ()
		{
			updateSum = new TimeSpan ();
			drawSum = new TimeSpan ();
		}

		public override void Update ( GameTime gameTime )
		{
			updateSum += gameTime.ElapsedGameTime;
			++updateFrame;

			if ( updateSum.TotalSeconds >= 1 )
			{
				updateFps = updateFrame + updateFrame * ( updateSum.TotalSeconds - 1 );
				updateFrame = 0;
				updateSum = new TimeSpan ();
			}

			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			drawSum += gameTime.ElapsedGameTime;
			++drawFrame;

			if ( drawSum.TotalSeconds >= 1 )
			{
				drawFps = drawFrame + drawFrame * ( drawSum.TotalSeconds - 1 );
				drawFrame = 0;
				drawSum = new TimeSpan ();
			}

			base.Draw ( gameTime );
		}
	}
}
