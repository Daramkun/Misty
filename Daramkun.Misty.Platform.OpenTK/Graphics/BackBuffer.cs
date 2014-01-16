using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	class BackBuffer : StandardDispose, IRenderBuffer
	{
		OpenTK.GameWindow window;

		public int Width { get { return window.ClientSize.Width; } }
		public int Height { get { return window.ClientSize.Height; } }

		public object Handle { get { return 0; } }

		public Vector2 Size { get { return new Vector2 ( Width, Height ); } }

		public Color [] Buffer
		{
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
		}

		public BackBuffer ( OpenTK.GameWindow window )
		{
			this.window = window;
		}
	}
}
