using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.FileSystems;

namespace Daramkun.Misty.Contents.Loaders
{
	public class ZipContentLoader : IContentLoader
	{
		public Type ContentType { get { return typeof ( ZipFileSystem ); } }
		public IEnumerable<string> FileExtensions { get { yield return "zip"; } }
		public bool IsSelfStreamDispose { get { return false; } }

		public object Load ( Stream stream, params object [] args )
		{
			return new ZipFileSystem ( stream );
		}
	}
}
