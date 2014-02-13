using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Log.Writers
{
	public class DebugLogWriter : ILogWriter
	{
		[Conditional ( "DEBUG" )]
		public void WriteLog ( string message ) { Debug.WriteLine ( message ); }
	}
}
