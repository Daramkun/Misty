using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	public interface ITexture : IDisposable
	{
		Color [] Buffer { get; set; }
		object Handle { get; }
	}

	public interface ITexture<T> : ITexture where T : struct
	{
		T Size { get; }
	}

	public interface ITexture1D : ITexture<float>
	{
		int Width { get; }
	}

	public interface ITexture2D : ITexture<Vector2>
	{
		int Width { get; }
		int Height { get; }
	}

	public interface ITexture3D : ITexture<Vector3>
	{
		int Width { get; }
		int Height { get; }
		int Depth { get; }
	}
}
