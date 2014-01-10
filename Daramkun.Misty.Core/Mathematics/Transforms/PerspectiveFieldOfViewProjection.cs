using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public class PerspectiveFieldOfViewProjection : ITransform
	{
		public float FieldOfView { get; set; }
		public float AspectRatio { get; set; }
		public float Near { get; set; }
		public float Far { get; set; }

		public PerspectiveFieldOfViewProjection ( float fieldOfView, float aspectRatio = 3.141592f / 4, float near = 0.0001f, float far = 10000.0f )
		{
			FieldOfView = fieldOfView;
			AspectRatio = aspectRatio;
			Near = near;
			Far = far;
		}

		public Matrix4x4 Matrix { get { Matrix4x4 result; GetMatrix ( out result ); return result; } }
		public void GetMatrix ( out Matrix4x4 result )
		{
			if ( CommonTransform.HandDirection == HandDirection.RightHand ) CommonTransform.PerspectiveFieldOfViewRH ( FieldOfView, AspectRatio, Near, Far, out result );
			else CommonTransform.PerspectiveFieldOfViewLH ( FieldOfView, AspectRatio, Near, Far, out result );
		}
	}
}
