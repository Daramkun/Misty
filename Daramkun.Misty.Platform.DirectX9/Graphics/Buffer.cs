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
		public BufferType BufferType { get; private set; }
		public Type RecordType { get; private set; }
		public int RecordTypeSize { get; private set; }
		public object Handle { get; private set; }
		public int Length { get; private set; }

		public Buffer ( IGraphicsDevice graphicsDevice, Type recordType, int bufferCount, BufferType bufferType )
		{
			BufferType = bufferType;

			RecordType = recordType;
			RecordTypeSize = Marshal.SizeOf ( recordType );
			Length = bufferCount;

			SharpDX.Direct3D9.Device device = graphicsDevice.Handle as SharpDX.Direct3D9.Device;
			if ( BufferType.HasFlag ( BufferType.VertexBuffer ) )
				Handle = new SharpDX.Direct3D9.VertexBuffer ( device, RecordTypeSize * bufferCount, SharpDX.Direct3D9.Usage.None,
					0, SharpDX.Direct3D9.Pool.Managed );
			else if ( BufferType.HasFlag ( BufferType.IndexBuffer ) )
				Handle = new SharpDX.Direct3D9.IndexBuffer ( graphicsDevice.Handle as SharpDX.Direct3D9.Device,
					bufferCount * RecordTypeSize, SharpDX.Direct3D9.Usage.None, SharpDX.Direct3D9.Pool.Managed, bufferType.HasFlag ( BufferType.Index16 ) );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				( Handle as IDisposable ).Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		public T [] GetBufferDatas<T> () where T : struct
		{
			SharpDX.DataStream stream = null;

			if ( BufferType.HasFlag ( BufferType.VertexBuffer ) )
				stream = ( Handle as SharpDX.Direct3D9.VertexBuffer ).Lock ( 0, 0, SharpDX.Direct3D9.LockFlags.None );
			else if ( BufferType.HasFlag ( BufferType.IndexBuffer ) )
				stream = ( Handle as SharpDX.Direct3D9.IndexBuffer ).Lock ( 0, 0, SharpDX.Direct3D9.LockFlags.None );

			T [] arr = stream.ReadRange<T> ( Length );
			stream.Dispose ();

			if ( BufferType.HasFlag ( BufferType.VertexBuffer ) )
				( Handle as SharpDX.Direct3D9.VertexBuffer ).Unlock ();
			else if ( BufferType.HasFlag ( BufferType.IndexBuffer ) )
				( Handle as SharpDX.Direct3D9.IndexBuffer ).Unlock ();
			return arr;
		}

		public void SetBufferDatas<T> ( T [] buffer ) where T : struct
		{
			SharpDX.DataStream stream = null;

			if ( BufferType.HasFlag ( BufferType.VertexBuffer ) )
				stream = ( Handle as SharpDX.Direct3D9.VertexBuffer ).Lock ( 0, 0, SharpDX.Direct3D9.LockFlags.None );
			else if ( BufferType.HasFlag ( BufferType.IndexBuffer ) )
				stream = ( Handle as SharpDX.Direct3D9.IndexBuffer ).Lock ( 0, 0, SharpDX.Direct3D9.LockFlags.None );

			stream.WriteRange<T> ( buffer );
			stream.Dispose ();

			if ( BufferType.HasFlag ( BufferType.VertexBuffer ) )
				( Handle as SharpDX.Direct3D9.VertexBuffer ).Unlock ();
			else if ( BufferType.HasFlag ( BufferType.IndexBuffer ) )
				( Handle as SharpDX.Direct3D9.IndexBuffer ).Unlock ();
		}
	}
}
