using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class VertexDeclaration : StandardDispose, IVertexDeclaration
	{
		SharpDX.Direct3D9.VertexDeclaration vertexDeclaration;
		System.Collections.IEnumerator elements;

		public object Handle { get { return vertexDeclaration; } }

		public int Length { get { return vertexDeclaration.Elements.Length - 1; } }

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return elements;
		}

		public VertexDeclaration ( IGraphicsDevice graphicsDevice, VertexElement [] elements )
		{
			this.elements = elements.GetEnumerator ();
			SharpDX.Direct3D9.Device device = graphicsDevice.Handle as SharpDX.Direct3D9.Device;

			SharpDX.Direct3D9.VertexElement [] convedElements = new SharpDX.Direct3D9.VertexElement [ elements.Length + 1 ];
			for ( int i = 0, offset = 0; i < elements.Length; ++i )
			{
				convedElements [ i ] = new SharpDX.Direct3D9.VertexElement ( 0, ( short ) offset,
					ConvertType ( elements [ i ].Size ), SharpDX.Direct3D9.DeclarationMethod.Default,
					ConvertType ( elements [ i ].Type ), ( byte ) elements [ i ].UsageIndex );
				offset += ElementSizeToRealSize ( elements [ i ].Size );
			}
			convedElements [ elements.Length ] = SharpDX.Direct3D9.VertexElement.VertexDeclarationEnd;

			vertexDeclaration = new SharpDX.Direct3D9.VertexDeclaration ( device, convedElements );
		}

		private SharpDX.Direct3D9.DeclarationUsage ConvertType ( ElementType elementType )
		{
			switch ( elementType )
			{
				case ElementType.Position: return SharpDX.Direct3D9.DeclarationUsage.Position;
				case ElementType.Normal: return SharpDX.Direct3D9.DeclarationUsage.Normal;
				case ElementType.Diffuse: return SharpDX.Direct3D9.DeclarationUsage.Color;
				case ElementType.TextureCoord: return SharpDX.Direct3D9.DeclarationUsage.TextureCoordinate;
				default: throw new ArgumentException ();
			}
		}

		private SharpDX.Direct3D9.DeclarationType ConvertType ( ElementSize elementSize )
		{
			switch ( elementSize )
			{
				case ElementSize.Float1: return SharpDX.Direct3D9.DeclarationType.Float1;
				case ElementSize.Float2: return SharpDX.Direct3D9.DeclarationType.Float2;
				case ElementSize.Float3: return SharpDX.Direct3D9.DeclarationType.Float3;
				case ElementSize.Float4: return SharpDX.Direct3D9.DeclarationType.Float4;
				default: throw new ArgumentException ();
			}
		}

		private int ElementSizeToRealSize ( ElementSize elementSize )
		{
			switch ( elementSize )
			{
				case ElementSize.Float1: return 4;
				case ElementSize.Float2: return 8;
				case ElementSize.Float3: return 12;
				case ElementSize.Float4: return 16;
				default: throw new ArgumentException ();
			}
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				vertexDeclaration.Dispose ();
			}
			base.Dispose ( isDisposing );
		}
	}
}
