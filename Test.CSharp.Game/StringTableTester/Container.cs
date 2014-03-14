using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;

namespace Test.CSharp.Game.StringTableTester
{
	[MainNode]
	public class Container : Node
	{
		ResourceTable contentManager;
		StringTable stt;
		TrueTypeFont font, font2;

		public override void Intro ( params object [] args )
		{
			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( FileSystemManager.ManifestFileSystem ) );

			Core.GraphicsDevice.BlendState = true;
			Core.GraphicsDevice.BlendOperation = BlendOperation.AlphaBlend;
			Add ( InputHelper.Instance );

			stt = contentManager.Load<StringTable> ( "Resources/stringTable.json" );

			font = contentManager.Load<TrueTypeFont> ( "Resources/test.ttf", 22 );
			font.IsPrerenderMode = true;
			font2 = contentManager.Load<TrueTypeFont> ( "Resources/test.ttf", 32 );
			font2.SeconaryFont = contentManager.Load<TrueTypeFont> ( "Resources/segoeui.ttf", 32 );
			
			base.Intro ( args );
		}

		public override void Outro ()
		{
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.Grave ) )
				Core.CurrentCulture = CultureInfo.CurrentCulture;
			else if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D1 ) )
				Core.CurrentCulture = new CultureInfo ( "ko-KR" );
			else if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D2 ) )
				Core.CurrentCulture = new CultureInfo ( "en-US" );
			else if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D3 ) )
				Core.CurrentCulture = new CultureInfo ( "ja-JP" );
			else if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D4 ) )
				Core.CurrentCulture = new CultureInfo ( "de-DE" );

			base.Update ( gameTime );
		}



		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );

			font.DrawFont ( @"Current culture: `
Korean of Republic of Korea: 1
English of USA: 2
Japanese: 3
Deutch of Deutchland: 4", Color.White, new Vector2 () );
			font2.DrawFont ( string.Format ( @"Culture code: {0}
string1: {1}
string2: {2}
string3: {3}", Core.CurrentCulture, stt [ "string1" ], stt [ "string2" ], stt [ "string3" ] ),
				Color.White, new Vector2 ( 0, 128 ) );

			base.Draw ( gameTime );

			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "String Table Tester"; }
	}
}
