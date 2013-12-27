using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics.Spirit;
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

		string text;
		int processingIndex;
		Timer timer;

		public string Text { get { return text; } set { text = value; } }
		public TextSpeed TextSpeed { get; set; }

		public Font Font { get; set; }

		public TextWindow ( Font font, TextSpeed textSpeed = Walnut.TextSpeed.Medium )
		{
			Font = font;
			TextSpeed = textSpeed;

			text = "";
			processingIndex = 0;
		}

		public override void Update ( GameTime gameTime )
		{
			if ( processingIndex < text.Length )
			{
				timer.Update ( gameTime.ElapsedGameTime );
				if ( timer.Check )
				{
					++processingIndex;
					timer.Reset ();
				}
			}
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			base.Draw ( gameTime );
		}
	}
}
