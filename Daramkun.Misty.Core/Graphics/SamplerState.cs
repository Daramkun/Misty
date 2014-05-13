using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public enum TextureFilter : byte
	{
		Nearest,
		Linear,
		Anisotropic,
	}

	public enum TextureAddressing : byte
	{
		Wrap,
		Mirror,
		Clamp,
	}

	public class SamplerState
	{
		public ITexture Texture { get; set; }
		public TextureFilter Filter { get; set; }
		public TextureAddressing Addressing { get; set; }
		public int AnisotropicLevel { get; set; }

		public SamplerState ( ITexture t, TextureFilter f, TextureAddressing a, int al )
		{
			Texture = t; Filter = f; Addressing = a; AnisotropicLevel = al;
		}
	}
}
