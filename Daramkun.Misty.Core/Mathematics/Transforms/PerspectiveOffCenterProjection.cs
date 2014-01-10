using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public class PerspectiveOffCenterProjection : ITransform
	{
		public Vector2 OffCenterPosition;
		public Vector2 OffCenterSize;
		public float Near;
		public float Far;

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

		public Matrix4x4 Matrix { get { Matrix4x4 result; GetMatrix ( out result ); return result; } }
		public void GetMatrix ( out Matrix4x4 result )
		{
			if ( CommonTransform.HandDirection == HandDirection.RightHand ) CommonTransform.PerspectiveOffCenterRH ( OffCenterPosition.X, OffCenterPosition.X + OffCenterSize.X,
				OffCenterPosition.Y + OffCenterSize.Y, OffCenterPosition.Y, Near, Far, out result );
			else CommonTransform.PerspectiveOffCenterLH ( OffCenterPosition.X, OffCenterPosition.X + OffCenterSize.X, OffCenterPosition.Y + OffCenterSize.Y,
				OffCenterPosition.Y, Near, Far, out result );
		}
	}
}
