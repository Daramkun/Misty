using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Nodes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Daramkun.Misty.Platforms.ChooseForm;

namespace Daramkun.Misty.Platforms
{
	public static class ChooseWindow
	{
		#region For UNIX Kernel Detect
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		struct utsname
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string sysname;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string nodename;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string release;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string version;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string machine;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
			public string extraJustInCase;
		}

		[DllImport("libc")]
		private static extern void uname(out utsname uname_struct);
		#endregion

		static ILauncher [] pafs;
		static Node [] mainNodes;
		static IGameLooper [] gameLoopers;

		public static void Show ( string gameName, Assembly [] pa, Assembly [] ma, Icon icon = null, Image coverImage = null, Assembly [] ga = null )
		{
			AnalyzePAFs ( pa );
			if ( ga == null )
				ga = new Assembly [] { Assembly.Load ( "Daramkun.Misty.Core" ) };
			AnalyzeGameLoopers ( ga );
			AnalyzeMainNodes ( ma );

			IChooseForm chooseWindow = null;
			if ( Environment.OSVersion.Platform != PlatformID.Unix )
			{
				utsname un;
				uname ( out un );
				if ( un.sysname.ToString () == "Darwin" )
					chooseWindow = new CocoaChooseForm ();
			}
			if ( chooseWindow == null )
				chooseWindow = new FormChooseForm ();

			chooseWindow.InitializePlatform ();
			chooseWindow.InitializeWindow ( gameName, coverImage, pafs, gameLoopers, mainNodes );

			if ( chooseWindow.IsClickedOK )
			{
				Core.Run ( pafs [ chooseWindow.SelectedPAF ], mainNodes [ chooseWindow.SelectedMainNode ], 
					gameLoopers [ chooseWindow.SelectedGameLooper ], typeof ( HighResolutionGameTime ) );
			}
		}

		static void AnalyzePAFs ( Assembly [] pafs )
		{
			List<ILauncher> launchers = new List<ILauncher> ();
			foreach ( Assembly asm in pafs )
			{
				foreach ( Type type in asm.GetTypes () )
				{
					if ( Utilities.IsSubtypeOf ( type, typeof ( ILauncher ) ) )
					{
						ILauncher launcher = Activator.CreateInstance ( type ) as ILauncher;
						if ( launcher.IsSupportPlatform )
							launchers.Add ( launcher );
					}
				}
			}
			launchers.Sort ( ( ILauncher launcher1, ILauncher launcher2 ) => { return launcher1.SupportWeight > launcher2.SupportWeight ? 1 : 0; } );
			ChooseWindow.pafs = launchers.ToArray ();
		}

		static void AnalyzeGameLoopers ( Assembly [] gameLoopers )
		{
			List<IGameLooper> loopers = new List<IGameLooper> ();
			foreach ( Assembly asm in gameLoopers )
			{
				foreach ( Type type in asm.GetTypes () )
				{
					if ( Utilities.IsSubtypeOf ( type, typeof ( IGameLooper ) ) && !type.IsInterface )
						loopers.Add ( Activator.CreateInstance ( type ) as IGameLooper );
				}
			}
			ChooseWindow.gameLoopers = loopers.ToArray ();
		}

		static void AnalyzeMainNodes ( Assembly [] mainNodes )
		{
			List<Node> nodes = new List<Node> ();
			foreach ( Assembly asm in mainNodes )
			{
				foreach ( Type type in asm.GetTypes () )
				{
					if ( Utilities.IsSubtypeOf ( type, typeof ( Node ) ) )
					{
						object [] attrs = type.GetCustomAttributes ( typeof ( MainNodeAttribute ), false );
						if ( attrs != null && attrs.Length > 0 )
						{
							nodes.Add ( Activator.CreateInstance ( type ) as Node );
						}
					}
				}
			}
			ChooseWindow.mainNodes = nodes.ToArray ();
		}
	}
}
