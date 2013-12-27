using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Nodes.Spirit;

namespace Daramkun.Misty.Walnut
{
	public class WalnutContainer : Node
	{
		SpriteNode background;

		public ITexture2D Background { get { return background.Texture; } set { background.Texture = value; } }
		public TextWindow TextWindow { get; private set; }

		public WalnutContainer ( Font textWindowFont )
		{
			background = new SpriteNode ( null );
			TextWindow = new TextWindow ( textWindowFont );
		}

		public override void Update ( GameTime gameTime )
		{
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			background.Draw ( gameTime );
			base.Draw ( gameTime );
			TextWindow.Draw ( gameTime );
		}
	}
}
