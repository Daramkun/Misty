using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public class LookAt : ITransform
	{
		public Vector3 Position;
		public Vector3 Target;
		public Vector3 UpVector;

		public LookAt ( Vector3 position, Vector3 target, Vector3 upVector )
		{
			Position = position;
			Target = target;
			UpVector = upVector;
		}

		public LookAt ( Vector3 position )
			: this ( position, new Vector3 (), new Vector3 ( 0, 1, 0 ) )
		{ }

		public LookAt ( Vector3 position, Vector3 target )
			: this ( position, target, new Vector3 ( 0, 1, 0 ) )
		{ }

		public Matrix4x4 Matrix { get { Matrix4x4 result; GetMatrix ( out result ); return result; } }
		public void GetMatrix ( out Matrix4x4 result )
		{
			if ( CommonTransform.HandDirection == HandDirection.RightHand ) CommonTransform.LookAtRH ( ref Position, ref Target, ref UpVector, out result );
			else CommonTransform.LookAtLH ( ref Position, ref Target, ref UpVector, out result );
		}
	}
}
