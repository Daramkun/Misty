using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public static partial class CommonTransform
	{
		#region Orthographic Projection
		public static Matrix4x4 OrthographicRH ( float w, float h, float zn, float zf )
		{
			Matrix4x4 result;
			OrthographicRH ( w, h, zn, zf, out result );
			return result;
		}

		public static void OrthographicRH ( float w, float h, float zn, float zf, out Matrix4x4 result )
		{
			result = new Matrix4x4
			(
				2 / w, 0, 0, 0,
				0, 2 / h, 0, 0,
				0, 0, 1 / ( zn - zf ), 0,
				0, 0, zn / ( zn - zf ), 0
			);
		}
		
		public static Matrix4x4 OrthographicOffCenterLH ( float l, float r, float b, float t, float zn, float zf )
		{
			Matrix4x4 result;
			OrthographicOffCenterLH ( l, r, b, t, zn, zf, out result );
			return result;
		}

		public static void OrthographicOffCenterLH ( float l, float r, float b, float t, float zn, float zf, out Matrix4x4 result )
		{
			result = new Matrix4x4
			(
				2 / ( r - l ), 0, 0, 0,
				0, 2 / ( t - b ), 0, 0,
				0, 0, 1 / ( zf - zn ), 0,
				( l + r ) / ( l - r ), ( t + b ) / ( b - t ), -zn / ( zf - zn ), 1
			);
		}
		
		public static Matrix4x4 OrthographicOffCenterRH ( float l, float r, float b, float t, float zn, float zf )
		{
			Matrix4x4 result;
			OrthographicOffCenterRH ( l, r, b, t, zn, zf, out result );
			return result;
		}

		public static void OrthographicOffCenterRH ( float l, float r, float b, float t, float zn, float zf, out Matrix4x4 result )
		{
			result = new Matrix4x4
			(
				2 / ( r - l ), 0, 0, 0,
				0, 2 / ( t - b ), 0, 0,
				0, 0, 1 / ( zn - zf ), 0,
				( l + r ) / ( l - r ), ( t + b ) / ( b - t ), zn / ( zf - zn ), 1
			);
		}
		#endregion

		#region Perspective Projection
		public static Matrix4x4 PerspectiveFieldOfViewRH ( float fov, float aspect, float zn, float zf )
		{
			Matrix4x4 result;
			PerspectiveFieldOfViewRH ( fov, aspect, zn, zf, out result );
			return result;
		}

		public static void PerspectiveFieldOfViewRH ( float fov, float aspect, float zn, float zf, out Matrix4x4 result )
		{
			float yScale = ( float ) ( Math.Cos ( fov / 2 ) / Math.Sin ( fov / 2 ) ), xScale = yScale / aspect;
			result = new Matrix4x4
			(
				xScale, 0, 0, 0,
				0, yScale, 0, 0,
				0, 0, zf / ( zn - zf ), -1,
				0, 0, zn * zf / ( zn - zf ), 0
			);
		}
		public static Matrix4x4 PerspectiveRH ( float w, float h, float zn, float zf )
		{
			Matrix4x4 result;
			PerspectiveRH ( w, h, zn, zf, out result );
			return result;
		}

		public static void PerspectiveRH ( float w, float h, float zn, float zf, out Matrix4x4 result )
		{
			result = new Matrix4x4
			(
				2 * zn / w, 0, 0, 0,
				0, 2 * zn / h, 0, 0,
				0, 0, zf / ( zn - zf ), 1,
				0, 0, zn * zf / ( zn - zf ), 0
			);
		}
		public static Matrix4x4 PerspectiveOffCenterRH ( float l, float r, float b, float t, float zn, float zf )
		{
			Matrix4x4 result;
			PerspectiveOffCenterRH ( l, r, b, t, zn, zf, out result );
			return result;
		}

		public static void PerspectiveOffCenterRH ( float l, float r, float b, float t, float zn, float zf, out Matrix4x4 result )
		{
			result = new Matrix4x4
			(
				2 * zn / ( r - l ), 0, 0, 0,
				0, 2 * zn / ( t - b ), 0, 0,
				( l + r ) / ( l - r ), ( t + b ) / ( t - b ), zf / ( zn - zf ), 1,
				0, 0, zn * zf / ( zn - zf ), 0
			);
		}
		#endregion
	}
}
