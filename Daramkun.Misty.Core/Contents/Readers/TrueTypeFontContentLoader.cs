﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics.Spirit.Fonts;

namespace Daramkun.Misty.Contents.Readers
{
	public class TrueTypeFontContentLoader : IContentReader
	{
		public Type ContentType { get { return typeof ( TrueTypeFont ); } }

		public IEnumerable<string> FileExtensions { get { yield return "ttf"; } }

		public bool AutoStreamDispose { get { return false; } }

		public object Read ( Stream stream, ResourceTable resourceTable, params object [] args )
		{
			return new TrueTypeFont ( stream, ( int ) args [ 0 ] );
		}
	}
}
