﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Platforms
{
	public interface ILauncher : IDisposable
	{
		PlatformInformation PlatformInformation { get; }
		bool IsSupportPlatform { get; }
		float SupportWeight { get; }

		bool Initialize ();
	}
}
