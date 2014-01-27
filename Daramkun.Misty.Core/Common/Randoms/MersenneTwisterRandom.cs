using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Common.Randoms
{
	// Source from http://www.codeproject.com/Articles/5147/A-C-Mersenne-Twister-class
	public class MersenneTwisterRandom : Random
	{
		private const int N = 624;
		private const int M = 397;
		private const uint K = 0x9908B0DFU;
		private const uint DEFAULT_SEED = 4357;

		private uint [] state = new uint [ N + 1 ];
		private int next = 0;
		private uint seedValue = DEFAULT_SEED;

		public MersenneTwisterRandom () { Seed ( DEFAULT_SEED ); }
		public MersenneTwisterRandom ( int seed ) { seedValue = Convert.ToUInt32 ( seed ); Seed ( seedValue ); }

		protected override double Sample ()
		{
			uint y;
			if ( ( next + 1 ) > N )
				return ( Reload () / ( double ) uint.MaxValue );
			y = state [ next++ ];
			y ^= ( y >> 11 );
			y ^= ( y << 7 ) & 0x9D2C5680U;
			y ^= ( y << 15 ) & 0xEFC60000U;
			return ( y ^ ( y >> 18 ) ) / ( double ) uint.MaxValue;
		}

		private void Seed ( uint _seed )
		{
			uint x = ( _seed | 1U ) & 0xFFFFFFFFU;
			for ( int j = N; j >= 0; j-- )
				state [ j ] = ( x *= 69069U ) & 0xFFFFFFFFU;
			next = 0;
		}

		private ulong Reload ()
		{
			uint [] p0 = state, p2 = state, pM = state;
			int p0pos = 0, p2pos = 2, pMpos = M;
			uint s0, s1;
			int j;

			if ( ( next + 1 ) > N ) Seed ( seedValue );

			for ( s0 = state [ 0 ], s1 = state [ 1 ], j = N - M + 1; --j > 0; s0 = s1, s1 = p2 [ p2pos++ ] )
				p0 [ p0pos++ ] = pM [ pMpos++ ] ^ ( mixBits ( s0, s1 ) >> 1 ) ^ ( loBit ( s1 ) != 0 ? K : 0U );

			for ( pM [ 0 ] = state [ 0 ], pMpos = 0, j = M; --j > 0; s0 = s1, s1 = p2 [ p2pos++ ] )
				p0 [ p0pos++ ] = pM [ pMpos++ ] ^ ( mixBits ( s0, s1 ) >> 1 ) ^ ( loBit ( s1 ) != 0 ? K : 0U );

			s1 = state [ 0 ];
			p0 [ p0pos ] = pM [ pMpos ] ^ ( mixBits ( s0, s1 ) >> 1 ) ^ ( loBit ( s1 ) != 0 ? K : 0U );
			s1 ^= ( s1 >> 11 );
			s1 ^= ( s1 << 7 ) & 0x9D2C5680U;
			s1 ^= ( s1 << 15 ) & 0xEFC60000U;
			return ( s1 ^ ( s1 >> 18 ) );
		}

		private uint hiBit ( uint _u ) { return ( ( _u ) & 0x80000000U ); }
		private uint loBit ( uint _u ) { return ( ( _u ) & 0x00000001U ); }
		private uint loBits ( uint _u ) { return ( ( _u ) & 0x7FFFFFFFU ); }
		private uint mixBits ( uint _u, uint _v ) { return ( hiBit ( _u ) | loBits ( _v ) ); }
	}
}
