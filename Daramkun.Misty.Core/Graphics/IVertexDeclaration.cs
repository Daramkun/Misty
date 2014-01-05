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
		Binormal,
		Tangent,
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

	[AttributeUsage ( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
	public class VertexElementationAttribute : Attribute
	{
		public ElementType ElementType { get; private set; }
		public int UsageIndex { get; private set; }

		public VertexElementationAttribute ( ElementType elementType, int usageIndex = 0 )
		{
			ElementType = elementType;
			UsageIndex = usageIndex;
		}
	}
}
