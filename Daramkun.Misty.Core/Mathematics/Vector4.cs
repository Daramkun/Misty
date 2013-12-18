using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public struct Vector4
	{
		public static readonly Vector4 Zero = new Vector4 ( 0 );

		public float X, Y, Z, W;
		
		public Vector4 ( float value ) { X = Y = Z = W = value; }
		public Vector4 ( float x, float y, float z, float w ) { X = x; Y = y; Z = z; W = w; }
		public Vector4 ( Vector2 vector, float z, float w ) { X = vector.X; Y = vector.Y; Z = z; W = w; }
		public Vector4 ( Vector3 vector, float w ) { X = vector.X; Y = vector.Y; Z = vector.Z; W = w; }
		public Vector4 ( float [] xyzw ) { X = xyzw [ 0 ]; Y = xyzw [ 1 ]; Z = xyzw [ 2 ]; W = xyzw [ 3 ]; }

		public float LengthSquared { get { return X * X + Y * Y + Z * Z + W * W; } }
		public float Length { get { return ( float ) System.Math.Sqrt ( LengthSquared ); } }

		public static Vector4 operator + ( Vector4 a, Vector4 b ) { return new Vector4 ( a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W ); }
		public static Vector4 operator - ( Vector4 a ) { return new Vector4 ( -a.X, -a.Y, -a.Z, -a.W ); }
		public static Vector4 operator - ( Vector4 a, Vector4 b ) { return new Vector4 ( a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W ); }
		public static Vector4 operator * ( Vector4 a, Vector4 b ) { return new Vector4 ( a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W ); }
		public static Vector4 operator * ( Vector4 a, float b ) { return new Vector4 ( a.X * b, a.Y * b, a.Z * b, a.W * b ); }
		public static Vector4 operator * ( float a, Vector4 b ) { return new Vector4 ( a * b.X, a * b.Y, a * b.Z, a * b.W ); }
		public static Vector4 operator / ( Vector4 a, Vector4 b ) { return new Vector4 ( a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W ); }
		public static Vector4 operator / ( Vector4 a, float b ) { return new Vector4 ( a.X / b, a.Y / b, a.Z / b, a.W / b ); }

		public static float Dot ( Vector4 value1, Vector4 value2 )
		{
			return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z + value1.W * value2.W;
		}

		public static Vector4 Cross ( Vector4 value1, Vector4 value2, Vector4 value3 )
		{
			return new Vector4 (
				value1.W * value3.Y - value2.W * value3.Z + value1.W * value3.W,
				-value1.W * value3.X + value1.X * value1.Y * value3.Z - value2.W * value3.W,
				value2.W * value3.X - value1.X * value1.Y * value3.Y + value1.W * value3.W,
				-value1.W * value3.X + value2.W * value3.Y - value1.W - value3.Z
			);
		}

		public static float Distance ( Vector4 v1, Vector4 v2 )
		{
			return ( float ) Math.Sqrt ( DistanceSquared ( v1, v2 ) );
		}

		public static float DistanceSquared ( Vector4 v1, Vector4 v2 )
		{
			return ( float ) ( System.Math.Pow ( v2.X - v1.X, 2 ) +
				System.Math.Pow ( v2.Y - v1.Y, 2 ) + System.Math.Pow ( v2.Z - v1.Z, 2 ) +
				System.Math.Pow ( v2.W - v1.W, 2 ) );
		}

		public static Vector4 Max ( Vector4 v1, Vector4 v2 )
		{
			return new Vector4 ( ( float ) Math.Max ( v1.X, v2.X ), ( float ) Math.Max ( v1.Y, v2.Y ),
				( float ) Math.Max ( v1.Z, v2.Z ), ( float ) Math.Max ( v1.W, v2.W ) );
		}

		public static Vector4 Min ( Vector4 v1, Vector4 v2 )
		{
			return new Vector4 ( ( float ) Math.Min ( v1.X, v2.X ), ( float ) Math.Min ( v1.Y, v2.Y ),
				( float ) Math.Min ( v1.Z, v2.Z ), ( float ) Math.Min ( v1.W, v2.W ) );
		}

		public static Vector4 Clamp ( Vector4 v1, Vector4 v2, Vector4 v3 )
		{
			return Max ( v2, Min ( v1, v3 ) );
		}

		public static Vector4 Absolute ( Vector4 v )
		{
			return new Vector4 ( ( float ) Math.Abs ( v.X ), ( float ) Math.Abs ( v.Y ),
				( float ) Math.Abs ( v.Z ), ( float ) Math.Abs ( v.W ) );
		}

		public void Reset () { this = new Vector4 (); }
		public Vector4 Normalize () { return new Vector4 ( X / Length, Y / Length, Z / Length, W / Length ); }

		public override bool Equals ( object obj )
		{
			if ( !( obj is Vector4 ) ) return false;
			return Length == ( ( Vector4 ) obj ).Length;
		}

		public override int GetHashCode ()
		{
			return ( int ) ( Length );
		}

		public static bool operator == ( Vector4 v1, Vector4 v2 )
		{
			return ( v1.X == v2.X && v1.Y == v2.Y ) && ( v1.Z == v2.Z && v1.W == v2.W );
		}

		public static bool operator != ( Vector4 v1, Vector4 v2 )
		{
			return !( v1 == v2 );
		}

		public override string ToString ()
		{
			return string.Format ( "{{X:{0}, Y:{1}, Z:{2}, W:{3}}}", X, Y, Z, W );
		}

		public float [] ToArray () { return new float [] { X, Y, Z, W }; }

		public float this [ int index ]
		{
			get { return ( index == 0 ) ? X : ( ( index == 1 ) ? Y : ( ( index == 2 ) ? Z : ( ( index == 3 ) ? W : float.NaN ) ) ); }
			set
			{
				switch ( index )
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					case 3: W = value; break;
					default: throw new IndexOutOfRangeException ();
				}
			}
		}
	}
}
