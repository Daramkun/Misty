﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Blockar.Common;

namespace Daramkun.Misty.Graphics
{
	public struct Color
	{
		[Record ( Name = "r" )]
		public float RedScalar { get; set; }
		[Record ( Name = "g" )]
		public float GreenScalar { get; set; }
		[Record ( Name = "b" )]
		public float BlueScalar { get; set; }
		[Record ( Name = "a" )]
		public float AlphaScalar { get; set; }

		public byte AlphaValue { get { return ( byte ) ( AlphaScalar * 255 ); } set { AlphaScalar = value / 255.0f; } }
		public byte RedValue { get { return ( byte ) ( RedScalar * 255 ); } set { RedScalar = value / 255.0f; } }
		public byte GreenValue { get { return ( byte ) ( GreenScalar * 255 ); } set { GreenScalar = value / 255.0f; } }
		public byte BlueValue { get { return ( byte ) ( BlueScalar * 255 ); } set { BlueScalar = value / 255.0f; } }
		
		public int ColorValue { get { return ( ( ( int ) RedValue ) << 24 ) + ( ( ( int ) GreenValue ) << 16 ) + ( ( ( int ) BlueValue ) << 8 ) + AlphaValue; } }
		public int ARGBValue { get { return ( ( ( int ) AlphaValue ) << 24 ) + ( ( ( int ) RedValue ) << 16 ) + ( ( ( int ) GreenValue ) << 8 ) + BlueValue; } }

		public Color ( byte red, byte green, byte blue ) : this ( red, green, blue, 255 ) { }
		public Color ( float red, float green, float blue ) : this ( red, green, blue, 1 ) { }

		public Color ( Color sourceColor, byte alpha )
			: this ( sourceColor.RedScalar, sourceColor.GreenScalar, sourceColor.BlueScalar, alpha / 255.0f )
		{ }

		public Color ( Color sourceColor, float alpha )
			: this ( sourceColor.RedScalar, sourceColor.GreenScalar, sourceColor.BlueScalar, alpha )
		{ }

		public Color ( float red, float green, float blue, float alpha )
			: this ()
		{
			RedScalar = red;
			GreenScalar = green;
			BlueScalar = blue;
			AlphaScalar = alpha;
		}

		public Color ( byte red, byte green, byte blue, byte alpha )
			: this ()
		{
			RedValue = red;
			GreenValue = green;
			BlueValue = blue;
			AlphaValue = alpha;
		}

		public Color ( int argbColorValue, bool isArgb = false )
			: this ()
		{
			RedValue = ( byte ) ( ( argbColorValue >> ( isArgb ? 0 : 8 ) ) & 0xff );
			GreenValue = ( byte ) ( ( argbColorValue >> ( isArgb ? 8 : 16 ) ) & 0xff );
			BlueValue = ( byte ) ( ( argbColorValue >> ( isArgb ? 16 : 24 ) ) & 0xff );
			AlphaValue = ( byte ) ( ( argbColorValue >> ( isArgb ? 24 : 0 ) ) & 0xff );
		}

		public override int GetHashCode ()
		{
			return ColorValue;
		}

		public override bool Equals ( object obj )
		{
			if ( !( obj is Color ) ) return false;
			Color color = ( Color ) obj;
			return AlphaValue == color.AlphaValue && RedValue == color.RedValue &&
				GreenValue == color.GreenValue && BlueValue == color.BlueValue;
		}

		public static bool operator == ( Color v1, Color v2 )
		{
			return v1.Equals ( v2 );
		}

		public static bool operator != ( Color v1, Color v2 )
		{
			return !v1.Equals ( v2 );
		}

		public override string ToString ()
		{
			return String.Format ( "Red:{0}, Green:{1}, Blue:{2}, Alpha:{3}",
				RedValue, GreenValue, BlueValue, AlphaValue );
		}

		public static Color White { get { return new Color ( 1.0f, 1.0f, 1.0f ); } }
		public static Color Black { get { return new Color ( 0, 0, 0 ); } }

		public static Color Red { get { return new Color ( 1.0f, 0, 0 ); } }
		public static Color Green { get { return new Color ( 0, 1.0f, 0 ); } }
		public static Color Blue { get { return new Color ( 0, 0, 1.0f ); } }

		public static Color Cyan { get { return new Color ( 0, 1.0f, 1.0f ); } }
		public static Color Magenta { get { return new Color ( 1.0f, 0, 1.0f ); } }
		public static Color Yellow { get { return new Color ( 1.0f, 1.0f, 0 ); } }

		public static Color Transparent { get { return new Color ( 0, 0, 0, 0 ); } }
	}
}
