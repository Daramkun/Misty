using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Daramkun.Misty.Audios;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Inputs;
using Daramkun.Misty.Log;
using Daramkun.Misty.Nodes;
using Daramkun.Misty.Platforms;

namespace Daramkun.Misty
{
    public static class Core
    {
		public static ILauncher Launcher { get; private set; }
		public static IWindow Window { get; private set; }
		public static IGraphicsDevice GraphicsDevice { get; private set; }
		public static IAudioDevice AudioDevice { get; private set; }
		public static InputCollection Inputs { get; private set; }
		public static Node MainNode { get; private set; }

		public static PackageInfo MainPackage { get; private set; }
		static List<PackageInfo> subPacks = new List<PackageInfo> ();
		public static PackageInfo [] SubPackages { get { return subPacks.ToArray (); } }

		public static CultureInfo CurrentCulture { get; set; }

		public static TimeSpan FixedUpdateTimeStep { get; set; }
		public static TimeSpan FixedDrawTimeStep { get; set; }

		public static void SetWindow ( IWindow window ) { Window = window; }
		public static void SetGraphicsDevice ( IGraphicsDevice graphicsDevice ) { GraphicsDevice = graphicsDevice; }
		public static void SetAudioDevice ( IAudioDevice audioDevice ) { AudioDevice = audioDevice; }
		public static void SetMainNode ( Node node ) { MainNode = node; }

		static Core ()
		{
			Inputs = new InputCollection ();
			CurrentCulture = CultureInfo.CurrentCulture;

			FixedUpdateTimeStep = TimeSpan.FromSeconds ( 1.0f / 60 );
			FixedDrawTimeStep = TimeSpan.FromSeconds ( 1.0f / 60 );
		}

		public static void Dispose ()
		{
			if ( AudioDevice != null ) AudioDevice.Dispose ();
			if ( GraphicsDevice != null ) GraphicsDevice.Dispose ();
			if ( Window != null ) Window.Dispose ();
		}

		public static void Run ( ILauncher launcher, Node mainNode )
		{
			Launcher = launcher;
			MainNode = mainNode;

			TimeSpan elapsedUpdateTimeStep = new TimeSpan (),
				elapsedDrawTimeStep = new TimeSpan ();
			TimeSpan lastUpdateTimeStep = TimeSpan.FromMilliseconds ( Environment.TickCount ),
				lastDrawTimeStep = TimeSpan.FromMilliseconds ( Environment.TickCount );

			GameTime updateGameTime = new GameTime (), drawGameTime = new GameTime ();

			launcher.Initialize ( true );
			Window.Show ();
			mainNode.Intro ();
			while ( Window.IsAlive )
			{
				if ( AudioDevice != null )
					AudioDevice.Update ();

				if ( elapsedUpdateTimeStep >= FixedUpdateTimeStep || FixedUpdateTimeStep.TotalMilliseconds == 0 )
				{
					updateGameTime.Update ();
					if ( mainNode != null )
						mainNode.Update ( updateGameTime );
					elapsedUpdateTimeStep -= FixedUpdateTimeStep;
				}
				else
				{
					TimeSpan temp = TimeSpan.FromMilliseconds ( Environment.TickCount );
					elapsedUpdateTimeStep += ( temp - lastUpdateTimeStep );
					lastUpdateTimeStep = temp;
				}

				if ( elapsedDrawTimeStep >= FixedDrawTimeStep || FixedDrawTimeStep.TotalMilliseconds == 0 )
				{
					drawGameTime.Update ();
					if ( mainNode != null )
						mainNode.Draw ( drawGameTime );
					elapsedDrawTimeStep -= FixedDrawTimeStep;
				}
				else
				{
					TimeSpan temp = TimeSpan.FromMilliseconds ( Environment.TickCount );
					elapsedDrawTimeStep += ( temp - lastDrawTimeStep );
					lastDrawTimeStep = temp;
				}
				Window.DoEvents ();
			}
			mainNode.Outro ();
			launcher.Dispose ();
		}
    }
}