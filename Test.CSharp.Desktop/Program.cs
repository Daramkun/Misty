using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Daramkun.Misty;
using Daramkun.Misty.Audios;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.Decoders.Audios;
using Daramkun.Misty.Contents.Decoders.Images;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Log;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Platforms;
using Daramkun.Misty.Platforms.GameLoopers;

namespace Test.Desktop
{
	static class Program
	{
		[STAThread]
		static void Main ()
		{
			Logger.AddDefaultLogWriter ();
			Core.FixedUpdateTimeStep = new TimeSpan ();
			Core.FixedDrawTimeStep = new TimeSpan ();
			FileSystemManager.AddFileSystem ( "LocalFileSystem", typeof ( LocalFileSystem ) );
			ChooseWindow chooseWindow = new ChooseWindow ( "Tester via C#",
				new Assembly []
				{
					Assembly.Load ( "Daramkun.Misty.Platform.OpenTK" ),
					Assembly.Load ( "Daramkun.Misty.Platform.DirectX9" ),
				},
				new Assembly []
				{
					Assembly.Load ( "Test.CSharp.Game" )
				},
				null,
				null,
				new Assembly []
				{
					Assembly.Load ( "Daramkun.Misty.Core" ),
					Assembly.Load ( "Daramkun.Misty.Platform.Desktop" )
				}
			);
			chooseWindow.Run ();
		}
	}
}
