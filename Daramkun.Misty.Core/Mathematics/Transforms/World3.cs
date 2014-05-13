using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public class World3 : ITransform
	{
		public Vector3 Translate;
		public Vector3 ScaleCenter;
		public Vector3 Scale;
		public Vector3 Rotation;
		public Vector3 RotationCenter;

		public static World3 Identity
		{
			get { return new World3 () { Scale = new Vector3 ( 1, 1, 1 ) }; }
		}

		public World3 ()
		{
			Scale = new Vector3 ( 1 );
		}

		public World3 ( Vector3 translate )
			: this ( translate, new Vector3 (), new Vector3 ( 1 ), new Vector3 (), new Vector3 () )
		{

		}

		public World3 ( Vector3 translate, Vector3 scaleCenter, Vector3 scale, Vector3 rotation, Vector3 rotationCenter )
			: this ()
		{
			Translate = translate;
			ScaleCenter = scaleCenter;
			Scale = scale;
			Rotation = rotation;
			RotationCenter = rotationCenter;
		}

		public static World3 operator + ( World3 v1, World3 v2 )
		{
			return new World3 ( v1.Translate + v2.Translate, v1.ScaleCenter + v2.ScaleCenter,
				v1.Scale * v2.Scale, v1.Rotation + v2.Rotation, v1.RotationCenter + v2.RotationCenter );
		}

		public static World3 operator - ( World3 v1, World3 v2 )
		{
			return new World3 ( v1.Translate - v2.Translate, v1.ScaleCenter - v2.ScaleCenter,
				v1.Scale / v2.Scale, v1.Rotation - v2.Rotation, v1.RotationCenter - v2.RotationCenter );
		}

		public Matrix4x4 Matrix { get { Matrix4x4 result; GetMatrix ( out result ); return result; } }
		public void GetMatrix ( out Matrix4x4 result )
		{
			result = Matrix4x4.Identity;
			Matrix4x4 temp = CommonTransform.Translate ( -RotationCenter );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.RotationXYZ ( Rotation );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Translate ( RotationCenter );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Translate ( -ScaleCenter );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Scale ( Scale );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Translate ( ScaleCenter );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Translate ( Translate );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
		}
	}
}
