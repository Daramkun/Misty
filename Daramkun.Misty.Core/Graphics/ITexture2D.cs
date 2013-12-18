using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	public enum TextureFilter
	{
		Nearest,
		Linear,
		Anisotropic,
	}

	public enum TextureAddressing
	{
		Wrap,
		Mirror,
		Clamp,
	}

	public struct TextureArgument
	{
		public string Uniform { get; set; }
		public ITexture2D Texture { get; set; }
		public TextureFilter Filter { get; set; }
		public TextureAddressing Addressing { get; set; }
		public int AnisotropicLevel { get; set; }

		public TextureArgument ( string u, ITexture2D t, TextureFilter f, TextureAddressing a, int al )
			: this ()
		{
			Uniform = u; Texture = t; Filter = f; Addressing = a; AnisotropicLevel = al;
		}
	}

	public interface ITexture2D : IDisposable
	{
		int Width { get; }
		int Height { get; }
		Vector2 Size { get; }
		
		Color [] Buffer { get; set; }

		object Handle { get; }
	}
}
