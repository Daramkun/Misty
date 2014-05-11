using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Readers;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Nodes.Scenes;
using Test.Game.Dodge.Controllers;

namespace Test.Game.Dodge
{
	public class GameScene : Node
	{
		public ResourceTable Contents { get; private set; }

		bool isGameOver;
		public bool IsGameOver
		{
			get { return isGameOver; }
			set
			{
				isGameOver = value;
				this [ 0 ].IsEnabled = this [ 1 ].IsEnabled = !value;
			}
		}

		Font gameOverFont, timeStampFont;
		TimeSpan timeStamp;

		public override void Intro ( params object [] args )
		{
			Contents = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			Contents.AddDefaultContentLoader ();

			Add ( new PlayerController () );
			Add ( new BulletController () );

			gameOverFont = Contents.Load<TrueTypeFont> ( "Resources/test.ttf", 64 );
			timeStampFont = Contents.Load<TrueTypeFont> ( "Resources/test.ttf", 32 );
			base.Intro ( args );
		}

		public override void Outro ()
		{
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			if ( !isGameOver )
			{
				timeStamp += gameTime.ElapsedGameTime;
			}
			else
			{
				if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.Space ) )
				{
					( Parent as SceneContainer ).Transition ( new MenuScene () );
				}
			}
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			base.Draw ( gameTime );

			string currentTime = string.Format ( "ELAPSED: {0:0.00}sec", timeStamp.TotalSeconds );
			timeStampFont.DrawFont ( currentTime, Color.White,
				new Vector2 () );

			if ( isGameOver )
			{
				gameOverFont.DrawFont ( "GAME OVER", Color.White,
					Core.GraphicsDevice.ImmediateContext.CurrentRenderBuffer.Size () / 2 - gameOverFont.MeasureString ( "GAME OVER" ) / 2 );
				string noticeString = "PRESS SPACEBAR TO MENU";
				timeStampFont.DrawFont ( noticeString, Color.White, ( Core.GraphicsDevice.ImmediateContext.CurrentRenderBuffer.Size () / 2 -
					timeStampFont.MeasureString ( noticeString ) / 2 ) + new Vector2 ( 0, 48 ) );
			}
		}
	}
}
