using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Mathematics.Transforms
{
	public enum HandDirection
	{
		LeftHand,
		RightHand,
	}

	public interface ITransform
	{
		Matrix4x4 Matrix { get; }
		void GetMatrix ( out Matrix4x4 result );
	}

	public interface IHandDirectionTransform : ITransform
	{
		HandDirection HandDirection { get; set; }
	}
}
