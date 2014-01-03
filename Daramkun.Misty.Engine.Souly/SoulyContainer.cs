using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Nodes;

namespace Daramkun.Misty.Souly
{
	public sealed class SoulyContainer : Node
	{
		public static Theme GeneralTheme { get; set; }

		public Node Background { get; set; }

		public override void Intro ( params object [] args )
		{
			Add ( InputHelper.CreateInstance () );
			base.Intro ( args );
		}

		public override void Update ( GameTime gameTime )
		{
			Background.Update ( gameTime );
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Background.Draw ( gameTime );
			base.Draw ( gameTime );
		}
	}
}
