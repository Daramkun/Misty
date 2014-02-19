using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Mathematics.Transforms;

namespace Daramkun.Misty.Graphics.Spirit
{
	public abstract class Font : StandardDispose, IDisposable
	{
		public string FontFamily { get; protected set; }
		public float FontSize { get; protected set; }

		public bool IsPrerenderMode { get; set; }

		public int SpacingOfChars { get; set; }
		public int SpacingOfLines { get; set; }

		Sprite spriteEngine;
		World2 fontWorld;

		Dictionary<string, IRenderBuffer> cachedRenderBuffer;

		public IEffect Effect { get { return spriteEngine.Effect; } set { spriteEngine.Effect = value; } }

		public Font ()
		{
			spriteEngine = new Sprite ( null );
			fontWorld = new World2 ();

			SpacingOfChars = 1;
			SpacingOfLines = 2;

			IsPrerenderMode = false;
			cachedRenderBuffer = new Dictionary<string, IRenderBuffer> ();
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				foreach ( IRenderBuffer buffer in cachedRenderBuffer.Values )
					buffer.Dispose ();
				cachedRenderBuffer = null;

				if ( spriteEngine != null )
				{
					spriteEngine.Dispose ();
					spriteEngine = null;
				}
			}
			base.Dispose ( isDisposing );
		}

		protected abstract ITexture2D this [ char ch ] { get; }

		public void DrawFont ( string text, Color color, Vector2 position, int startIndex = 0, int length = -1 )
		{
			DrawFont ( text, color, position, MeasureString ( text ), startIndex, length );
		}

		public void DrawFont ( string text, Color color, Vector2 position, Vector2 area, int startIndex = 0, int length = -1 )
		{
			if ( text == null ) return;
			if ( length == -1 ) length = text.Length;
			
			if ( IsPrerenderMode && startIndex != 0 || length != text.Length )
			{
				text = text.Substring ( startIndex, length );
				startIndex = 0;
				length = text.Length;
			}

			if ( IsPrerenderMode && cachedRenderBuffer.ContainsKey ( text ) )
			{
				spriteEngine.Reset ( cachedRenderBuffer [ text ] );
				spriteEngine.OverlayColor = color;
				fontWorld.Translate = position;
				spriteEngine.Draw ( fontWorld );
			}
			else
			{
				Vector2 renderPos = ( IsPrerenderMode ) ? new Vector2 () : position;

				List<Vector2> lines = new List<Vector2> ();
				int i = 0;
				float height = 0;

				IRenderBuffer renderBuffer = null, lastRenderBuffer = null;

				if ( IsPrerenderMode )
				{
					renderBuffer = Core.GraphicsDevice.CreateRenderBuffer ( ( int ) area.X, ( int ) area.Y );
					lastRenderBuffer = Core.GraphicsDevice.CurrentRenderBuffer;

					Core.GraphicsDevice.EndScene ();
					Core.GraphicsDevice.BeginScene ( renderBuffer );
					Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, new Color ( 1.0f, 1, 1, 0 ) );
				}

				for ( i = startIndex; i < startIndex + length; i++ )
				{
					char ch = text [ i ];
					if ( ch == '\n' )
					{
						lines.Add ( new Vector2 () );
						continue;
					}
					ITexture2D image = this [ ch ];
					if ( image == null ) image = this [ '?' ];
					if ( image == null ) image = this [ ' ' ];

					if ( lines.Count == 0 || lines [ lines.Count - 1 ].X + image.Width > area.X )
						if ( height + image.Height > area.Y ) return;
						else lines.Add ( new Vector2 () );

					lines [ lines.Count - 1 ] += new Vector2 ( image.Width + SpacingOfChars, 0 );
					if ( lines [ lines.Count - 1 ].Y == 0 )
					{
						lines [ lines.Count - 1 ] += new Vector2 ( 0, image.Height + SpacingOfLines );
						height += image.Height;
					}

					spriteEngine.Reset ( image );
					if ( !IsPrerenderMode )
						spriteEngine.OverlayColor = color;
					else spriteEngine.OverlayColor = Color.White;
					fontWorld.Translate = renderPos + new Vector2 ( lines [ lines.Count - 1 ].X - image.Width, height - image.Height );
					spriteEngine.Draw ( fontWorld );
				}

				if ( IsPrerenderMode )
				{
					Core.GraphicsDevice.EndScene ();
					Core.GraphicsDevice.BeginScene ( lastRenderBuffer );
					cachedRenderBuffer.Add ( text, renderBuffer );
					DrawFont ( text, color, position, area, startIndex, length );
				}
			}
		}

		public Vector2 MeasureString ( string text, int startIndex = 0, int length = -1 )
		{
			if ( text == null ) return new Vector2 ();
			if ( length == -1 ) length = text.Length;

			List<Vector2> lines = new List<Vector2> ();

			for ( int i = startIndex; i < startIndex + length; ++i )
			{
				char ch = text [ i ];
				if ( ch == '\n' )
				{
					lines.Add ( new Vector2 () );
					continue;
				}
				ITexture2D image = this [ ch ];
				if ( image == null ) image = this [ '?' ];

				if ( lines.Count == 0 )
					lines.Add ( new Vector2 () );

				lines [ lines.Count - 1 ] += new Vector2 ( image.Width + SpacingOfChars, 0 );
				if ( lines [ lines.Count - 1 ].Y == 0 )
					lines [ lines.Count - 1 ] += new Vector2 ( 0, image.Height + SpacingOfLines );
			}

			Vector2 measure = new Vector2 ( 0, 0 );
			foreach ( Vector2 v in lines )
			{
				measure.X = ( float ) System.Math.Max ( measure.X, v.X );
				measure.Y += lines [ lines.Count - 1 ].Y;
			}

			return measure;
		}

		public int MeasureString ( string text, Vector2 area, int startIndex = 0, int length = -1 )
		{
			if ( text == null ) return 0;
			if ( length == -1 ) length = text.Length;

			List<Vector2> lines = new List<Vector2> ();
			int i = 0;
			float height = 0;
			for ( i = startIndex; i < startIndex + length; i++ )
			{
				char ch = text [ i ];
				if ( ch == '\n' )
				{
					lines.Add ( new Vector2 () );
					continue;
				}
				ITexture2D image = this [ ch ];
				if ( image == null ) image = this [ '?' ];

				if ( lines.Count == 0 || lines [ lines.Count - 1 ].X + image.Width > area.X )
					if ( height + image.Height + SpacingOfLines > area.Y ) return i;
					else lines.Add ( new Vector2 () );

				lines [ lines.Count - 1 ] += new Vector2 ( image.Width + SpacingOfChars, 0 );
				if ( lines [ lines.Count - 1 ].Y == 0 )
				{
					lines [ lines.Count - 1 ] += new Vector2 ( 0, image.Height + SpacingOfLines );
					height += image.Height + SpacingOfLines;
				}
			}

			return i;
		}
	}
}
