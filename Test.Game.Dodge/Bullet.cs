using System;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Common;
using Daramkun.Misty.Nodes.Spirit;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Common.Randoms;

namespace Test.Game.Dodge
{
	public class Bullet : SpriteNode
	{
		static Random random = new Well512Random ( Environment.TickCount );

		float angle;

		public Bullet ( ITexture2D texture )
			: base ( texture )
		{
			Alignment = SpriteAlignment.CenterMiddle;
			World.RotationCenter = texture.Size / 2;
		}

		private void InitializeBullet()
		{
			SpriteNode playerSprite = Parent.Parent [ 0 ] [ 0 ] as SpriteNode;
			switch ( random.Next ( 8 ) )
			{
				case 0:
				case 7:
					World.Translate = new Vector2 ( 0, random.Next ( 600 ) );
					break;
				case 1:
				case 6:
					World.Translate = new Vector2 ( 800, random.Next ( 600 ) );
					break;
				case 2:
				case 5:
					World.Translate = new Vector2 ( random.Next ( 800 ), 0 );
					break;
				case 3:
				case 4:
					World.Translate = new Vector2 ( random.Next ( 800 ), 600 );
					break;
			}
			angle = ( float ) Math.Atan2 ( playerSprite.World.Translate.Y - World.Translate.Y, playerSprite.World.Translate.X - World.Translate.X );
		}

		public override void Intro ( params object [] args )
		{
			InitializeBullet ();
			base.Intro ( args );
		}

		public override void Outro ()
		{
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			World.Translate += new Vector2 ( ( float ) Math.Cos ( angle ), ( float ) Math.Sin ( angle ) ) *
				( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 10.0f );

			if ( World.Translate.X < 0 || World.Translate.X > 800 )
				InitializeBullet ();
			if ( World.Translate.Y < 0 || World.Translate.Y > 600 )
				InitializeBullet ();

			SpriteNode chr = Parent.Parent [ 0 ] [ 1 ] as SpriteNode;
			if ( CheckCollaps ( World.Translate, Texture.Width / 2, chr.World.Translate, chr.Texture.Width / 2 ) )
				( Parent.Parent as GameScene ).IsGameOver = true;

			base.Update ( gameTime );
		}

		private bool CheckCollaps ( Vector2 v1, float r1, Vector2 v2, float r2 )
		{
			return Vector2.Distance ( v1, v2 ) <= r1 + r2;
		}

		public override void Draw ( GameTime gameTime )
		{
			base.Draw ( gameTime );
		}
	}
}