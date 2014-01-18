using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Loaders;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Nodes;

namespace Test.Game.PerformanceTester
{
	[MainNode]
	public class Container : Node
	{
		Node [] nodes;
		ITexture2D [] textures;

		FpsCalculator calc;

		ResourceTable contentManager;

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.BlendState = true;
			Core.GraphicsDevice.BlendOperation = BlendOperation.AlphaBlend;

			Add ( InputHelper.CreateInstance () );
			Add ( calc = new FpsCalculator () );

			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			Texture2DContentLoader.AddDefaultDecoders ();

			textures = new ITexture2D [ 6 ];
			textures [ 0 ] = contentManager.Load<ITexture2D> ( "Resources/0096x0096.png" );
			textures [ 1 ] = contentManager.Load<ITexture2D> ( "Resources/0128x0128.png" );
			textures [ 2 ] = contentManager.Load<ITexture2D> ( "Resources/0256x0256.png" );
			textures [ 3 ] = contentManager.Load<ITexture2D> ( "Resources/0512x0512.png" );
			textures [ 4 ] = contentManager.Load<ITexture2D> ( "Resources/1024x1024.png" );
			textures [ 5 ] = contentManager.Load<ITexture2D> ( "Resources/2048x2048.png" );

			nodes = new Node [ 6 ];
			for ( int i = 0; i < 6; ++i )
			{
				nodes [ i ] = Add ( new Node () );
			}

			base.Intro ( args );
		}

		public override void Outro ()
		{
			contentManager.Dispose ();
			base.Outro ();
		}

		private void Add ( int mode, int count )
		{
			nodes [ mode ].IsManuallyChildrenCacheMode = true;
			//Core.Launcher.InvokeInMainThread ( () =>
			//{
				for ( int i = 0; i < count; ++i )
				{
					nodes [ mode ].Add ( new PerformanceSpriteNode ( textures [ mode ] ) );
				}
				nodes [ mode ].RefreshChildrenCache ();
			//}, false );
		}

		private void Remove ( int mode, int count )
		{
			nodes [ mode ].IsManuallyChildrenCacheMode = true;
			for ( int i = 0; i < count; ++i )
			{
				if ( nodes [ mode ].ChildrenCount <= 0 ) continue;
				nodes [ mode ].Remove ( nodes [ mode ] [ nodes [ mode ].ChildrenCount - 1 ] );
			}
			nodes [ mode ].RefreshChildrenCache ();
		}

		public override void Update ( GameTime gameTime )
		{
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.Q ) ) Add ( 0, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.W ) ) Add ( 1, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.E ) ) Add ( 2, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.R ) ) Add ( 3, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.T ) ) Add ( 4, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.Y ) ) Add ( 5, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.A ) ) Remove ( 0, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.S ) ) Remove ( 1, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D ) ) Remove ( 2, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.F ) ) Remove ( 3, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.G ) ) Remove ( 4, 100 );
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.H ) ) Remove ( 5, 100 );

			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.Z ) ) Sprite.IsStripDrawingMode = !Sprite.IsStripDrawingMode;

			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );

			base.Draw ( gameTime );

			int childrenCount = 0;
			for ( int i = 0; i < 6; ++i )
				childrenCount += nodes [ i ].ChildrenCount;
			Core.Window.Title = string.Format ( "Update FPS: {0}, Draw FPS: {1}, Children count: {2}, Is Strip Drawing Mode: {3}",
				calc.UpdateFPS, calc.DrawFPS, childrenCount, Sprite.IsStripDrawingMode );

			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Performance Tester"; }
	}
}
