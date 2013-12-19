using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Daramkun.Misty;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.Decoders.Images;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Log;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Platforms;

namespace Test.Desktop
{
	class TestNode : Node
	{
		World2 world;
		Sprite sprite;
		Sprite sprite2;
		Font font;

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.BlendState = true;
			Core.GraphicsDevice.BlendOperation = BlendOperation.AlphaBlend;

			ImageInfo imageInfo;
			new BitmapDecoder ().Decode ( Assembly.GetEntryAssembly ().GetManifestResourceStream ( "Test.Desktop.logo.bmp" ), out imageInfo );
			sprite = new Sprite ( Core.GraphicsDevice.CreateTexture2D ( imageInfo, null, 5 ) );
			new PngDecoder ().Decode ( Assembly.GetEntryAssembly ().GetManifestResourceStream ( "Test.Desktop.test.png" ), out imageInfo );
			sprite2 = new Sprite ( Core.GraphicsDevice.CreateTexture2D ( imageInfo, null, 5 ) );
			sprite2.TextureFilter = TextureFilter.Linear;
			world = World2.Identity;
			world.Translate = Core.GraphicsDevice.BackBuffer.Size / 2 - sprite.Texture.Size / 2;
			font = new TrueTypeFont ( Assembly.GetEntryAssembly ().GetManifestResourceStream ( "Test.Desktop.GameFont.ttf" ), 64 );
			base.Intro ( args );
		}

		public override void Outro ()
		{
			font.Dispose ();
			sprite2.Dispose ();
			sprite.Dispose ();
			base.Outro ();
		}

		public override void Draw ( TimeSpan gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Magenta, 1, 0 );

			sprite.Draw ( world );
			//sprite2.Draw ( world );
			font.DrawFont ( "Hello, Mr! 한글", Color.Black, new Daramkun.Misty.Mathematics.Vector2 ( 0.0001f, 0.0001f ) );

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
			Core.Run ( new Launcher (), new TestNode () );
		}
	}
}
