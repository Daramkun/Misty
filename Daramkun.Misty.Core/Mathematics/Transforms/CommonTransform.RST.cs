using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public static partial class CommonTransform
	{
		public static Matrix4x4 RotationX ( float angle )
		{
			Matrix4x4 result;
			RotationX ( angle, out result );
			return result;
		}

		public static void RotationX ( float angle, out Matrix4x4 result )
		{
			result = new Matrix4x4
			(
				1, 0, 0, 0,
				0, ( float ) Math.Cos ( angle ), ( float ) Math.Sin ( angle ), 0,
				0, -( float ) Math.Sin ( angle ), ( float ) Math.Cos ( angle ), 0,
				0, 0, 0, 1
			);
		}

		public static Matrix4x4 RotationY ( float angle )
		{
			Matrix4x4 result;
			RotationY ( angle, out result );
			return result;
		}

		public static void RotationY ( float angle, out Matrix4x4 result )
		{
			result = new Matrix4x4
			(
				( float ) Math.Cos ( angle ), 0, -( float ) Math.Sin ( angle ), 0,
				0, 1, 0, 0,
				( float ) Math.Sin ( angle ), 0, ( float ) Math.Cos ( angle ), 0,
				0, 0, 0, 1
			);
		}

		public static Matrix4x4 RotationZ ( float angle )
		{
			Matrix4x4 result;
			RotationZ ( angle, out result );
			return result;
		}

		public static void RotationZ ( float angle, out Matrix4x4 result )
		{
			result = new Matrix4x4
			(
				( float ) Math.Cos ( angle ), ( float ) Math.Sin ( angle ), 0, 0,
				-( float ) Math.Sin ( angle ), ( float ) Math.Cos ( angle ), 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1
			);
		}
		public static Matrix4x4 RotationXYZ ( float xa, float ya, float za ) { Matrix4x4 result; RotationXYZ ( xa, ya, za, out result ); return result; }
		public static void RotationXYZ ( float xa, float ya, float za, out Matrix4x4 result ) { RotationXYZ ( xa, ya, za, out result ); }
		public static Matrix4x4 RotationXYZ ( Vector3 xyza ) { Matrix4x4 result; RotationXYZ ( ref xyza, out result ); return result; }
		public static void RotationXYZ ( ref Vector3 xyza, out Matrix4x4 result )
		{
			Matrix4x4 temp;
			RotationX ( xyza.X, out result );
			RotationY ( xyza.Y, out temp );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			RotationZ ( xyza.Z, out temp );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
		}

		public static Matrix4x4 Scale ( float sx, float sy, float sz ) { Matrix4x4 result; Scale ( sx, sy, sz, out result ); return result; }
		public static void Scale ( float sx, float sy, float sz, out Matrix4x4 result ) { Vector3 v = new Vector3 ( sx, sy, sz ); Scale ( ref v, out result ); }
		public static Matrix4x4 Scale ( Vector3 s ) { Matrix4x4 result; Scale ( ref s, out result ); return result; }
		public static void Scale ( ref Vector3 s, out Matrix4x4 result )
		{
			result = new Matrix4x4
			(
				s.X, 0, 0, 0,
				0, s.Y, 0, 0,
				0, 0, s.Z, 0,
				0, 0, 0, 1
			);
		}

		public static Matrix4x4 Translate ( float x, float y, float z ) { Matrix4x4 result; Scale ( x, y, z, out result ); return result; }
		public static void Translate ( float x, float y, float z, out Matrix4x4 result ) { Vector3 v = new Vector3 ( x, y, z ); Translate ( ref v, out result ); }
		public static Matrix4x4 Translate ( Vector3 p ) { Matrix4x4 result; Translate ( ref p, out result ); return result; }
		public static void Translate ( ref  Vector3 p, out Matrix4x4 result )
		{
			result = new Matrix4x4 (
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				p.X, p.Y, p.Z, 1
			);
		}
	}
}
