using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.Decoders.Packs;

namespace Daramkun.Misty.Contents.Loaders
{
	public class WalnutPackageContentLoader : IContentLoader
	{
		public Type ContentType { get { return typeof ( PackageInfo ); } }
		public IEnumerable<string> FileExtensions { get { yield return "wlnt"; } }
		public bool IsSelfStreamDispose { get { return true; } }

		public object Load ( Stream stream, params object [] args )
		{
			PackageInfo packageInfo;
			if ( new PackageDecoder ().Decode ( stream, out packageInfo ) )
				return packageInfo;
			else throw new ArgumentException ();
		}
	}
}
