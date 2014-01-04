using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics
{
	public partial struct Vector2
	{
		public static Vector2 Transform ( Vector2 position, Matrix4x4 matrix )
		{
			Vector2 result;
			Transform ( ref position, ref matrix, out result );
			return result;
		}
		public static void Transform ( ref Vector2 position, ref Matrix4x4 matrix, out Vector2 result )
		{
			result = new Vector2 (
				( position.X * matrix.M11 ) + ( position.Y * matrix.M21 ) + matrix.M41,
				( position.X * matrix.M12 ) + ( position.Y * matrix.M22 ) + matrix.M42
			);
		}

		public static Vector2 TransformNormal ( Vector2 normal, Matrix4x4 matrix )
		{
			Vector2 result;
			TransformNormal ( ref normal, ref matrix, out result );
			return result;
		}
		public static void TransformNormal ( ref Vector2 normal, ref Matrix4x4 matrix, out Vector2 result )
		{
			result = new Vector2 (
				( normal.X * matrix.M11 ) + ( normal.Y * matrix.M21 ),
				( normal.X * matrix.M12 ) + ( normal.Y * matrix.M22 )
			);
		}
	}
}
