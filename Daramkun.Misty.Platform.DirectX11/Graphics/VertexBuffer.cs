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
		SharpDX.Direct3D11.Buffer vertexBuffer;

		public Type VertexType { get; private set; }
		public int VertexTypeSize { get; private set; }
		public object Handle { get { return vertexBuffer; } }
		public int Length { get; private set; }

		public VertexBuffer ( IGraphicsDevice graphicsDevice, Type vertexType, int vertexCount )
		{
			VertexType = vertexType;
			VertexTypeSize = Marshal.SizeOf ( VertexType );
			Length = vertexCount;
			vertexBuffer = new SharpDX.Direct3D11.Buffer ( graphicsDevice.Handle as SharpDX.Direct3D11.Device, new SharpDX.Direct3D11.BufferDescription ()
			{
				Usage = SharpDX.Direct3D11.ResourceUsage.Default,
				StructureByteStride = VertexTypeSize,
				SizeInBytes = VertexTypeSize * Length,
				OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
				CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.Read | SharpDX.Direct3D11.CpuAccessFlags.Write,
				BindFlags = SharpDX.Direct3D11.BindFlags.VertexBuffer
			} );
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
			SharpDX.DataStream stream;
			vertexBuffer.Device.ImmediateContext.MapSubresource ( vertexBuffer, SharpDX.Direct3D11.MapMode.Read, SharpDX.Direct3D11.MapFlags.None, out stream );
			T [] temp = stream.ReadRange<T> ( Length );
			vertexBuffer.Device.ImmediateContext.UnmapSubresource ( vertexBuffer, 0 );
			return temp;
		}

		public void SetBufferDatas<T> ( T [] buffer ) where T : struct
		{
			SharpDX.DataStream stream;
			vertexBuffer.Device.ImmediateContext.MapSubresource ( vertexBuffer, SharpDX.Direct3D11.MapMode.Write, SharpDX.Direct3D11.MapFlags.None, out stream );
			stream.WriteRange<T> ( buffer );
			vertexBuffer.Device.ImmediateContext.UnmapSubresource ( vertexBuffer, 0 );
		}
	}
}
