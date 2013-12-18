using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Contents.Tables
{
	public interface ITable
	{
		bool Load ( Stream stream );
		bool Save ( Stream stream );
	}
}
