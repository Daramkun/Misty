using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class VertexBuffer : StandardDispose, IVertexBuffer
	{
		SharpDX.Direct3D9.VertexBuffer vertexBuffer;

		public Type VertexType { get; private set; }
		public int VertexTypeSize { get; private set; }
		public object Handle { get { return vertexBuffer; } }
		public int Length { get; private set; }

		public VertexBuffer ( IGraphicsDevice graphicsDevice, Type vertexType, int vertexCount )
		{
			VertexType = vertexType;
			VertexTypeSize = Marshal.SizeOf ( vertexType );
			Length = vertexCount;

			SharpDX.Direct3D9.Device device = graphicsDevice.Handle as SharpDX.Direct3D9.Device;
			vertexBuffer = new SharpDX.Direct3D9.VertexBuffer ( device, VertexTypeSize * vertexCount, SharpDX.Direct3D9.Usage.None,
				0, SharpDX.Direct3D9.Pool.Managed );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				vertexBuffer.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		public T [] GetBufferDatas<T> () where T : struct
		{
			SharpDX.DataStream stream = vertexBuffer.Lock ( 0, 0, SharpDX.Direct3D9.LockFlags.None );
			T [] arr = stream.ReadRange<T> ( Length );
			vertexBuffer.Unlock ();
			return arr;
		}

		public void SetBufferDatas<T> ( T [] buffer ) where T : struct
		{
			SharpDX.DataStream stream = vertexBuffer.Lock ( 0, 0, SharpDX.Direct3D9.LockFlags.None );
			stream.WriteRange<T> ( buffer );
			vertexBuffer.Unlock ();
		}
	}
}
