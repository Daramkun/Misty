using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents.Loaders
{
	public class EffectContentLoader : IContentLoader
	{
		public Type ContentType { get { return typeof ( IEffect ); } }

		public IEnumerable<string> FileExtensions { get { yield return "sxml"; yield return "lfx"; yield return "xml"; } }

		public bool IsSelfStreamDispose { get { return false; } }

		public object Load ( Stream stream, params object [] args )
		{
			string [] args2 = new string [ args.Length ];
			for ( int i = 0; i < args.Length; ++i ) args2 [ i ] = args [ i ] as string;
			return Core.GraphicsDevice.CreateEffect ( stream, args2 );
		}
	}
}
