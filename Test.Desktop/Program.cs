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
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Graphics.Spirit.Fonts;
using Daramkun.Misty.Log;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Platforms;

namespace Test.Desktop
{
	static class Program
	{
		[STAThread]
		static void Main ()
		{
			Logger.AddDefaultLogWriter ();
			Core.FixedDrawTimeStep = new TimeSpan ();
			ChooseWindow chooseWindow = new ChooseWindow ( "Tester",
				new Assembly []
				{
					Assembly.Load ( "Daramkun.Misty.Platform.OpenTK" ),
					Assembly.Load ( "Daramkun.Misty.Platform.DirectX9" ),
				},
				new Assembly []
				{
					Assembly.Load ( "Test.Game.Cube" ),
					Assembly.Load ( "Test.Game.Dodge" ),
					Assembly.Load ( "Test.Game.InfoViewer" ),
					Assembly.Load ( "Test.Game.PerformanceTester" ),
					Assembly.Load ( "Test.Game.PlayAudios" ),
				}
			);
			chooseWindow.Run ();
		}
	}
}
