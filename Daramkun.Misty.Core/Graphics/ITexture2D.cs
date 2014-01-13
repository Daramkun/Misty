﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	public interface ITexture2D : ITexture
	{
		int Width { get; }
		int Height { get; }
		Vector2 Size { get; }
	}
}
