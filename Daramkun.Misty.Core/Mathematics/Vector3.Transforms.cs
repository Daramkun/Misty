using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector3
	{
		public static Vector3 Transform ( Vector2 position, Matrix4x4 matrix )
		{
			Vector3 result;
			Transform ( ref position, ref matrix, out result );
			return result;
		}
		public static Vector3 Transform ( Vector3 position, Matrix4x4 matrix )
		{
			Vector3 result;
			Transform ( ref position, ref matrix, out result );
			return result;
		}
		public static Vector3 Transform ( Vector3 value, Quaternion rotation )
		{
			Vector3 result;
			Transform ( ref value, ref rotation, out result );
			return result;
		}

		public static void Transform ( ref Vector2 position, ref Matrix4x4 matrix, out Vector3 result )
		{
			result = new Vector3 (
				( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) + matrix.M41,
				( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) + matrix.M42,
				( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) + matrix.M43
			);
		}
		public static void Transform ( ref Vector3 position, ref Matrix4x4 matrix, out Vector3 result )
		{
			result = new Vector3 (
				( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) + ( position.Z * matrix.M31 ) + matrix.M41,
				( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) + ( position.Z * matrix.M32 ) + matrix.M42,
				( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) + ( position.Z * matrix.M33 ) + matrix.M43
			);
		}
		public static void Transform ( ref Vector3 value, ref Quaternion rotation, out Vector3 result )
		{
			float x = 2 * ( rotation.Y * value.Z - rotation.Z * value.Y );
			float y = 2 * ( rotation.Z * value.X - rotation.X * value.Z );
			float z = 2 * ( rotation.X * value.Y - rotation.Y * value.X );
			result.X = value.X + x * rotation.W + ( rotation.Y * z - rotation.Z * y );
			result.Y = value.Y + y * rotation.W + ( rotation.Z * x - rotation.X * z );
			result.Z = value.Z + z * rotation.W + ( rotation.X * y - rotation.Y * x );
		}

		public static Vector3 TransformNormal ( Vector3 normal, Matrix4x4 matrix )
		{ Vector3 result; TransformNormal ( ref normal, ref matrix, out result ); return result; }
		public static void TransformNormal ( ref Vector3 normal, ref Matrix4x4 matrix, out Vector3 result )
		{
			result = new Vector3 (
				( normal.X * matrix.M11 ) + ( normal.Y * matrix.M21 ) + ( normal.Z * matrix.M31 ),
				( normal.X * matrix.M12 ) + ( normal.Y * matrix.M22 ) + ( normal.Z * matrix.M32 ),
				( normal.X * matrix.M13 ) + ( normal.Y * matrix.M23 ) + ( normal.Z * matrix.M33 )
			);
		}
	}
}
