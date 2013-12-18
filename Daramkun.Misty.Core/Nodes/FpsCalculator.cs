using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Nodes
{
	public class FpsCalculator : Node
	{
		TimeSpan updateSum, drawSum;
		int updateFrame, drawFrame;

		float updateFps, drawFps;

		public float UpdateFPS { get { return updateFps; } }
		public float DrawFPS { get { return drawFps; } }

		public FpsCalculator ()
		{
			updateSum = new TimeSpan ();
			drawSum = new TimeSpan ();
		}

		public override void Update ( TimeSpan gameTime )
		{
			updateSum += gameTime;
			++updateFrame;

			if ( updateSum.TotalSeconds >= 1 )
			{
				updateFps = updateFrame + updateFrame * ( float ) ( updateSum.TotalSeconds - 1 );
				updateFrame = 0;
				updateSum = new TimeSpan ();
			}

			base.Update ( gameTime );
		}

		public override void Draw ( TimeSpan gameTime )
		{
			drawSum += gameTime;
			++drawFrame;

			if ( drawSum.TotalSeconds >= 1 )
			{
				drawFps = drawFrame + drawFrame * ( float ) ( drawSum.TotalSeconds - 1 );
				drawFrame = 0;
				drawSum = new TimeSpan ();
			}

			base.Draw ( gameTime );
		}
	}
}
