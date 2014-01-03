using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Contents
{
	public struct ModelInfo
	{

		public Stream ModelStream { get; private set; }

		public ModelInfo ( Stream modelStream, object rawModel )
			: this ()
		{
			
		}
	}
}
