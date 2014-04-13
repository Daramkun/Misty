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

		public FadeTransitor () : this ( Color.Black, 500 ) { }

		public FadeTransitor ( Color fadeColor, float fadeUnit )
		{
			FadeColor = fadeColor;
			FadeUnit = fadeUnit;
			fadeTexture = Core.GraphicsDevice.CreateTexture2D ( 1, 1 );
			fadeTexture.Buffer = new Color [ 1 ] { Color.White };
			fadeSprite = new Sprite ( fadeTexture );
			fadeSpriteWorld = new World2 ();
			fadeSpriteWorld.Scale = Core.GraphicsDevice.BackBuffer.Size;
		}

		private void DrawFade ()
		{
			if ( fadeAlpha > 0 )
			{
				fadeSpriteWorld.Scale = Core.GraphicsDevice.ImmediateContext.CurrentRenderBuffer.Size;
				fadeSprite.OverlayColor = new Color ( FadeColor, fadeAlpha / 255 );
				fadeSprite.Draw ( fadeSpriteWorld );
			}
		}

		public TransitionState Transitioning ( TransitionState currentState, Node scene, GameTime gameTime )
		{
			switch ( currentState )
			{
				case TransitionState.Begin:
					fadeAlpha += ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f * FadeUnit;
					if ( fadeAlpha >= 255 ) fadeAlpha = 255;
					DrawFade ();
					if ( fadeAlpha >= 255 ) return TransitionState.PretransitionEnd;
					return TransitionState.Begin;
				case TransitionState.Posttransition:
				case TransitionState.PretransitionEnd:
					fadeAlpha -= ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f * FadeUnit;
					DrawFade ();
					if ( fadeAlpha <= 0 ) { fadeAlpha = 0; return TransitionState.End; }
					return TransitionState.Posttransition;
				default:
					throw new ArgumentException ();
			}
		}
	}
}
