using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class RenderBuffer : StandardDispose, IRenderBuffer
	{
		public int Width
		{
			get { throw new NotImplementedException (); }
		}

		public int Height
		{
			get { throw new NotImplementedException (); }
		}

		public Mathematics.Vector2 Size
		{
			get { throw new NotImplementedException (); }
		}

		public Color [] Buffer
		{
			get
			{
				throw new NotImplementedException ();
			}
			set
			{
				throw new NotImplementedException ();
			}
		}

		public object Handle
		{
			get { throw new NotImplementedException (); }
		}
	}
}
