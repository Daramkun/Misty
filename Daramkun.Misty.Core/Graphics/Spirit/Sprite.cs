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
	public sealed class Sprite
	{
		[StructLayout ( LayoutKind.Sequential )]
		private struct SpriteVertex
		{
			[VertexElementation ( Graphics.ElementType.Position )]
			public Vector2 Position;
			[VertexElementation ( Graphics.ElementType.Diffuse )]
			public Color Diffuse;
			[VertexElementation ( Graphics.ElementType.TextureCoord )]
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

		public static bool IsStripDrawingMode { get; set; }

		static Sprite () { IsStripDrawingMode = true; }

		OrthographicOffCenterProjection projectionMatrix = new OrthographicOffCenterProjection ( 0, 800, 600, 0, 0.001f, 1000.0f );

		SpriteVertex [] vertices = new SpriteVertex [ 4 ];
		IVertexBuffer vertexBuffer;
		Rectangle clippingArea;
		Color overlayColor;
		TextureArgument textureArgument;

		public IEffect Effect { get; set; }
		public ITexture2D Texture { get { return textureArgument.Texture as ITexture2D; } set { textureArgument.Texture = value; } }

		public TextureFilter TextureFilter { get { return textureArgument.Filter; } set { textureArgument.Filter = value; } }
		public int AnisotropicLevel { get { return textureArgument.AnisotropicLevel; } set { textureArgument.AnisotropicLevel = value; } }

		public Rectangle ClippingArea
		{
			get { return clippingArea; }
			set
			{
				clippingArea = value;
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

			Effect = effect;

			if ( indexBuffer == null )
			{
				indexBuffer = Core.GraphicsDevice.CreateIndexBuffer ( new int [] { 0, 1, 2, 1, 3, 2 } );
				vertexDeclaration = Core.GraphicsDevice.CreateVertexDeclaration ( Utilities.CreateVertexElementArray<SpriteVertex> () );
			}
			indexReference++;

			vertexBuffer = Core.GraphicsDevice.CreateVertexBuffer ( typeof ( SpriteVertex ), 4 );

			textureArgument = new TextureArgument ( "texture0", texture, Graphics.TextureFilter.Nearest, TextureAddressing.Clamp, 0 );
			Reset ( texture );
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
			Matrix4x4 matrix;
			Effect.Begin ();
			Effect.SetTextures ( textureArgument );
			projectionMatrix.OffCenterSize = ( Core.GraphicsDevice.CurrentRenderBuffer != null ) ? Core.GraphicsDevice.CurrentRenderBuffer.Size :
				Core.GraphicsDevice.BackBuffer.Size;
			projectionMatrix.GetMatrix ( out matrix );
			Effect.SetUniform<Matrix4x4> ( "projectionMatrix", ref matrix );
			transform.GetMatrix ( out matrix );
			Effect.SetUniform<Matrix4x4> ( "worldMatrix", ref matrix );
			bool isZWriteMode = Core.GraphicsDevice.IsZWriteEnable, isStencilEnable = Core.GraphicsDevice.StencilState;
			Core.GraphicsDevice.IsZWriteEnable = false;
			Core.GraphicsDevice.StencilState = false;
			if(IsStripDrawingMode) Core.GraphicsDevice.Draw ( PrimitiveType.TriangleStrip, vertexBuffer, vertexDeclaration, 0, 2 );
			else Core.GraphicsDevice.Draw ( PrimitiveType.TriangleList, vertexBuffer, vertexDeclaration, indexBuffer, 0, 2 );
			Core.GraphicsDevice.IsZWriteEnable = isZWriteMode;
			Core.GraphicsDevice.StencilState = isStencilEnable;
			Effect.End ();
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
			vertices [ 1 ] = new SpriteVertex ( new Vector2 ( width, plusUnit ), Color.White, new Vector2 ( 1f, 0.0001f ) );
			vertices [ 2 ] = new SpriteVertex ( new Vector2 ( plusUnit, height ), Color.White, new Vector2 ( 0.0001f, 1 ) );
			vertices [ 3 ] = new SpriteVertex ( new Vector2 ( width, height ), Color.White, new Vector2 ( 1f, 1f ) );

			vertexBuffer.SetBufferDatas<SpriteVertex> ( vertices );
			clippingArea = new Rectangle ( new Vector2 (), new Vector2 ( width, height ) );
		}
	}
}
