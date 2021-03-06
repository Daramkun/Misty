﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Graphics.Spirit
{
	public class SpriteEffect : StandardDispose, IEffect
	{
		IEffect baseEffect;

		public object Handle { get { return baseEffect; } }
		public IShader VertexShader { get { return baseEffect.VertexShader; } }
		public IShader PixelShader { get { return baseEffect.PixelShader; } }
		public IShader GeometryShader { get { return baseEffect.GeometryShader; } }

		public SpriteEffect ()
		{
			baseEffect = Core.GraphicsDevice.CreateEffect (
				Assembly.GetExecutingAssembly ().GetManifestResourceStream ( "Daramkun.Misty.Resources.Spirit.SpriteEffect.xml" )
			);
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				baseEffect.Dispose ();
			}
			base.Dispose ( isDisposing );
		}

		public void Use ( IGraphicsContext graphicsContext )
		{
			baseEffect.Use ( graphicsContext );
		}

		public void SetUniform<T> ( string name, T value ) where T : struct
		{
			baseEffect.SetUniform<T> ( name, value );
		}

		public void SetUniform<T> ( string name, ref T value ) where T : struct
		{
			baseEffect.SetUniform<T> ( name, ref value );
		}

		public void SetUniform ( string name, params int [] value )
		{
			baseEffect.SetUniform ( name, value );
		}

		public void SetUniform ( string name, params float [] value )
		{
			baseEffect.SetUniform ( name, value );
		}
	}
}
