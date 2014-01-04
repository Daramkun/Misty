using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector2
	{
		public static Vector2 operator + ( Vector2 a, Vector2 b ) { Vector2 result; Add ( ref a, ref b, out result ); return result; }
		public static Vector2 operator - ( Vector2 a ) { Vector2 result; Negate ( ref a, out result ); return result; }
		public static Vector2 operator - ( Vector2 a, Vector2 b ) { Vector2 result; Subtract ( ref a, ref b, out result ); return result; }
		public static Vector2 operator * ( Vector2 a, Vector2 b ) { Vector2 result; Multiply ( ref a, ref b, out result ); return result; }
		public static Vector2 operator * ( Vector2 a, float b ) { Vector2 result; Multiply ( ref a, b, out result ); return result; }
		public static Vector2 operator * ( float a, Vector2 b ) { Vector2 result; Multiply ( ref b, a, out result ); return result; }
		public static Vector2 operator / ( Vector2 a, Vector2 b ) { Vector2 result; Divide ( ref a, ref b, out result ); return result; }
		public static Vector2 operator / ( Vector2 a, float b ) { Vector2 result; Divide ( ref a, b, out result ); return result; }
		public static bool operator == ( Vector2 v1, Vector2 v2 ) { return v1.X == v2.X && v1.Y == v2.Y; }
		public static bool operator != ( Vector2 v1, Vector2 v2 ) { return !( v1 == v2 ); }

		public override bool Equals ( object obj ) { if ( !( obj is Vector2 ) ) return false; return this == ( Vector2 ) obj; }

		public static Vector2 Add ( Vector2 a, Vector2 b ) { return a + b; }
		public static Vector2 Negate ( Vector2 b ) { return -b; }
		public static Vector2 Subtract ( Vector2 a, Vector2 b ) { return a - b; }
		public static Vector2 Multiply ( Vector2 a, Vector2 b ) { return a * b; }
		public static Vector2 Multiply ( Vector2 a, float b ) { return a * b; }
		public static Vector2 Multiply ( float a, Vector2 b ) { return b * a; }
		public static Vector2 Divide ( Vector2 a, Vector2 b ) { return a / b; }
		public static Vector2 Divide ( Vector2 a, float b ) { return a / b; }

		public static void Add ( ref Vector2 a, ref Vector2 b, out Vector2 result ) { result = new Vector2 ( a.X + b.X, a.Y + b.Y ); }
		public static void Negate ( ref Vector2 a, out Vector2 result ) { result = new Vector2 ( -a.X, -a.Y ); }
		public static void Subtract ( ref Vector2 a, ref Vector2 b, out Vector2 result ) { result = new Vector2 ( a.X - b.X, a.Y - b.Y ); }
		public static void Multiply ( ref Vector2 a, ref Vector2 b, out Vector2 result ) { result = new Vector2 ( a.X * b.X, a.Y * b.Y ); }
		public static void Multiply ( ref Vector2 a, float b, out Vector2 result ) { result = new Vector2 ( a.X * b, a.Y * b ); }
		public static void Multiply ( float a, ref Vector2 b, out Vector2 result ) { Multiply ( ref b, a, out result ); }
		public static void Divide ( ref Vector2 a, ref Vector2 b, out Vector2 result ) { result = new Vector2 ( a.X / b.X, a.Y / b.Y ); }
		public static void Divide ( ref Vector2 a, float b, out Vector2 result ) { result = new Vector2 ( a.X / b, a.Y / b ); }

		public static float Dot ( Vector2 v1, Vector2 v2 ) { float result; Dot ( ref v1, ref v2, out result ); return result; }
		public static Vector2 Cross ( Vector2 v1, float v2 ) { Vector2 result; Cross ( ref v1, v2, out result ); return result; }
		public static Vector2 Cross ( float v1, Vector2 v2 ) { Vector2 result; Cross ( ref v2, v1, out result ); return result; }
		public static Vector2 Cross ( Vector2 v1, Vector2 v2 ) { Vector2 result; Cross ( ref v1, ref v2, out result ); return result; }

		public static void Dot ( ref Vector2 v1, ref Vector2 v2, out float result ) { result = v1.X * v2.X + v1.Y * v2.Y; }
		public static void Cross ( ref Vector2 v1, float v2, out Vector2 result ) { result = new Vector2 ( v1.Y * v2, v1.X * -v2 ); }
		public static void Cross ( float v1, ref Vector2 v2, out Vector2 result ) { Cross ( ref v2, v1, out result ); }
		public static void Cross ( ref Vector2 v1, ref Vector2 v2, out Vector2 result ) { result = new Vector2 ( v1.X * v2.Y, v1.Y * v2.X ); }
	}
}
