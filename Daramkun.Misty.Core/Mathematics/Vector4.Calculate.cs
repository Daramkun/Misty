using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector4
	{
		public static Vector4 operator + ( Vector4 a, Vector4 b ) { Vector4 result; Add ( ref a, ref b, out result ); return result; }
		public static Vector4 operator - ( Vector4 a ) { Vector4 result; Negate ( ref a, out result ); return result; }
		public static Vector4 operator - ( Vector4 a, Vector4 b ) { Vector4 result; Subtract ( ref a, ref b, out result ); return result; }
		public static Vector4 operator * ( Vector4 a, Vector4 b ) { Vector4 result; Multiply ( ref a, ref b, out result ); return result; }
		public static Vector4 operator * ( Vector4 a, float b ) { Vector4 result; Multiply ( ref a, b, out result ); return result; }
		public static Vector4 operator * ( float a, Vector4 b ) { Vector4 result; Multiply ( ref b, a, out result ); return result; }
		public static Vector4 operator / ( Vector4 a, Vector4 b ) { Vector4 result; Divide ( ref a, ref b, out result ); return result; }
		public static Vector4 operator / ( Vector4 a, float b ) { Vector4 result; Divide ( ref a, b, out result ); return result; }
		public static bool operator == ( Vector4 v1, Vector4 v2 ) { return ( v1.X == v2.X && v1.Y == v2.Y ) && v1.Z == v2.Z; }
		public static bool operator != ( Vector4 v1, Vector4 v2 ) { return !( v1 == v2 ); }

		public override bool Equals ( object obj ) { if ( !( obj is Vector4 ) ) return false; return this == ( Vector4 ) obj; }

		public static Vector4 Add ( Vector4 a, Vector4 b ) { return a + b; }
		public static Vector4 Negate ( Vector4 b ) { return -b; }
		public static Vector4 Subtract ( Vector4 a, Vector4 b ) { return a - b; }
		public static Vector4 Multiply ( Vector4 a, Vector4 b ) { return a * b; }
		public static Vector4 Multiply ( Vector4 a, float b ) { return a * b; }
		public static Vector4 Multiply ( float a, Vector4 b ) { return b * a; }
		public static Vector4 Divide ( Vector4 a, Vector4 b ) { return a / b; }
		public static Vector4 Divide ( Vector4 a, float b ) { return a / b; }

		public static void Add ( ref Vector4 a, ref Vector4 b, out Vector4 result ) { result = new Vector4 ( a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W ); }
		public static void Negate ( ref Vector4 a, out Vector4 result ) { result = new Vector4 ( -a.X, -a.Y, -a.Z, -a.W ); }
		public static void Subtract ( ref Vector4 a, ref Vector4 b, out Vector4 result ) { result = new Vector4 ( a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W ); }
		public static void Multiply ( ref Vector4 a, ref Vector4 b, out Vector4 result ) { result = new Vector4 ( a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W ); }
		public static void Multiply ( ref Vector4 a, float b, out Vector4 result ) { result = new Vector4 ( a.X * b, a.Y * b, a.Z * b, a.W * b ); }
		public static void Multiply ( float a, ref Vector4 b, out Vector4 result ) { Multiply ( ref b, a, out result ); }
		public static void Divide ( ref Vector4 a, ref Vector4 b, out Vector4 result ) { result = new Vector4 ( a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W ); }
		public static void Divide ( ref Vector4 a, float b, out Vector4 result ) { result = new Vector4 ( a.X / b, a.Y / b, a.Z / b, a.W / b ); }

		public static float Dot ( Vector4 v1, Vector4 v2 ) { float result; Dot ( ref v1, ref v2, out result ); return result; }
		public static Vector4 Cross ( Vector4 v1, Vector4 v2, Vector4 v3 ) { Vector4 result; Cross ( ref v1, ref v2, ref v3, out result ); return result; }

		public static void Dot ( ref Vector4 v1, ref Vector4 v2, out float result )
		{ result = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W; }
		public static void Cross ( ref Vector4 v1, ref Vector4 v2, ref Vector4 v3, out Vector4 result )
		{
			result = new Vector4 (
				v1.W * v3.Y - v2.W * v3.Z + v1.W * v3.W,
				-v1.W * v3.X + v1.X * v1.Y * v3.Z - v2.W * v3.W,
				v2.W * v3.X - v1.X * v1.Y * v3.Y + v1.W * v3.W,
				-v1.W * v3.X + v2.W * v3.Y - v1.W - v3.Z
			);
		}
	}
}
