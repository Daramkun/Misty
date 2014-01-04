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

		public override int GetHashCode () { return ( int ) ( Length ); }

		public override string ToString () { return string.Format ( "{{X:{0}, Y:{1}, Z:{2}, W:{3}}}", X, Y, Z, W ); }

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
					case 0: X = value; break; case 1: Y = value; break; case 2: Z = value; break; case 3: W = value; break;
					default: throw new IndexOutOfRangeException ();
				}
			}
		}
	}
}
