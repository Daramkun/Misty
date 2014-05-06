using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public struct InputAssembler
	{
		public PrimitiveType PrimitiveType { get; private set; }
		public IBuffer VertexBuffer { get; private set; }
		public IVertexDeclaration VertexDeclaration { get; private set; }
		public IBuffer IndexBuffer { get; private set; }

		public InputAssembler ( PrimitiveType primitiveType, IBuffer vertexBuffer, IVertexDeclaration vertexDeclaration, IBuffer indexBuffer = null )
			: this ()
		{
			PrimitiveType = primitiveType;
			VertexBuffer = vertexBuffer;
			VertexDeclaration = vertexDeclaration;
			IndexBuffer = indexBuffer;
		}
	}
}
