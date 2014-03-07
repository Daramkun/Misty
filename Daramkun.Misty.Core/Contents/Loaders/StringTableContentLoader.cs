using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.Tables;

namespace Daramkun.Misty.Contents.Loaders
{
	public class StringTableContentLoader : IContentLoader
	{
		public Type ContentType { get { return typeof ( StringTable ); } }
		public IEnumerable<string> FileExtensions { get { yield return "jst"; yield return "json"; yield return "bson"; } }
		public bool IsSelfStreamDispose { get { return true; } }

		public object Load ( Stream stream, ResourceTable resourceTable, params object [] args )
		{
			return new StringTable ( stream );
		}
	}
}
