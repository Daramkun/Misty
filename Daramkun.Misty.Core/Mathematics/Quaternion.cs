using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Quaternion
	{
		public static readonly Quaternion Identity = new Quaternion ( 0, 0, 0, 1 );

		public float X, Y, Z, W;

		public float LengthSquared { get { return X * X + Y * Y + Z * Z + W * W; } }
		public float Length { get { return ( float ) Math.Sqrt ( LengthSquared ); } }

		public Quaternion ( float x, float y, float z, float w ) { X = x; Y = y; Z = z; W = w; }
		public Quaternion ( Vector3 vectorPart, float scalarPart ) : this ( vectorPart.X, vectorPart.Y, vectorPart.Z, scalarPart ) { }
		public Quaternion ( Vector4 vector ) : this ( vector.X, vector.Y, vector.Z, vector.W ) { }
		public Quaternion ( float yaw, float pitch, float roll )
		{
			float num9 = roll * 0.5f, num6 = ( float ) Math.Sin ( num9 ), num5 = ( float ) Math.Cos ( num9 );
			float num8 = pitch * 0.5f, num4 = ( float ) Math.Sin ( num8 ), num3 = ( float ) Math.Cos ( num8 );
			float num7 = yaw * 0.5f, num2 = ( float ) Math.Sin ( num7 ), num = ( float ) Math.Cos ( num7 );
			X = ( ( num * num4 ) * num5 ) + ( ( num2 * num3 ) * num6 );
			Y = ( ( num2 * num3 ) * num5 ) - ( ( num * num4 ) * num6 );
			Z = ( ( num * num3 ) * num6 ) - ( ( num2 * num4 ) * num5 );
			W = ( ( num * num3 ) * num5 ) + ( ( num2 * num4 ) * num6 );
		}
		public Quaternion ( Matrix4x4 rotMatrix )
		{
			float num8 = ( rotMatrix.M11 + rotMatrix.M22 ) + rotMatrix.M33;
			if ( num8 > 0f )
			{
				float num = ( float ) Math.Sqrt ( ( double ) ( num8 + 1f ) );
				num = 0.5f / num;
				X = ( rotMatrix.M23 - rotMatrix.M32 ) * num;
				Y = ( rotMatrix.M31 - rotMatrix.M13 ) * num;
				Z = ( rotMatrix.M12 - rotMatrix.M21 ) * num;
				W = num * 0.5f;
			}
			else if ( ( rotMatrix.M11 >= rotMatrix.M22 ) && ( rotMatrix.M11 >= rotMatrix.M33 ) )
			{
				float num7 = ( float ) Math.Sqrt ( ( double ) ( ( ( 1f + rotMatrix.M11 ) - rotMatrix.M22 ) - rotMatrix.M33 ) );
				float num4 = 0.5f / num7;
				X = 0.5f * num7;
				Y = ( rotMatrix.M12 + rotMatrix.M21 ) * num4;
				Z = ( rotMatrix.M13 + rotMatrix.M31 ) * num4;
				W = ( rotMatrix.M23 - rotMatrix.M32 ) * num4;
			}
			else if ( rotMatrix.M22 > rotMatrix.M33 )
			{
				float num6 = ( float ) Math.Sqrt ( ( ( 1f + rotMatrix.M22 ) - rotMatrix.M11 ) - rotMatrix.M33 );
				float num3 = 0.5f / num6;
				X = ( rotMatrix.M21 + rotMatrix.M12 ) * num3;
				Y = 0.5f * num6;
				Z = ( rotMatrix.M32 + rotMatrix.M23 ) * num3;
				W = ( rotMatrix.M31 - rotMatrix.M13 ) * num3;
			}
			else
			{
				float num5 = ( float ) Math.Sqrt ( ( double ) ( ( ( 1f + rotMatrix.M33 ) - rotMatrix.M11 ) - rotMatrix.M22 ) );
				float num2 = 0.5f / num5;
				X = ( rotMatrix.M31 + rotMatrix.M13 ) * num2;
				Y = ( rotMatrix.M32 + rotMatrix.M23 ) * num2;
				Z = 0.5f * num5;
				W = ( rotMatrix.M12 - rotMatrix.M21 ) * num2;
			}
		}

		public Quaternion Normalize () { return Normalize ( this ); }
		public void Normalize ( out Quaternion result ) { Normalize ( ref this, out result ); }
		public static Quaternion Normalize ( Quaternion v ) { Quaternion result; Normalize ( ref v, out result ); return result; }
		public static void Normalize ( ref Quaternion v, out Quaternion result )
		{ result = new Quaternion ( v.X / v.Length, v.Y / v.Length, v.Z / v.Length, v.W / v.Length ); }

		public override int GetHashCode () { return ( int ) ( Length ); }

		public override string ToString () { return string.Format ( "{{X:{0}, Y:{1}, Z:{2}, W:{3}}}", X, Y, Z, W ); }

		public Matrix4x4 ToMatrix4x4 () { Matrix4x4 result; ToMatrix4x4 ( out result ); return result; }
		public void ToMatrix4x4 ( out Matrix4x4 result ) { result = new Matrix4x4 ( this ); }
		public Vector4 ToVector4 () { Vector4 result; ToVector4 ( out result ); return result; }
		public void ToVector4 ( out Vector4 result ) { result = new Vector4 ( X, Y, Z, W ); }
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
