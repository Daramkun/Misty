using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Nodes.Spirit
{
	public class FontNode : Node
	{
		Font font;

		public Color Color { get; set; }
		public string Text { get; set; }
		public IEffect Effect { get { return font.Effect; } set { font.Effect = value; } }
		public Vector2 Position { get; set; }
		public Font Font { get { return font; } set { font = value; } }
		public SpriteAlignment Alignment { get; set; }

		public FontNode ( Font font )
		{
			this.font = font;
		}

		public override void Draw ( TimeSpan gameTime )
		{
			Vector2 position = Position;
			Vector2 measure = ( Alignment != 0 ) ? font.MeasureString ( Text ) : new Vector2 ();

			if ( ( Alignment & SpriteAlignment.Center ) != 0 ) position += new Vector2 ( -measure.X / 2, 0 );
			if ( ( Alignment & SpriteAlignment.Right ) != 0 ) position += new Vector2 ( -measure.X, 0 );

			if ( ( Alignment & SpriteAlignment.Middle ) != 0 ) position += new Vector2 ( 0, -measure.Y / 2 );
			if ( ( Alignment & SpriteAlignment.Bottom ) != 0 ) position += new Vector2 ( 0, -measure.Y );

			font.DrawFont ( Text, Color, position );

			base.Draw ( gameTime );
		}
	}
}
