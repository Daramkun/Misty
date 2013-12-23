using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class VertexDeclaration : StandardDispose, IVertexDeclaration
	{


		public object Handle { get { throw new NotImplementedException (); } }
		public int Length { get { throw new NotImplementedException (); } }

		public VertexDeclaration ( IGraphicsDevice graphicsDevice, VertexElement [] elements )
		{

		}

		public System.Collections.IEnumerator GetEnumerator ()
		{
			throw new NotImplementedException ();
		}
	}
}
