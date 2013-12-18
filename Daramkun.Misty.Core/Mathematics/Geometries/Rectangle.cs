using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Geometries
{
	public struct Rectangle
	{
		public Vector2 Position;
		public Vector2 Size;

		public Rectangle ( float x, float y, float width, float height )
		{
			Position = new Vector2 ( x, y );
			Size = new Vector2 ( width, height );
		}

		public Rectangle ( Vector2 position, Vector2 size )
		{
			this.Position = position;
			this.Size = size;
		}

		public override string ToString ()
		{
			return string.Format ( "{{X:{0}, Y:{1}, Width:{2}, Height:{3}}}",
				Position.X, Position.Y, Size.X, Size.Y );
		}
	}
}
