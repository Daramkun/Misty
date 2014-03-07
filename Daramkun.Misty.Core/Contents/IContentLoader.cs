using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.Tables;

namespace Daramkun.Misty.Contents
{
	public interface IContentLoader
	{
		Type ContentType { get; }
		IEnumerable<string> FileExtensions { get; }
		bool IsSelfStreamDispose { get; }
		object Load ( Stream stream, ResourceTable resourceTable, params object [] args );
	}
}
