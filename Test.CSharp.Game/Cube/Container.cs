using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;

namespace Test.Game.CSharp.Cube
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

		[StructLayout ( LayoutKind.Sequential )]
		private struct CubeIndex
		{
			public int I0, I1, I2;
		}

		IVertexBuffer cubeVertices;
		IIndexBuffer cubeIndices;
		IVertexDeclaration vertexDeclarataion;
		IEffect cubeEffect;

		PerspectiveFieldOfViewProjection proj;
		LookAt lookAt;
		World3 world;

		ResourceTable contentManager;

		public override void Intro ( params object [] args )
		{
			Core.Window.Title = "Cube";
			
			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			cubeEffect = contentManager.Load<IEffect> ( "Resources/Cube/CubeShader.xml", "i_pos", "i_col" );

			cubeVertices = Core.GraphicsDevice.CreateVertexBuffer<CubeVertex>( new CubeVertex []
			{
				new CubeVertex () { Position = new Vector3 ( -1, -1, -1 ), Diffuse = Color.Red },
				new CubeVertex () { Position = new Vector3 ( +1, -1, -1 ), Diffuse = Color.Blue },
				new CubeVertex () { Position = new Vector3 ( -1, -1, +1 ), Diffuse = Color.Green },
				new CubeVertex () { Position = new Vector3 ( +1, -1, +1 ), Diffuse = Color.White },

				new CubeVertex () { Position = new Vector3 ( -1, +1, -1 ), Diffuse = Color.Magenta },
				new CubeVertex () { Position = new Vector3 ( -1, +1, +1 ), Diffuse = Color.Cyan },
				new CubeVertex () { Position = new Vector3 ( +1, +1, -1 ), Diffuse = Color.Yellow },
				new CubeVertex () { Position = new Vector3 ( +1, +1, +1 ), Diffuse = Color.White },
			} );
			cubeIndices = Core.GraphicsDevice.CreateIndexBuffer<CubeIndex> ( new CubeIndex []
			{
				// TOP
				new CubeIndex () { I0 = 0, I1 = 1, I2 = 2 },
				new CubeIndex () { I0 = 1, I1 = 3, I2 = 2 },
				// LEFT
				new CubeIndex () { I0 = 0, I1 = 2, I2 = 4 },
				new CubeIndex () { I0 = 2, I1 = 5, I2 = 4 },
				// FRONT
				new CubeIndex () { I0 = 2, I1 = 3, I2 = 5 },
				new CubeIndex () { I0 = 3, I1 = 7, I2 = 5 },
				// RIGHT
				new CubeIndex () { I0 = 3, I1 = 1, I2 = 7 },
				new CubeIndex () { I0 = 1, I1 = 6, I2 = 7 },
				// BACK
				new CubeIndex () { I0 = 1, I1 = 0, I2 = 6 },
				new CubeIndex () { I0 = 6, I1 = 0, I2 = 4 },
				// BOTTOM
				new CubeIndex () { I0 = 5, I1 = 6, I2 = 4 },
				new CubeIndex () { I0 = 5, I1 = 7, I2 = 6 },
			} );
			vertexDeclarataion = Core.GraphicsDevice.CreateVertexDeclaration ( Utilities.CreateVertexElementArray<CubeVertex> () );

			proj = new PerspectiveFieldOfViewProjection ();
			lookAt = new LookAt ( new Vector3 ( 5, 5, 5 ), new Vector3 ( 0, 0, 0 ), new Vector3 ( 0, 1, 0 ) );
			world = World3.Identity;

			base.Intro ( args );
		}

		public override void Outro ()
		{
			vertexDeclarataion.Dispose ();
			cubeIndices.Dispose ();
			cubeVertices.Dispose ();
			contentManager.Dispose ();
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			world.Rotation.X += ( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f );
			world.Rotation.Y += ( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f );
			world.Rotation.Z += ( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f );
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );

			Matrix4x4 projMatrix, lookAtMatrix, worldMatrix;
			proj.GetMatrix ( out projMatrix );
			lookAt.GetMatrix ( out lookAtMatrix );
			world.GetMatrix ( out worldMatrix );

			cubeEffect.Begin ();
			cubeEffect.SetUniform ( "projMatrix", ref projMatrix );
			cubeEffect.SetUniform ( "viewMatrix", ref lookAtMatrix );
			cubeEffect.SetUniform ( "worldMatrix", ref worldMatrix );
			Core.GraphicsDevice.Draw ( PrimitiveType.TriangleList, cubeVertices, vertexDeclarataion, cubeIndices, 0, 12 );
			cubeEffect.End ();

			base.Draw ( gameTime );
			
			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Cube"; }
    }
}
