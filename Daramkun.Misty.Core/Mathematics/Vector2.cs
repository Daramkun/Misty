using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector2
	{
		public static readonly Vector2 Zero = new Vector2 ( 0 );
		public static readonly Vector2 One = new Vector2 ( 1 );

		public float X, Y;

		public float LengthSquared { get { return X * X + Y * Y; } }
		public float Length { get { return ( float ) Math.Sqrt ( LengthSquared ); } }

		public Vector2 ( float value ) { X = Y = value; }
		public Vector2 ( float x, float y ) { X = x; Y = y; }
		public Vector2 ( float [] xy ) { X = xy [ 0 ]; Y = xy [ 1 ]; }

		public Vector2 Normalize () { return Normalize ( this ); }
		public void Normalize ( out Vector2 result ) { Normalize ( ref this, out result ); }
		public static Vector2 Normalize ( Vector2 v ) { Vector2 result; Normalize ( ref v, out result ); return result; }
		public static void Normalize ( ref Vector2 v, out Vector2 result ) { result = new Vector2 ( v.X / v.Length, v.Y / v.Length ); }

		public override int GetHashCode () { return ( int ) ( Length ); }
		public override string ToString () { return String.Format ( "{{X:{0}, Y:{1}}}", X, Y ); }

		public float [] ToArray () { return new float [] { X, Y }; }

		public float this [ int index ]
		{
			get { return ( index == 0 ) ? X : ( ( index == 1 ) ? Y : float.NaN ); }
			set { switch ( index ) { case 0: X = value; break; case 1: Y = value; break; default: throw new IndexOutOfRangeException (); } }
		}
	}
}
