using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public interface ITexture1D : ITexture
	{
		int Width { get; }

		float Size { get; }
	}
}
