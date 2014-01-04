using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Quaternion
	{
		public static Quaternion operator + ( Quaternion a, Quaternion b ) { return new Quaternion ( a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W ); }
		public static Quaternion operator - ( Quaternion a ) { return new Quaternion ( -a.X, -a.Y, -a.Z, -a.W ); }
		public static Quaternion operator - ( Quaternion a, Quaternion b ) { return new Quaternion ( a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W ); }
		public static Quaternion operator * ( Quaternion a, Quaternion b )
		{
			float x = a.X, y = a.Y, z = a.Z, w = a.W;
			float num4 = b.X, num3 = b.Y, num2 = b.Z, num1 = b.W;
			float num12 = ( y * num2 ) - ( z * num3 ), num11 = ( z * num4 ) - ( x * num2 ),
				num10 = ( x * num3 ) - ( y * num4 ), num9 = ( ( x * num4 ) + ( y * num3 ) ) + ( z * num2 );
			return new Quaternion ( ( ( x * num1 ) + ( num4 * w ) ) + num12, ( ( y * num1 ) + ( num3 * w ) ) + num11,
				( ( z * num1 ) + ( num2 * w ) ) + num10, ( w * num1 ) - num9 );
		}
		public static Quaternion operator * ( Quaternion a, float b ) { return new Quaternion ( a.X * b, a.Y * b, a.Z * b, a.W * b ); }
		public static Quaternion operator * ( float a, Quaternion b ) { return new Quaternion ( a * b.X, a * b.Y, a * b.Z, a * b.W ); }
		public static Quaternion operator / ( Quaternion a, Quaternion b )
		{
			float x = a.X, y = a.Y, z = a.Z, w = a.W;
			float num14 = ( ( ( b.X * b.X ) + ( b.Y * b.Y ) ) +
				( b.Z * b.Z ) ) + ( b.W * b.W );
			float num5 = 1f / num14;
			float num4 = -b.X * num5;
			float num3 = -b.Y * num5;
			float num2 = -b.Z * num5;
			float num1 = b.W * num5;
			float num13 = ( y * num2 ) - ( z * num3 );
			float num12 = ( z * num4 ) - ( x * num2 );
			float num11 = ( x * num3 ) - ( y * num4 );
			float num10 = ( ( x * num4 ) + ( y * num3 ) ) + ( z * num2 );
			return new Quaternion ( ( ( x * num1 ) + ( num4 * w ) ) + num13, ( ( y * num1 ) + ( num3 * w ) ) + num12,
				( ( z * num1 ) + ( num2 * w ) ) + num11, ( w * num1 ) - num10 );
		}
		public static Quaternion operator / ( Quaternion a, float b ) { return new Quaternion ( a.X / b, a.Y / b, a.Z / b, a.W / b ); }
		public static Quaternion operator ~ ( Quaternion a )
		{
			Quaternion q;
			float num2 = ( ( ( a.X * a.X ) + ( a.Y * a.Y ) ) + ( a.Z * a.Z ) ) + ( a.W * a.W );
			float num = 1f / num2;
			q.X = -a.X * num;
			q.Y = -a.Y * num;
			q.Z = -a.Z * num;
			q.W = a.W * num;
			return q;
		}

		public static float Dot ( Quaternion a, Quaternion b ) { float result; Dot ( ref a, ref b, out result ); return result; }
		public static void Dot ( ref Quaternion a, ref Quaternion b, out float result )
		{
			result = ( ( ( ( a.X * b.X ) + ( a.Y * b.Y ) ) + ( a.Z * b.Z ) ) + ( a.W * b.W ) );
		}

		public static bool operator == ( Quaternion quaternion1, Quaternion quaternion2 ) { return quaternion1.Equals ( quaternion2 ); }
		public static bool operator != ( Quaternion quaternion1, Quaternion quaternion2 ) { return !( quaternion1 == quaternion2 ); }

		public override bool Equals ( object obj )
		{
			if ( !( obj is Quaternion ) ) return false;
			Quaternion q = ( Quaternion ) obj;
			return X == q.X && Y == q.Y && Z == q.Z && W == q.W;
		}
	}
}
