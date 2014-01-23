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
using Daramkun.Misty.Platforms.GameLoopers;

namespace Daramkun.Misty
{
    public static class Core
    {
		static bool isRunningMode;
		static Thread thisThread;
		static SpinLock invokeSpinLock = new SpinLock ();
		static List<Action> invokedMethod = new List<Action> ();

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

		public static IGameLooper GameLooper { get; private set; }

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

		public static void Run ( ILauncher launcher, Node mainNode, bool isInitializeAudio = true, IGameLooper gameLooper = null )
		{
			Launcher = launcher;
			MainNode = mainNode;

			if ( gameLooper == null )
				GameLooper = new PlainGameLooper ();
			else
				GameLooper = gameLooper;

			TimeSpan elapsedUpdateTimeStep = new TimeSpan (),
				elapsedDrawTimeStep = new TimeSpan ();
			TimeSpan lastUpdateTimeStep = TimeSpan.FromMilliseconds ( Environment.TickCount ),
				lastDrawTimeStep = TimeSpan.FromMilliseconds ( Environment.TickCount );

			GameTime updateGameTime = new GameTime (), drawGameTime = new GameTime ();

			thisThread = Thread.CurrentThread;

			launcher.Initialize ( isInitializeAudio );
			Window.Show ();
			mainNode.Intro ();
			isRunningMode = true;
			GameLooper.Run (
				() =>
				{
					if ( AudioDevice != null )
						AudioDevice.Update ();

					if ( elapsedUpdateTimeStep >= FixedUpdateTimeStep || FixedUpdateTimeStep.TotalMilliseconds == 0 )
					{
						updateGameTime.Update ();
						mainNode.Update ( updateGameTime );
						elapsedUpdateTimeStep -= FixedUpdateTimeStep;
					}
					else
					{
						TimeSpan temp = TimeSpan.FromMilliseconds ( Environment.TickCount );
						elapsedUpdateTimeStep += ( temp - lastUpdateTimeStep );
						lastUpdateTimeStep = temp;
					}
				},
				() =>
				{
					if ( invokedMethod.Count > 0 )
					{
						foreach ( Action action in invokedMethod.ToArray () )
						{
							action ();
							invokeSpinLock.Enter ();
							invokedMethod.Remove ( action );
							invokeSpinLock.Exit ();
						}
					}

					if ( elapsedDrawTimeStep >= FixedDrawTimeStep || FixedDrawTimeStep.TotalMilliseconds == 0 )
					{
						drawGameTime.Update ();
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
				},
				ref isRunningMode
			);

			mainNode.Outro ();
			launcher.Dispose ();
		}

		public static void Exit () { isRunningMode = false; }

		public static void Dispatch ( Action action, bool isWaitForEndOfMethod = true )
		{
			if ( thisThread == Thread.CurrentThread ) { action (); return; }

			invokeSpinLock.Enter ();
			invokedMethod.Add ( action );
			invokeSpinLock.Exit ();

			while ( isWaitForEndOfMethod && invokedMethod.Contains ( action ) )
				;
		}
    }
}