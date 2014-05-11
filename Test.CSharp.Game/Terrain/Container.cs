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
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;
using Daramkun.Misty.Nodes;

namespace Test.Game.Terrain
{
	[MainNode]
	public class Container : Node
	{
		ResourceTable contentManager;
		ITexture2D texture1, texture2;
		IBuffer vertexBuffer;
		IVertexDeclaration vertexDeclaration;
		IBuffer indexBuffer;
		IEffect effect;

		Sprite sprite;
		World2 spriteWorld;

		int numOfIndices;
		TextureArgument textureArgs;

		PerspectiveFieldOfViewProjection proj;
		LookAt look;
		World3 world;

		public override void Intro ( params object [] args )
		{
			Core.GraphicsDevice.ImmediateContext.CullMode = CullMode.ClockWise;

			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );
			contentManager.AddDefaultContentLoader ();

			texture1 = contentManager.Load<ITexture2D> ( "Resources/Terrain/terrain_02.png" );
			texture2 = contentManager.Load<ITexture2D> ( "Resources/Terrain/terrain_01.png" );
			Color [] colours = texture2.Buffer;
			effect = contentManager.Load<IEffect> ( "Resources/Terrain/TerrainShader.xml" );

			textureArgs = new TextureArgument ( "texture0", texture1, TextureFilter.Anisotropic, TextureAddressing.Clamp,
				Core.GraphicsDevice.Information.MaximumAnisotropicLevel );

			vertexBuffer = Core.GraphicsDevice.CreateBuffer ( BufferType.VertexBuffer, typeof ( TerrainVertex ), texture2.Width * texture2.Height );
			vertexDeclaration = Core.GraphicsDevice.CreateVertexDeclaration ( Utilities.CreateVertexElementArray<TerrainVertex> () );
			numOfIndices = ( texture2.Width - 1 ) * ( texture2.Height - 1 ) * 2;
			indexBuffer = Core.GraphicsDevice.CreateBuffer ( BufferType.IndexBuffer, typeof ( TerrainIndex ), numOfIndices );

			TerrainVertex [] vertices = new TerrainVertex [ texture2.Width * texture2.Height ];
			int index = 0;
			for ( int x = 0; x < texture2.Height; x++ )
			{
				for ( int z = 0; z < texture2.Width; z++ )
				{
					int location = x * texture2.Width + z;
					vertices [ index ] = new TerrainVertex ()
					{
						Position = new Vector3 (
							( x - texture2.Height / 2 ) * 5.0f,
							colours [ location ].RedValue * 5.0f / 3,
							( z - texture2.Width / 2 ) * 5.0f
						),
						UV = new Vector2 (
							z / ( float ) texture2.Width,
							x / ( float ) texture2.Height
						)
					};
					++index;
				}
			}
			vertexBuffer.SetBufferDatas<TerrainVertex> ( vertices );

			TerrainIndex [] indices = new TerrainIndex [ numOfIndices ];
			index = 0;
			for ( int z = 0; z < texture2.Height - 1; z++ )
			{
				for ( int x = 0; x < texture2.Width - 1; x++ )
				{
					indices [ index ]._0 = z * texture2.Width + x;
					indices [ index ]._1 = z * texture2.Width + ( x + 1 );
					indices [ index ]._2 = ( z + 1 ) * texture2.Width + x;
					++index;
					indices [ index ]._0 = ( z + 1 ) * texture2.Width + x;
					indices [ index ]._1 = z * texture2.Width + ( x + 1 );
					indices [ index ]._2 = ( z + 1 ) * texture2.Width + ( x + 1 );
					++index;
				}
			}
			indexBuffer.SetBufferDatas<TerrainIndex> ( indices );

			proj = new PerspectiveFieldOfViewProjection ( 3.141592f / 4, 4 / 3f, 1, 10000 );
			look = new LookAt ( new Vector3 ( 1000, 2000, 1000 ), new Vector3 ( 0, 0, 0 ), new Vector3 ( 0, 1, 0 ) );
			world = new World3 ();

			sprite = new Sprite ( null );
			spriteWorld = new World2 ();

			Add ( new FpsCalculator () );

			base.Intro ( args );
		}

		public override void Outro ()
		{
			sprite.Dispose ();
			indexBuffer.Dispose ();
			vertexBuffer.Dispose ();
			contentManager.Dispose ();
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			world.Rotation.Y += ( float ) gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.ImmediateContext.BeginScene ();
			Core.GraphicsDevice.ImmediateContext.Clear ( ClearBuffer.AllBuffer, Color.Black );

			base.Draw ( gameTime );

			effect.Use ( Core.GraphicsDevice.ImmediateContext );
			effect.SetUniform<Matrix4x4> ( "worldMatrix", world.Matrix );
			effect.SetUniform<Matrix4x4> ( "viewMatrix", look.Matrix );
			effect.SetUniform<Matrix4x4> ( "projMatrix", proj.Matrix );
			effect.SetTextures ( textureArgs );
			Core.GraphicsDevice.ImmediateContext.InputAssembler = new InputAssembler ( vertexBuffer, vertexDeclaration, PrimitiveType.TriangleList, indexBuffer );
			Core.GraphicsDevice.ImmediateContext.Draw ( 0, numOfIndices );

			spriteWorld.Translate = new Vector2 ( 0, Core.GraphicsDevice.ImmediateContext.CurrentRenderBuffer.Height - texture1.Height );
			sprite.Reset ( texture1 );
			sprite.Draw ( spriteWorld );

			spriteWorld.Translate = new Vector2 ( Core.GraphicsDevice.ImmediateContext.CurrentRenderBuffer.Width - texture2.Width,
				Core.GraphicsDevice.ImmediateContext.CurrentRenderBuffer.Height - texture1.Height );
			sprite.Reset ( texture2 );
			sprite.Draw ( spriteWorld );

			FpsCalculator calc = Children.First () as FpsCalculator;
			Core.Window.Title = string.Format ( "Terrain {{ Update FPS: {0}, Draw FPS: {1} }}", calc.UpdateFPS, calc.DrawFPS );

			Core.GraphicsDevice.ImmediateContext.EndScene ();
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
