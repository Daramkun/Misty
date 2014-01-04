using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector3
	{
		public static readonly Vector3 Zero = new Vector3 ( 0 );
		public static readonly Vector3 One = new Vector3 ( 1 );

		public float X, Y, Z;

		public float LengthSquared { get { return X * X + Y * Y + Z * Z; } }
		public float Length { get { return ( float ) Math.Sqrt ( LengthSquared ); } }
		
		public Vector3 ( float value ) { X = Y = Z = value; }
		public Vector3 ( float x, float y, float z ) { X = x; Y = y; Z = z; }
		public Vector3 ( Vector2 vector, float z ) { X = vector.X; Y = vector.Y; Z = z; }
		public Vector3 ( float [] xyz ) { X = xyz [ 0 ]; Y = xyz [ 1 ]; Z = xyz [ 2 ]; }

		public Vector3 Normalize () { return Normalize ( this ); }
		public void Normalize ( out Vector3 result ) { Normalize ( ref this, out result ); }
		public static Vector3 Normalize ( Vector3 v ) { Vector3 result; Normalize ( ref v, out result ); return result; }
		public static void Normalize ( ref Vector3 v, out Vector3 result ) { result = new Vector3 ( v.X / v.Length, v.Y / v.Length, v.Z / v.Length ); }

		public override int GetHashCode () { return ( int ) ( Length ); }
		public override string ToString () { return String.Format ( "{{X:{0}, Y:{1}, Z:{2}}}", X, Y, Z ); }

		public float [] ToArray () { return new float [] { X, Y, Z }; }

		public float this [ int index ]
		{
			get { return ( index == 0 ) ? X : ( ( index == 1 ) ? Y : ( ( index == 2 ) ? Z : float.NaN ) ); }
			set { switch ( index ) { case 0: X = value; break; case 1: Y = value; break; case 2: Z = value; break; default: throw new IndexOutOfRangeException (); } }
		}
	}
}
