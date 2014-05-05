using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.Decoders.Packs;
using Daramkun.Misty.Contents.Tables;

namespace Daramkun.Misty.Contents.Readers
{
	public class WalnutPackageContentLoader : IContentReader
	{
		public Type ContentType { get { return typeof ( PackageInfo ); } }
		public IEnumerable<string> FileExtensions { get { yield return "wlnt"; } }
		public bool AutoStreamDispose { get { return true; } }

		public object Read ( Stream stream, ResourceTable resourceTable, params object [] args )
		{
			PackageInfo packageInfo;
			if ( new PackageDecoder ().Decode ( stream, out packageInfo ) )
				return packageInfo;
			else throw new ArgumentException ();
		}
	}
}
