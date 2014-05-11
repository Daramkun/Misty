using System;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	public class PassedEffect : StandardDispose, IEffect
	{
		private IEffect [] effects;

		public object Handle { get { return effects; } }

		public IShader VertexShader { get { throw new NotImplementedException (); } }
		public IShader PixelShader { get { throw new NotImplementedException (); } }
		public IShader GeometryShader { get { throw new NotImplementedException (); } }

		public uint Pass { get; set; }
		public int Count { get { return effects.Length; } }
		public IEffect CurrentEffect { get { return effects [ Pass ]; } }

		public void Use ( IGraphicsContext graphicsContext ) { effects [ Pass ].Use ( graphicsContext ); }

		public void SetUniform<T> ( string name, T value ) where T : struct
		{
			effects [ Pass ].SetUniform<T> ( name, value );
		}

		public void SetUniform<T> ( string name, ref T value ) where T : struct
		{
			effects [ Pass ].SetUniform<T> ( name, ref value );
		}

		public void SetUniform ( string name, params int[] value )
		{
			effects [ Pass ].SetUniform ( name, value );
		}

		public void SetUniform ( string name, params float[] value )
		{
			effects [ Pass ].SetUniform ( name, value );
		}

		public void SetTextures ( params TextureArgument[] args )
		{
			effects [ Pass ].SetTextures ( args );
		}

		public PassedEffect ( params IEffect [] args )
		{
			effects = args.Clone () as IEffect [];
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				foreach ( IEffect effect in effects )
					effect.Dispose ();
			}
		}
	}
}

