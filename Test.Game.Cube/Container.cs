using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;

namespace Test.Game.Cube
{
	[MainNode]
    public class Container : Node
    {
		[StructLayout ( LayoutKind.Sequential )]
		private struct CubeVertex
		{
			[VertexElementation ( ElementType.Position )]
			public Vector3 Position;
			[VertexElementation ( ElementType.Diffuse )]
			public Color Diffuse;
		}

		IVertexBuffer cubeVertices;
		IVertexDeclaration vertexDeclarataion;

		public override void Intro ( params object [] args )
		{

			base.Intro ( args );
		}

		public override void Outro ()
		{
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			base.Draw ( gameTime );
		}
    }
}
