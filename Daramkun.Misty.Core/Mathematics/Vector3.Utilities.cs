using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector3
	{
		public static float Distance ( Vector3 v1, Vector3 v2 ) { float result; Distance ( ref v1, ref v2, out result ); return result; }
		public static float DistanceSquared ( Vector3 v1, Vector3 v2 ) { float result; DistanceSquared ( ref v1, ref v2, out result ); return result; }

		public static void Distance ( ref Vector3 v1, ref Vector3 v2, out float result ) { result = ( v2 - v1 ).Length; }
		public static void DistanceSquared ( ref Vector3 v1, ref Vector3 v2, out float result ) { result = ( v2 - v1 ).LengthSquared; }

		public static Vector3 Max ( Vector3 v1, Vector3 v2 ) { Vector3 result; Max ( ref v1, ref v2, out result ); return result; }
		public static Vector3 Min ( Vector3 v1, Vector3 v2 ) { Vector3 result; Min ( ref v1, ref v2, out result ); return result; }
		public static Vector3 Clamp ( Vector3 v1, Vector3 v2, Vector3 v3 ) { Vector3 result; Clamp ( ref v1, ref v2, ref v3, out result ); return result; }
		public static Vector3 Absolute ( Vector3 v ) { Vector3 result; Absolute ( ref v, out result ); return result; }
		public static Vector3 Barycentric ( Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2 )
		{
			Vector3 result;
			Barycentric ( ref value1, ref value2, ref value3, amount1, amount2, out result );
			return result;
		}
		public static Vector3 CatmullRom ( Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount )
		{
			Vector3 result;
			CatmullRom ( ref value1, ref value2, ref value3, ref value4, amount, out result );
			return result;
		}
		public static Vector3 Reflect ( Vector3 vector, Vector3 normal )
		{
			Vector3 result;
			Reflect ( ref vector, ref normal, out result );
			return result;
		}
		public static Vector3 Hermite ( Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount )
		{
			Vector3 result;
			Hermite ( ref value1, ref tangent1, ref value2, ref tangent2, amount, out result );
			return result;
		}
		public static Vector3 Lerp ( Vector3 value1, Vector3 value2, float amount )
		{
			Vector3 result;
			Lerp ( ref value1, ref value2, amount, out result );
			return result;
		}
		public static Vector3 SmoothStep ( Vector3 value1, Vector3 value2, float amount )
		{
			Vector3 result;
			SmoothStep ( ref value1, ref value2, amount, out result );
			return result;
		}

		public static void Max ( ref Vector3 v1, ref Vector3 v2, out Vector3 result ) { result = new Vector3 ( Math.Max ( v1.X, v2.X ), Math.Max ( v1.Y, v2.Y ), Math.Max ( v1.Z, v2.Z ) ); }
		public static void Min ( ref Vector3 v1, ref Vector3 v2, out Vector3 result ) { result = new Vector3 ( Math.Min ( v1.X, v2.X ), Math.Min ( v1.Y, v2.Y ), Math.Min ( v1.Z, v2.Z ) ); }
		public static void Clamp ( ref Vector3 v1, ref Vector3 v2, ref Vector3 v3, out Vector3 result ) { result = Max ( v2, Min ( v1, v3 ) ); }
		public static void Absolute ( ref Vector3 v, out Vector3 result ) { result = new Vector3 ( Math.Abs ( v.X ), Math.Abs ( v.Y ), Math.Abs ( v.Z ) ); }
		public static void Barycentric ( ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result )
		{
			result = new Vector3 (
				MistyMath.Barycentric ( value1.X, value2.X, value3.X, amount1, amount2 ),
				MistyMath.Barycentric ( value1.Y, value2.Y, value3.Y, amount1, amount2 ),
				MistyMath.Barycentric ( value1.Z, value2.Z, value3.Z, amount1, amount2 )
			);
		}
		public static void CatmullRom ( ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result )
		{
			result = new Vector3 (
				MistyMath.CatmullRom ( value1.X, value2.X, value3.X, value4.X, amount ),
				MistyMath.CatmullRom ( value1.Y, value2.Y, value3.Y, value4.Y, amount ),
				MistyMath.CatmullRom ( value1.Z, value2.Z, value3.Z, value4.Z, amount )
			);
		}
		public static void Reflect ( ref Vector3 vector, ref Vector3 normal, out Vector3 result )
		{
			float val = 2.0f * ( ( vector.X * normal.X ) + ( vector.Y * normal.Y ) );
			result = new Vector3 ( vector.X - ( normal.X * val ), vector.Y - ( normal.Y * val ), vector.Z - ( normal.Z * val ) );
		}
		public static void Hermite ( ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result )
		{
			result = new Vector3 (
				MistyMath.Hermite ( value1.X, tangent1.X, value2.X, tangent2.X, amount ),
				MistyMath.Hermite ( value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount ),
				MistyMath.Hermite ( value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount )
			);
		}
		public static void Lerp ( ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result )
		{
			result = new Vector3 (
				MistyMath.Lerp ( value1.X, value2.X, amount ),
				MistyMath.Lerp ( value1.Y, value2.Y, amount ),
				MistyMath.Lerp ( value1.Z, value2.Z, amount )
			);
		}
		public static void SmoothStep ( ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result )
		{
			result = new Vector3 (
				MistyMath.SmoothStep ( value1.X, value2.X, amount ),
				MistyMath.SmoothStep ( value1.Y, value2.Y, amount ),
				MistyMath.SmoothStep ( value1.Z, value2.Z, amount )
			);
		}
	}
}
