using System;
using System.Drawing;
using Daramkun.Misty.Nodes;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;

namespace Daramkun.Misty.Platforms.ChooseForm
{
	class CocoaChooseForm : IChooseForm
	{
		#region IChooseForm implementation

		public bool IsClickedOK { get; private set; }

		public int SelectedPAF { get; private set; }
		public int SelectedGameLooper { get; private set; }
		public int SelectedMainNode { get; private set; }

		public bool InitializePlatform ()
		{
			throw new NotImplementedException ();
		}

		public bool InitializeWindow ( string gameName, Image coverImage,
			ILauncher [] launchers, IGameLooper [] gameLoopers, Node [] mainNodes )
		{
			throw new NotImplementedException ();
		}

		public void RunWindow ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}