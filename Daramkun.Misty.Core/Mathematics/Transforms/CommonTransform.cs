using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public static partial class CommonTransform
	{
		public static Matrix4x4 FromAxisAngle ( float x, float y, float z, float angle )
		{
			float num2 = ( float ) Math.Sin ( angle );
			float num = ( float ) Math.Cos ( angle );
			float num11 = x * x;
			float num10 = y * y;
			float num9 = z * z;
			float num8 = x * y;
			float num7 = x * z;
			float num6 = y * z;

			return new Matrix4x4
			(
				num11 + ( num * ( 1 - num11 ) ), ( num8 - ( num * num8 ) ) + ( num2 * z ), ( num7 - ( num * num7 ) ) - ( num2 * y ), 0,
				( num8 - ( num * num8 ) ) - ( num2 * z ), num10 + ( num * ( 1f - num10 ) ), ( num6 - ( num * num6 ) ) + ( num2 * x ), 0,
				( num7 - ( num * num7 ) ) + ( num2 * y ), ( num6 - ( num * num6 ) ) - ( num2 * x ), num9 + ( num * ( 1f - num9 ) ), 0,
				0, 0, 0, 1
			);
		}

		public static Matrix4x4 FromAxisAngle ( Vector3 n, float angle ) { return FromAxisAngle ( n.X, n.Y, n.Z, angle ); }

		public static Matrix4x4 FromYawPitchRoll ( float yaw, float pitch, float roll )
		{
			return new Quaternion ( yaw, pitch, roll ).ToMatrix4x4 ();
		}

		public static Matrix4x4 FromYawPitchRoll ( Vector3 ypr ) { return FromYawPitchRoll ( ypr.X, ypr.Y, ypr.Z ); }
	}
}
