using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public struct Vector2
	{
		public static readonly Vector2 Zero = new Vector2 ( 0 );

		public float X, Y;

		public float LengthSquared { get { return X * X + Y * Y; } }
		public float Length { get { return ( float ) Math.Sqrt ( LengthSquared ); } }

		public Vector2 ( float value ) { X = Y = value; }
		public Vector2 ( float x, float y ) { X = x; Y = y; }
		public Vector2 ( float [] xy ) { X = xy [ 0 ]; Y = xy [ 1 ]; }

		public static Vector2 operator + ( Vector2 a, Vector2 b ) { return new Vector2 ( a.X + b.X, a.Y + b.Y ); }
		public static Vector2 operator - ( Vector2 a ) { return new Vector2 ( -a.X, -a.Y ); }
		public static Vector2 operator - ( Vector2 a, Vector2 b ) { return new Vector2 ( a.X - b.X, a.Y - b.Y ); }
		public static Vector2 operator * ( Vector2 a, Vector2 b ) { return new Vector2 ( a.X * b.X, a.Y * b.Y ); }
		public static Vector2 operator * ( Vector2 a, float b ) { return new Vector2 ( a.X * b, a.Y * b ); }
		public static Vector2 operator * ( float a, Vector2 b ) { return new Vector2 ( a * b.X, a * b.Y ); }
		public static Vector2 operator / ( Vector2 a, Vector2 b ) { return new Vector2 ( a.X / b.X, a.Y / b.Y ); }
		public static Vector2 operator / ( Vector2 a, float b ) { return new Vector2 ( a.X / b, a.Y / b ); }

		public static float Dot ( Vector2 v1, Vector2 v2 )
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}

		public static Vector2 Cross ( Vector2 v1, float v2 )
		{
			return new Vector2 ( v1.Y * v2, v1.X * -v2 );
		}

		public static Vector2 Cross ( float v1, Vector2 v2 )
		{
			return new Vector2 ( -v1 * v2.Y, v1 * v2.X );
		}

		public static Vector2 Cross ( Vector2 v1, Vector2 v2 )
		{
			return new Vector2 ( v1.X * v2.Y, v1.Y * v2.X );
		}

		public static float Distance ( Vector2 v1, Vector2 v2 )
		{
			return ( float ) System.Math.Sqrt ( DistanceSquared ( v1, v2 ) );
		}

		public static float DistanceSquared ( Vector2 v1, Vector2 v2 )
		{
			return ( float ) ( System.Math.Pow ( v2.X - v1.X, 2 ) + System.Math.Pow ( v2.Y - v1.Y, 2 ) );
		}

		public static Vector2 Max ( Vector2 v1, Vector2 v2 )
		{
			return new Vector2 ( ( float ) Math.Max ( v1.X, v2.X ), ( float ) Math.Max ( v1.Y, v2.Y ) );
		}

		public static Vector2 Min ( Vector2 v1, Vector2 v2 )
		{
			return new Vector2 ( ( float ) Math.Min ( v1.X, v2.X ), ( float ) Math.Min ( v1.Y, v2.Y ) );
		}

		public static Vector2 Clamp ( Vector2 v1, Vector2 v2, Vector2 v3 )
		{
			return Max ( v2, Min ( v1, v3 ) );
		}

		public static Vector2 Absolute ( Vector2 v )
		{
			return new Vector2 ( ( float ) Math.Abs ( v.X ), ( float ) Math.Abs ( v.Y ) );
		}

		public void Reset () { this = new Vector2 (); }
		public Vector2 Normalize () { return new Vector2 ( X / Length, Y / Length ); }

		public void Skew () { this = new Vector2 ( -Y, X ); }

		public override bool Equals ( object obj )
		{
			if ( !( obj is Vector2 ) ) return false;
			return Length == ( ( Vector2 ) obj ).Length;
		}

		public static bool operator == ( Vector2 v1, Vector2 v2 )
		{
			return v1.X == v2.X && v1.Y == v2.Y;
		}

		public static bool operator != ( Vector2 v1, Vector2 v2 )
		{
			return !( v1 == v2 );
		}

		public override int GetHashCode ()
		{
			return ( int ) ( Length );
		}

		public override string ToString ()
		{
			return String.Format ( "{{X:{0}, Y:{1}}}", X, Y );
		}

		public float [] ToArray () { return new float [] { X, Y }; }

		public float this [ int index ]
		{
			get { return ( index == 0 ) ? X : ( ( index == 1 ) ? Y : float.NaN ); }
			set
			{
				switch ( index )
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					default: throw new IndexOutOfRangeException ();
				}
			}
		}
	}
}
