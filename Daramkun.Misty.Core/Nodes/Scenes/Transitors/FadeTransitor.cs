using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Log;
using Daramkun.Misty.Mathematics.Transforms;

namespace Daramkun.Misty.Nodes.Scenes.Transitors
{
	public class FadeTransitor : ISceneTransitor
	{
		ITexture2D fadeTexture;
		Sprite fadeSprite;
		World2 fadeSpriteWorld;
		float fadeAlpha;

		public Color FadeColor { get; set; }
		public float FadeUnit { get; set; }

		public FadeTransitor ()
		{
			FadeColor = Color.Black;
			fadeTexture = Core.GraphicsDevice.CreateTexture2D ( 1, 1 );
			fadeTexture.Buffer = new Color [ 1 ] { Color.White };
			fadeSprite = new Sprite ( fadeTexture );
			fadeSpriteWorld = new World2 ();
			fadeSpriteWorld.Scale = Core.GraphicsDevice.BackBuffer.Size;
			FadeUnit = 500;
		}

		public TransitionState Transitioning ( TransitionState currentState, Node scene, GameTime gameTime )
		{
			if ( fadeAlpha != 0 )
			{
				fadeSpriteWorld.Scale = Core.GraphicsDevice.CurrentRenderBuffer.Size;
				fadeSprite.OverlayColor = new Color ( FadeColor, fadeAlpha / 255 );
				fadeSprite.Draw ( ref fadeSpriteWorld );
			}
			Logger.Write ( LogLevel.Level1, "{0}, {1}", currentState, fadeAlpha );

			switch ( currentState )
			{
				case TransitionState.Begin:
					fadeAlpha += gameTime.ElapsedGameTime.Milliseconds / 1000.0f * FadeUnit;
					if ( fadeAlpha >= 255 ) { fadeAlpha = 255; return TransitionState.PretransitionEnd; }
					return TransitionState.Begin;
				case TransitionState.Posttransition:
				case TransitionState.PretransitionEnd:
					fadeAlpha -= gameTime.ElapsedGameTime.Milliseconds / 1000.0f * FadeUnit;
					if ( fadeAlpha <= 0 ) { fadeAlpha = 0; return TransitionState.End; }
					return TransitionState.Posttransition;
				default:
					throw new ArgumentException ();
			}
		}
	}
}
