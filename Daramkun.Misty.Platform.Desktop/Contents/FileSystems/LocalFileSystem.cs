using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Daramkun.Misty.Contents.FileSystems
{
	public class LocalFileSystem : IFileSystem, IWritableFileSystem, IDirectorableFileSystem, IWritableDirectorableFileSystem
	{
		string basePath = "";

		public string BasePath { get { return basePath; } }

		public LocalFileSystem () : this ( Environment.CurrentDirectory ) { }
		public LocalFileSystem ( string path )
		{
			basePath = path;
			if ( basePath [ basePath.Length - 1 ] != Path.AltDirectorySeparatorChar )
				basePath += Path.AltDirectorySeparatorChar;
		}

		public bool IsFileExist ( string filename ) { return File.Exists ( basePath + filename ); }
		public Stream OpenFile ( string filename ) { return File.Open ( basePath + filename, FileMode.Open ); }
		
		public string [] Files { get { return Directory.GetFiles ( basePath, "*", SearchOption.AllDirectories ); } }

		public void CreateFile ( string filename ) { File.Create ( basePath + filename ).Dispose (); }
		public void DeleteFile ( string filename ) { File.Delete ( basePath + filename ); }

		public bool IsDirectoryExist ( string directoryname ) { return Directory.Exists ( basePath + directoryname ); }

		public string [] Directories { get { return Directory.GetDirectories ( basePath, "*", SearchOption.AllDirectories ); } }

		public void CreateDirectory ( string directoryname ) { Directory.CreateDirectory ( basePath + directoryname ); }
		public void DeleteDirectory ( string directoryname ) { Directory.Delete ( basePath + directoryname, true ); }
	}
}
