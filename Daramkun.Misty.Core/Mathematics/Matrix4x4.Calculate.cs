using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Matrix4x4
	{
		public static Matrix4x4 operator + ( Matrix4x4 v1, Matrix4x4 v2 ) { Matrix4x4 result; Add ( ref v1, ref v2, out result ); return result; }
		public static Matrix4x4 operator - ( Matrix4x4 v1, Matrix4x4 v2 ) { Matrix4x4 result; Subtract ( ref v1, ref v2, out result ); return result; }
		public static Matrix4x4 operator * ( Matrix4x4 v1, Matrix4x4 v2 ) { Matrix4x4 result; Multiply ( ref v1, ref v2, out result ); return result; }
		public static Matrix4x4 operator * ( Matrix4x4 v1, float v2 ) { Matrix4x4 result; Multiply ( ref v1, v2, out result ); return result; }
		public static Matrix4x4 operator * ( float v1, Matrix4x4 v2 ) { Matrix4x4 result; Multiply ( ref v2, v1, out result ); return result; }
		public static Matrix4x4 operator / ( Matrix4x4 v1, Matrix4x4 v2 ) { Matrix4x4 result; Divide ( ref v1, ref v2, out result ); return result; }
		public static Matrix4x4 operator / ( Matrix4x4 v1, float v2 ) { Matrix4x4 result; Divide ( ref v1, v2, out result ); return result; }
		public static Matrix4x4 operator ! ( Matrix4x4 v1 ) { Matrix4x4 result; Transpose ( ref v1, out result ); return result; }
		public static Matrix4x4 operator ~ ( Matrix4x4 v1 ) { Matrix4x4 result; Invert ( ref v1, out result ); return result; }

		public static bool operator == ( Matrix4x4 v1, Matrix4x4 v2 ) { return v1.Equals ( v2 ); }
		public static bool operator != ( Matrix4x4 v1, Matrix4x4 v2 ) { return !v1.Equals ( v2 ); }

		public override bool Equals ( object obj )
		{
			if ( !( obj is Matrix4x4 ) ) return false;
			Matrix4x4 t = ( Matrix4x4 ) obj;
			return Column1 == t.Column1 && Column2 == t.Column2 && Column3 == t.Column3 && Column4 == t.Column4;
		}

		public static Matrix4x4 Add ( Matrix4x4 v1, Matrix4x4 v2 ) { return v1 + v2; }
		public static Matrix4x4 Subtract ( Matrix4x4 v1, Matrix4x4 v2 ) { return v1 - v2; }
		public static Matrix4x4 Multiply ( Matrix4x4 v1, Matrix4x4 v2 ) { return v1 * v2; }
		public static Matrix4x4 Multiply ( Matrix4x4 v1, float v2 ) { return v1 * v2; }
		public static Matrix4x4 Multiply ( float v1, Matrix4x4 v2 ) { return v2 * v1; }
		public static Matrix4x4 Divide ( Matrix4x4 v1, Matrix4x4 v2 ) { return v1 / v2; }
		public static Matrix4x4 Divide ( Matrix4x4 v1, float v2 ) { return v1 / v2; }
		public static Matrix4x4 Transpose ( Matrix4x4 v1 ) { return !v1; }
		public static Matrix4x4 Invert ( Matrix4x4 v1 ) { return ~v1; }

		public static void Add ( ref Matrix4x4 v1, ref Matrix4x4 v2, out Matrix4x4 result )
		{
			result = new Matrix4x4 (
				v1.M11 + v2.M11, v1.M12 + v2.M12, v1.M13 + v2.M13, v1.M14 + v2.M14,
				v1.M21 + v2.M21, v1.M22 + v2.M22, v1.M23 + v2.M23, v1.M24 + v2.M24,
				v1.M31 + v2.M31, v1.M32 + v2.M32, v1.M33 + v2.M33, v1.M34 + v2.M34,
				v1.M41 + v2.M41, v1.M42 + v2.M42, v1.M43 + v2.M43, v1.M44 + v2.M44
			);
		}
		public static void Subtract ( ref Matrix4x4 v1, ref Matrix4x4 v2, out Matrix4x4 result )
		{
			result = new Matrix4x4 (
				v1.M11 - v2.M11, v1.M12 - v2.M12, v1.M13 - v2.M13, v1.M14 - v2.M14,
				v1.M21 - v2.M21, v1.M22 - v2.M22, v1.M23 - v2.M23, v1.M24 - v2.M24,
				v1.M31 - v2.M31, v1.M32 - v2.M32, v1.M33 - v2.M33, v1.M34 - v2.M34,
				v1.M41 - v2.M41, v1.M42 - v2.M42, v1.M43 - v2.M43, v1.M44 - v2.M44
			);
		}
		public static void Multiply ( ref Matrix4x4 v1, ref Matrix4x4 v2, out Matrix4x4 result )
		{
			var m11 = ( ( ( v1.M11 * v2.M11 ) + ( v1.M12 * v2.M21 ) ) + ( v1.M13 * v2.M31 ) ) + ( v1.M14 * v2.M41 );
			var m12 = ( ( ( v1.M11 * v2.M12 ) + ( v1.M12 * v2.M22 ) ) + ( v1.M13 * v2.M32 ) ) + ( v1.M14 * v2.M42 );
			var m13 = ( ( ( v1.M11 * v2.M13 ) + ( v1.M12 * v2.M23 ) ) + ( v1.M13 * v2.M33 ) ) + ( v1.M14 * v2.M43 );
			var m14 = ( ( ( v1.M11 * v2.M14 ) + ( v1.M12 * v2.M24 ) ) + ( v1.M13 * v2.M34 ) ) + ( v1.M14 * v2.M44 );
			var m21 = ( ( ( v1.M21 * v2.M11 ) + ( v1.M22 * v2.M21 ) ) + ( v1.M23 * v2.M31 ) ) + ( v1.M24 * v2.M41 );
			var m22 = ( ( ( v1.M21 * v2.M12 ) + ( v1.M22 * v2.M22 ) ) + ( v1.M23 * v2.M32 ) ) + ( v1.M24 * v2.M42 );
			var m23 = ( ( ( v1.M21 * v2.M13 ) + ( v1.M22 * v2.M23 ) ) + ( v1.M23 * v2.M33 ) ) + ( v1.M24 * v2.M43 );
			var m24 = ( ( ( v1.M21 * v2.M14 ) + ( v1.M22 * v2.M24 ) ) + ( v1.M23 * v2.M34 ) ) + ( v1.M24 * v2.M44 );
			var m31 = ( ( ( v1.M31 * v2.M11 ) + ( v1.M32 * v2.M21 ) ) + ( v1.M33 * v2.M31 ) ) + ( v1.M34 * v2.M41 );
			var m32 = ( ( ( v1.M31 * v2.M12 ) + ( v1.M32 * v2.M22 ) ) + ( v1.M33 * v2.M32 ) ) + ( v1.M34 * v2.M42 );
			var m33 = ( ( ( v1.M31 * v2.M13 ) + ( v1.M32 * v2.M23 ) ) + ( v1.M33 * v2.M33 ) ) + ( v1.M34 * v2.M43 );
			var m34 = ( ( ( v1.M31 * v2.M14 ) + ( v1.M32 * v2.M24 ) ) + ( v1.M33 * v2.M34 ) ) + ( v1.M34 * v2.M44 );
			var m41 = ( ( ( v1.M41 * v2.M11 ) + ( v1.M42 * v2.M21 ) ) + ( v1.M43 * v2.M31 ) ) + ( v1.M44 * v2.M41 );
			var m42 = ( ( ( v1.M41 * v2.M12 ) + ( v1.M42 * v2.M22 ) ) + ( v1.M43 * v2.M32 ) ) + ( v1.M44 * v2.M42 );
			var m43 = ( ( ( v1.M41 * v2.M13 ) + ( v1.M42 * v2.M23 ) ) + ( v1.M43 * v2.M33 ) ) + ( v1.M44 * v2.M43 );
			var m44 = ( ( ( v1.M41 * v2.M14 ) + ( v1.M42 * v2.M24 ) ) + ( v1.M43 * v2.M34 ) ) + ( v1.M44 * v2.M44 );
			result = new Matrix4x4 ( m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44 );
		}
		public static void Multiply ( ref Matrix4x4 v1, float v2, out Matrix4x4 result )
		{
			result = new Matrix4x4 (
				v1.M11 * v2, v1.M12 * v2, v1.M13 * v2, v1.M14 * v2,
				v1.M21 * v2, v1.M22 * v2, v1.M23 * v2, v1.M24 * v2,
				v1.M31 * v2, v1.M32 * v2, v1.M33 * v2, v1.M34 * v2,
				v1.M41 * v2, v1.M42 * v2, v1.M43 * v2, v1.M44 * v2
			);
		}
		public static void Multiply ( float v1, ref Matrix4x4 v2, out Matrix4x4 result ) { Multiply ( ref v2, v1, out result ); }
		public static void Divide ( ref Matrix4x4 v1, ref Matrix4x4 v2, out Matrix4x4 result )
		{
			result = new Matrix4x4 (
				v1.M11 / v2.M11, v1.M12 / v2.M12, v1.M13 / v2.M13, v1.M14 / v2.M14,
				v1.M21 / v2.M21, v1.M22 / v2.M22, v1.M23 / v2.M23, v1.M24 / v2.M24,
				v1.M31 / v2.M32, v1.M32 / v2.M32, v1.M33 / v2.M33, v1.M34 / v2.M34,
				v1.M41 / v2.M41, v1.M42 / v2.M42, v1.M43 / v2.M43, v1.M44 / v2.M44
			);
		}
		public static void Divide ( ref Matrix4x4 v1, float v2, out Matrix4x4 result )
		{
			result = new Matrix4x4 (
				v1.M11 / v2, v1.M12 / v2, v1.M13 / v2, v1.M14 / v2,
				v1.M21 / v2, v1.M22 / v2, v1.M23 / v2, v1.M24 / v2,
				v1.M31 / v2, v1.M32 / v2, v1.M33 / v2, v1.M34 / v2,
				v1.M41 / v2, v1.M42 / v2, v1.M43 / v2, v1.M44 / v2
			);
		}
		public static void Transpose ( ref Matrix4x4 v1, out Matrix4x4 result )
		{
			result = new Matrix4x4 (
				v1.M11, v1.M21, v1.M31, v1.M41,
				v1.M12, v1.M22, v1.M32, v1.M42,
				v1.M13, v1.M23, v1.M33, v1.M43,
				v1.M14, v1.M24, v1.M34, v1.M44
			);
		}
		public static void Invert ( ref Matrix4x4 v1, out Matrix4x4 result )
		{
			float det1 = v1.M11 * v1.M22 - v1.M12 * v1.M21;
			float det2 = v1.M11 * v1.M23 - v1.M13 * v1.M21;
			float det3 = v1.M11 * v1.M24 - v1.M14 * v1.M21;
			float det4 = v1.M12 * v1.M23 - v1.M13 * v1.M22;
			float det5 = v1.M12 * v1.M24 - v1.M14 * v1.M22;
			float det6 = v1.M13 * v1.M24 - v1.M14 * v1.M23;
			float det7 = v1.M31 * v1.M42 - v1.M32 * v1.M41;
			float det8 = v1.M31 * v1.M43 - v1.M33 * v1.M41;
			float det9 = v1.M31 * v1.M44 - v1.M34 * v1.M41;
			float det10 = v1.M32 * v1.M43 - v1.M33 * v1.M42;
			float det11 = v1.M32 * v1.M44 - v1.M34 * v1.M42;
			float det12 = v1.M33 * v1.M44 - v1.M34 * v1.M43;

			float detMatrix = ( float ) ( det1 * det12 - det2 * det11 + det3 * det10 + det4 * det9 - det5 * det8 + det6 * det7 );
			float invDetMatrix = 1f / detMatrix;

			result = new Matrix4x4 ();
			result.M11 = ( v1.M22 * det12 - v1.M23 * det11 + v1.M24 * det10 ) * invDetMatrix;
			result.M12 = ( -v1.M12 * det12 + v1.M13 * det11 - v1.M14 * det10 ) * invDetMatrix;
			result.M13 = ( v1.M42 * det6 - v1.M43 * det5 + v1.M44 * det4 ) * invDetMatrix;
			result.M14 = ( -v1.M32 * det6 + v1.M33 * det5 - v1.M34 * det4 ) * invDetMatrix;
			result.M21 = ( -v1.M21 * det12 + v1.M23 * det9 - v1.M24 * det8 ) * invDetMatrix;
			result.M22 = ( v1.M11 * det12 - v1.M13 * det9 + v1.M14 * det8 ) * invDetMatrix;
			result.M23 = ( -v1.M41 * det6 + v1.M43 * det3 - v1.M44 * det2 ) * invDetMatrix;
			result.M24 = ( v1.M31 * det6 - v1.M33 * det3 + v1.M34 * det2 ) * invDetMatrix;
			result.M31 = ( v1.M21 * det11 - v1.M22 * det9 + v1.M24 * det7 ) * invDetMatrix;
			result.M32 = ( -v1.M11 * det11 + v1.M12 * det9 - v1.M14 * det7 ) * invDetMatrix;
			result.M33 = ( v1.M41 * det5 - v1.M42 * det3 + v1.M44 * det1 ) * invDetMatrix;
			result.M34 = ( -v1.M31 * det5 + v1.M32 * det3 - v1.M34 * det1 ) * invDetMatrix;
			result.M41 = ( -v1.M21 * det10 + v1.M22 * det8 - v1.M23 * det7 ) * invDetMatrix;
			result.M42 = ( v1.M11 * det10 - v1.M12 * det8 + v1.M13 * det7 ) * invDetMatrix;
			result.M43 = ( -v1.M41 * det4 + v1.M42 * det2 - v1.M43 * det1 ) * invDetMatrix;
			result.M44 = ( v1.M31 * det4 - v1.M32 * det2 + v1.M33 * det1 ) * invDetMatrix;
		}
	}
}
