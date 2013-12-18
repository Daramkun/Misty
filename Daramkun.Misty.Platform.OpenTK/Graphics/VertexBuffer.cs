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
	class VertexBuffer : StandardDispose, IVertexBuffer
	{
		int vertexBuffer;

		public object Handle { get { return vertexBuffer; } }
		public Type VertexType { get; private set; }
		public int VertexTypeSize { get; private set; }
		public int Length { get; private set; }

		public VertexBuffer ( IGraphicsDevice graphicsDevice, Type vertexType, int vertexCount )
		{
			VertexType = vertexType;
			VertexTypeSize = Marshal.SizeOf ( vertexType );
			Length = vertexCount;

			GL.GenBuffers ( 1, out vertexBuffer );

			GL.BindBuffer ( BufferTarget.ArrayBuffer, vertexBuffer );
			GL.BufferData ( BufferTarget.ArrayBuffer, new IntPtr ( VertexTypeSize * Length ), IntPtr.Zero, BufferUsageHint.StaticDraw );
			GL.BindBuffer ( BufferTarget.ArrayBuffer, 0 );
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				GL.DeleteBuffers ( 1, ref vertexBuffer );
				vertexBuffer = 0;
			}
			base.Dispose ( isDisposing );
		}

		public T [] GetBufferDatas<T> () where T : struct
		{
			GL.BindBuffer ( BufferTarget.ArrayBuffer, vertexBuffer );
			T [] data = new T [ Length ];
			IntPtr unmanagedHandle = GL.MapBuffer ( BufferTarget.ArrayBuffer, BufferAccess.ReadOnly );
			for ( int i = 0; i < Length; i++ )
			{
				data [ i ] = ( T ) Marshal.PtrToStructure ( unmanagedHandle, typeof ( T ) );
				unmanagedHandle += VertexTypeSize;
			}
			GL.UnmapBuffer ( BufferTarget.ArrayBuffer );
			GL.BindBuffer ( BufferTarget.ArrayBuffer, 0 );
			return data;
		}

		public void SetBufferDatas<T> ( T [] buffer ) where T : struct
		{
			GL.BindBuffer ( BufferTarget.ArrayBuffer, vertexBuffer );
			GL.BufferSubData<T> ( BufferTarget.ArrayBuffer, new IntPtr ( 0 ), new IntPtr ( VertexTypeSize * Length ), buffer );
			GL.BindBuffer ( BufferTarget.ArrayBuffer, 0 );
		}
	}
}
