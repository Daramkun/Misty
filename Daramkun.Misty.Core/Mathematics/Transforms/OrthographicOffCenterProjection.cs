using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public class OrthographicOffCenterProjection : IProjectionTransform
	{
		public float Near { get; set; }
		public float Far { get; set; }
		public Vector2 OffCenterPosition { get; set; }
		public Vector2 OffCenterSize { get; set; }
		public HandDirection HandDirection { get; set; }

		public OrthographicOffCenterProjection ( float width, float height, float near = 0.0001f, float far = 10000.0f )
		{
			OffCenterPosition = new Vector2 ();
			OffCenterSize = new Vector2 ( width, height );
			Near = near;
			Far = far;
		}

		public OrthographicOffCenterProjection ( float left, float right, float bottom, float top, float near, float far )
		{
			OffCenterPosition = new Vector2 ( left, top );
			OffCenterSize = new Vector2 ( right - left, bottom - top );
			Near = near;
			Far = far;
		}

		public Matrix4x4 Matrix
		{
			get
			{
				float left = OffCenterPosition.X, right = OffCenterPosition.X + OffCenterSize.X,
					top = OffCenterPosition.Y, bottom = OffCenterPosition.Y + OffCenterSize.Y;

				Func<float, float, float, float, float, float, Matrix4x4> orthoOffCenter;

				if ( HandDirection == HandDirection.RightHand ) orthoOffCenter = CommonTransform.OrthographicOffCenterRH;
				else orthoOffCenter = CommonTransform.OrthographicOffCenterLH;

				return orthoOffCenter ( left, right, bottom, top, Near, Far );
			}
		}
	}
}
