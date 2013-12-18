using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Daramkun.Misty.Common
{
	public struct SpinLock
	{
		private Thread owner;
		private int recursion;

		public void Enter ()
		{
			var caller = Thread.CurrentThread;

			if ( owner == caller )
			{
				Interlocked.Increment ( ref recursion );
				return;
			}

			while ( Interlocked.CompareExchange ( ref owner, caller, null ) != null ) ;
			Interlocked.Increment ( ref recursion );
		}

		public bool TryEnter ()
		{
			var caller = Thread.CurrentThread;

			if ( owner == caller )
			{
				Interlocked.Increment ( ref recursion );
				return true;
			}

			bool success = Interlocked.CompareExchange ( ref owner, caller, null ) == null;
			if ( success )
				Interlocked.Increment ( ref recursion );
			return success;
		}

		public bool TryEnter ( TimeSpan timeout )
		{
			var startTime = DateTime.Now;
			var caller = Thread.CurrentThread;

			if ( owner == caller )
			{
				Interlocked.Increment ( ref recursion );
				return true;
			}

			while ( Interlocked.CompareExchange ( ref owner, caller, null ) != null )
			{
				if ( DateTime.Now - startTime > timeout )
					return false;
			}

			Interlocked.Increment ( ref recursion );
			return true;
		}

		public void Exit ()
		{
			var caller = Thread.CurrentThread;

			if ( caller == owner )
			{
				Interlocked.Decrement ( ref recursion );
				if ( recursion == 0 )
					owner = null;
			}
			else
				throw new InvalidOperationException ( "Exit cannot be called by a thread which does not currently own the lock." );
		}
	}
}
