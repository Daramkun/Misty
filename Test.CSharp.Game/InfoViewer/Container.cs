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
using Daramkun.Misty.Nodes;

namespace Test.Game.InfoViewer
{
	[MainNode]
	public class Container : Node
	{
		ResourceTable contentManager;
		Font font;
		float offset;
		string infoText;

		public Container ()
		{
			Texture2DContentLoader.AddDefaultDecoders ();
		}

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.BlendState = BlendState.AlphaBlend;

			Core.Window.Title = "Information Viewer";

			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			font = contentManager.Load<TrueTypeFont> ( "Resources/test.ttf", 24 );
			font.IsPrerenderMode = true;

			Add ( InputHelper.Instance );

			infoText = string.Format (
@"Platform Type: {0}
Platform Version: {1}
Machine Unique ID: {2}
User Name: {3}

Base Renderer: {4}
Renderer Version: {5}
Shader Version: {6}
Device Vendor: {7}
Maximum Anisotropic Level: {8}
Is Support Geometry Shader: {9}

==Available Resolutions ==
{10}",
				Core.Launcher.PlatformInformation.PlatformType,
				Core.Launcher.PlatformInformation.PlatformVersion,
				Core.Launcher.PlatformInformation.MachineUniqueIdentifier,
				Core.Launcher.PlatformInformation.UserName,

				Core.GraphicsDevice.Information.BaseRenderer,
				Core.GraphicsDevice.Information.RendererVersion,
				Core.GraphicsDevice.Information.ShaderVersion,
				Core.GraphicsDevice.Information.DeviceVendor,
				Core.GraphicsDevice.Information.MaximumAnisotropicLevel,
				Core.GraphicsDevice.Information.IsSupportGeometryShader,

				ConvertToString ( Core.GraphicsDevice.Information.AvailableScreenResolution )
			);

			base.Intro ( args );
		}

		private object ConvertToString ( ScreenResolution [] screenResolution )
		{
			string temp = "";
			foreach ( ScreenResolution r in screenResolution )
				temp += string.Format ( "\n{0:0}x{1:0}:{2:0}", r.ScreenSize.X, r.ScreenSize.Y, r.RefreshRate );
			return temp.Substring ( 1, temp.Length - 1 );
		}

		public override void Outro ()
		{
			contentManager.Dispose ();
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			if ( InputHelper.CurrentKeyboardState.IsKeyDown ( Key.Down ) )
				offset -= ( float ) gameTime.ElapsedGameTime.TotalMilliseconds * 0.1f;
			if ( InputHelper.CurrentKeyboardState.IsKeyDown ( Key.Up ) )
			{
				offset += ( float ) gameTime.ElapsedGameTime.TotalMilliseconds * 0.1f;
				if ( offset >= 0 ) offset = 0;
			}

			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );
			font.DrawFont ( infoText, Color.White, new Vector2 ( 10, 10 + offset ) );
			base.Draw ( gameTime );
			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Information Viewer"; }
	}
}
