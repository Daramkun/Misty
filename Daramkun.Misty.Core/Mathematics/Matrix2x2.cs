using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Matrix2x2
	{
		public float M11, M12, M21, M22;

		public Vector2 Column1 { get { return new Vector2 ( M11, M12 ); } set { M11 = value.X; M12 = value.Y; } }
		public Vector2 Column2 { get { return new Vector2 ( M21, M22 ); } set { M21 = value.X; M22 = value.Y; } }

		public static readonly Matrix2x2 Identity = new Matrix2x2 (
																	1, 0,
																	0, 1
																	);

		public Matrix2x2 ( float m11, float m12, float m21, float m22 )
		{
			M11 = m11; M12 = m12;
			M21 = m21; M22 = m22;
		}
		public Matrix2x2 ( Vector2 column1, Vector2 column2 ) : this ( column1.X, column1.Y, column2.X, column2.Y ) { }
		public Matrix2x2 ( float [] matrix2x2 )
		{
			M11 = matrix2x2 [ 0 ]; M12 = matrix2x2 [ 1 ];
			M21 = matrix2x2 [ 2 ]; M22 = matrix2x2 [ 3 ];
		}

		public Vector2 Solve ( Vector2 vector )
		{
			float a11 = M11, a12 = M21, a21 = M12, a22 = M22;
			float det = a11 * a22 - a12 * a21;
			if ( det != 0.0f ) det = 1.0f / det;
			return new Vector2 ( det * ( a22 * vector.X - a12 * vector.Y ), det * ( a11 * vector.Y - a21 * vector.X ) );
		}

		public override int GetHashCode () { return ToString ().GetHashCode (); }

		public override string ToString ()
		{
			return String.Format ( "{{11:{0}, 12:{1}} {21:{2}, 22:{3}}}", M11, M12, M21, M22 );
		}

		public float [] ToArray ()
		{
			return new float []
			{
				M11, M12,
				M21, M22,
			};
		}

		public float this [ int index ]
		{
			get
			{
				switch ( index )
				{
					case 0: return M11;
					case 1: return M12;
					case 2: return M21;
					case 3: return M22;
					default: throw new IndexOutOfRangeException ();
				}
			}
			set
			{
				switch ( index )
				{
					case 0: M11 = value; break;
					case 1: M12 = value; break;
					case 2: M21 = value; break;
					case 3: M22 = value; break;
					default: throw new IndexOutOfRangeException ();
				}
			}
		}

		public float this [ int x, int y ]
		{
			get { return this [ x + ( y * 2 ) ]; }
			set { this [ x + ( y * 2 ) ] = value; }
		}
	}
}
