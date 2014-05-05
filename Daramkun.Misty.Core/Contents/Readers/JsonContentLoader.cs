using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Blockar.Json;
using Daramkun.Misty.Contents.Tables;

namespace Daramkun.Misty.Contents.Readers
{
	public class JsonContentLoader : IContentReader
	{
		public Type ContentType { get { return typeof ( JsonContainer ); } }
		public bool AutoStreamDispose { get { return false; } }
		public IEnumerable<string> FileExtensions { get { yield return "json"; } }

		public object Read ( Stream stream, ResourceTable resourceTable, params object [] args )
		{
			return new JsonContainer ( stream );
		}
	}
}
