using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Graphics.Spirit.Fonts;

namespace Daramkun.Misty.Contents.Loaders
{
	public class LsfFontContentLoader : IContentLoader
	{
		public Type ContentType { get { return typeof ( LsfFont ); } }

		public IEnumerable<string> FileExtensions { get { yield return "lsf"; } }

		public bool IsSelfStreamDispose { get { return false; } }

		public object Load ( Stream stream, params object [] args )
		{
			return new LsfFont ( stream );
		}
	}
}
