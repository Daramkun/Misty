using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Quaternion
	{
		public static Quaternion operator + ( Quaternion a, Quaternion b ) { Quaternion result; Add ( ref a, ref b, out result ); return result; }
		public static Quaternion operator - ( Quaternion a ) { Quaternion result; Negate ( ref a, out result ); return result; }
		public static Quaternion operator - ( Quaternion a, Quaternion b ) { Quaternion result; Subtract ( ref a, ref b, out result ); return result; }
		public static Quaternion operator * ( Quaternion a, Quaternion b ) { Quaternion result; Multiply ( ref a, ref b, out result ); return result; }
		public static Quaternion operator * ( Quaternion a, float b ) { Quaternion result; Multiply ( ref a, b, out result ); return result; }
		public static Quaternion operator * ( float a, Quaternion b ) { Quaternion result; Multiply ( ref b, a, out result ); return result; }
		public static Quaternion operator / ( Quaternion a, Quaternion b ) { Quaternion result; Divide ( ref a, ref b, out result ); return result; }
		public static Quaternion operator / ( Quaternion a, float b ) { Quaternion result; Divide ( ref a, b, out result ); return result; }
		public static Quaternion operator ~ ( Quaternion a ) { Quaternion result; Invert ( ref a, out result ); return result; }

		public static bool operator == ( Quaternion a, Quaternion b ) { return a.Equals ( b ); }
		public static bool operator != ( Quaternion a, Quaternion b ) { return !( a == b ); }

		public override bool Equals ( object obj )
		{
			if ( !( obj is Quaternion ) ) return false;
			Quaternion q = ( Quaternion ) obj;
			return X == q.X && Y == q.Y && Z == q.Z && W == q.W;
		}

		public static Quaternion Add ( Quaternion a, Quaternion b ) { return a + b; }
		public static Quaternion Subtract ( Quaternion a, Quaternion b ) { return a - b; }
		public static Quaternion Multiply ( Quaternion a, Quaternion b ) { return a * b; }
		public static Quaternion Multiply ( Quaternion a, float b ) { return a * b; }
		public static Quaternion Multiply ( float a, Quaternion b ) { return b * a; }
		public static Quaternion Dividie ( Quaternion a, Quaternion b ) { return a / b; }
		public static Quaternion Divide ( Quaternion a, float b ) { return a / b; }
		public static Quaternion Invert ( Quaternion a ) { return ~a; }

		public static void Add ( ref Quaternion a, ref Quaternion b, out Quaternion result )
		{ result = new Quaternion ( a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W ); }
		public static void Negate ( ref Quaternion a, out Quaternion result )
		{ result = new Quaternion ( -a.X, -a.Y, -a.Z, -a.W ); }
		public static void Subtract ( ref Quaternion a, ref Quaternion b, out Quaternion result )
		{ result = new Quaternion ( a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W ); }
		public static void Multiply ( ref Quaternion a, ref Quaternion b, out Quaternion result )
		{
			float x = a.X, y = a.Y, z = a.Z, w = a.W;
			float num4 = b.X, num3 = b.Y, num2 = b.Z, num1 = b.W;
			float num12 = ( y * num2 ) - ( z * num3 ), num11 = ( z * num4 ) - ( x * num2 ),
				num10 = ( x * num3 ) - ( y * num4 ), num9 = ( ( x * num4 ) + ( y * num3 ) ) + ( z * num2 );
			result = new Quaternion ( ( ( x * num1 ) + ( num4 * w ) ) + num12, ( ( y * num1 ) + ( num3 * w ) ) + num11,
				( ( z * num1 ) + ( num2 * w ) ) + num10, ( w * num1 ) - num9 );
		}
		public static void Multiply ( ref Quaternion a, float b, out Quaternion result )
		{ result = new Quaternion ( a.X * b, a.Y * b, a.Z * b, a.W * b ); }
		public static void Multiply ( float a, ref Quaternion b, out Quaternion result ) { Multiply ( ref b, a, out result ); }
		public static void Divide ( ref Quaternion a, ref Quaternion b, out Quaternion result )
		{
			float x = a.X, y = a.Y, z = a.Z, w = a.W;
			float num14 = ( ( ( b.X * b.X ) + ( b.Y * b.Y ) ) + ( b.Z * b.Z ) ) + ( b.W * b.W );
			float num5 = 1f / num14, num4 = -b.X * num5, num3 = -b.Y * num5, num2 = -b.Z * num5, num1 = b.W * num5;
			float num13 = ( y * num2 ) - ( z * num3 ), num12 = ( z * num4 ) - ( x * num2 );
			float num11 = ( x * num3 ) - ( y * num4 ), num10 = ( ( x * num4 ) + ( y * num3 ) ) + ( z * num2 );
			result = new Quaternion ( ( ( x * num1 ) + ( num4 * w ) ) + num13, ( ( y * num1 ) + ( num3 * w ) ) + num12,
				( ( z * num1 ) + ( num2 * w ) ) + num11, ( w * num1 ) - num10 );
		}
		public static void Divide ( ref Quaternion a, float b, out Quaternion result )
		{ result = new Quaternion ( a.X / b, a.Y / b, a.Z / b, a.W / b ); }
		public static void Invert ( ref Quaternion a, out Quaternion result )
		{ result = new Quaternion ( -a.X / a.LengthSquared, -a.Y / a.LengthSquared, -a.Z / a.LengthSquared, a.W / a.LengthSquared ); }

		public static float Dot ( Quaternion a, Quaternion b ) { float result; Dot ( ref a, ref b, out result ); return result; }
		public static void Dot ( ref Quaternion a, ref Quaternion b, out float result )
		{
			result = ( ( ( ( a.X * b.X ) + ( a.Y * b.Y ) ) + ( a.Z * b.Z ) ) + ( a.W * b.W ) );
		}
	}
}
