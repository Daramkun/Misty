using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;

namespace Test.CSharp.Game.InputTester
{
	[MainNode]
	public class Container : Node
	{
		ResourceTable contentManager;
		Font font;
		string inputInfo = "";

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.ImmediateContext.BlendState = BlendState.AlphaBlend;
			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( FileSystemManager.ManifestFileSystem ) );
			font = contentManager.Load<TrueTypeFont> ( "Resources/test.ttf",  32 );
			Add ( InputHelper.Instance );
			base.Intro ( args );
		}

		public override void Outro ()
		{
			contentManager.Dispose ();
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			StringBuilder builder = new StringBuilder ();
			
			builder.Append ( "Input keys: " );
			if ( InputHelper.CurrentKeyboardState.PressedKeys != null )
				foreach ( Key key in InputHelper.CurrentKeyboardState.PressedKeys )
					builder.Append ( key.ToString () ).Append ( " " );
			builder.AppendLine ();

			builder.AppendFormat ( "Mouse position: {0}", InputHelper.CurrentMouseState.Position );
			builder.AppendLine ();

			builder.Append ( InputHelper.CurrentMouseState.MouseButtons );

			inputInfo = builder.ToString ();

			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.ImmediateContext.BeginScene ();
			Core.GraphicsDevice.ImmediateContext.Clear ( ClearBuffer.AllBuffer, Color.Black );

			font.DrawFont ( inputInfo, Color.White, new Vector2 ( 0, 0 ) );
			
			base.Draw ( gameTime );

			Core.GraphicsDevice.ImmediateContext.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Input Tester"; }
	}
}
