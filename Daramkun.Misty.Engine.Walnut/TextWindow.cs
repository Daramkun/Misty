using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;

namespace Daramkun.Misty.Walnut
{
	public enum TextSpeed
	{
		Low,
		Medium,
		High,
		Direct,
	}

	public class TextWindow : Node
	{
		private static readonly Timer [] SpeedTimer = new Timer []
		{
			new Timer ( TimeSpan.FromSeconds ( 0.3f ) ),
			new Timer ( TimeSpan.FromSeconds ( 0.2f ) ),
			new Timer ( TimeSpan.FromSeconds ( 0.1f ) ),
		};

		Sprite frameSprite;
		string text;
		int processingIndex;

		public string Text { get { return text; } set { text = value; } }
		public TextSpeed TextSpeed { get; set; }
		public Color ForegroundColor { get; set; }
		public int Height { get; set; }

		public ITexture2D FrameTexture { get { return frameSprite.Texture; } set { frameSprite.Reset ( value ); } }
		public Font Font { get; set; }

		public bool IsDrawToEnd { get { return processingIndex == text.Length; } }

		public TextWindow ( Font font, TextSpeed textSpeed = Walnut.TextSpeed.Medium )
		{
			Font = font;
			TextSpeed = textSpeed;
			Height = 200;

			frameSprite = new Sprite ( null );

			text = "";
			processingIndex = 0;
		}

		public override void Update ( GameTime gameTime )
		{
			if ( processingIndex < text.Length )
			{
				if ( TextSpeed == Walnut.TextSpeed.Direct )
				{
					processingIndex = text.Length;
				}
				else
				{
					SpeedTimer [ ( int ) TextSpeed ].Update ( gameTime.ElapsedGameTime );
					if ( SpeedTimer [ ( int ) TextSpeed ].Check )
					{
						++processingIndex;
						SpeedTimer [ ( int ) TextSpeed ].Reset ();
					}
				}
			}
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			if ( frameSprite.Texture != null )
			{
				
			}

			if ( Font != null )
			{
				Font.DrawFont ( Text, ForegroundColor, new Vector2 ( 10, Core.GraphicsDevice.BackBuffer.Height - Height + 10 ),
					new Vector2 ( Core.GraphicsDevice.BackBuffer.Width - ( 10 + 10 ), Height - ( 10 + 10 ) ),
					0, processingIndex - 1 );
			}
			base.Draw ( gameTime );
		}
	}
}
