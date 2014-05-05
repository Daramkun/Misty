using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Daramkun.Misty.Graphics
{
	public interface IGraphicsContext : IDisposable
	{
		Thread Owner { get; }

		IGraphicsDevice GraphicsDevice { get; }
		IRenderBuffer CurrentRenderBuffer { get; }

		CullMode CullMode { get; set; }
		FillMode FillMode { get; set; }

		BlendState BlendState { get; set; }
		DepthStencil DepthStencil { get; set; }

		Viewport Viewport { get; set; }

		void BeginScene ( IRenderBuffer renderBuffer = null );
		void EndScene ();

		void Clear ( ClearBuffer clearBuffer, Color color, float depth = 1, int stencil = 0 );

		void Draw ( PrimitiveType primitiveType, IBuffer vertexBuffer, IVertexDeclaration vertexDeclaration,
			int startVertex, int primitiveCount );
		void Draw ( PrimitiveType primitiveType, IBuffer vertexBuffer, IVertexDeclaration vertexDeclaration,
			IBuffer indexBuffer, int startIndex, int primitiveCount );
	}
}
