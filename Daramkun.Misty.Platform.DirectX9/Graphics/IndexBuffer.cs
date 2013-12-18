using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class IndexBuffer : StandardDispose, IIndexBuffer
	{
		SharpDX.Direct3D9.IndexBuffer indexBuffer;

		public object Handle { get { return indexBuffer; } }
		public bool Is16bitIndex { get; private set; }
		public Type IndexType { get; private set; }
		public int IndexTypeSize { get; private set; }
		public int Length { get; private set; }

		public IndexBuffer ( IGraphicsDevice graphicsDevice, Type indexType, int indexCount, bool is16bit )
		{
			Is16bitIndex = is16bit;
			IndexType = indexType;
			IndexTypeSize = Marshal.SizeOf ( indexType );
			Length = indexCount;
			
			indexBuffer = new SharpDX.Direct3D9.IndexBuffer ( graphicsDevice.Handle as SharpDX.Direct3D9.Device,
				indexCount * IndexTypeSize, SharpDX.Direct3D9.Usage.None, SharpDX.Direct3D9.Pool.Managed, is16bit );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				indexBuffer.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		public T [] GetBufferDatas<T> () where T : struct
		{
			SharpDX.DataStream stream = indexBuffer.Lock ( 0, 0, SharpDX.Direct3D9.LockFlags.None );
			T [] arr = stream.ReadRange<T> ( Length );
			indexBuffer.Unlock ();
			return arr;
		}

		public void SetBufferDatas<T> ( T [] buffer ) where T : struct
		{
			SharpDX.DataStream stream = indexBuffer.Lock ( 0, 0, SharpDX.Direct3D9.LockFlags.None );
			stream.WriteRange<T> ( buffer );
			indexBuffer.Unlock ();
		}
	}
}
