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
		List<VertexElement> elements = new List<VertexElement> ();

		public int Length { get; private set; }
		public object Handle { get { return elements; } }

		public VertexDeclaration ( IGraphicsDevice graphicsDevice, params VertexElement [] elements )
		{
			Length = elements.Length;
			foreach ( VertexElement e in elements )
				this.elements.Add ( e );
		}

		protected override void Dispose ( bool isDisposing )
		{
			base.Dispose ( isDisposing );
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return elements.GetEnumerator ();
		}
	}
}
