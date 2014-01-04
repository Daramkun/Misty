using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
    public partial struct Vector3
	{
		public static Vector3 operator + ( Vector3 a, Vector3 b ) { Vector3 result; Add ( ref a, ref b, out result ); return result; }
		public static Vector3 operator - ( Vector3 a ) { Vector3 result; Negate ( ref a, out result ); return result; }
		public static Vector3 operator - ( Vector3 a, Vector3 b ) { Vector3 result; Subtract ( ref a, ref b, out result ); return result; }
		public static Vector3 operator * ( Vector3 a, Vector3 b ) { Vector3 result; Multiply ( ref a, ref b, out result ); return result; }
		public static Vector3 operator * ( Vector3 a, float b ) { Vector3 result; Multiply ( ref a, b, out result ); return result; }
		public static Vector3 operator * ( float a, Vector3 b ) { Vector3 result; Multiply ( ref b, a, out result ); return result; }
		public static Vector3 operator / ( Vector3 a, Vector3 b ) { Vector3 result; Divide ( ref a, ref b, out result ); return result; }
		public static Vector3 operator / ( Vector3 a, float b ) { Vector3 result; Divide ( ref a, b, out result ); return result; }
		public static bool operator == ( Vector3 v1, Vector3 v2 ) { return ( v1.X == v2.X && v1.Y == v2.Y ) && v1.Z == v2.Z; }
		public static bool operator != ( Vector3 v1, Vector3 v2 ) { return !( v1 == v2 ); }

		public override bool Equals ( object obj ) { if ( !( obj is Vector3 ) ) return false; return this == ( Vector3 ) obj; }

		public static Vector3 Add ( Vector3 a, Vector3 b ) { return a + b; }
		public static Vector3 Negate ( Vector3 b ) { return -b; }
		public static Vector3 Subtract ( Vector3 a, Vector3 b ) { return a - b; }
		public static Vector3 Multiply ( Vector3 a, Vector3 b ) { return a * b; }
		public static Vector3 Multiply ( Vector3 a, float b ) { return a * b; }
		public static Vector3 Multiply ( float a, Vector3 b ) { return b * a; }
		public static Vector3 Divide ( Vector3 a, Vector3 b ) { return a / b; }
		public static Vector3 Divide ( Vector3 a, float b ) { return a / b; }

		public static void Add ( ref Vector3 a, ref Vector3 b, out Vector3 result ) { result = new Vector3 ( a.X + b.X, a.Y + b.Y, a.Z + b.Z ); }
		public static void Negate ( ref Vector3 a, out Vector3 result ) { result = new Vector3 ( -a.X, -a.Y, -a.Z ); }
		public static void Subtract ( ref Vector3 a, ref Vector3 b, out Vector3 result ) { result = new Vector3 ( a.X - b.X, a.Y - b.Y, a.Z - b.Z ); }
		public static void Multiply ( ref Vector3 a, ref Vector3 b, out Vector3 result ) { result = new Vector3 ( a.X * b.X, a.Y * b.Y, a.Z * b.Z ); }
		public static void Multiply ( ref Vector3 a, float b, out Vector3 result ) { result = new Vector3 ( a.X * b, a.Y * b, a.Z * b ); }
		public static void Multiply ( float a, ref Vector3 b, out Vector3 result ) { Multiply ( ref b, a, out result ); }
		public static void Divide ( ref Vector3 a, ref Vector3 b, out Vector3 result ) { result = new Vector3 ( a.X / b.X, a.Y / b.Y, a.Z / b.Z ); }
		public static void Divide ( ref Vector3 a, float b, out Vector3 result ) { result = new Vector3 ( a.X / b, a.Y / b, a.Z / b ); }

		public static float Dot ( Vector3 v1, Vector3 v2 ) { float result; Dot ( ref v1, ref v2, out result ); return result; }
		public static Vector3 Cross ( Vector3 v1, Vector3 v2 ) { Vector3 result; Cross ( ref v1, ref v2, out result ); return result; }

		public static void Dot ( ref Vector3 v1, ref Vector3 v2, out float result ) { result = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z; }
		public static void Cross ( ref Vector3 v1, ref Vector3 v2, out Vector3 result )
		{
			result = new Vector3 (
				v1.Y * v2.Z - v1.Z * v2.Y,
				v1.Z * v2.X - v1.X * v2.Z,
				v1.X * v2.Y - v1.Y * v2.X
			);
		}
    }
}
