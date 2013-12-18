using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public interface IVertexBuffer : IDisposable
	{
		Type VertexType { get; }
		int VertexTypeSize { get; }

		object Handle { get; }
		int Length { get; }

		T [] GetBufferDatas<T> () where T : struct;
		void SetBufferDatas<T> ( T [] buffer ) where T : struct;
	}
}
