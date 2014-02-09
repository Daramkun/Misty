using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Nodes.Scenes;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Mathematics.Transforms;

namespace Test.Game.Dodge
{
    public class MenuScene : Node
    {
		ResourceTable contentManager;
		Font titleFont;
		Font menuFont;
		Sprite logo;
		World2 logoWorld;

		public override void Intro ( params object [] args )
		{
			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			contentManager.AddDefaultContentLoader ();
			//Core.Launcher.InvokeInMainThread ( () =>
			//{
				titleFont = contentManager.Load<TrueTypeFont> ( "Resources/test.ttf", 64 );
				menuFont = contentManager.Load<TrueTypeFont> ( "Resources/test.ttf", 24 );
			//} );
			logo = new Sprite ( contentManager.Load<ITexture2D> ( "Resources/Dodge/logo.png" ) );
			logoWorld = new World2 ();
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
			{
				( Parent as SceneContainer ).Transition ( new GameScene () );
			}
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.B ) )
			{
				//LiqueurSystem.Launcher.InvokeInMainThread ( () =>
				//{
					Core.Exit ();
				//} );
			}
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			titleFont.DrawFont ( "Dodge", Color.White, new Vector2 ( 24 ) );

			menuFont.DrawFont ( "A. START", Color.White, new Vector2 ( 24, 256 ) );
			menuFont.DrawFont ( "B. EXIT", Color.White, new Vector2 ( 24, 304 ) );

			logoWorld.Translate = Core.GraphicsDevice.CurrentRenderBuffer.Size / 2 - logo.Texture.Size / 2;
			logo.Draw ( logoWorld );

			base.Draw ( gameTime );
		}
    }
}
