using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Audios;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Loaders;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;

namespace Test.Game.PlayAudios
{
	[MainNode]
	public class Container : Node
	{
		Font font;
		IAudioBuffer audio1, audio2, audio3, audio4;
		ResourceTable contentManager;

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.BlendState = true;
			Core.GraphicsDevice.BlendOperation = BlendOperation.AlphaBlend;

			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			Texture2DContentLoader.AddDefaultDecoders ();
			AudioContentLoader.AddDefaultDecoders ();

			font = contentManager.Load<TrueTypeFont> ( "Resources/Audio/test.ttf", 24 );

			audio1 = contentManager.Load<IAudioBuffer> ( "Resources/Audio/audio1.ogg" );
			audio2 = contentManager.Load<IAudioBuffer> ( "Resources/Audio/audio2.ogg" );
			audio3 = contentManager.Load<IAudioBuffer> ( "Resources/Audio/audio3.ogg" );
			audio4 = contentManager.Load<IAudioBuffer> ( "Resources/Audio/audio4.ogg" );

			Add ( InputHelper.CreateInstance () );

			base.Intro ( args );
		}

		public override void Outro ()
		{
			contentManager.Dispose ();
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D1 ) )
			{ if ( !audio1.IsPlaying ) audio1.Play (); else audio1.Pause (); }
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D2 ) )
			{ if ( !audio2.IsPlaying ) audio2.Play (); else audio2.Pause (); }
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D3 ) )
			{ if ( !audio3.IsPlaying ) audio3.Play (); else audio3.Pause (); }
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D4 ) )
			{ if ( !audio4.IsPlaying ) audio4.Play (); else audio4.Pause (); }

			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );

			font.DrawFont ( string.Format ( "Audio1: {0}/{1}, Is Playing? {2}", audio1.Position, audio1.Duration, audio1.IsPlaying ),
				Color.White, new Vector2 ( 0, 0 ) );
			font.DrawFont ( string.Format ( "Audio2: {0}/{1}, Is Playing? {2}", audio2.Position, audio2.Duration, audio2.IsPlaying ),
				Color.White, new Vector2 ( 0, 30 ) );
			font.DrawFont ( string.Format ( "Audio3: {0}/{1}, Is Playing? {2}", audio3.Position, audio3.Duration, audio3.IsPlaying ),
				Color.White, new Vector2 ( 0, 60 ) );
			font.DrawFont ( string.Format ( "Audio4: {0}/{1}, Is Playing? {2}", audio4.Position, audio4.Duration, audio4.IsPlaying ),
				Color.White, new Vector2 ( 0, 90 ) );

			base.Draw ( gameTime );

			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Play Audios"; }
	}
}
