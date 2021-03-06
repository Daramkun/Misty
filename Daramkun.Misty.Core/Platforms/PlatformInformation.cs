﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Platforms
{
	public enum PlatformType
	{
		Unknown = 0,

		WindowsNT = 1,
		Unix = 2,
		OSX = 3,
		Cosmos = 4,

		WindowsPhone = 12,
		WindowsRT = 13,
		Android = 14,
		iOS = 15,
		Blackberry = 16,

		Xbox360 = 25,
		PlaystationMobile = 26,
		OUYA = 27,
	}

	public struct PlatformInformation
	{
		public PlatformType PlatformType { get; set; }
		public Version PlatformVersion { get; set; }

		public string UserName { get; set; }
		public string MachineUniqueIdentifier { get; set; }
	}
}
