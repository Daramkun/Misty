using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public interface IIndexBuffer : IDisposable
	{
		bool Is16bitIndex { get; }

		object Handle { get; }
		int Length { get; }

		Type IndexType { get; }
		int IndexTypeSize { get; }

		T [] GetBufferDatas<T> () where T : struct;
		void SetBufferDatas<T> ( T [] buffer ) where T : struct;
	}
}