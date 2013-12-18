using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public enum ElementType
	{
		Position,
		Diffuse,
		Normal,
		TextureCoord,
	}

	public enum ElementSize
	{
		Float1, Float2, Float3, Float4,
	}

	public struct VertexElement
	{
		public ElementType Type;
		public int UsageIndex;
		public ElementSize Size;

		public VertexElement ( ElementType t, int i, ElementSize s ) { Type = t; UsageIndex = i; Size = s; }
	}

	public interface IVertexDeclaration : IEnumerable, IDisposable
	{
		object Handle { get; }
		int Length { get; }
	}
}
