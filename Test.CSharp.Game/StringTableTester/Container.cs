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
			else if ( InputHelper.IsKeyboardKeyUpRightNow ( Key.D5 ) )
				Core.CurrentCulture = new CultureInfo ( "ru-RU" );

			base.Update ( gameTime );
		}



		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );

			font.DrawFont ( @"Current culture: `
Korean: 1
English: 2
Japanese: 3
Deutchland: 4
Unknown Culture(Russian): 5", Color.White, new Vector2 () );
			font2.DrawFont ( string.Format ( @"Culture code: {0}({1})
string1: {2}
string2: {3}
string3: {4}", Core.CurrentCulture, Core.CurrentCulture.NativeName,
			 stt [ "string1" ], stt [ "string2" ], stt [ "string3" ] ),
				Color.White, new Vector2 ( 0, 160 ) );

			base.Draw ( gameTime );

			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "String Table Tester"; }
	}
}
