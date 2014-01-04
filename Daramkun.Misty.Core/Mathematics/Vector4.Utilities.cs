using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector4
	{
		public static float Distance ( Vector4 v1, Vector4 v2 ) { float result; Distance ( ref v1, ref v2, out result ); return result; }
		public static float DistanceSquared ( Vector4 v1, Vector4 v2 ) { float result; DistanceSquared ( ref v1, ref v2, out result ); return result; }

		public static void Distance ( ref Vector4 v1, ref Vector4 v2, out float result ) { result = ( v2 - v1 ).Length; }
		public static void DistanceSquared ( ref Vector4 v1, ref Vector4 v2, out float result ) { result = ( v2 - v1 ).LengthSquared; }

		public static Vector4 Max ( Vector4 v1, Vector4 v2 ) { Vector4 result; Max ( ref v1, ref v2, out result ); return result; }
		public static Vector4 Min ( Vector4 v1, Vector4 v2 ) { Vector4 result; Min ( ref v1, ref v2, out result ); return result; }
		public static Vector4 Clamp ( Vector4 v1, Vector4 v2, Vector4 v3 ) { Vector4 result; Clamp ( ref v1, ref v2, ref v3, out result ); return result; }
		public static Vector4 Absolute ( Vector4 v ) { Vector4 result; Absolute ( ref v, out result ); return result; }
		public static Vector4 Barycentric ( Vector4 value1, Vector4 value2, Vector4 value3, float amount1, float amount2 )
		{
			Vector4 result;
			Barycentric ( ref value1, ref value2, ref value3, amount1, amount2, out result );
			return result;
		}
		public static Vector4 CatmullRom ( Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount )
		{
			Vector4 result;
			CatmullRom ( ref value1, ref value2, ref value3, ref value4, amount, out result );
			return result;
		}
		public static Vector4 Reflect ( Vector4 vector, Vector4 normal )
		{
			Vector4 result;
			Reflect ( ref vector, ref normal, out result );
			return result;
		}
		public static Vector4 Hermite ( Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount )
		{
			Vector4 result;
			Hermite ( ref value1, ref tangent1, ref value2, ref tangent2, amount, out result );
			return result;
		}
		public static Vector4 Lerp ( Vector4 value1, Vector4 value2, float amount )
		{
			Vector4 result;
			Lerp ( ref value1, ref value2, amount, out result );
			return result;
		}
		public static Vector4 SmoothStep ( Vector4 value1, Vector4 value2, float amount )
		{
			Vector4 result;
			SmoothStep ( ref value1, ref value2, amount, out result );
			return result;
		}

		public static void Max ( ref Vector4 v1, ref Vector4 v2, out Vector4 result )
		{ result = new Vector4 ( Math.Max ( v1.X, v2.X ), Math.Max ( v1.Y, v2.Y ), Math.Max ( v1.Z, v2.Z ), Math.Max ( v1.W, v2.W ) ); }
		public static void Min ( ref Vector4 v1, ref Vector4 v2, out Vector4 result )
		{ result = new Vector4 ( Math.Min ( v1.X, v2.X ), Math.Min ( v1.Y, v2.Y ), Math.Min ( v1.Z, v2.Z ), Math.Min ( v1.W, v2.W ) ); }
		public static void Clamp ( ref Vector4 v1, ref Vector4 v2, ref Vector4 v3, out Vector4 result ) { result = Max ( v2, Min ( v1, v3 ) ); }
		public static void Absolute ( ref Vector4 v, out Vector4 result )
		{ result = new Vector4 ( Math.Abs ( v.X ), Math.Abs ( v.Y ), Math.Abs ( v.Z ), Math.Abs ( v.W ) ); }
		public static void Barycentric ( ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, float amount1, float amount2, out Vector4 result )
		{
			result = new Vector4 (
				MistyMath.Barycentric ( value1.X, value2.X, value3.X, amount1, amount2 ),
				MistyMath.Barycentric ( value1.Y, value2.Y, value3.Y, amount1, amount2 ),
				MistyMath.Barycentric ( value1.Z, value2.Z, value3.Z, amount1, amount2 ),
				MistyMath.Barycentric ( value1.W, value2.W, value3.W, amount1, amount2 )
			);
		}
		public static void CatmullRom ( ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, ref Vector4 value4, float amount, out Vector4 result )
		{
			result = new Vector4 (
				MistyMath.CatmullRom ( value1.X, value2.X, value3.X, value4.X, amount ),
				MistyMath.CatmullRom ( value1.Y, value2.Y, value3.Y, value4.Y, amount ),
				MistyMath.CatmullRom ( value1.Z, value2.Z, value3.Z, value4.Z, amount ),
				MistyMath.CatmullRom ( value1.W, value2.W, value3.W, value4.W, amount )
			);
		}
		public static void Reflect ( ref Vector4 vector, ref Vector4 normal, out Vector4 result )
		{
			float val = 2.0f * ( ( vector.X * normal.X ) + ( vector.Y * normal.Y ) );
			result = new Vector4 ( vector.X - ( normal.X * val ), vector.Y - ( normal.Y * val ), vector.Z - ( normal.Z * val ), vector.W - ( normal.W * val ) );
		}
		public static void Hermite ( ref Vector4 value1, ref Vector4 tangent1, ref Vector4 value2, ref Vector4 tangent2, float amount, out Vector4 result )
		{
			result = new Vector4 (
				MistyMath.Hermite ( value1.X, tangent1.X, value2.X, tangent2.X, amount ),
				MistyMath.Hermite ( value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount ),
				MistyMath.Hermite ( value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount ),
				MistyMath.Hermite ( value1.W, tangent1.W, value2.W, tangent2.W, amount )
			);
		}
		public static void Lerp ( ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result )
		{
			result = new Vector4 (
				MistyMath.Lerp ( value1.X, value2.X, amount ),
				MistyMath.Lerp ( value1.Y, value2.Y, amount ),
				MistyMath.Lerp ( value1.Z, value2.Z, amount ),
				MistyMath.Lerp ( value1.W, value2.W, amount )
			);
		}
		public static void SmoothStep ( ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result )
		{
			result = new Vector4 (
				MistyMath.SmoothStep ( value1.X, value2.X, amount ),
				MistyMath.SmoothStep ( value1.Y, value2.Y, amount ),
				MistyMath.SmoothStep ( value1.Z, value2.Z, amount ),
				MistyMath.SmoothStep ( value1.W, value2.W, amount )
			);
		}
	}
}
