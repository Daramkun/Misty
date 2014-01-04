using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Matrix2x2
	{
		public static Matrix2x2 operator + ( Matrix2x2 v1, Matrix2x2 v2 ) { Matrix2x2 result; Add ( ref v1, ref v2, out result ); return result; }
		public static Matrix2x2 operator - ( Matrix2x2 v1, Matrix2x2 v2 ) { Matrix2x2 result; Subtract ( ref v1, ref v2, out result ); return result; }
		public static Matrix2x2 operator * ( Matrix2x2 v1, Matrix2x2 v2 ) { Matrix2x2 result; Multiply ( ref v1, ref v2, out result ); return result; }
		public static Matrix2x2 operator * ( Matrix2x2 v1, float v2 ) { Matrix2x2 result; Multiply ( ref v1, v2, out result ); return result; }
		public static Matrix2x2 operator * ( float v1, Matrix2x2 v2 ) { Matrix2x2 result; Multiply ( ref v2, v1, out result ); return result; }
		public static Matrix2x2 operator / ( Matrix2x2 v1, Matrix2x2 v2 ) { Matrix2x2 result; Divide ( ref v1, ref v2, out result ); return result; }
		public static Matrix2x2 operator / ( Matrix2x2 v1, float v2 ) { Matrix2x2 result; Divide ( ref v1, v2, out result ); return result; }
		public static Matrix2x2 operator ! ( Matrix2x2 v1 ) { Matrix2x2 result; Transpose ( ref v1, out result ); return result; }
		public static Matrix2x2 operator ~ ( Matrix2x2 v1 ) { Matrix2x2 result; Invert ( ref v1, out result ); return result; }

		public static bool operator == ( Matrix2x2 v1, Matrix2x2 v2 ) { return v1.Equals ( v2 ); }
		public static bool operator != ( Matrix2x2 v1, Matrix2x2 v2 ) { return !v1.Equals ( v2 ); }

		public override bool Equals ( object obj )
		{
			if ( !( obj is Matrix2x2 ) ) return false;
			Matrix2x2 t = ( Matrix2x2 ) obj;
			return Column1 == t.Column1 && Column2 == t.Column2;
		}

		public static Matrix2x2 Add ( Matrix2x2 v1, Matrix2x2 v2 ) { return v1 + v2; }
		public static Matrix2x2 Subtract ( Matrix2x2 v1, Matrix2x2 v2 ) { return v1 - v2; }
		public static Matrix2x2 Multiply ( Matrix2x2 v1, Matrix2x2 v2 ) { return v1 * v2; }
		public static Matrix2x2 Multiply ( Matrix2x2 v1, float v2 ) { return v1 * v2; }
		public static Matrix2x2 Multiply ( float v1, Matrix2x2 v2 ) { return v2 * v1; }
		public static Matrix2x2 Divide ( Matrix2x2 v1, Matrix2x2 v2 ) { return v1 / v2; }
		public static Matrix2x2 Divide ( Matrix2x2 v1, float v2 ) { return v1 / v2; }
		public static Matrix2x2 Transpose ( Matrix2x2 v1 ) { return !v1; }
		public static Matrix2x2 Invert ( Matrix2x2 v1 ) { return ~v1; }

		public static void Add ( ref Matrix2x2 v1, ref Matrix2x2 v2, out Matrix2x2 result )
		{ result = new Matrix2x2 ( v1.M11 + v2.M11, v1.M12 + v2.M12, v1.M21 + v2.M21, v1.M22 + v2.M22 ); }
		public static void Subtract ( ref Matrix2x2 v1, ref Matrix2x2 v2, out Matrix2x2 result )
		{ result = new Matrix2x2 ( v1.M11 - v2.M11, v1.M12 - v2.M12, v1.M21 - v2.M21, v1.M22 - v2.M22 ); }
		public static void Multiply ( ref Matrix2x2 v1, ref Matrix2x2 v2, out Matrix2x2 result )
		{
			result = new Matrix2x2 (
				( v1.M11 * v2.M11 ) + ( v1.M12 * v2.M21 ),
				( v1.M11 * v2.M12 ) + ( v1.M12 * v2.M22 ),
				( v1.M21 * v2.M11 ) + ( v1.M22 * v2.M21 ),
				( v1.M21 * v2.M12 ) + ( v1.M22 * v2.M22 )
			);
		}
		public static void Multiply ( ref Matrix2x2 v1, float v2, out Matrix2x2 result ) { result = new Matrix2x2 ( v1.M11 * v2, v1.M12 * v2, v1.M21 * v2, v1.M22 * v2 ); }
		public static void Multiply ( float v1, ref Matrix2x2 v2, out Matrix2x2 result ) { Multiply ( ref v2, v1, out result ); }
		public static void Divide ( ref Matrix2x2 v1, ref Matrix2x2 v2, out Matrix2x2 result ) 
		{ result = new Matrix2x2 ( v1.M11 / v2.M11, v1.M12 / v2.M12, v1.M21 / v2.M21, v1.M22 / v2.M22 ); }
		public static void Divide ( ref Matrix2x2 v1, float v2, out Matrix2x2 result ) { result = new Matrix2x2 ( v1.M11 / v2, v1.M12 / v2, v1.M21 / v2, v1.M22 / v2 ); }
		public static void Transpose ( ref Matrix2x2 v1, out Matrix2x2 result ) { result = new Matrix2x2 ( v1.M11, v1.M21, v1.M12, v1.M22 ); }
		public static void Invert ( ref Matrix2x2 v1, out Matrix2x2 result )
		{
			float d = 1 / ( v1.M11 * v1.M22 - v1.M12 * v1.M21 );
			result = new Matrix2x2 ( d * v1.M22, d * v1.M12, d * v1.M21, d * v1.M11 );
		}
	}
}
