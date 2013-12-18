using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public struct Viewport
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		public Viewport ( int x, int y, int width, int height ) : this () { X = x; Y = y; Width = width; Height = height; }
		public Viewport ( int [] viewport ) : this ( viewport [ 0 ], viewport [ 1 ], viewport [ 2 ], viewport [ 3 ] ) { }

		public override string ToString ()
		{
			return string.Format ( "{{X:{0}, Y:{1}, Width:{2}, Height:{3}}}", X, Y, Width, Height );
		}
	}
}
