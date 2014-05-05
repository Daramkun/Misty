using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class Buffer : StandardDispose, IBuffer
	{
		SharpDX.Direct3D11.Buffer bufferObj;

		public BufferType BufferType { get; private set; }

		public Type RecordType { get; private set; }
		public int RecordTypeSize { get; private set; }
		public object Handle { get { return bufferObj; } }
		public int Length { get; private set; }

		public Buffer ( IGraphicsDevice graphicsDevice, Type vertexType, int vertexCount, BufferType bufferType )
		{
			BufferType = bufferType;

			RecordType = vertexType;
			RecordTypeSize = Marshal.SizeOf ( RecordType );
			Length = vertexCount;

			SharpDX.Direct3D11.BindFlags flags = 0;
			if ( bufferType.HasFlag ( BufferType.VertexBuffer ) )
				flags = SharpDX.Direct3D11.BindFlags.VertexBuffer;
			else if ( bufferType.HasFlag ( BufferType.IndexBuffer ) )
				flags = SharpDX.Direct3D11.BindFlags.IndexBuffer;

			bufferObj = new SharpDX.Direct3D11.Buffer ( graphicsDevice.Handle as SharpDX.Direct3D11.Device, new SharpDX.Direct3D11.BufferDescription ()
			{
				Usage = SharpDX.Direct3D11.ResourceUsage.Default,
				StructureByteStride = RecordTypeSize,
				SizeInBytes = RecordTypeSize * Length,
				OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
				CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.Read | SharpDX.Direct3D11.CpuAccessFlags.Write,
				BindFlags = flags
			} );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				bufferObj.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		public T [] GetBufferDatas<T> () where T : struct
		{
			SharpDX.DataStream stream;
			bufferObj.Device.ImmediateContext.MapSubresource ( bufferObj, SharpDX.Direct3D11.MapMode.Read, SharpDX.Direct3D11.MapFlags.None, out stream );
			T [] temp = stream.ReadRange<T> ( Length );
			bufferObj.Device.ImmediateContext.UnmapSubresource ( bufferObj, 0 );
			return temp;
		}

		public void SetBufferDatas<T> ( T [] buffer ) where T : struct
		{
			SharpDX.DataStream stream;
			bufferObj.Device.ImmediateContext.MapSubresource ( bufferObj, SharpDX.Direct3D11.MapMode.Write, SharpDX.Direct3D11.MapFlags.None, out stream );
			stream.WriteRange<T> ( buffer );
			bufferObj.Device.ImmediateContext.UnmapSubresource ( bufferObj, 0 );
		}
	}
}
