using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Daramkun.Misty;
using Daramkun.Misty.Audios;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.Decoders.Audios;
using Daramkun.Misty.Contents.Decoders.Images;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Log;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Platforms;

namespace Test.Desktop
{
	class TestNode : Node
	{
		World2 world;
		Sprite sprite;
		Font font;
		Font font2;

		IAudioBuffer testAudio;

		FpsCalculator calc;

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.CullMode = CullingMode.None;

			Core.GraphicsDevice.BlendState = true;
			Core.GraphicsDevice.BlendOperation = BlendOperation.AlphaBlend;

			Add ( calc = new FpsCalculator () );

			ImageInfo imageInfo;
			new BitmapDecoder ().Decode ( Assembly.GetEntryAssembly ().GetManifestResourceStream ( "Test.Desktop.logo.bmp" ), out imageInfo );
			sprite = new Sprite ( Core.GraphicsDevice.CreateTexture2D ( imageInfo, null, 5 ) );
			new PngDecoder ().Decode ( Assembly.GetEntryAssembly ().GetManifestResourceStream ( "Test.Desktop.test.png" ), out imageInfo );
			world = World2.Identity;
			world.Translate = Core.GraphicsDevice.BackBuffer.Size / 2 - sprite.Texture.Size / 2;
			font = new TrueTypeFont ( Assembly.GetEntryAssembly ().GetManifestResourceStream ( "Test.Desktop.GameFont.ttf" ), 64 );
			font.IsPrerenderMode = true;
			font2 = new TrueTypeFont ( Assembly.GetEntryAssembly ().GetManifestResourceStream ( "Test.Desktop.GameFont.ttf" ), 24 );
			font2.IsPrerenderMode = true;

			AudioInfo audioInfo;
			new OggVorbisDecoder ().Decode ( Assembly.GetEntryAssembly ().GetManifestResourceStream ( "Test.Desktop.test.ogg" ), out audioInfo );
			testAudio = Core.AudioDevice.CreateAudioBuffer ( audioInfo );
			testAudio.Play ();
			base.Intro ( args );
		}

		public override void Outro ()
		{
			testAudio.Dispose ();
			font2.Dispose ();
			font.Dispose ();
			sprite.Dispose ();
			base.Outro ();
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black, 1, 0 );

			sprite.Draw ( world );
			font.DrawFont ( "English 한글 漢字 にほんご Märchen", Color.Black, new Daramkun.Misty.Mathematics.Vector2 ( 0.0001f, 0.0001f ) );
			font2.DrawFont ( Core.GraphicsDevice.Information.BaseRenderer.ToString () + Core.GraphicsDevice.Information.RendererVersion.ToString (),
				Color.White, new Vector2 ( 0, 64 ) );
			font2.DrawFont ( "Draw FPS: " + calc.DrawFPS.ToString (),
				Color.White, new Vector2 ( 0, 64 + 24 ) );
			font2.DrawFont ( string.Format ( "Memory Usage: {0}MB", GC.GetTotalMemory ( false ) / 1024 / 1024 ),
				Color.White, new Vector2 ( 0, 64 + 24 + 24 ) );

			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
			base.Draw ( gameTime );
		}
	}

	static class Program
	{
		[STAThread]
		static void Main ()
		{
			Logger.AddDefaultLogWriter ();
			Core.FixedDrawTimeStep = new TimeSpan ();
			Core.Run ( new Launcher (), new TestNode () );
		}
	}
}
