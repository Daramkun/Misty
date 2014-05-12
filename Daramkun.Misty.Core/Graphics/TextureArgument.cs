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
		public ITexture Texture { get; set; }
		public TextureFilter Filter { get; set; }
		public TextureAddressing Addressing { get; set; }
		public int AnisotropicLevel { get; set; }

		public TextureArgument ( ITexture t, TextureFilter f, TextureAddressing a, int al )
		{
			Texture = t; Filter = f; Addressing = a; AnisotropicLevel = al;
		}
	}
}
