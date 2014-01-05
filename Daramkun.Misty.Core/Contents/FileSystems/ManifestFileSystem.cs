using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Daramkun.Misty.Contents.FileSystems
{
	public class ManifestFileSystem : IFileSystem
	{
		Assembly assembly;
		string [] files;
		string assemblyTitle;

		public ManifestFileSystem ()
			: this ( Assembly.GetCallingAssembly () )
		{ }

		public ManifestFileSystem ( Assembly assembly )
		{
			this.assembly = assembly;
			files = assembly.GetManifestResourceNames ();
			assemblyTitle = assembly.FullName.Split ( ',' ) [ 0 ];
		}

		public bool IsFileExist ( string filename )
		{
			foreach ( string resname in assembly.GetManifestResourceNames () )
				if ( resname == filename ) return true;
				else if ( resname == string.Format ( "{0}.{1}", assemblyTitle, filename.Replace ( '\\', '.' ).Replace ( '/', '.' ) ) ) return true;
			return false;
		}

		public Stream OpenFile ( string filename )
		{
			return assembly.GetManifestResourceStream ( string.Format ( "{0}.{1}", assemblyTitle, filename.Replace ( '\\', '.' ).Replace ( '/', '.' ) ) );
		}

		public string [] Files { get { return files; } }
	}
}
