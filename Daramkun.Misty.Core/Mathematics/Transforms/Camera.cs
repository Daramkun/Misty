using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public class Camera : ITransform
	{
		float cameraPitch, cameraYaw, cameraRoll;
		Vector3 cameraPos = Vector3.Zero, cameraTarget, cameraUpVector;
		Vector3 move = Vector3.Zero;

		public Vector3 Position;
		public bool IsFlyingMode;

		public Matrix4x4 Matrix { get { Matrix4x4 result; GetMatrix ( out result ); return result; } }
		public void GetMatrix ( out Matrix4x4 result )
		{
			Matrix4x4 t; Vector3 t2;
			CommonTransform.FromYawPitchRoll ( cameraYaw, ( IsFlyingMode ) ? cameraPitch : 0, 0, out t );
			Vector3.Transform ( ref move, ref t, out t2 );
			cameraPos += t2;
			CommonTransform.FromYawPitchRoll ( cameraYaw, cameraPitch, cameraRoll, out t );

			Vector3 targetVector = new Vector3 ( 0, 0, -1 ), upVector = new Vector3 ( 0, 1, 0 );
			Vector3.Transform ( ref targetVector, ref t, out cameraTarget );
			Vector3.Transform ( ref upVector, ref t, out cameraUpVector );

			t2 = cameraTarget + cameraPos;
				CommonTransform.LookAtRH ( ref cameraPos, ref t2, ref cameraUpVector, out result );
			move = Vector3.Zero;
		}

		public void Strafe ( float u )
		{
			move.X += u;
		}

		public void Fly ( float u )
		{
			move.Y += u;
		}

		public void Walk ( float u )
		{
			move.Z -= u;
		}

		public float Yaw
		{
			get { return cameraYaw; }
			set
			{
				cameraYaw = value;
				if ( cameraYaw > 3.141592f * 2 ) cameraYaw -= 3.141592f * 2;
				if ( cameraYaw < -3.141592f * 2 ) cameraYaw += 3.141592f * 2;
			}
		}

		public float Pitch
		{
			get { return cameraPitch; }
			set
			{
				cameraPitch = value;
				if ( cameraPitch > 3.141592f / 2 ) cameraPitch = 3.141592f / 2;
				if ( cameraPitch < -3.141592f / 2 ) cameraPitch = -3.141592f / 2;
			}
		}

		public float Roll
		{
			get { return cameraRoll; }
			set { cameraRoll = value; }
		}
	}
}
