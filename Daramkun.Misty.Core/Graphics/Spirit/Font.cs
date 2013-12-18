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

		Sprite spriteEngine;
		World2 fontWorld;

		public IEffect Effect { get { return spriteEngine.Effect; } set { spriteEngine.Effect = value; } }

		public Font ()
		{
			spriteEngine = new Sprite ( null );
			fontWorld = new World2 ();
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				if ( spriteEngine != null )
				{
					spriteEngine.Dispose ();
					spriteEngine = null;
				}
			}
			base.Dispose ( isDisposing );
		}

		protected abstract ITexture2D this [ char ch ] { get; }

		public void DrawFont ( string text, Color color, Vector2 position )
		{
			DrawFont ( text, color, position, MeasureString ( text ) );
		}

		public void DrawFont ( string text, Color color, Vector2 position, Vector2 area )
		{
			if ( text == null ) return;

			List<Vector2> lines = new List<Vector2> ();
			int i = 0;
			float height = 0;
			for ( i = 0; i < text.Length; i++ )
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
					if ( height + image.Height > area.Y ) return;
					else lines.Add ( new Vector2 () );

				lines [ lines.Count - 1 ] += new Vector2 ( image.Width, 0 );
				if ( lines [ lines.Count - 1 ].Y == 0 )
				{
					lines [ lines.Count - 1 ] += new Vector2 ( 0, image.Height );
					height += image.Height;
				}

				spriteEngine.OverlayColor = color;
				spriteEngine.Reset ( image );

				fontWorld.Translate = position + new Vector2 ( lines [ lines.Count - 1 ].X - image.Width, height - image.Height );
				spriteEngine.Draw ( fontWorld );
			}
		}

		public Vector2 MeasureString ( string text )
		{
			if ( text == null ) return new Vector2 ();

			List<Vector2> lines = new List<Vector2> ();

			foreach ( char ch in text )
			{
				if ( ch == '\n' )
				{
					lines.Add ( new Vector2 () );
					continue;
				}
				ITexture2D image = this [ ch ];
				if ( image == null ) image = this [ '?' ];

				if ( lines.Count == 0 )
					lines.Add ( new Vector2 () );

				lines [ lines.Count - 1 ] += new Vector2 ( image.Width, 0 );
				if ( lines [ lines.Count - 1 ].Y == 0 )
					lines [ lines.Count - 1 ] += new Vector2 ( 0, image.Height );
			}

			Vector2 measure = new Vector2 ( 0, 0 );
			foreach ( Vector2 v in lines )
			{
				measure.X = ( float ) System.Math.Max ( measure.X, v.X );
				measure.Y += lines [ lines.Count - 1 ].Y;
			}

			return measure;
		}

		public int MeasureString ( string text, Vector2 area )
		{
			if ( text == null ) return 0;

			List<Vector2> lines = new List<Vector2> ();
			int i = 0;
			float height = 0;
			for ( i = 0; i < text.Length; i++ )
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
					if ( height + image.Height > area.Y ) return i;
					else lines.Add ( new Vector2 () );

				lines [ lines.Count - 1 ] += new Vector2 ( image.Width, 0 );
				if ( lines [ lines.Count - 1 ].Y == 0 )
				{
					lines [ lines.Count - 1 ] += new Vector2 ( 0, image.Height );
					height += image.Height;
				}
			}

			return i;
		}
	}
}
