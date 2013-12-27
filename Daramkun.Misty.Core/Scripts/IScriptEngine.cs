using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Daramkun.Misty.Scripts
{
	public interface IScriptEngine : IDisposable
	{
		string ScriptType { get; }
		Version ScriptVersion { get; }

		object Handle { get; }
		bool IsSupportDLR { get; }

		void AddAssembly ( Assembly assembly );
		object this [ string varname ] { get; set; }

		object Run ( Stream stream );
		object Run ( string script );
		T Run<T> ( Stream stream );
		T Run<T> ( string script );
	}
}
