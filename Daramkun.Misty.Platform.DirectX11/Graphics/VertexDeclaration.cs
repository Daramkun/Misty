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
		SharpDX.Direct3D11.InputLayout layout;
		SharpDX.Direct3D11.InputElement [] e;

		VertexElement [] elements;

		public object Handle { get { return layout; } }
		public int Length { get { return e.Length; } }

		internal void GenerateInputLayout ( IGraphicsDevice graphicsDevice, Shader vertexShader )
		{
			if ( layout != null ) return;
			layout = new SharpDX.Direct3D11.InputLayout ( graphicsDevice.Handle as SharpDX.Direct3D11.Device,
				( vertexShader.Handle as Shader ).bytecode, e );
		}

		public VertexDeclaration ( IGraphicsDevice graphicsDevice, VertexElement [] elements )
		{
			e = ConvertElements ( elements );
			this.elements = elements;
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				if ( layout != null )
					layout.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		private SharpDX.Direct3D11.InputElement [] ConvertElements ( VertexElement [] elements )
		{
			SharpDX.Direct3D11.InputElement [] e = new SharpDX.Direct3D11.InputElement [ elements.Length ];
			int offset = 0;
			for ( int i = 0; i < e.Length; ++i )
			{
				e [ i ] = new SharpDX.Direct3D11.InputElement ( ConvertUsage ( elements [ i ].Type ), 
					elements [ i ].UsageIndex, ConvertType ( elements[i].Size ), offset, 0 );
				offset += ConvertRealSize ( elements [ i ].Size );
			}
			return e;
		}

		private string ConvertUsage ( ElementType elementType )
		{
			switch ( elementType )
			{
				case ElementType.Position: return "POSITION";
				case ElementType.Diffuse: return "COLOR";
				case ElementType.Normal: return "NORMAL";
				case ElementType.TextureCoord: return "TEXCOORD";
				default: throw new ArgumentException ();
			}
		}

		private SharpDX.DXGI.Format ConvertType ( ElementSize elementSize )
		{
			switch ( elementSize )
			{
				case ElementSize.Float1: return SharpDX.DXGI.Format.R32_Float;
				case ElementSize.Float2: return SharpDX.DXGI.Format.R32G32_Float;
				case ElementSize.Float3: return SharpDX.DXGI.Format.R32G32B32_Float;
				case ElementSize.Float4: return SharpDX.DXGI.Format.R32G32B32A32_Float;
				default: throw new ArgumentException ();
			}
		}

		private int ConvertRealSize ( ElementSize elementSize )
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

		public System.Collections.IEnumerator GetEnumerator () { return elements.GetEnumerator (); }
	}
}
