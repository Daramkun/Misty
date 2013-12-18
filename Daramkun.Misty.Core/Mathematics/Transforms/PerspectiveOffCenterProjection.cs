using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public class PerspectiveOffCenterProjection : IProjectionTransform
	{
		public Vector2 OffCenterPosition { get; set; }
		public Vector2 OffCenterSize { get; set; }
		public float Near { get; set; }
		public float Far { get; set; }
		public HandDirection HandDirection { get; set; }

		public PerspectiveOffCenterProjection ( int left, int top, int right, int bottom, float near = 0.0001f, float far = 10000.0f )
		{
			OffCenterPosition = new Vector2 ( left, top );
			OffCenterSize = new Vector2 ( right - left, bottom - top );
			Near = near;
			Far = far;
		}

		public PerspectiveOffCenterProjection ( int width, int height, float near = 0.0001f, float far = 10000.0f )
		{
			OffCenterPosition = new Vector2 ();
			OffCenterSize = new Vector2 ( width, height );
			Near = near;
			Far = far;
		}

		public Matrix4x4 Matrix
		{
			get
			{
				Func<float, float, float, float, float, float, Matrix4x4> perspOffCenter;

				if ( HandDirection == HandDirection.RightHand ) perspOffCenter = CommonTransform.PerspectiveOffCenterRH;
				else perspOffCenter = CommonTransform.PerspectiveOffCenterLH;

				return perspOffCenter ( OffCenterPosition.X,
					OffCenterPosition.X + OffCenterSize.X, OffCenterPosition.Y + OffCenterSize.Y, OffCenterPosition.Y, Near, Far );
			}
		}
	}
}
