using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	class Buffer : StandardDispose, IBuffer
	{
		int bufferId;

		public BufferType BufferType { get;private set; }
		public object Handle { get { return bufferId; } }
		public Type RecordType { get; private set; }
		public int RecordTypeSize { get; private set; }
		public int Length { get; private set; }

		public Buffer ( IGraphicsDevice graphicsDevice, Type recordType, int bufferCount, BufferType bufferType )
		{
			BufferType = bufferType;

			RecordType = recordType;
			RecordTypeSize = Marshal.SizeOf ( recordType );
			Length = bufferCount;

			GL.GenBuffers ( 1, out bufferId );

			BufferTarget bufferTarget = 0;
			if ( bufferType.HasFlag ( BufferType.VertexBuffer ) )
				bufferTarget = BufferTarget.ArrayBuffer;
			else if ( bufferType.HasFlag ( BufferType.IndexBuffer ) )
				bufferTarget = BufferTarget.ElementArrayBuffer;

			GL.BindBuffer ( bufferTarget, bufferId );
			GL.BufferData ( bufferTarget, new IntPtr ( RecordTypeSize * Length ), IntPtr.Zero, BufferUsageHint.StaticDraw );
			GL.BindBuffer ( bufferTarget, 0 );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				GL.DeleteBuffers ( 1, ref bufferId );
				bufferId = 0;
			}
			base.Dispose ( isDisposing );
		}

		public T [] GetBufferDatas<T> () where T : struct
		{
			BufferTarget bufferTarget = 0;
			if ( BufferType.HasFlag ( BufferType.VertexBuffer ) )
				bufferTarget = BufferTarget.ArrayBuffer;
			else if ( BufferType.HasFlag ( BufferType.IndexBuffer ) )
				bufferTarget = BufferTarget.ElementArrayBuffer;

			GL.BindBuffer ( bufferTarget, bufferId );
			T [] data = new T [ Length ];
			IntPtr unmanagedHandle = GL.MapBuffer ( bufferTarget, BufferAccess.ReadOnly );
			for ( int i = 0; i < Length; i++ )
			{
				data [ i ] = ( T ) Marshal.PtrToStructure ( unmanagedHandle, typeof ( T ) );
				unmanagedHandle += RecordTypeSize;
			}
			GL.UnmapBuffer ( bufferTarget );
			GL.BindBuffer ( bufferTarget, 0 );
			return data;
		}

		public void SetBufferDatas<T> ( T [] buffer ) where T : struct
		{
			BufferTarget bufferTarget = 0;
			if ( BufferType.HasFlag ( BufferType.VertexBuffer ) )
				bufferTarget = BufferTarget.ArrayBuffer;
			else if ( BufferType.HasFlag ( BufferType.IndexBuffer ) )
				bufferTarget = BufferTarget.ElementArrayBuffer;

			GL.BindBuffer ( bufferTarget, bufferId );
			GL.BufferSubData<T> ( bufferTarget, new IntPtr ( 0 ), new IntPtr ( RecordTypeSize * Length ), buffer );
			GL.BindBuffer ( bufferTarget, 0 );
		}
	}
}
