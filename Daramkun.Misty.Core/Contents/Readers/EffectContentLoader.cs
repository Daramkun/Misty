using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Readers
{
	public class EffectContentLoader : IContentReader
	{
		public Type ContentType { get { return typeof ( IEffect ); } }

		public IEnumerable<string> FileExtensions { get { yield return "sxml"; yield return "lfx"; yield return "xml"; } }

		public bool AutoStreamDispose { get { return false; } }

		public object Read ( Stream stream, ResourceTable resourceTable, params object [] args )
		{
			return Core.GraphicsDevice.CreateEffect ( stream );
		}
	}
}
