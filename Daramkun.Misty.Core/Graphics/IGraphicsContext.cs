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
		object Handle { get; }

		CullMode CullMode { get; set; }
		FillMode FillMode { get; set; }

		BlendState BlendState { get; set; }
		DepthStencil DepthStencil { get; set; }
		InputAssembler InputAssembler { get; set; }

		Viewport Viewport { get; set; }

		void BeginScene ( IRenderBuffer renderBuffer = null );
		void EndScene ();

		void Clear ( ClearBuffer clearBuffer, Color color, float depth = 1, int stencil = 0 );

		void Draw ( int startVertex, int primitiveCount );
	}
}
