using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Geometries;
using Daramkun.Misty.Mathematics.Transforms;
using System.Runtime.InteropServices;

namespace Daramkun.Misty.Graphics.Spirit
{
	[StructLayout ( LayoutKind.Sequential )]
	struct SpriteVertex
	{
		[VertexElementation ( ElementType.Position )]
		public Vector2 Position;
		[VertexElementation ( ElementType.Diffuse )]
		public Color Diffuse;
		[VertexElementation ( ElementType.TextureCoord )]
		public Vector2 TexCoord;

		public SpriteVertex ( Vector2 pos, Color dif, Vector2 tex )
		{
			Position = pos;
			Diffuse = dif;
			TexCoord = tex;
		}
	}

	public sealed class Sprite
	{
		static IVertexDeclaration vertexDeclaration;
		static int indexReference;
		static SpriteEffect baseSpriteEffect;

		SpriteVertex [] vertices = new SpriteVertex [ 4 ];
		IBuffer vertexBuffer;
		Rectangle clippingArea;
		Color overlayColor = Color.White;
		SamplerState textureArgument;

		World2 innerWorld;
		OrthographicOffCenterProjection projectionMatrix;

		public IEffect Effect { get; set; }
		public ITexture2D Texture { get { return textureArgument.Texture as ITexture2D; } set { textureArgument.Texture = value; } }

		public TextureFilter TextureFilter { get { return textureArgument.Filter; } set { textureArgument.Filter = value; } }
		public TextureAddressing TextureAddressing { get { return textureArgument.Addressing; } set { textureArgument.Addressing = value; } }
		public int AnisotropicLevel { get { return textureArgument.AnisotropicLevel; } set { textureArgument.AnisotropicLevel = value; } }

		public Rectangle ClippingArea
		{
			get { return clippingArea; }
			set
			{
				clippingArea = value;
				vertices [ 0 ].Position = new Vector2 ( 0, 0 );
				vertices [ 1 ].Position = new Vector2 ( 0, clippingArea.Size.Y );
				vertices [ 2 ].Position = new Vector2 ( clippingArea.Size.X, 0 );
				vertices [ 3 ].Position = new Vector2 ( clippingArea.Size.X, clippingArea.Size.Y );
				vertices [ 0 ].TexCoord = new Vector2 ( clippingArea.Position.X / Texture.Width, clippingArea.Position.Y / Texture.Height );
				vertices [ 1 ].TexCoord = new Vector2 ( clippingArea.Position.X / Texture.Width, clippingArea.Size.Y / Texture.Height );
				vertices [ 2 ].TexCoord = new Vector2 ( ( clippingArea.Size.X + clippingArea.Position.X ) / Texture.Width, ( clippingArea.Position.Y + clippingArea.Position.Y ) / Texture.Height );
				vertices [ 3 ].TexCoord = new Vector2 ( ( clippingArea.Size.X + clippingArea.Position.X ) / Texture.Width, ( clippingArea.Size.Y + clippingArea.Position.Y ) / Texture.Height );
				vertexBuffer.SetBufferDatas<SpriteVertex> ( vertices );
			}
		}

		public Color OverlayColor
		{
			get { return overlayColor; }
			set
			{
				overlayColor = value;
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

			if ( projectionMatrix == null )
				projectionMatrix = new OrthographicOffCenterProjection ( 0, 800, 600, 0, 0.001f, 1000.0f );

			Effect = effect;

			if ( vertexDeclaration == null )
			{
				vertexDeclaration = Core.GraphicsDevice.CreateVertexDeclaration ( Utilities.CreateVertexElementArray<SpriteVertex> () );
			}
			indexReference++;

			vertexBuffer = Core.GraphicsDevice.CreateBuffer ( BufferType.VertexBuffer, typeof ( SpriteVertex ), 4 );

			textureArgument = new SamplerState ( texture, Graphics.TextureFilter.Nearest, TextureAddressing.Clamp, 0 );
			Reset ( texture );

			innerWorld = new World2();
		}

		public void Dispose ()
		{
			if ( vertexBuffer != null )
			{
				if ( --indexReference == 0 )
				{
					vertexDeclaration.Dispose ();
					vertexDeclaration = null;

					if ( baseSpriteEffect != null )
					{
						baseSpriteEffect.Dispose ();
						baseSpriteEffect = null;
					}
				}

				vertexBuffer.Dispose ();
				vertexBuffer = null;
			}
		}

		public void Draw ( World2 transform, IGraphicsContext graphicsContext = null )
		{
			if ( graphicsContext == null )
				graphicsContext = Core.GraphicsDevice.ImmediateContext;

			Matrix4x4 matrix;
			Effect.Use ( graphicsContext );
			//Effect.SetTextures ( textureArgument );
			graphicsContext.SetSampler ( 0, textureArgument );
			projectionMatrix.OffCenterSize = graphicsContext.CurrentRenderBuffer.Size ();
			projectionMatrix.GetMatrix ( out matrix );
			Effect.SetUniform<Matrix4x4> ( "projectionMatrix", ref matrix );
			transform.GetMatrix ( out matrix );
			Effect.SetUniform<Matrix4x4> ( "worldMatrix", ref matrix );
			graphicsContext.InputAssembler = new InputAssembler ( vertexBuffer, vertexDeclaration, PrimitiveType.TriangleStrip );
			graphicsContext.Draw ( 0, 2 );
		}

		public void Draw ( ref Vector2 position, ref Vector2 scale, ref Vector2 scaleCenter, float rotation, ref Vector2 rotationCenter, IGraphicsContext graphicsContext = null )
		{
			innerWorld.Translate = position;
			innerWorld.Scale = scale;
			innerWorld.ScaleCenter = scaleCenter;
			innerWorld.Rotation = rotation;
			innerWorld.RotationCenter = rotationCenter;
			Draw ( innerWorld, graphicsContext );
		}

		public void Draw ( Vector2 position, Vector2 scale, Vector2 scaleCenter, float rotation, Vector2 rotationCenter, IGraphicsContext graphicsContext = null )
		{
			Draw ( ref position, ref scale, ref scaleCenter, rotation, ref rotationCenter, graphicsContext );
		}

		public void Reset ()
		{
			Reset ( this.Texture );
		}

		public void Reset ( ITexture2D texture )
		{
			Texture = texture;

			float plusUnit = 0.000001f;
			float width = plusUnit, height = plusUnit;
			if ( texture != null ) { width += texture.Width; height += texture.Height; }
			else { width += 1; height += 1; }

			vertices [ 0 ] = new SpriteVertex ( new Vector2 ( plusUnit, plusUnit ), Color.White, new Vector2 ( 0.0001f, 0.0001f ) );
			vertices [ 1 ] = new SpriteVertex ( new Vector2 ( plusUnit, height ), Color.White, new Vector2 ( 0.0001f, 1 ) );
			vertices [ 2 ] = new SpriteVertex ( new Vector2 ( width, plusUnit ), Color.White, new Vector2 ( 1f, 0.0001f ) );
			vertices [ 3 ] = new SpriteVertex ( new Vector2 ( width, height ), Color.White, new Vector2 ( 1f, 1f ) );

			vertexBuffer.SetBufferDatas<SpriteVertex> ( vertices );
			clippingArea = new Rectangle ( new Vector2 (), new Vector2 ( width, height ) );
		}
	}
}
