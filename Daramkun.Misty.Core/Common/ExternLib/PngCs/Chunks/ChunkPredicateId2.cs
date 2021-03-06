﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hjg.Pngcs.Chunks
{
	internal class ChunkPredicateId2 : ChunkPredicate
	{
		private readonly string id;
		private readonly string innerid;

		public ChunkPredicateId2 ( string id, string inner )
		{
			this.id = id;
			this.innerid = inner;
		}

		public bool Matches ( PngChunk c )
		{
			if ( !c.Id.Equals ( id ) )
				return false;
			if ( c is PngChunkTextVar && !( ( PngChunkTextVar ) c ).GetKey ().Equals ( innerid ) )
				return false;
			if ( c is PngChunkSPLT && !( ( PngChunkSPLT ) c ).PalName.Equals ( innerid ) )
				return false;

			return true;
		}
	}
}
