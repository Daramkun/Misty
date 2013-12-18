using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Contents
{
	public static class FileSystemManager
	{
		static Dictionary<string, Type> fileSystems = new Dictionary<string, Type> ();
		
		public static void AddFileSystemType ( string key, Type fileSystem )
		{
			if ( Utilities.IsSubtypeOf ( fileSystem, typeof ( IFileSystem ) ) )
				fileSystems.Add ( key, fileSystem );
			else throw new ArgumentException ();
		}

		public static IFileSystem CreateFileSystem ( string key, params object [] args )
		{
			return Activator.CreateInstance ( fileSystems [ key ], args ) as IFileSystem;
		}
	}

	public interface IFileSystem
	{
		bool IsFileExist ( string filename );
		Stream OpenFile ( string filename );
		string [] Files { get; }
	}

	public interface IWritableFileSystem : IFileSystem
	{
		void CreateFile ( string filename );
		void DeleteFile ( string filename );
	}

	public interface IDirectorableFileSystem
	{
		bool IsDirectoryExist ( string directoryname );
		string [] Directories { get; }
	}

	public interface IWritableDirectorableFileSystem
	{
		void CreateDirectory ( string directoryname );
		void DeleteDirectory ( string directoryname );
	}
}
