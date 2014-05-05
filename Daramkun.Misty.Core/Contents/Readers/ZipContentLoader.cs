using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Tables;

namespace Daramkun.Misty.Contents.Readers
{
	public class ZipContentLoader : IContentReader
	{
		public Type ContentType { get { return typeof ( ZipFileSystem ); } }
		public IEnumerable<string> FileExtensions { get { yield return "zip"; } }
		public bool AutoStreamDispose { get { return false; } }

		public object Read ( Stream stream, ResourceTable resourceTable, params object [] args )
		{
			return new ZipFileSystem ( stream );
		}
	}
}
