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

		public override void Intro ( params object [] args )
		{
			ImageInfo imageInfo;
			new BitmapDecoder ().Decode ( Assembly.GetEntryAssembly ().GetManifestResourceStream ( "Test.Desktop.logo.bmp" ), out imageInfo );
			sprite = new Sprite ( Core.GraphicsDevice.CreateTexture2D ( imageInfo, null, 5 ) );
			world = World2.Identity;
			world.Translate = Core.GraphicsDevice.BackBuffer.Size / 2 - sprite.Texture.Size / 2;
			base.Intro ( args );
		}

		public override void Outro ()
		{
			sprite.Dispose ();
			base.Outro ();
		}

		public override void Draw ( TimeSpan gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black, 1, 0 );

			sprite.Draw ( world );

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
