using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	public interface ITexture3D : ITexture
	{
		int Width { get; }
		int Height { get; }
		int Depth { get; }
		Vector3 Size { get; }
	}
}
