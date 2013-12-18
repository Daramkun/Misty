using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Contents.Encoders
{
	public interface IEncoder<T>
	{
		bool Encode ( Stream stream, T data, params object [] args );
	}
}
