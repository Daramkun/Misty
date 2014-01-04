using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector4
	{
		public static readonly Vector4 Zero = new Vector4 ( 0 );
		public static readonly Vector4 One = new Vector4 ( 1 );

		public float X, Y, Z, W;

		public float LengthSquared { get { return X * X + Y * Y + Z * Z + W * W; } }
		public float Length { get { return ( float ) Math.Sqrt ( LengthSquared ); } }
		
		public Vector4 ( float value ) { X = Y = Z = W = value; }
		public Vector4 ( float x, float y, float z, float w ) { X = x; Y = y; Z = z; W = w; }
		public Vector4 ( Vector2 vector, float z, float w ) { X = vector.X; Y = vector.Y; Z = z; W = w; }
		public Vector4 ( Vector3 vector, float w ) { X = vector.X; Y = vector.Y; Z = vector.Z; W = w; }
		public Vector4 ( float [] xyzw ) { X = xyzw [ 0 ]; Y = xyzw [ 1 ]; Z = xyzw [ 2 ]; W = xyzw [ 3 ]; }

		public Vector4 Normalize () { return Normalize ( this ); }
		public void Normalize ( out Vector4 result ) { Normalize ( ref this, out result ); }
		public static Vector4 Normalize ( Vector4 v ) { Vector4 result; Normalize ( ref v, out result ); return result; }
		public static void Normalize ( ref Vector4 v, out Vector4 result ) { result = new Vector4 ( v.X / v.Length, v.Y / v.Length, v.Z / v.Length, v.W / v.Length ); }

		public override int GetHashCode () { return ( int ) ( Length ); }
		public override string ToString () { return string.Format ( "{{X:{0}, Y:{1}, Z:{2}, W:{3}}}", X, Y, Z, W ); }

		public float [] ToArray () { return new float [] { X, Y, Z, W }; }

		public float this [ int index ]
		{
			get { return ( index == 0 ) ? X : ( ( index == 1 ) ? Y : ( ( index == 2 ) ? Z : ( ( index == 3 ) ? W : float.NaN ) ) ); }
			set
			{
				switch ( index )
				{
					case 0: X = value; break; case 1: Y = value; break; case 2: Z = value; break; case 3: W = value; break;
					default: throw new IndexOutOfRangeException ();
				}
			}
		}
	}
}
