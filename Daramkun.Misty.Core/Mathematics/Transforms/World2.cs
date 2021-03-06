﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public class World2 : ITransform
	{
		public Vector2 Translate;
		public Vector2 ScaleCenter;
		public Vector2 Scale;
		public float Rotation;
		public Vector2 RotationCenter;

		public static World2 Identity
		{
			get { return new World2 () { Scale = new Vector2 ( 1, 1 ) }; }
		}

		public World2 ()
		{
			Scale = new Vector2 ( 1 );
		}

		public World2 ( Vector2 translate, Vector2 scale, Vector2 scaleCenter, float rotation, Vector2 rotationCenter )
			: this ()
		{
			Translate = translate;
			ScaleCenter = scaleCenter;
			Scale = scale;
			Rotation = rotation;
			RotationCenter = rotationCenter;
		}

		public static World2 operator + ( World2 v1, World2 v2 )
		{
			return new World2 ( v1.Translate + v2.Translate, v1.ScaleCenter + v2.ScaleCenter,
				v1.Scale * v2.Scale, v1.Rotation + v2.Rotation, v1.RotationCenter + v2.RotationCenter );
		}

		public static World2 operator - ( World2 v1, World2 v2 )
		{
			return new World2 ( v1.Translate - v2.Translate, v1.ScaleCenter - v2.ScaleCenter,
				v1.Scale / v2.Scale, v1.Rotation - v2.Rotation, v1.RotationCenter - v2.RotationCenter );
		}

		public Matrix4x4 Matrix { get { Matrix4x4 result; GetMatrix ( out result ); return result; } }
		public void GetMatrix ( out Matrix4x4 result )
		{
			result = Matrix4x4.Identity;
			Matrix4x4 temp = CommonTransform.Translate ( new Vector3 ( -RotationCenter, 0 ) );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.RotationZ ( Rotation );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Translate ( new Vector3 ( RotationCenter, 0 ) );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Translate ( new Vector3 ( -ScaleCenter, 0 ) );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Scale ( new Vector3 ( Scale, 1 ) );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Translate ( new Vector3 ( ScaleCenter, 0 ) );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
			temp = CommonTransform.Translate ( new Vector3 ( Translate, 0 ) );
			Matrix4x4.Multiply ( ref result, ref temp, out result );
		}
	}
}
