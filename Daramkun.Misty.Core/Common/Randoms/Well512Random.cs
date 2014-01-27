using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Common.Randoms
{
	// Source from http://www.gamedevforever.com/114
	public class Well512Random : Random
	{
		uint [] state;
		uint index;

		public Well512Random () { state = new uint [ 16 ] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }; index = 0; }
		public Well512Random ( int seed ) : this () { index = Convert.ToUInt32 ( seed ) % 16; }
		public Well512Random ( uint [] states, int seed ) { state = states.Clone () as uint []; index = Convert.ToUInt32 ( seed ) % 16; }

		protected override double Sample ()
		{
			uint a, b, c, d;
			a = state [ index ];
			c = state [ ( index + 13 ) & 15 ];
			b = a ^ c ^ ( a << 16 ) ^ ( c << 15 );
			c = state [ ( index + 9 ) & 15 ];
			c ^= ( c >> 11 );
			a = state [ index ] = b ^ c;
			d = a ^ ( ( a << 5 ) & 0xDA442D20 );
			index = ( index + 15 ) & 15;
			a = state [ index ];
			state [ index ] = a ^ b ^ d ^ ( a << 2 ) ^ ( b << 18 ) ^ ( c << 28 );
			return ( double ) state [ index ] / ( double ) uint.MaxValue;
		}
	}
}
