using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Contents.Decoders
{
	public interface IDecoder<T>
	{
		bool Decode ( Stream stream, out T to, params object [] args );
	}
}
