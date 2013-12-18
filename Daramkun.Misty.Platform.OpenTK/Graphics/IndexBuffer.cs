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
	class IndexBuffer : StandardDispose, IIndexBuffer
	{
		int indexBuffer;

		public object Handle { get { return indexBuffer; } }
		public bool Is16bitIndex { get; private set; }
		public Type IndexType { get; private set; }
		public int IndexTypeSize { get; private set; }
		public int Length { get; private set; }

		public IndexBuffer ( IGraphicsDevice graphicsDevice, Type indexType, int indexCount, bool is16bit )
		{
			Length = indexCount;
			Is16bitIndex = is16bit;

			IndexType = indexType;
			IndexTypeSize = Marshal.SizeOf ( indexType );

			GL.GenBuffers ( 1, out indexBuffer );

			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, indexBuffer );
			GL.BufferData ( BufferTarget.ElementArrayBuffer, new IntPtr ( indexCount * IndexTypeSize ), IntPtr.Zero, BufferUsageHint.StaticDraw );
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, 0 );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				GL.DeleteBuffers ( 1, ref indexBuffer );
				indexBuffer = 0;
			}
			base.Dispose ( isDisposing );
		}

		public T [] GetBufferDatas<T> () where T : struct
		{
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, indexBuffer );
			T [] data = new T [ Length ];
			IntPtr unmanagedHandle = GL.MapBuffer ( BufferTarget.ElementArrayBuffer, BufferAccess.ReadOnly );
			for ( int i = 0; i < Length; i++ )
			{
				data [ i ] = ( T ) Marshal.PtrToStructure ( unmanagedHandle, typeof ( T ) );
				unmanagedHandle += IndexTypeSize;
			}
			GL.UnmapBuffer ( BufferTarget.ElementArrayBuffer );
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, 0 );
			return data;
		}

		public void SetBufferDatas<T> ( T [] buffer ) where T : struct
		{
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, indexBuffer );
			GL.BufferSubData<T> ( BufferTarget.ElementArrayBuffer, new IntPtr ( 0 ), new IntPtr ( Length * IndexTypeSize ), buffer );
			GL.BindBuffer ( BufferTarget.ElementArrayBuffer, 0 );
		}
	}
}
