using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	[Flags]
	public enum BufferType : ushort
	{
		VertexBuffer = 1 << 0,
		IndexBuffer = 1 << 1,

		Index16 = 1 << 15,
		Index32 = 0,
	}

	public interface IBuffer : IDisposable
	{
		BufferType BufferType { get; }

		object Handle { get; }
		int Length { get; }

		Type RecordType { get; }
		int RecordTypeSize { get; }

		T [] GetBufferDatas<T> () where T : struct;
		void SetBufferDatas<T> ( T [] buffer ) where T : struct;
	}
}
