using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.Tables;

namespace Daramkun.Misty.Contents
{
	public interface IContentWriter
	{
		Type ContentType { get; }
		IEnumerable<string> FileExtensions { get; }
		bool Write ( Stream stream, ResourceTable resourceTable, object data, params object [] args );
	}
}
