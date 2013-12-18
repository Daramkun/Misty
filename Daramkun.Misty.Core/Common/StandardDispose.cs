using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Common
{
	public class StandardDispose : IDisposable
	{
		~StandardDispose () { Dispose ( false ); }

		protected virtual void Dispose ( bool isDisposing ) { }

		public void Dispose ()
		{
			Dispose ( true );
			GC.SuppressFinalize ( this );
		}
	}
}
