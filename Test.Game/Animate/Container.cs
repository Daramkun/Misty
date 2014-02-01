using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Nodes;

namespace Test.Game.Animate
{
	[MainNode]
	public class Container : Node
	{
		public override void Intro ( params object [] args )
		{
			base.Intro ( args );
		}

		public override void Outro ()
		{
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			base.Draw ( gameTime );
		}

		public override string ToString () { return "Animate"; }
	}
}
