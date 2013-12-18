using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	public struct ScreenResolution
	{
		public Vector2 ScreenSize;
		public float RefreshRate;

		public ScreenResolution ( Vector2 s, float r ) { ScreenSize = s; RefreshRate = r; }

		public override string ToString ()
		{
			return string.Format ( "Screen Size: {0}, Refresh Rate: {1}", ScreenSize, RefreshRate );
		}
	}
}
