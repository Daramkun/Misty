using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Readers;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;

namespace Test.Game.CSharp.AnimateObject
{
	[MainNode]
	public class Container : Node
	{
		ResourceTable contentManager;

		Sprite sprite;
		Font font;
		World2 world;

		Animate animate;
		Animate loopAnimate;

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.ImmediateContext.BlendState = BlendState.AlphaBlend;

			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			contentManager.AddDefaultContentLoader ();
			Texture2DContentLoader.AddDefaultDecoders ();
			sprite = contentManager.Load<Sprite> ( "Resources/test.jpg", Color.Magenta );
			font = contentManager.Load<TrueTypeFont> ( "Resources/test.ttf", 24 );
			
			animate = new Animate ( TimeSpan.FromSeconds ( 4 ), 400 );
			loopAnimate = new Animate ( TimeSpan.FromSeconds ( 4 ), 400 ) { IsLoopingMode = true };
			loopAnimate.Start ();
			Add ( InputHelper.Instance );
			
			world = new World2 ();
			
			base.Intro ( args );
		}

		public override void Outro ()
		{
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
			loopAnimate.Update ( gameTime );

			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.ImmediateContext.BeginScene ();
			Core.GraphicsDevice.ImmediateContext.Clear ( ClearBuffer.AllBuffer, Color.Black );

			font.DrawFont ( string.Format ( "Animate state: {0}, Position: {1}/{2}, Animated: {3:0.000}/{4:0.000}", animate.IsAnimating,
				animate.Position, animate.Duration, animate.Animated, animate.TotalAnimated ), Color.White,
				new Vector2 ( ( float ) animate.Animated, 0 ) );
			font.DrawFont ( string.Format ( "Animate state: {0}, Position: {1}/{2}, Animated: {3:0.000}/{4:0.000}", loopAnimate.IsAnimating,
				loopAnimate.Position, loopAnimate.Duration, loopAnimate.Animated, loopAnimate.TotalAnimated ), Color.White,
				new Vector2 ( ( float ) loopAnimate.Animated, 60 ) );

			world.Translate = new Vector2 ( ( float ) animate.TotalAnimated, 100 );
			sprite.Draw ( world );
			sprite.Draw ( new Vector2 ( ( float ) loopAnimate.TotalAnimated, 300 ), Vector2.One, Vector2.Zero, 0, Vector2.Zero );
			base.Draw ( gameTime );

			Core.GraphicsDevice.ImmediateContext.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Animate"; }
	}
}
