using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Matrix3x3
	{
		public static Matrix3x3 operator + ( Matrix3x3 v1, Matrix3x3 v2 ) { Matrix3x3 result; Add ( ref v1, ref v2, out result ); return result; }
		public static Matrix3x3 operator - ( Matrix3x3 v1, Matrix3x3 v2 ) { Matrix3x3 result; Subtract ( ref v1, ref v2, out result ); return result; }
		public static Matrix3x3 operator * ( Matrix3x3 v1, Matrix3x3 v2 ) { Matrix3x3 result; Multiply ( ref v1, ref v2, out result ); return result; }
		public static Matrix3x3 operator * ( Matrix3x3 v1, float v2 ) { Matrix3x3 result; Multiply ( ref v1, v2, out result ); return result; }
		public static Matrix3x3 operator * ( float v1, Matrix3x3 v2 ) { Matrix3x3 result; Multiply ( ref v2, v1, out result ); return result; }
		public static Matrix3x3 operator / ( Matrix3x3 v1, Matrix3x3 v2 ) { Matrix3x3 result; Divide ( ref v1, ref v2, out result ); return result; }
		public static Matrix3x3 operator / ( Matrix3x3 v1, float v2 ) { Matrix3x3 result; Divide ( ref v1, v2, out result ); return result; }
		public static Matrix3x3 operator ! ( Matrix3x3 v1 ) { Matrix3x3 result; Transpose ( ref v1, out result ); return result; }
		public static Matrix3x3 operator ~ ( Matrix3x3 v1 ) { Matrix3x3 result; Invert ( ref v1, out result ); return result; }

		public static bool operator == ( Matrix3x3 v1, Matrix3x3 v2 ) { return v1.Equals ( v2 ); }
		public static bool operator != ( Matrix3x3 v1, Matrix3x3 v2 ) { return !v1.Equals ( v2 ); }

		public override bool Equals ( object obj )
		{
			if ( !( obj is Matrix3x3 ) ) return false;
			Matrix3x3 t = ( Matrix3x3 ) obj;
			return Column1 == t.Column1 && Column2 == t.Column2 && Column3 == t.Column3;
		}

		public static Matrix3x3 Add ( Matrix3x3 v1, Matrix3x3 v2 ) { return v1 + v2; }
		public static Matrix3x3 Subtract ( Matrix3x3 v1, Matrix3x3 v2 ) { return v1 - v2; }
		public static Matrix3x3 Multiply ( Matrix3x3 v1, Matrix3x3 v2 ) { return v1 * v2; }
		public static Matrix3x3 Multiply ( Matrix3x3 v1, float v2 ) { return v1 * v2; }
		public static Matrix3x3 Multiply ( float v1, Matrix3x3 v2 ) { return v2 * v1; }
		public static Matrix3x3 Divide ( Matrix3x3 v1, Matrix3x3 v2 ) { return v1 / v2; }
		public static Matrix3x3 Divide ( Matrix3x3 v1, float v2 ) { return v1 / v2; }
		public static Matrix3x3 Transpose ( Matrix3x3 v1 ) { return !v1; }
		public static Matrix3x3 Invert ( Matrix3x3 v1 ) { return ~v1; }

		public static void Add ( ref Matrix3x3 v1, ref Matrix3x3 v2, out Matrix3x3 result )
		{
			result = new Matrix3x3 (
				v1.M11 + v2.M11, v1.M12 + v2.M12, v1.M13 + v2.M13,
				v1.M21 + v2.M21, v1.M22 + v2.M22, v1.M23 + v2.M23,
				v1.M31 + v2.M31, v1.M32 + v2.M32, v1.M33 + v2.M33
			);
		}
		public static void Subtract ( ref Matrix3x3 v1, ref Matrix3x3 v2, out Matrix3x3 result )
		{
			result = new Matrix3x3 (
				v1.M11 - v2.M11, v1.M12 - v2.M12, v1.M13 - v2.M13,
				v1.M21 - v2.M21, v1.M22 - v2.M22, v1.M23 - v2.M23,
				v1.M31 - v2.M31, v1.M32 - v2.M32, v1.M33 - v2.M33
			);
		}
		public static void Multiply ( ref Matrix3x3 v1, ref Matrix3x3 v2, out Matrix3x3 result )
		{
			result = new Matrix3x3 (
				( v1.M11 * v2.M11 ) + ( v1.M12 * v2.M21 ) + ( v1.M13 * v2.M31 ),
				( v1.M11 * v2.M12 ) + ( v1.M12 * v2.M22 ) + ( v1.M13 * v2.M32 ),
				( v1.M11 * v2.M13 ) + ( v1.M12 * v2.M23 ) + ( v1.M13 * v2.M33 ),
				( v1.M21 * v2.M11 ) + ( v1.M22 * v2.M21 ) + ( v1.M23 * v2.M31 ),
				( v1.M21 * v2.M12 ) + ( v1.M22 * v2.M22 ) + ( v1.M23 * v2.M32 ),
				( v1.M21 * v2.M13 ) + ( v1.M22 * v2.M23 ) + ( v1.M23 * v2.M33 ),
				( v1.M31 * v2.M11 ) + ( v1.M32 * v2.M21 ) + ( v1.M33 * v2.M31 ),
				( v1.M31 * v2.M12 ) + ( v1.M32 * v2.M22 ) + ( v1.M33 * v2.M32 ),
				( v1.M31 * v2.M13 ) + ( v1.M32 * v2.M23 ) + ( v1.M33 * v2.M33 )
			);
		}
		public static void Multiply ( ref Matrix3x3 v1, float v2, out Matrix3x3 result )
		{
			result = new Matrix3x3 (
				v1.M11 * v2, v1.M12 * v2, v1.M13 * v2,
				v1.M21 * v2, v1.M22 * v2, v1.M23 * v2,
				v1.M31 * v2, v1.M32 * v2, v1.M33 * v2
			);
		}
		public static void Multiply ( float v1, ref Matrix3x3 v2, out Matrix3x3 result ) { Multiply ( ref v2, v1, out result ); }
		public static void Divide ( ref Matrix3x3 v1, ref Matrix3x3 v2, out Matrix3x3 result )
		{
			result = new Matrix3x3 (
				v1.M11 / v2.M11, v1.M12 / v2.M12, v1.M13 / v2.M13,
				v1.M21 / v2.M21, v1.M22 / v2.M22, v1.M23 / v2.M23,
				v1.M31 / v2.M32, v1.M32 / v2.M32, v1.M33 / v2.M33
			);
		}
		public static void Divide ( ref Matrix3x3 v1, float v2, out Matrix3x3 result )
		{
			result = new Matrix3x3 (
				v1.M11 / v2, v1.M12 / v2, v1.M13 / v2,
				v1.M21 / v2, v1.M22 / v2, v1.M23 / v2,
				v1.M31 / v2, v1.M32 / v2, v1.M33 / v2
			);
		}
		public static void Transpose ( ref Matrix3x3 v1, out Matrix3x3 result )
		{
			result = new Matrix3x3 ( v1.M11, v1.M21, v1.M31, v1.M12, v1.M22, v1.M32, v1.M13, v1.M23, v1.M33 );
		}
		public static void Invert ( ref Matrix3x3 v1, out Matrix3x3 result )
		{
			float det = Vector3.Dot ( new Vector3 ( v1.M11, v1.M12, v1.M13 ), new Vector3 ( v1.M31, v1.M32, v1.M33 ) );
			if ( det != 0 ) det = 1.0f / det;
			float a11 = v1.M11, a12 = v1.M21, a13 = v1.M31;
			float a22 = v1.M22, a23 = v1.M32;
			float a33 = v1.M33;
			result = new Matrix3x3 (
				det * ( a22 * a33 - a23 * a23 ), det * ( a13 * a23 - a12 * a33 ), det * ( a12 * a23 - a13 * a22 ),
				v1.M12, det * ( a11 * a33 - a13 * a13 ), det * ( a13 * a12 - a11 * a23 ),
				v1.M13, v1.M23, det * ( a11 * a22 - a12 * a12 )
			);
		}
	}
}
