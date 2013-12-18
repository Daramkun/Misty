using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Log;

namespace Daramkun.Misty.Platform.Log.Writers
{
	public class ConsoleLogWriter : ILogWriter
	{
		public void WriteLog ( string message )
		{
			Console.WriteLine ( message );
		}
	}
}
