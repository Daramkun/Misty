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
			foreach ( string resname in files )
				if ( resname == filename ) return true;
				else if ( resname == string.Format ( "{0}.{1}", assemblyTitle, filename.Replace ( '\\', '.' ).Replace ( '/', '.' ) ) ) return true;
				else
				{
					StringBuilder sb = new StringBuilder ( filename.Replace ( '\\', '.' ).Replace ( '/', '.' ) );
					while ( sb.ToString ().IndexOf ( '.' ) != -1 )
					{
						sb.Remove ( 0, sb.ToString ().IndexOf ( '.' ) + 1 );
						if ( resname == string.Format ( "{0}.{1}", assemblyTitle, sb.ToString () ) )
							return true;
					}
				}
			return false;
		}

		public Stream OpenFile ( string filename )
		{
			if ( files.Contains ( filename ) ) return assembly.GetManifestResourceStream ( filename );
			else if ( files.Contains ( string.Format ( "{0}.{1}", assemblyTitle, filename.Replace ( '\\', '.' ).Replace ( '/', '.' ) ) ) )
				return assembly.GetManifestResourceStream ( string.Format ( "{0}.{1}", assemblyTitle, filename.Replace ( '\\', '.' ).Replace ( '/', '.' ) ) );
			else
			{
				StringBuilder sb = new StringBuilder ( filename.Replace ( '\\', '.' ).Replace ( '/', '.' ) );
				while ( sb.ToString ().IndexOf ( '.' ) != -1 )
				{
					sb.Remove ( 0, sb.ToString ().IndexOf ( '.' ) + 1 );
					if ( files.Contains ( string.Format ( "{0}.{1}", assemblyTitle, sb.ToString () ) ) )
						return assembly.GetManifestResourceStream ( string.Format ( "{0}.{1}", assemblyTitle, sb.ToString () ) );
				}
			}
			return null;
		}

		public string [] Files { get { return files; } }
	}
}
