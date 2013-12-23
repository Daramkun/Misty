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
		SharpDX.Direct3D11.Buffer indexBuffer;

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
			indexBuffer = new SharpDX.Direct3D11.Buffer ( graphicsDevice.Handle as SharpDX.Direct3D11.Device, new SharpDX.Direct3D11.BufferDescription ()
			{
				Usage = SharpDX.Direct3D11.ResourceUsage.Default,
				StructureByteStride = IndexTypeSize,
				SizeInBytes = IndexTypeSize * Length,
				OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
				CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.Read | SharpDX.Direct3D11.CpuAccessFlags.Write,
				BindFlags = SharpDX.Direct3D11.BindFlags.IndexBuffer
			} );
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
			SharpDX.DataStream stream;
			indexBuffer.Device.ImmediateContext.MapSubresource ( indexBuffer, SharpDX.Direct3D11.MapMode.Read, SharpDX.Direct3D11.MapFlags.None, out stream );
			T [] temp = stream.ReadRange<T> ( Length );
			indexBuffer.Device.ImmediateContext.UnmapSubresource ( indexBuffer, 0 );
			return temp;
		}

		public void SetBufferDatas<T> ( T [] buffer ) where T : struct
		{
			SharpDX.DataStream stream;
			indexBuffer.Device.ImmediateContext.MapSubresource ( indexBuffer, SharpDX.Direct3D11.MapMode.Write, SharpDX.Direct3D11.MapFlags.None, out stream );
			stream.WriteRange<T> ( buffer );
			indexBuffer.Device.ImmediateContext.UnmapSubresource ( indexBuffer, 0 );
		}
	}
}
