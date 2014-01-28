using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;

namespace Test.Game.Terrain
{
	[MainNode]
	public class Container : Node
	{
		ResourceTable contentManager;
		ITexture2D texture;
		IVertexBuffer vertexBuffer;
		IVertexDeclaration vertexDeclaration;
		IIndexBuffer indexBuffer;
		IEffect effect;

		int numOfIndices;
		TextureArgument textureArgs;

		PerspectiveFieldOfViewProjection proj;
		LookAt look;

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.CullMode = CullingMode.None;

			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			contentManager.AddDefaultContentLoader ();

			ITexture2D grayscale = contentManager.Load<ITexture2D> ( "Resources/terrain_01.png" );
			Color [] colours = grayscale.Buffer;
			texture = contentManager.Load<ITexture2D> ( "Resources/terrain_02.png" );
			effect = contentManager.Load<IEffect> ( "Resources/TerrainShader.xml" );

			textureArgs = new TextureArgument ( "texture0", texture, TextureFilter.Linear, TextureAddressing.Clamp, 0 );

			vertexBuffer = Core.GraphicsDevice.CreateVertexBuffer ( typeof ( TerrainVertex ), grayscale.Width * grayscale.Height );
			vertexDeclaration = Core.GraphicsDevice.CreateVertexDeclaration ( Utilities.CreateVertexElementArray<TerrainVertex> () );
			numOfIndices = ( grayscale.Width - 1 ) * ( grayscale.Height - 1 ) * 2;
			indexBuffer = Core.GraphicsDevice.CreateIndexBuffer ( typeof ( TerrainIndex ), numOfIndices );

			TerrainVertex [] vertices = new TerrainVertex [ grayscale.Width * grayscale.Height ];
			int index = 0;
			for ( int x = 0; x < grayscale.Height; x++ )
			{
				for ( int z = 0; z < grayscale.Width; z++ )
				{
					int location = x * grayscale.Width + z;
					vertices [ index ] = new TerrainVertex ()
					{
						Position = new Vector3 (
							( x - grayscale.Height / 2 ) * 5.0f,
							colours [ location ].RedValue * 5.0f / 2,
							( z - grayscale.Width / 2 ) * 5.0f
						),
						UV = new Vector2 (
							z / ( float ) grayscale.Width,
							x / ( float ) grayscale.Height
						)
					};
					++index;
				}
			}
			vertexBuffer.SetBufferDatas<TerrainVertex> ( vertices );

			TerrainIndex [] indices = new TerrainIndex [ numOfIndices ];
			index = 0;
			for ( int z = 0; z < grayscale.Height - 1; z++ )
			{
				for ( int x = 0; x < grayscale.Width - 1; x++ )
				{
					indices [ index ]._0 = z * grayscale.Width + x;
					indices [ index ]._1 = z * grayscale.Width + ( x + 1 );
					indices [ index ]._2 = ( z + 1 ) * grayscale.Width + x;
					++index;
					indices [ index ]._0 = ( z + 1 ) * grayscale.Width + x;
					indices [ index ]._1 = z * grayscale.Width + ( x + 1 );
					indices [ index ]._2 = ( z + 1 ) * grayscale.Width + ( x + 1 );
					++index;
				}
			}
			indexBuffer.SetBufferDatas<TerrainIndex> ( indices );

			proj = new PerspectiveFieldOfViewProjection ( 3.141592f / 4, 4 / 3f, 1, 10000 );
			look = new LookAt ( new Vector3 ( 1000, 2000, 1000 ), new Vector3 ( 0, 0, 0 ), new Vector3 ( 0, 1, 0 ) );

			base.Intro ( args );
		}

		public override void Outro ()
		{
			indexBuffer.Dispose ();
			vertexBuffer.Dispose ();
			contentManager.Dispose ();
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );

			base.Draw ( gameTime );

			effect.Begin ();
			effect.SetUniform<Matrix4x4> ( "worldMatrix", Matrix4x4.Identity );
			effect.SetUniform<Matrix4x4> ( "viewMatrix", look.Matrix );
			effect.SetUniform<Matrix4x4> ( "projMatrix", proj.Matrix );
			effect.SetTextures ( textureArgs );
			Core.GraphicsDevice.Draw ( PrimitiveType.TriangleList, vertexBuffer, vertexDeclaration, indexBuffer, 0, numOfIndices );
			effect.End ();

			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Terrain"; }

		[StructLayout ( LayoutKind.Sequential )]
		struct TerrainVertex
		{
			[VertexElementation ( ElementType.Position )]
			public Vector3 Position;
			[VertexElementation ( ElementType.TextureCoord )]
			public Vector2 UV;
		}
		[StructLayout ( LayoutKind.Sequential )]
		struct TerrainIndex { public int _0, _1, _2; }
	}
}
