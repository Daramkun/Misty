using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Log.Writers
{
	public class StreamLogWriter : ILogWriter
	{
		public Stream BaseStream { get; private set; }
		public Encoding StringEncoding { get; set; }

		public StreamLogWriter ( Stream stream )
		{
			BaseStream = stream;
			StringEncoding = Encoding.UTF8;
		}

		public void WriteLog ( string message )
		{
			if ( BaseStream != null )
			{
				byte [] messageData = StringEncoding.GetBytes ( message + Environment.NewLine );
				BaseStream.Write ( messageData, 0, messageData.Length );
			}
		}
	}
}
