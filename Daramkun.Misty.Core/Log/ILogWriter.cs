using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Log
{
	public interface ILogWriter
	{
		void WriteLog ( string message );
	}
}
