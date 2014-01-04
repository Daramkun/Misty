using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector4
	{
		public static Vector4 Transform ( Vector2 position, Matrix4x4 matrix )
		{
			Vector4 result;
			Transform ( ref position, ref matrix, out result );
			return result;
		}
		public static Vector4 Transform ( Vector3 position, Matrix4x4 matrix )
		{
			Vector4 result;
			Transform ( ref position, ref matrix, out result );
			return result;
		}
		public static Vector4 Transform ( Vector4 position, Matrix4x4 matrix )
		{
			Vector4 result;
			Transform ( ref position, ref matrix, out result );
			return result;
		}

		public static void Transform ( ref Vector2 position, ref Matrix4x4 matrix, out Vector4 result )
		{
			result = new Vector4 (
				( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) + matrix.M41,
				( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) + matrix.M42,
				( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) + matrix.M43,
				( position.X * matrix.M14 ) + ( position.Y * matrix.M24 ) + matrix.M44
			);
		}
		public static void Transform ( ref Vector3 position, ref Matrix4x4 matrix, out Vector4 result )
		{
			result = new Vector4 (
				( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) + ( position.Z * matrix.M31 ) + matrix.M41,
				( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) + ( position.Z * matrix.M32 ) + matrix.M42,
				( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) + ( position.Z * matrix.M33 ) + matrix.M43,
				( position.X * matrix.M14 ) + ( position.Y * matrix.M24 ) + ( position.Z * matrix.M34 ) + matrix.M44
			);
		}
		public static void Transform ( ref Vector4 position, ref Matrix4x4 matrix, out Vector4 result )
		{
			result = new Vector4 (
				( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) + ( position.Z * matrix.M31 ) + ( position.W * matrix.M41 ),
				( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) + ( position.Z * matrix.M32 ) + ( position.W * matrix.M42 ),
				( position.X * matrix.M13 ) + ( position.Y * matrix.M23 ) + ( position.Z * matrix.M33 ) + ( position.W * matrix.M43 ),
				( position.X * matrix.M14 ) + ( position.Y * matrix.M24 ) + ( position.Z * matrix.M34 ) + ( position.W * matrix.M44 )
			);
		}
	}
}
