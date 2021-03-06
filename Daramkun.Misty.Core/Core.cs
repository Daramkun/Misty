﻿using System;
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

		static int fixedUpdateTimeStep, fixedDrawTimeStep;

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

		public static IFileSystem BaseFileSystem { get; set; }

		public static TimeSpan FixedUpdateTimeStep
		{
			get { return TimeSpan.FromMilliseconds ( fixedUpdateTimeStep ); }
			set { fixedUpdateTimeStep = ( int ) value.TotalMilliseconds; }
		}
		public static TimeSpan FixedDrawTimeStep
		{
			get { return TimeSpan.FromMilliseconds ( fixedDrawTimeStep ); }
			set { fixedDrawTimeStep = ( int ) value.TotalMilliseconds; }
		}

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

		public static void Run ( ILauncher launcher, Node mainNode, IGameLooper gameLooper = null, Type gameTimeType = null )
		{
			Launcher = launcher;
			MainNode = mainNode;

			if ( gameLooper == null )
				GameLooper = new PlainGameLooper ();
			else
				GameLooper = gameLooper;

			int elapsedUpdateTimeStep = 0, elapsedDrawTimeStep = 0;
			int lastUpdateTimeStep = Environment.TickCount, lastDrawTimeStep = Environment.TickCount;

			GameTime updateGameTime, drawGameTime;
			if ( gameTimeType == null ) { updateGameTime = new GameTime (); drawGameTime = new GameTime (); }
			else { updateGameTime = Activator.CreateInstance ( gameTimeType ) as GameTime; drawGameTime = Activator.CreateInstance ( gameTimeType ) as GameTime; }

			thisThread = Thread.CurrentThread;

			launcher.Initialize ();
			Window.Show ();
			mainNode.Intro ();
			isRunningMode = true;
			GameLooper.Run (
				() =>
				{
					if ( AudioDevice != null )
						AudioDevice.Update ();

					if ( elapsedUpdateTimeStep >= fixedUpdateTimeStep )
					{
						updateGameTime.Update ();
						mainNode.Update ( updateGameTime );
						elapsedUpdateTimeStep -= fixedUpdateTimeStep;
					}
					else
					{
						int temp = Environment.TickCount;
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

					if ( elapsedDrawTimeStep >= fixedDrawTimeStep )
					{
						drawGameTime.Update ();
						mainNode.Draw ( drawGameTime );
						elapsedDrawTimeStep -= fixedDrawTimeStep;
					}
					else
					{
						int temp = Environment.TickCount;
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