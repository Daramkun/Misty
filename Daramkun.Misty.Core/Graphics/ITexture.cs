using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	public static class TextureExtensions
	{
		public static float Size ( this ITexture1D me ) { return me.Width; }
		public static Vector2 Size ( this ITexture2D me ) { return new Vector2 ( me.Width, me.Height ); }
		public static Vector3 Size ( this ITexture3D me ) { return new Vector3 ( me.Width, me.Height, me.Depth ); }
	}

	public interface ITexture : IDisposable
	{
		Color [] Buffer { get; set; }
		object Handle { get; }
	}

	public interface ITexture1D : ITexture
	{
		int Width { get; }
	}

	public interface ITexture2D : ITexture
	{
		int Width { get; }
		int Height { get; }
	}

	public interface ITexture3D : ITexture
	{
		int Width { get; }
		int Height { get; }
		int Depth { get; }
	}

	public interface IRenderBuffer : ITexture2D
	{

	}
}
