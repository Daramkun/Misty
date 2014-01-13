using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

	public class TextureArgument
	{
		public string Uniform { get; set; }
		public ITexture Texture { get; set; }
		public TextureFilter Filter { get; set; }
		public TextureAddressing Addressing { get; set; }
		public int AnisotropicLevel { get; set; }

		public TextureArgument ( string u, ITexture t, TextureFilter f, TextureAddressing a, int al )
		{
			Uniform = u; Texture = t; Filter = f; Addressing = a; AnisotropicLevel = al;
		}
	}
}
