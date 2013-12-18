using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Geometries;
using Daramkun.Misty.Mathematics.Transforms;

namespace Daramkun.Misty.Graphics.Spirit
{
	public sealed class Sprite
	{
		private struct SpriteVertex
		{
			public Vector2 Position;
			public Color Diffuse;
			public Vector2 TexCoord;

			public SpriteVertex ( Vector2 pos, Color dif, Vector2 tex )
			{
				Position = pos;
				Diffuse = dif;
				TexCoord = tex;
			}
		}

		static IIndexBuffer indexBuffer;
		static IVertexDeclaration vertexDeclaration;
		static int indexReference;
		static SpriteEffect baseSpriteEffect;

		IVertexBuffer vertexBuffer;
		Rectangle clippingArea;
		Color overlayColor;

		public IEffect Effect { get; set; }
		public ITexture2D Texture { get; private set; }

		public TextureFilter TextureFilter { get; set; }
		public int AnisotropicLevel { get; set; }

		public Rectangle ClippingArea
		{
			get { return clippingArea; }
			set
			{
				clippingArea = value;
				SpriteVertex [] vertices = vertexBuffer.GetBufferDatas<SpriteVertex> ();
				vertices [ 0 ].Position = new Vector2 ( 0, 0 );
				vertices [ 1 ].Position = new Vector2 ( clippingArea.Size.X, 0 );
				vertices [ 2 ].Position = new Vector2 ( 0, clippingArea.Size.Y );
				vertices [ 3 ].Position = new Vector2 ( clippingArea.Size.X, clippingArea.Size.Y );
				vertices [ 0 ].TexCoord = new Vector2 ( clippingArea.Position.X / Texture.Width, clippingArea.Position.Y / Texture.Height );
				vertices [ 1 ].TexCoord = new Vector2 ( clippingArea.Size.X / Texture.Width, clippingArea.Position.Y / Texture.Height );
				vertices [ 2 ].TexCoord = new Vector2 ( clippingArea.Position.X / Texture.Width, clippingArea.Size.Y / Texture.Height );
				vertices [ 3 ].TexCoord = new Vector2 ( clippingArea.Size.X / Texture.Width, clippingArea.Size.Y / Texture.Height );
				vertexBuffer.SetBufferDatas<SpriteVertex> ( vertices );
			}
		}

		public Color OverlayColor
		{
			get { return overlayColor; }
			set
			{
				overlayColor = value;
				SpriteVertex [] vertices = vertexBuffer.GetBufferDatas<SpriteVertex> ();
				vertices [ 0 ].Diffuse = vertices [ 1 ].Diffuse = vertices [ 2 ].Diffuse = vertices [ 3 ].Diffuse = value;
				vertexBuffer.SetBufferDatas<SpriteVertex> ( vertices );
			}
		}

		public Sprite ( ITexture2D texture )
			: this ( texture, baseSpriteEffect )
		{ }

		public Sprite ( ITexture2D texture, IEffect effect )
		{
			if ( effect == null )
			{
				if ( baseSpriteEffect == null )
					baseSpriteEffect = new SpriteEffect ();
				effect = baseSpriteEffect;
			}

			Texture = texture;
			Effect = effect;

			int width = 1, height = 1;
			if ( texture != null ) { width = texture.Width; height = texture.Height; }

			vertexBuffer = Core.GraphicsDevice.CreateVertexBuffer<SpriteVertex> ( new SpriteVertex []
			{
				new SpriteVertex ( new Vector2 ( 0, 0 ), Color.White, new Vector2 ( 0, 0 ) ),
				new SpriteVertex ( new Vector2 ( width, 0 ), Color.White, new Vector2 ( 1, 0 ) ),
				new SpriteVertex ( new Vector2 ( 0, height ), Color.White, new Vector2 ( 0, 1 ) ),
				new SpriteVertex ( new Vector2 ( width, height ), Color.White, new Vector2 ( 1, 1 ) ),
			} );
			if ( indexBuffer == null )
			{
				indexBuffer = Core.GraphicsDevice.CreateIndexBuffer ( new int [] { 0, 1, 2, 1, 3, 2 } );
				vertexDeclaration = Core.GraphicsDevice.CreateVertexDeclaration (
					new VertexElement ( ElementType.Position, 0, ElementSize.Float2 ),
					new VertexElement ( ElementType.Diffuse, 0, ElementSize.Float4 ),
					new VertexElement ( ElementType.TextureCoord, 0, ElementSize.Float2 )
				);
			}
			indexReference++;

			clippingArea = new Rectangle ( 0, 0, width, height );

			TextureFilter = TextureFilter.Linear;
		}

		public void Dispose ()
		{
			if ( indexReference != 0 )
			{
				indexReference--;
				if ( indexReference == 0 )
				{
					vertexDeclaration.Dispose ();
					vertexDeclaration = null;

					indexBuffer.Dispose ();
					indexBuffer = null;

					if ( Effect != null )
					{
						if ( Effect is SpriteEffect )
							Effect.Dispose ();
						Effect = null;
					}
				}
			}

			if ( vertexBuffer != null )
			{
				vertexBuffer.Dispose ();
				vertexBuffer = null;
			}
		}

		public void Draw ( World2 transform )
		{
			Effect.Begin ();
			Effect.SetTextures ( new TextureArgument () { Texture = Texture, Uniform = "texture0", Filter = TextureFilter, AnisotropicLevel = AnisotropicLevel } );
			Effect.SetUniform<Matrix4x4> ( "projectionMatrix", new OrthographicOffCenterProjection (
				0, Core.GraphicsDevice.BackBuffer.Width, Core.GraphicsDevice.BackBuffer.Height, 0,
				0.001f, 1000.0f
			).Matrix );
			Effect.SetUniform<Matrix4x4> ( "worldMatrix", transform.Matrix );
			Core.GraphicsDevice.Draw ( PrimitiveType.TriangleList, vertexBuffer, vertexDeclaration, indexBuffer, 0, 2 );
			Effect.End ();
		}

		public void Reset ()
		{
			Reset ( this.Texture );
		}

		public void Reset ( ITexture2D texture )
		{
			Texture = texture;
			vertexBuffer.SetBufferDatas<SpriteVertex> ( new SpriteVertex []
			{
				new SpriteVertex ( new Vector2 ( 0, 0 ), Color.White, new Vector2 ( 0, 0 ) ),
				new SpriteVertex ( new Vector2 ( Texture.Width, 0 ), Color.White, new Vector2 ( 1, 0 ) ),
				new SpriteVertex ( new Vector2 ( 0, Texture.Height ), Color.White, new Vector2 ( 0, 1 ) ),
				new SpriteVertex ( new Vector2 ( Texture.Width, Texture.Height ), Color.White, new Vector2 ( 1, 1 ) ),
			} );
			clippingArea = new Rectangle ( new Vector2 (), Texture.Size );
		}
	}
}
