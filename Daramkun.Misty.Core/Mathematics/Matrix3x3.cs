using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Matrix3x3
	{
		public float M11, M12, M13, M21, M22, M23, M31, M32, M33;

		public Vector3 Column1 { get { return new Vector3 ( M11, M12, M13 ); } set { M11 = value.X; M12 = value.Y; M13 = value.Z; } }
		public Vector3 Column2 { get { return new Vector3 ( M21, M22, M23 ); } set { M21 = value.X; M22 = value.Y; M23 = value.Z; } }
		public Vector3 Column3 { get { return new Vector3 ( M31, M32, M33 ); } set { M31 = value.X; M32 = value.Y; M33 = value.Z; } }

		public static readonly Matrix3x3 Identity = new Matrix3x3 (
																	1, 0, 0,
																	0, 1, 0,
																	0, 0, 1
																	);

		public Matrix3x3 ( float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33 )
		{
			M11 = m11; M12 = m12; M13 = m13;
			M21 = m21; M22 = m22; M23 = m23;
			M31 = m31; M32 = m32; M33 = m33;
		}

		public Matrix3x3 ( Vector3 column1, Vector3 column2, Vector3 column3 )
			: this (
				column1.X, column1.Y, column1.Z,
				column2.X, column2.Y, column2.Z,
				column3.X, column3.Y, column3.Z
			) { }

		public Matrix3x3 ( ref Vector3 column1, ref Vector3 column2, ref Vector3 column3 )
			: this (
				column1.X, column1.Y, column1.Z,
				column2.X, column2.Y, column2.Z,
				column3.X, column3.Y, column3.Z
			) { }

		public Matrix3x3 ( float [] matrix3x3 )
		{
			M11 = matrix3x3 [ 0 ]; M12 = matrix3x3 [ 1 ]; M13 = matrix3x3 [ 2 ];
			M21 = matrix3x3 [ 3 ]; M22 = matrix3x3 [ 4 ]; M23 = matrix3x3 [ 5 ];
			M31 = matrix3x3 [ 6 ]; M32 = matrix3x3 [ 7 ]; M33 = matrix3x3 [ 8 ];
		}

		public Vector2 Solve ( Vector2 vector )
		{
			float a11 = M11, a12 = M21, a21 = M12, a22 = M22;
			float det = a11 * a22 - a12 * a21;
			if ( det != 0.0f ) det = 1.0f / det;
			return new Vector2 ( det * ( a22 * vector.X - a12 * vector.Y ), det * ( a11 * vector.Y - a21 * vector.X ) );
		}

		public Vector3 Solve ( Vector3 vector )
		{
			float det = Vector3.Dot ( new Vector3 ( M11, M12, M13 ),
				Vector3.Cross ( new Vector3 ( M21, M22, M23 ), new Vector3 ( M31, M32, M33 ) ) );
			if ( det != 0 ) det = 1.0f / det;
			return new Vector3 (
				det * Vector3.Dot ( vector, Vector3.Cross ( new Vector3 ( M21, M22, M23 ), new Vector3 ( M31, M32, M33 ) ) ),
				det * Vector3.Dot ( new Vector3 ( M11, M12, M13 ), Vector3.Cross ( vector, new Vector3 ( M31, M32, M33 ) ) ),
				det * Vector3.Dot ( new Vector3 ( M11, M12, M13 ), Vector3.Cross ( new Vector3 ( M21, M22, M23 ), vector ) )
			);
		}

		public override int GetHashCode () { return ToString ().GetHashCode (); }

		public override string ToString ()
		{
			return String.Format ( "{{11:{0}, 12:{1}, 13:{2}} {21:{3}, 22:{4}, 23:{5}} {31:{6}, 32:{7}, 33:{8}}}",
				M11, M12, M13, M21, M22, M23, M31, M32, M33 );
		}

		public float [] ToArray ()
		{
			return new float []
			{
				M11, M12, M13,
				M21, M22, M23,
				M31, M32, M33,
			};
		}

		public float this [ int index ]
		{
			get
			{
				switch ( index )
				{
					case 0: return M11; case 1: return M12; case 2: return M13;
					case 3: return M21; case 4: return M22; case 5: return M23;
					case 6: return M31; case 7: return M32; case 8: return M33;
					default: throw new IndexOutOfRangeException ();
				}
			}
			set
			{
				switch ( index )
				{
					case 0: M11 = value; break; case 1: M12 = value; break; case 2: M13 = value; break;
					case 3: M21 = value; break; case 4: M22 = value; break; case 5: M23 = value; break;
					case 6: M31 = value; break; case 7: M32 = value; break; case 8: M33 = value; break;
					default: throw new IndexOutOfRangeException ();
				}
			}
		}

		public float this [ int x, int y ]
		{
			get { return this [ x + ( y * 3 ) ]; }
			set { this [ x + ( y * 3 ) ] = value; }
		}
	}
}
