using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public static class MistyMath
	{
		public const float PI = 3.1415926536f;
		public const float PIOver2 = PI / 2;
		public const float PIOver4 = PI / 4;
		public const float TwoPI = PI * 2;
		public const float E = 2.7182818285f;
		public const float Log10E = 0.4342944819f;
		public const float Log2E = 1.4426950409f;

		public static float Barycentric ( float value1, float value2, float value3, float amount1, float amount2 )
		{ return value1 + ( value2 - value1 ) * amount1 + ( value3 - value1 ) * amount2; }

		public static float CatmullRom ( float value1, float value2, float value3, float value4, float amount )
		{
			double amountSquared = amount * amount;
			double amountCubed = amountSquared * amount;
			return ( float ) ( 0.5 * ( 2.0 * value2 + ( value3 - value1 ) * amount +
				( 2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4 ) * amountSquared +
				( 3.0 * value2 - value1 - 3.0 * value3 + value4 ) * amountCubed ) );
		}

		public static float Clamp ( float value, float min, float max )
		{
			value = ( value > max ) ? max : value;
			value = ( value < min ) ? min : value;
			return value;
		}

		public static int Clamp ( int value, int min, int max )
		{
			value = ( value > max ) ? max : value;
			value = ( value < min ) ? min : value;
			return value;
		}

		public static float Distance ( float value1, float value2 ) { return Math.Abs ( value1 - value2 ); }

		public static float Hermite ( float value1, float tangent1, float value2, float tangent2, float amount )
		{
			float v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
			float sCubed = s * s * s, sSquared = s * s;

			if ( amount == 0f ) result = value1;
			else if ( amount == 1f ) result = value2;
			else result = ( 2 * v1 - 2 * v2 + t2 + t1 ) * sCubed + ( 3 * v2 - 3 * v1 - 2 * t1 - t2 ) * sSquared + t1 * s + v1;
			return result;
		}

		public static float Lerp ( float value1, float value2, float amount ) { return value1 + ( value2 - value1 ) * amount; }

		public static float SmoothStep ( float value1, float value2, float amount ) 
		{ return MistyMath.Hermite ( value1, 0f, value2, 0f, MistyMath.Clamp ( amount, 0f, 1f ) ); }

		public static float ToDegrees ( float radian ) { return radian * 180 / PI; }
		public static float ToRadians ( float degree ) { return degree * PI / 180; }

		public static float PowerOf2 ( float value ) { return value * value; }
	}
}
