using System;
using System.Drawing;
using Daramkun.Misty.Platforms;
using Daramkun.Misty.Nodes;

namespace Daramkun.Misty.Platforms.ChooseForm
{
	interface IChooseForm
	{
		bool IsClickedOK { get; }
		int SelectedPAF { get; }
		int SelectedGameLooper { get; }
		int SelectedMainNode { get; }

		bool InitializePlatform ();
		bool InitializeWindow ( string gameName, Image image,
			ILauncher [] launchers, IGameLooper [] gameLoopers, Node [] mainNodes );
		void RunWindow ();
	}
}

