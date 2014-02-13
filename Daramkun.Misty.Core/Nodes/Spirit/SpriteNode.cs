using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Geometries;
using Daramkun.Misty.Mathematics.Transforms;

namespace Daramkun.Misty.Nodes.Spirit
{
	public enum SpriteAlignment : byte
	{
		Left = 0,
		Center = 1 << 0,
		Right = 1 << 1,

		Top = 0,
		Middle = 1 << 2,
		Bottom = 1 << 3,

		LeftTop = 0,
		LeftMiddle = Left | Middle,
		LeftBottom = Left | Bottom,

		CenterTop = Center,
		CenterMiddle = Center | Middle,
		CenterBottom = Center | Bottom,

		RightTop = Right,
		RightMiddle = Right | Middle,
		RightBottom = Right | Bottom,
	}

	public class SpriteNode : Node
	{
		Sprite sprite;
		World2 world;
		World2 tempWorld;
		Vector2 moveUnit;
		SpriteAlignment align;

		public Color OverlayColor { get { return sprite.OverlayColor; } set { sprite.OverlayColor = value; } }
		public Rectangle ClippingArea { get { return sprite.ClippingArea; } set { sprite.ClippingArea = value; CalculateMoveUnit (); } }
		public IEffect Effect { get { return sprite.Effect; } set { sprite.Effect = value; } }
		public World2 World { get { return world; } set { world = value; CalculateMoveUnit (); } }
		public ITexture2D Texture { get { return sprite.Texture; } set { sprite.Reset ( value ); CalculateMoveUnit (); } }
		public TextureFilter TextureFilter { get { return sprite.TextureFilter; } set { sprite.TextureFilter = value; } }
		public TextureAddressing TextureAddressing { get { return sprite.TextureAddressing; } set { sprite.TextureAddressing = value; } }
		public SpriteAlignment Alignment { get { return align; } set { align = value; CalculateMoveUnit (); } }

		public SpriteNode ( ITexture2D texture )
			: this ( new Sprite ( texture ) )
		{ }

		public SpriteNode ( ImageInfo imageInfo, Color? colorKey = null )
			: this ( Core.GraphicsDevice.CreateTexture2D ( imageInfo, colorKey ) )
		{ }

		public SpriteNode ( Sprite sprite )
		{
			this.sprite = sprite;
			OverlayColor = Color.White;
			World = World2.Identity;
			tempWorld = World2.Identity;
			Alignment = SpriteAlignment.LeftTop;
			CalculateMoveUnit ();
		}

		public override void Draw ( GameTime gameTime )
		{
			Vector2.Add ( ref World.Translate, ref moveUnit, out tempWorld.Translate );
			if ( tempWorld.Scale != World.Scale ) CalculateMoveUnit ();
			tempWorld.Scale = World.Scale;
			tempWorld.ScaleCenter = World.ScaleCenter;
			tempWorld.Rotation = World.Rotation;
			tempWorld.RotationCenter = World.RotationCenter;

			sprite.Draw ( tempWorld );

			base.Draw ( gameTime );
		}

		private void CalculateMoveUnit ()
		{
			moveUnit = new Vector2 ();
			if ( ( Alignment & SpriteAlignment.Center ) != 0 ) moveUnit += new Vector2 ( -sprite.ClippingArea.Size.X * tempWorld.Scale.X / 2, 0 );
			else if ( ( Alignment & SpriteAlignment.Right ) != 0 ) moveUnit += new Vector2 ( -sprite.ClippingArea.Size.X * tempWorld.Scale.X, 0 );
			if ( ( Alignment & SpriteAlignment.Middle ) != 0 ) moveUnit += new Vector2 ( 0, -sprite.ClippingArea.Size.Y * tempWorld.Scale.Y / 2 );
			else if ( ( Alignment & SpriteAlignment.Bottom ) != 0 ) moveUnit += new Vector2 ( 0, -sprite.ClippingArea.Size.Y * tempWorld.Scale.Y );
		}
	}
}
