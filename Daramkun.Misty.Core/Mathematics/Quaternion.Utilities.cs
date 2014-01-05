using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Quaternion
	{
		public static Quaternion Conjugate ( Quaternion v ) { Quaternion result; Conjugate ( ref v, out result ); return result; }
		public static Quaternion Concatenate ( Quaternion v1, Quaternion v2 ) { Quaternion result; Concatenate ( ref v1, ref v2, out result ); return result; }
		public static Quaternion Lerp ( Quaternion v1, Quaternion v2, float amount ) { Quaternion result; Lerp ( ref v1, ref v2, amount, out result ); return result; }
		public static Quaternion Slerp ( Quaternion v1, Quaternion v2, float amount ) { Quaternion result; Slerp ( ref v1, ref v2, amount, out result ); return result; }

		public static void Conjugate ( ref Quaternion v, out Quaternion result ) { result = new Quaternion ( -v.X, -v.Y, -v.Z, v.W ); }
		public static void Concatenate ( ref Quaternion v1, ref Quaternion v2, out Quaternion result )
		{
			float x = v2.X;
			float y = v2.Y;
			float z = v2.Z;
			float w = v2.W;
			float num4 = v1.X;
			float num3 = v1.Y;
			float num2 = v1.Z;
			float num = v1.W;
			float num12 = ( y * num2 ) - ( z * num3 );
			float num11 = ( z * num4 ) - ( x * num2 );
			float num10 = ( x * num3 ) - ( y * num4 );
			float num9 = ( ( x * num4 ) + ( y * num3 ) ) + ( z * num2 );
			result = new Quaternion (
				( ( x * num ) + ( num4 * w ) ) + num12,
				( ( y * num ) + ( num3 * w ) ) + num11,
				( ( z * num ) + ( num2 * w ) ) + num10,
				( w * num ) - num9
			);
		}
		public static void Lerp ( ref Quaternion v1, ref Quaternion v2, float amount, out Quaternion result )
		{
			float num = amount;
			float num2 = 1f - num;
			float num5 = ( ( ( v1.X * v2.X ) + ( v1.Y * v2.Y ) ) + ( v1.Z * v2.Z ) ) + ( v1.W * v2.W );
			if ( num5 >= 0f )
				result = new Quaternion (
					( num2 * v1.X ) + ( num * v2.X ),
					( num2 * v1.Y ) + ( num * v2.Y ),
					( num2 * v1.Z ) + ( num * v2.Z ),
					( num2 * v1.W ) + ( num * v2.W )
				);
			else
				result = new Quaternion (
					( num2 * v1.X ) - ( num * v2.X ),
					( num2 * v1.Y ) - ( num * v2.Y ),
					( num2 * v1.Z ) - ( num * v2.Z ),
					( num2 * v1.W ) - ( num * v2.W )
				);
			float num4 = ( ( ( result.X * result.X ) + ( result.Y * result.Y ) ) + ( result.Z * result.Z ) ) + ( result.W * result.W );
			float num3 = 1f / ( ( float ) Math.Sqrt ( ( double ) num4 ) );
			result *= num3;
		}
		public static void Slerp ( ref Quaternion v1, ref Quaternion v2, float amount, out Quaternion result )
		{
			float num2, num3, num = amount;
			float num4 = ( ( ( v1.X * v2.X ) + ( v1.Y * v2.Y ) ) + ( v1.Z * v2.Z ) ) + ( v1.W * v2.W );
			bool flag = false;
			if ( num4 < 0f ) { flag = true; num4 = -num4; }
			if ( num4 > 0.999999f ) { num3 = 1f - num; num2 = flag ? -num : num; }
			else
			{
				float num5 = ( float ) Math.Acos ( ( double ) num4 );
				float num6 = ( float ) ( 1.0 / Math.Sin ( ( double ) num5 ) );
				num3 = ( ( float ) Math.Sin ( ( double ) ( ( 1f - num ) * num5 ) ) ) * num6;
				num2 = flag ? ( ( ( float ) -Math.Sin ( ( double ) ( num * num5 ) ) ) * num6 ) : ( ( ( float ) Math.Sin ( ( double ) ( num * num5 ) ) ) * num6 );
			}
			result = new Quaternion(
				( num3 * v1.X ) + ( num2 * v2.X ),
				( num3 * v1.Y ) + ( num2 * v2.Y ),
				( num3 * v1.Z ) + ( num2 * v2.Z ),
				( num3 * v1.W ) + ( num2 * v2.W )
			);
		}
	}
}
