using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public struct Quaternion
	{
		public static readonly Quaternion Identity = new Quaternion ( 0, 0, 0, 1 );

		public float X, Y, Z, W;

		public Quaternion ( float x, float y, float z, float w ) { X = x; Y = y; Z = z; W = w; }
		public Quaternion ( Vector3 vectorPart, float scalarPart ) : this ( vectorPart.X, vectorPart.Y, vectorPart.Z, scalarPart ) { }
		public Quaternion ( Vector4 vector ) : this ( vector.X, vector.Y, vector.Z, vector.W ) { }
		public Quaternion ( float yaw, float pitch, float roll )
			: this ()
		{
			float num9 = roll * 0.5f;
			float num6 = ( float ) Math.Sin ( num9 );
			float num5 = ( float ) Math.Cos ( num9 );
			float num8 = pitch * 0.5f;
			float num4 = ( float ) Math.Sin ( num8 );
			float num3 = ( float ) Math.Cos ( num8 );
			float num7 = yaw * 0.5f;
			float num2 = ( float ) Math.Sin ( num7 );
			float num = ( float ) Math.Cos ( num7 );

			X = ( ( num * num4 ) * num5 ) + ( ( num2 * num3 ) * num6 );
			Y = ( ( num2 * num3 ) * num5 ) - ( ( num * num4 ) * num6 );
			Z = ( ( num * num3 ) * num6 ) - ( ( num2 * num4 ) * num5 );
			W = ( ( num * num3 ) * num5 ) + ( ( num2 * num4 ) * num6 );
		}
		public Quaternion ( Matrix4x4 rotMatrix )
			: this ()
		{
			Quaternion result = new Quaternion ();
			float num8 = ( rotMatrix.M11 + rotMatrix.M22 ) + rotMatrix.M33;
			if ( num8 > 0f )
			{
				float num = ( float ) Math.Sqrt ( ( double ) ( num8 + 1f ) );
				result.W = num * 0.5f;
				num = 0.5f / num;
				result.X = ( rotMatrix.M23 - rotMatrix.M32 ) * num;
				result.Y = ( rotMatrix.M31 - rotMatrix.M13 ) * num;
				result.Z = ( rotMatrix.M12 - rotMatrix.M21 ) * num;
			}
			else if ( ( rotMatrix.M11 >= rotMatrix.M22 ) && ( rotMatrix.M11 >= rotMatrix.M33 ) )
			{
				float num7 = ( float ) Math.Sqrt ( ( double ) ( ( ( 1f + rotMatrix.M11 ) - rotMatrix.M22 ) - rotMatrix.M33 ) );
				float num4 = 0.5f / num7;
				result.X = 0.5f * num7;
				result.Y = ( rotMatrix.M12 + rotMatrix.M21 ) * num4;
				result.Z = ( rotMatrix.M13 + rotMatrix.M31 ) * num4;
				result.W = ( rotMatrix.M23 - rotMatrix.M32 ) * num4;
			}
			else if ( rotMatrix.M22 > rotMatrix.M33 )
			{
				float num6 = ( float ) Math.Sqrt ( ( ( 1f + rotMatrix.M22 ) - rotMatrix.M11 ) - rotMatrix.M33 );
				float num3 = 0.5f / num6;
				result.X = ( rotMatrix.M21 + rotMatrix.M12 ) * num3;
				result.Y = 0.5f * num6;
				result.Z = ( rotMatrix.M32 + rotMatrix.M23 ) * num3;
				result.W = ( rotMatrix.M31 - rotMatrix.M13 ) * num3;
			}
			else
			{
				float num5 = ( float ) Math.Sqrt ( ( double ) ( ( ( 1f + rotMatrix.M33 ) - rotMatrix.M11 ) - rotMatrix.M22 ) );
				float num2 = 0.5f / num5;
				result.X = ( rotMatrix.M31 + rotMatrix.M13 ) * num2;
				result.Y = ( rotMatrix.M32 + rotMatrix.M23 ) * num2;
				result.Z = 0.5f * num5;
				result.W = ( rotMatrix.M12 - rotMatrix.M21 ) * num2;
			}
			this = result;
		}

		public float LengthSquared { get { return X * X + Y * Y + Z * Z + W * W; } }
		public float Length { get { return ( float ) Math.Sqrt ( LengthSquared ); } }

		public static Quaternion operator + ( Quaternion a, Quaternion b ) { return new Quaternion ( a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W ); }
		public static Quaternion operator - ( Quaternion a ) { return new Quaternion ( -a.X, -a.Y, -a.Z, -a.W ); }
		public static Quaternion operator - ( Quaternion a, Quaternion b ) { return new Quaternion ( a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W ); }
		public static Quaternion operator * ( Quaternion a, Quaternion b )
		{
			float x = a.X, y = a.Y, z = a.Z, w = a.W;
			float num4 = b.X, num3 = b.Y, num2 = b.Z, num1 = b.W;
			float num12 = ( y * num2 ) - ( z * num3 ), num11 = ( z * num4 ) - ( x * num2 ),
				num10 = ( x * num3 ) - ( y * num4 ), num9 = ( ( x * num4 ) + ( y * num3 ) ) + ( z * num2 );
			return new Quaternion ( ( ( x * num1 ) + ( num4 * w ) ) + num12, ( ( y * num1 ) + ( num3 * w ) ) + num11,
				( ( z * num1 ) + ( num2 * w ) ) + num10, ( w * num1 ) - num9 );
		}
		public static Quaternion operator * ( Quaternion a, float b ) { return new Quaternion ( a.X * b, a.Y * b, a.Z * b, a.W * b ); }
		public static Quaternion operator * ( float a, Quaternion b ) { return new Quaternion ( a * b.X, a * b.Y, a * b.Z, a * b.W ); }
		public static Quaternion operator / ( Quaternion a, Quaternion b )
		{
			float x = a.X, y = a.Y, z = a.Z, w = a.W;
			float num14 = ( ( ( b.X * b.X ) + ( b.Y * b.Y ) ) +
				( b.Z * b.Z ) ) + ( b.W * b.W );
			float num5 = 1f / num14;
			float num4 = -b.X * num5;
			float num3 = -b.Y * num5;
			float num2 = -b.Z * num5;
			float num1 = b.W * num5;
			float num13 = ( y * num2 ) - ( z * num3 );
			float num12 = ( z * num4 ) - ( x * num2 );
			float num11 = ( x * num3 ) - ( y * num4 );
			float num10 = ( ( x * num4 ) + ( y * num3 ) ) + ( z * num2 );
			return new Quaternion ( ( ( x * num1 ) + ( num4 * w ) ) + num13, ( ( y * num1 ) + ( num3 * w ) ) + num12,
				( ( z * num1 ) + ( num2 * w ) ) + num11, ( w * num1 ) - num10 );
		}
		public static Quaternion operator / ( Quaternion a, float b ) { return new Quaternion ( a.X / b, a.Y / b, a.Z / b, a.W / b ); }
		public static Quaternion operator ~ ( Quaternion a )
		{
			Quaternion q;
			float num2 = ( ( ( a.X * a.X ) + ( a.Y * a.Y ) ) + ( a.Z * a.Z ) ) + ( a.W * a.W );
			float num = 1f / num2;
			q.X = -a.X * num;
			q.Y = -a.Y * num;
			q.Z = -a.Z * num;
			q.W = a.W * num;
			return q;
		}

		public static float Dot ( Quaternion a, Quaternion b )
		{
			return ( ( ( ( a.X * b.X ) + ( a.Y * b.Y ) ) + ( a.Z * b.Z ) ) + ( a.W * b.W ) );
		}

		public Quaternion Conjugate ()
		{
			Quaternion quaternion;
			quaternion.X = -X;
			quaternion.Y = -Y;
			quaternion.Z = -Z;
			quaternion.W = W;
			return quaternion;
		}

		public Quaternion Concatenate ( Quaternion v )
		{
			Quaternion quaternion;
			float x = v.X;
			float y = v.Y;
			float z = v.Z;
			float w = v.W;
			float num4 = X;
			float num3 = Y;
			float num2 = Z;
			float num = W;
			float num12 = ( y * num2 ) - ( z * num3 );
			float num11 = ( z * num4 ) - ( x * num2 );
			float num10 = ( x * num3 ) - ( y * num4 );
			float num9 = ( ( x * num4 ) + ( y * num3 ) ) + ( z * num2 );
			quaternion.X = ( ( x * num ) + ( num4 * w ) ) + num12;
			quaternion.Y = ( ( y * num ) + ( num3 * w ) ) + num11;
			quaternion.Z = ( ( z * num ) + ( num2 * w ) ) + num10;
			quaternion.W = ( w * num ) - num9;
			return quaternion;
		}

		public void Reset () { this = new Quaternion (); }
		public Quaternion Normalize ()
		{
			Quaternion result;
			float num2 = ( ( ( X * X ) + ( Y * Y ) ) +
				( Z * Z ) ) + ( W * W );
			float num = 1f / ( ( float ) Math.Sqrt ( ( double ) num2 ) );
			result.X = X * num;
			result.Y = Y * num;
			result.Z = Z * num;
			result.W = W * num;
			return result;
		}

		public static bool operator == ( Quaternion quaternion1, Quaternion quaternion2 )
		{
			return ( ( quaternion1.X == quaternion2.X ) && ( quaternion1.Y == quaternion2.Y ) ) &&
				( ( quaternion1.Z == quaternion2.Z ) && ( quaternion1.W == quaternion2.W ) );
		}

		public static bool operator != ( Quaternion quaternion1, Quaternion quaternion2 )
		{
			return !( quaternion1 == quaternion2 );
		}

		public override bool Equals ( object obj )
		{
			if ( !( obj is Quaternion ) ) return false;
			return Length == ( ( Quaternion ) obj ).Length;
		}

		public override int GetHashCode ()
		{
			return ( int ) ( Length );
		}

		public override string ToString ()
		{
			return string.Format ( "{{X:{0}, Y:{1}, Z:{2}, W:{3}}}", X, Y, Z, W );
		}

		public Matrix4x4 ToMatrix4x4 () { return new Matrix4x4 ( this ); }
		public Vector4 ToVector4 () { return new Vector4 ( X, Y, Z, W ); }
		public float [] ToArray () { return new float [] { X, Y, Z, W }; }

		public float this [ int index ]
		{
			get { return ( index == 0 ) ? X : ( ( index == 1 ) ? Y : ( ( index == 2 ) ? Z : ( ( index == 3 ) ? W : float.NaN ) ) ); }
			set
			{
				switch ( index )
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					case 3: W = value; break;
					default: throw new IndexOutOfRangeException ();
				}
			}
		}
	}
}
