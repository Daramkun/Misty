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
	}

	public interface IProjectionTransform : ITransform
	{
		float Near { get; set; }
		float Far { get; set; }

		HandDirection HandDirection { get; set; }
	}
}
