using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public interface ITexture : IDisposable
	{
		Color [] Buffer { get; set; }
		object Handle { get; }
	}
}
