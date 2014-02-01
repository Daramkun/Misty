using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Loaders;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;

namespace Test.Game.AnimateObject
{
	[MainNode]
	public class Container : Node
	{
		ResourceTable contentManager;

		Sprite sprite;
		Font font;
		World2 world;

		Animate animate;

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.BlendState = true;
			Core.GraphicsDevice.BlendOperation = BlendOperation.AlphaBlend;

			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			contentManager.AddDefaultContentLoader ();
			Texture2DContentLoader.AddDefaultDecoders ();
			sprite = new Sprite ( contentManager.Load<ITexture2D> ( "Resources/Dodge/daram.bmp", Color.Magenta ) );
			font = contentManager.Load<TrueTypeFont> ( "Resources/test.ttf", 24 );
			
			animate = new Animate ( TimeSpan.FromSeconds ( 4 ), 400 );
			Add ( InputHelper.CreateInstance () );
			
			world = new World2 ();
			
			base.Intro ( args );
		}

		public override void Outro ()
		{
			sprite.Dispose ();
			contentManager.Dispose ();
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.A ) )
				animate.Start ();
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.S ) )
				animate.Pause ();
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D ) )
				animate.Stop ();

			animate.Update ( gameTime );

			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );

			font.DrawFont ( string.Format ( "Animate state: {0}, Position: {1}/{2}, Animated: {3:0.000}/{4:0.000}", animate.IsAnimating,
				animate.Position, animate.Duration, animate.Animated, animate.TotalAnimated ), Color.White,
				new Vector2 ( ( float ) animate.Animated, 0 ) );

			world.Translate = new Vector2 ( ( float ) animate.TotalAnimated, 100 );
			sprite.Draw ( world );

			base.Draw ( gameTime );
			
			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Animate"; }
	}
}
