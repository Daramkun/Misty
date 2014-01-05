using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Matrix4x4
	{
		static float [] cachedArray = new float [ 16 ];

		public float
			M11, M12, M13, M14,
			M21, M22, M23, M24,
			M31, M32, M33, M34,
			M41, M42, M43, M44;

		public Vector4 Column1 { get { return new Vector4 ( M11, M12, M13, M14 ); } set { M11 = value.X; M12 = value.Y; M13 = value.Z; M14 = value.W; } }
		public Vector4 Column2 { get { return new Vector4 ( M21, M22, M23, M24 ); } set { M21 = value.X; M22 = value.Y; M23 = value.Z; M24 = value.W; } }
		public Vector4 Column3 { get { return new Vector4 ( M31, M32, M33, M34 ); } set { M31 = value.X; M32 = value.Y; M33 = value.Z; M34 = value.W; } }
		public Vector4 Column4 { get { return new Vector4 ( M41, M42, M43, M44 ); } set { M41 = value.X; M42 = value.Y; M43 = value.Z; M44 = value.W; } }

		public static readonly Matrix4x4 Identity = new Matrix4x4 (
														1f, 0f, 0f, 0f,
														0f, 1f, 0f, 0f,
														0f, 0f, 1f, 0f,
														0f, 0f, 0f, 1f
														);

		public Matrix4x4 (
			float m11, float m12, float m13, float m14,
			float m21, float m22, float m23, float m24,
			float m31, float m32, float m33, float m34,
			float m41, float m42, float m43, float m44
			)
		{
			M11 = m11; M12 = m12; M13 = m13; M14 = m14;
			M21 = m21; M22 = m22; M23 = m23; M24 = m24;
			M31 = m31; M32 = m32; M33 = m33; M34 = m34;
			M41 = m41; M42 = m42; M43 = m43; M44 = m44;
		}

		public Matrix4x4 ( Vector4 column1, Vector4 column2, Vector4 column3, Vector4 column4 )
			: this (
				column1.X, column1.Y, column1.Z, column1.W,
				column2.X, column2.Y, column2.Z, column2.W,
				column3.X, column3.Y, column3.Z, column3.W,
				column4.X, column4.Y, column4.Z, column4.W
			) { }

		public Matrix4x4 ( ref Vector4 column1, ref Vector4 column2, ref Vector4 column3, ref Vector4 column4 )
			: this (
				column1.X, column1.Y, column1.Z, column1.W,
				column2.X, column2.Y, column2.Z, column2.W,
				column3.X, column3.Y, column3.Z, column3.W,
				column4.X, column4.Y, column4.Z, column4.W
			) { }

		public Matrix4x4 ( float [] matrix4x4 )
		{
			M11 = matrix4x4 [ 0 ]; M12 = matrix4x4 [ 1 ]; M13 = matrix4x4 [ 2 ]; M14 = matrix4x4 [ 3 ];
			M21 = matrix4x4 [ 4 ]; M22 = matrix4x4 [ 5 ]; M23 = matrix4x4 [ 6 ]; M24 = matrix4x4 [ 7 ];
			M31 = matrix4x4 [ 8 ]; M32 = matrix4x4 [ 9 ]; M33 = matrix4x4 [ 10 ]; M34 = matrix4x4 [ 11 ];
			M41 = matrix4x4 [ 12 ]; M42 = matrix4x4 [ 13 ]; M43 = matrix4x4 [ 14 ]; M44 = matrix4x4 [ 15 ];
		}

		public Matrix4x4 ( Quaternion q )
		{
			float num9 = q.X * q.X, num8 = q.Y * q.Y, num7 = q.Z * q.Z, num6 = q.X * q.Y;
			float num5 = q.Z * q.W, num4 = q.Z * q.X, num3 = q.Y * q.W, num2 = q.Y * q.Z;
			float num1 = q.X * q.W;
			M11 = 1f - ( 2f * ( num8 + num7 ) ); M12 = 2f * ( num6 + num5 ); M13 = 2f * ( num4 - num3 ); M14 = 0f;
			M21 = 2f * ( num6 - num5 ); M22 = 1f - ( 2f * ( num7 + num9 ) ); M23 = 2f * ( num2 + num1 ); M24 = 0f;
			M31 = 2f * ( num4 + num3 ); M32 = 2f * ( num2 - num1 ); M33 = 1f - ( 2f * ( num8 + num9 ) ); M34 = 0f;
			M41 = 0f; M42 = 0f; M43 = 0f; M44 = 1f;
		}

		public float Determinant ()
		{
			float num22 = M11, num21 = M12, num20 = M13, num19 = M14;
			float num12 = M21, num11 = M22, num10 = M23, num9 = M24;
			float num8 = M31, num7 = M32, num6 = M33, num5 = M34;
			float num4 = M41, num3 = M42, num2 = M43, num1 = M44;
			float num18 = ( num6 * num1 ) - ( num5 * num2 ), num17 = ( num7 * num1 ) - ( num5 * num3 );
			float num16 = ( num7 * num2 ) - ( num6 * num3 ), num15 = ( num8 * num1 ) - ( num5 * num4 );
			float num14 = ( num8 * num2 ) - ( num6 * num4 ), num13 = ( num8 * num3 ) - ( num7 * num4 );
			return (
				( ( ( num22 * ( ( ( num11 * num18 ) - ( num10 * num17 ) ) + ( num9 * num16 ) ) ) -
				( num21 * ( ( ( num12 * num18 ) - ( num10 * num15 ) ) + ( num9 * num14 ) ) ) ) +
				( num20 * ( ( ( num12 * num17 ) - ( num11 * num15 ) ) + ( num9 * num13 ) ) ) ) -
				( num19 * ( ( ( num12 * num16 ) - ( num11 * num14 ) ) + ( num10 * num13 ) ) )
			);
		}

		public override int GetHashCode () { return ToString ().GetHashCode (); }
		public override string ToString ()
		{
			return String.Format (
				"{{11:{0}, 12:{1}, 13:{2}, 14:{3}} {21:{4}, 22:{5}, 23:{6}, 24:{7}} " +
				"{31:{8}, 32:{9}, 33:{10}, 34:{11}} {41:{12}, 42:{13}, 43:{14}, 44:{15}}}",
				M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44
				);
		}

		public float [] ToArray ()
		{
			for ( int i = 0; i < 16; ++i )
				cachedArray [ i ] = this [ i ];
			return cachedArray;
		}

		public float this [ int index ]
		{
			get
			{
				switch ( index )
				{
					case 0: return M11; case 1: return M12; case 2: return M13; case 3: return M14;
					case 4: return M21; case 5: return M22; case 6: return M23; case 7: return M24;
					case 8: return M31; case 9: return M32; case 10: return M33; case 11: return M34;
					case 12: return M41; case 13: return M42; case 14: return M43; case 15: return M44;
					default: throw new IndexOutOfRangeException ();
				}
			}
			set
			{
				switch ( index )
				{
					case 0: M11 = value; break; case 1: M12 = value; break; case 2: M13 = value; break; case 3: M14 = value; break;
					case 4: M21 = value; break; case 5: M22 = value; break; case 6: M23 = value; break; case 7: M24 = value; break;
					case 8: M31 = value; break; case 9: M32 = value; break; case 10: M33 = value; break; case 11: M34 = value; break;
					case 12: M41 = value; break; case 13: M42 = value; break; case 14: M43 = value; break; case 15: M44 = value; break;
					default: throw new IndexOutOfRangeException ();
				}
			}
		}

		public float this [ int x, int y ]
		{
			get { return this [ x + ( y * 4 ) ]; }
			set { this [ x + ( y * 4 ) ] = value; }
		}
	}
}
