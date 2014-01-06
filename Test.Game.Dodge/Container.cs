using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Loaders;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Nodes.Scenes;

namespace Test.Game.Dodge
{
	[MainNode]
	public class Container : Node
	{
		ResourceTable contentManager;
		Font fpsFont;
		FpsCalculator calc;

		public SceneContainer SceneContainer { get; set; }

		public Container ()
		{
			Texture2DContentLoader.AddDefaultDecoders ();
		}

		public override void Intro ( params object [] args )
		{
			Core.Window.Title = "Simple Dodge";
			Core.GraphicsDevice.BlendState = true;
			Core.GraphicsDevice.BlendOperation = BlendOperation.AlphaBlend;

			Add ( InputHelper.CreateInstance () );
			InputHelper.IsKeyboardEnabled = true;
			Add ( SceneContainer = new SceneContainer ( new MenuScene () ) );
			
			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			contentManager.AddDefaultContentLoader ();
			//Core.Launcher.InvokeInMainThread ( () =>
			//{
				fpsFont = contentManager.Load<TrueTypeFont> ( "Resources/test.ttf", 20 );
			//} );

			Add ( calc = new FpsCalculator () );

			base.Intro ( args );
		}

		public override void Outro ()
		{
			contentManager.Dispose ();
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );
			
			base.Draw ( gameTime );
			
			string fpsString = string.Format ( "Update FPS:{0:0.00}\nRender FPS:{1:0.00}", calc.UpdateFPS, calc.DrawFPS );
			fpsFont.DrawFont ( fpsString, Color.White,
				Core.GraphicsDevice.CurrentRenderBuffer.Size - fpsFont.MeasureString ( fpsString ) - new Vector2 ( 10, 10 ) );

			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Dodge"; }
	}
}
