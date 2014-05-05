using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.Tables;

namespace Daramkun.Misty.Contents
{
	public interface IContentReader
	{
		Type ContentType { get; }
		IEnumerable<string> FileExtensions { get; }
		bool AutoStreamDispose { get; }
		object Read ( Stream stream, ResourceTable resourceTable, params object [] args );
	}
}
