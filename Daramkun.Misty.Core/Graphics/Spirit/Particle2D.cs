using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;

namespace Daramkun.Misty.Graphics.Spirit
{
	public class Particle2D : Node
	{
		Sprite sprite;
		World2 world;

		public ITexture2D Texture { get { return sprite.Texture; } set { sprite.Reset ( value ); } }
		public Vector2 Position { get { return world.Translate; } set { world.Translate = value; } }
		public Vector2 Velocity { get; set; }
		public float Angle { get { return world.Rotation; } set { world.Rotation = value; } }
		public float AngularVelocity { get; set; }
		public Color OverlayColor { get { return sprite.OverlayColor; } set { sprite.OverlayColor = value; } }
		public float Size { get; set; }
		public TimeSpan TTL { get; set; }

		TimeSpan totalTimeSpan;

		public override bool IsTailEndNode { get { return true; } }

		public Particle2D ( ITexture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color overlayColor, float size, TimeSpan ttl )
		{
			sprite = new Sprite ( texture );
			world = new World2 ( position, new Vector2 ( size ), texture.Size / 2, angle, texture.Size / 2 );
			sprite.OverlayColor = overlayColor;
			Velocity = velocity;
			AngularVelocity = angularVelocity;
			TTL = ttl;
		}

		public override void Update ( GameTime gameTime )
		{
			totalTimeSpan += gameTime.ElapsedGameTime;

			if ( totalTimeSpan >= TTL )
			{
				Parent.Remove ( this );
			}

			Position += Velocity;
			Angle += AngularVelocity;
		}

		public override void Draw ( GameTime gameTime )
		{
			sprite.Draw ( ref world );
		}
	}
}
