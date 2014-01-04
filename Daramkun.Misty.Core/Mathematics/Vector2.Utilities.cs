using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector2
	{
		public static float Distance ( Vector2 v1, Vector2 v2 ) { float result; Distance ( ref v1, ref v2, out result ); return result; }
		public static float DistanceSquared ( Vector2 v1, Vector2 v2 ) { float result; DistanceSquared ( ref v1, ref v2, out result ); return result; }

		public static void Distance ( ref Vector2 v1, ref Vector2 v2, out float result ) { result = ( v2 - v1 ).Length; }
		public static void DistanceSquared ( ref Vector2 v1, ref Vector2 v2, out float result ) { result = ( v2 - v1 ).LengthSquared; }

		public static Vector2 Skew ( Vector2 v ) { Vector2 result; Skew ( ref v, out result ); return result; }
		public static Vector2 Max ( Vector2 v1, Vector2 v2 ) { Vector2 result; Max ( ref v1, ref v2, out result ); return result; }
		public static Vector2 Min ( Vector2 v1, Vector2 v2 ) { Vector2 result; Min ( ref v1, ref v2, out result ); return result; }
		public static Vector2 Clamp ( Vector2 v1, Vector2 v2, Vector2 v3 ) { Vector2 result; Clamp ( ref v1, ref v2, ref v3, out result ); return result; }
		public static Vector2 Absolute ( Vector2 v ) { Vector2 result; Absolute ( ref v, out result ); return result; }
		public static Vector2 Barycentric ( Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2 )
		{
			Vector2 result;
			Barycentric ( ref value1, ref value2, ref value3, amount1, amount2, out result );
			return result;
		}
		public static Vector2 CatmullRom ( Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount )
		{
			Vector2 result;
			CatmullRom ( ref value1, ref value2, ref value3, ref value4, amount, out result );
			return result;
		}
		public static Vector2 Reflect ( Vector2 vector, Vector2 normal )
		{
			Vector2 result;
			Reflect ( ref vector, ref normal, out result );
			return result;
		}
		public static Vector2 Hermite ( Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount )
		{
			Vector2 result;
			Hermite ( ref value1, ref tangent1, ref value2, ref tangent2, amount, out result );
			return result;
		}
		public static Vector2 Lerp ( Vector2 value1, Vector2 value2, float amount )
		{
			Vector2 result;
			Lerp ( ref value1, ref value2, amount, out result );
			return result;
		}
		public static Vector2 SmoothStep ( Vector2 value1, Vector2 value2, float amount )
		{
			Vector2 result;
			SmoothStep ( ref value1, ref value2, amount, out result );
			return result;
		}

		public static void Skew ( ref Vector2 v, out Vector2 result ) { result = new Vector2 ( -v.Y, v.X ); }
		public static void Max ( ref Vector2 v1, ref Vector2 v2, out Vector2 result ) { result = new Vector2 ( Math.Max ( v1.X, v2.X ), Math.Max ( v1.Y, v2.Y ) ); }
		public static void Min ( ref Vector2 v1, ref Vector2 v2, out Vector2 result ) { result = new Vector2 ( Math.Min ( v1.X, v2.X ), Math.Min ( v1.Y, v2.Y ) ); }
		public static void Clamp ( ref Vector2 v1, ref Vector2 v2, ref Vector2 v3, out Vector2 result ) { result = Max ( v2, Min ( v1, v3 ) ); }
		public static void Absolute ( ref Vector2 v, out Vector2 result ) { result = new Vector2 ( Math.Abs ( v.X ), Math.Abs ( v.Y ) ); }
		public static void Barycentric ( ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result )
		{
			result = new Vector2 (
				MistyMath.Barycentric ( value1.X, value2.X, value3.X, amount1, amount2 ),
				MistyMath.Barycentric ( value1.Y, value2.Y, value3.Y, amount1, amount2 )
			);
		}
		public static void CatmullRom ( ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result )
		{
			result = new Vector2 (
				MistyMath.CatmullRom ( value1.X, value2.X, value3.X, value4.X, amount ),
				MistyMath.CatmullRom ( value1.Y, value2.Y, value3.Y, value4.Y, amount )
			);
		}
		public static void Reflect ( ref Vector2 vector, ref Vector2 normal, out Vector2 result )
		{
			float val = 2.0f * ( ( vector.X * normal.X ) + ( vector.Y * normal.Y ) );
			result = new Vector2 ( vector.X - ( normal.X * val ), vector.Y - ( normal.Y * val ) );
		}
		public static void Hermite ( ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result )
		{
			result = new Vector2 (
				MistyMath.Hermite ( value1.X, tangent1.X, value2.X, tangent2.X, amount ),
				MistyMath.Hermite ( value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount )
			);
		}
		public static void Lerp ( ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result )
		{
			result = new Vector2 (
				MistyMath.Lerp ( value1.X, value2.X, amount ),
				MistyMath.Lerp ( value1.Y, value2.Y, amount )
			);
		}
		public static void SmoothStep ( ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result )
		{
			result = new Vector2 (
				MistyMath.SmoothStep ( value1.X, value2.X, amount ),
				MistyMath.SmoothStep ( value1.Y, value2.Y, amount )
			);
		}

		public Vector2 Skew () { return Skew ( this ); }
		public void Skew ( out Vector2 result ) { Vector2.Skew ( ref this, out result ); }
	}
}
