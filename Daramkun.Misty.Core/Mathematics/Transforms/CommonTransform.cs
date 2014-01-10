using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public static partial class CommonTransform
	{
		public static HandDirection HandDirection { get; set; }

		public static Matrix4x4 FromAxisAngle ( float x, float y, float z, float angle ) { Matrix4x4 result; FromAxisAngle ( x, y, z, angle, out result ); return result; }
		public static void FromAxisAngle ( float x, float y, float z, float angle, out Matrix4x4 result )
		{
			float num2 = ( float ) Math.Sin ( angle );
			float num = ( float ) Math.Cos ( angle );
			float num11 = x * x;
			float num10 = y * y;
			float num9 = z * z;
			float num8 = x * y;
			float num7 = x * z;
			float num6 = y * z;

			result = new Matrix4x4
			(
				num11 + ( num * ( 1 - num11 ) ), ( num8 - ( num * num8 ) ) + ( num2 * z ), ( num7 - ( num * num7 ) ) - ( num2 * y ), 0,
				( num8 - ( num * num8 ) ) - ( num2 * z ), num10 + ( num * ( 1f - num10 ) ), ( num6 - ( num * num6 ) ) + ( num2 * x ), 0,
				( num7 - ( num * num7 ) ) + ( num2 * y ), ( num6 - ( num * num6 ) ) - ( num2 * x ), num9 + ( num * ( 1f - num9 ) ), 0,
				0, 0, 0, 1
			);
		}

		public static Matrix4x4 FromAxisAngle ( Vector3 n, float angle ) { return FromAxisAngle ( n.X, n.Y, n.Z, angle ); }

		public static Matrix4x4 FromYawPitchRoll ( float yaw, float pitch, float roll )
		{ Matrix4x4 result; FromYawPitchRoll ( yaw, pitch, roll, out result ); return result; }
		public static void FromYawPitchRoll ( float yaw, float pitch, float roll, out Matrix4x4 result )
		{
			Vector3 ypr = new Vector3 ( yaw, pitch, roll );
			FromYawPitchRoll ( ref ypr, out result );
		}
		public static Matrix4x4 FromYawPitchRoll ( Vector3 ypr ) { Matrix4x4 result; FromYawPitchRoll ( ref ypr, out result ); return result; }
		public static void FromYawPitchRoll ( ref Vector3 ypr, out Matrix4x4 result ) { result = new Quaternion ( ypr.X, ypr.Y, ypr.Z ).ToMatrix4x4 (); }
	}
}
