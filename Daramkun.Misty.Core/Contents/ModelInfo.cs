using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Contents
{
	public sealed class ModelInfo
	{
		public Stream ModelStream { get; private set; }

		object rawModel;
	}
}
