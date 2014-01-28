using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public static partial class CommonTransform
	{
		/*
		public static Matrix4x4 LookAtLH ( Vector3 eye, Vector3 at, Vector3 up )
		{
			Matrix4x4 result;
			LookAtLH ( ref eye, ref at, ref up, out result );
			return result;
		}

		public static void LookAtLH ( ref Vector3 eye, ref Vector3 at, ref Vector3 up, out Matrix4x4 result )
		{
			Vector3 zaxis = ( at - eye ).Normalize ();
			Vector3 xaxis = Vector3.Cross ( up, zaxis ).Normalize ();
			Vector3 yaxis = Vector3.Cross ( zaxis, xaxis );
			result = new Matrix4x4
			(
				xaxis.X, yaxis.X, zaxis.X, 0,
				xaxis.Y, yaxis.Y, zaxis.Y, 0,
				xaxis.Z, yaxis.Z, zaxis.Z, 0,
				-Vector3.Dot ( xaxis, eye ), -Vector3.Dot ( yaxis, eye ), -Vector3.Dot ( zaxis, eye ), 1
			);
		}
		*/
		public static Matrix4x4 LookAtRH ( Vector3 eye, Vector3 at, Vector3 up )
		{
			Matrix4x4 result;
			LookAtRH ( ref eye, ref at, ref up, out result );
			return result;
		}

		public static void LookAtRH ( ref Vector3 eye, ref Vector3 at, ref Vector3 up, out Matrix4x4 result )
		{
			Vector3 zaxis = ( eye - at ).Normalize ();
			Vector3 xaxis = Vector3.Cross ( up, zaxis ).Normalize ();
			Vector3 yaxis = Vector3.Cross ( zaxis, xaxis );
			result = new Matrix4x4
			(
				xaxis.X, yaxis.X, zaxis.X, 0,
				xaxis.Y, yaxis.Y, zaxis.Y, 0,
				xaxis.Z, yaxis.Z, zaxis.Z, 0,
				-Vector3.Dot ( xaxis, eye ), -Vector3.Dot ( yaxis, eye ), -Vector3.Dot ( zaxis, eye ), 1
			);
		}
	}
}
