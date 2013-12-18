using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public struct Vector3
	{
		public static readonly Vector3 Zero = new Vector3 ( 0 );

		public float X, Y, Z;

		public float LengthSquared { get { return X * X + Y * Y + Z * Z; } }
		public float Length { get { return ( float ) Math.Sqrt ( LengthSquared ); } }
		
		public Vector3 ( float value ) { X = Y = Z = value; }
		public Vector3 ( float x, float y, float z ) { X = x; Y = y; Z = z; }
		public Vector3 ( Vector2 vector, float z ) { X = vector.X; Y = vector.Y; Z = z; }
		public Vector3 ( float [] xyz ) { X = xyz [ 0 ]; Y = xyz [ 1 ]; Z = xyz [ 2 ]; }

		public static Vector3 operator + ( Vector3 a, Vector3 b ) { return new Vector3 ( a.X + b.X, a.Y + b.Y, a.Z + b.Z ); }
		public static Vector3 operator - ( Vector3 a ) { return new Vector3 ( -a.X, -a.Y, -a.Z ); }
		public static Vector3 operator - ( Vector3 a, Vector3 b ) { return new Vector3 ( a.X - b.X, a.Y - b.Y, a.Z - b.Z ); }
		public static Vector3 operator * ( Vector3 a, Vector3 b ) { return new Vector3 ( a.X * b.X, a.Y * b.Y, a.Z * b.Z ); }
		public static Vector3 operator * ( Vector3 a, float b ) { return new Vector3 ( a.X * b, a.Y * b, a.Z * b ); }
		public static Vector3 operator * ( float a, Vector3 b ) { return new Vector3 ( a * b.X, a * b.Y, a * b.Z ); }
		public static Vector3 operator / ( Vector3 a, Vector3 b ) { return new Vector3 ( a.X / b.X, a.Y / b.Y, a.Z / b.Z ); }
		public static Vector3 operator / ( Vector3 a, float b ) { return new Vector3 ( a.X / b, a.Y / b, a.Z / b ); }

		public static float Dot ( Vector3 v1, Vector3 v2 )
		{
			return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
		}

		public static Vector3 Cross ( Vector3 v1, Vector3 v2 )
		{
			return new Vector3 (
				v1.Y * v2.Z - v1.Z * v2.Y,
				v1.Z * v2.X - v1.X * v2.Z,
				v1.X * v2.Y - v1.Y * v2.X
				);
		}

		public static float Distance ( Vector3 v1, Vector3 v2 )
		{
			return ( float ) System.Math.Sqrt ( DistanceSquared ( v1, v2 ) );
		}

		public static float DistanceSquared ( Vector3 v1, Vector3 v2 )
		{
			return ( float ) ( System.Math.Pow ( v2.X - v1.X, 2 ) +
				System.Math.Pow ( v2.Y - v1.Y, 2 ) + System.Math.Pow ( v2.Z - v1.Z, 2 ) );
		}

		public static Vector3 Max ( Vector3 v1, Vector3 v2 )
		{
			return new Vector3 ( ( float ) Math.Max ( v1.X, v2.X ), ( float ) Math.Max ( v1.Y, v2.Y ), ( float ) Math.Max ( v1.Z, v2.Z ) );
		}

		public static Vector3 Min ( Vector3 v1, Vector3 v2 )
		{
			return new Vector3 ( ( float ) Math.Min ( v1.X, v2.X ), ( float ) Math.Min ( v1.Y, v2.Y ), ( float ) Math.Min ( v1.Z, v2.Z ) );
		}

		public static Vector3 Clamp ( Vector3 v1, Vector3 v2, Vector3 v3 )
		{
			return Max ( v2, Min ( v1, v3 ) );
		}

		public static Vector3 Absolute ( Vector3 v )
		{
			return new Vector3 ( ( float ) Math.Abs ( v.X ), ( float ) Math.Abs ( v.Y ), ( float ) Math.Abs ( v.Z ) );
		}

		public void Reset () { this = new Vector3 (); }
		public Vector3 Normalize () { return new Vector3 ( X / Length, Y / Length, Z / Length ); }

		public override bool Equals ( object obj )
		{
			if ( !( obj is Vector3 ) ) return false;
			return Length == ( ( Vector3 ) obj ).Length;
		}

		public static bool operator == ( Vector3 v1, Vector3 v2 )
		{
			return ( v1.X == v2.X && v1.Y == v2.Y ) && v1.Z == v2.Z;
		}

		public static bool operator != ( Vector3 v1, Vector3 v2 )
		{
			return !( v1 == v2 );
		}

		public override int GetHashCode ()
		{
			return ( int ) ( Length );
		}

		public override string ToString ()
		{
			return String.Format ( "{{X:{0}, Y:{1}, Z:{2}}}", X, Y, Z );
		}

		public float [] ToArray () { return new float [] { X, Y, Z }; }

		public float this [ int index ]
		{
			get { return ( index == 0 ) ? X : ( ( index == 1 ) ? Y : ( ( index == 2 ) ? Z : float.NaN ) ); }
			set
			{
				switch ( index )
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					default: throw new IndexOutOfRangeException ();
				}
			}
		}
	}
}
