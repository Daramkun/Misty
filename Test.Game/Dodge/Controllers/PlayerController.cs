using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Nodes.Spirit;

namespace Test.Game.Dodge.Controllers
{
	public class PlayerController : Node
	{
		public override void Intro ( params object [] args )
		{
			SpriteNode chr = new SpriteNode ( ( Parent as GameScene ).Contents.Load<ITexture2D> ( "Resources/Dodge/daram.bmp", Color.Magenta ) );
			chr.Alignment = SpriteAlignment.CenterMiddle;
			chr.World.Translate = Core.GraphicsDevice.CurrentRenderBuffer.Size / 2;
			Add ( chr );
			SpriteNode collaps = new SpriteNode ( ( Parent as GameScene ).Contents.Load<ITexture2D> ( "Resources/Dodge/target.bmp", Color.Magenta ) );
			collaps.Alignment = SpriteAlignment.CenterMiddle;
			collaps.World.Translate = Core.GraphicsDevice.CurrentRenderBuffer.Size / 2;
			collaps.OverlayColor = new Color ( 255, 255, 255, 120 );
			Add ( collaps );
			base.Intro ( args );
		}

		public override void Update ( GameTime gameTime )
		{
			Vector2 pos = ( this [ 0 ] as SpriteNode ).World.Translate;
			if ( InputHelper.CurrentKeyboardState.IsKeyDown ( Key.Up ) )
				pos += new Vector2 ( 0, -1 ) * ( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 5.0f );
			if ( InputHelper.CurrentKeyboardState.IsKeyDown ( Key.Down ) )
				pos += new Vector2 ( 0, 1 ) * ( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 5.0f );
			if ( InputHelper.CurrentKeyboardState.IsKeyDown ( Key.Left ) )
				pos += new Vector2 ( -1, 0 ) * ( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 5.0f );
			if ( InputHelper.CurrentKeyboardState.IsKeyDown ( Key.Right ) )
				pos += new Vector2 ( 1, 0 ) * ( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 5.0f );
			( this [ 0 ] as SpriteNode ).World.Translate = ( this [ 1 ] as SpriteNode ).World.Translate = pos;
			base.Update ( gameTime );
		}
	}
}
