using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Contents.Tables
{
	public sealed class ResourceTable : ITable, ISubCulture<IFileSystem>, IDisposable
	{
		public static List<Assembly> ContentLoaderAssemblies { get; private set; }

		List<IContentReader> contentLoaders = new List<IContentReader> ();
		Dictionary<string, object> loadedContent = new Dictionary<string, object> ();

		public IFileSystem FileSystem { get; set; }
		Dictionary<CultureInfo, IFileSystem> localeFileSystems;

		static ResourceTable ()
		{
			ContentLoaderAssemblies = new List<Assembly> ();
			ContentLoaderAssemblies.Add ( Assembly.GetExecutingAssembly () );
		}

		public ResourceTable ()
			: this ( null, false )
		{ }

		public ResourceTable ( IFileSystem fileSystem, bool addDefaultLoaders = true )
		{
			FileSystem = fileSystem;
			if ( addDefaultLoaders ) AddDefaultContentLoader ();
			localeFileSystems = new Dictionary<CultureInfo, IFileSystem> ();
		}

		public void AddContentLoader ( IContentReader contentLoader )
		{
			if ( contentLoaders.Contains ( contentLoader ) ) return;
			contentLoaders.Add ( contentLoader );
		}

		public void RemoveContentLoader ( IContentReader contentLoader )
		{
			contentLoaders.Remove ( contentLoader );
		}

		public void AddDefaultContentLoader ()
		{
			foreach ( Assembly assembly in ContentLoaderAssemblies )
			{
				foreach ( Type type in assembly.GetTypes () )
				{
					if ( Utilities.IsSubtypeOf ( type, typeof ( IContentReader ) ) && type != typeof ( IContentReader )
						&& !type.IsAbstract && !type.IsInterface && type.IsPublic )
						AddContentLoader ( Activator.CreateInstance ( type ) as IContentReader );
				}
			}
		}

		public void AddContent ( string key, object obj )
		{
			loadedContent.Add ( key, obj );
		}

		public void RemoveContent ( string key )
		{
			if ( !loadedContent.ContainsKey ( key ) ) return;
			if ( loadedContent [ key ] is IDisposable )
				( loadedContent [ key ] as IDisposable ).Dispose ();
			loadedContent.Remove ( key );
		}

		private string PathCombine ( string path, string filename )
		{
			if ( path == null || path.Length == 0 ) return filename;

			if ( path.IndexOf ( '\\' ) >= 0 )
			{
				if ( path [ path.Length - 1 ] == '\\' )
					return path + filename;
				else return string.Format ( "{0}\\{1}", path, filename );
			}
			else
			{
				if ( path [ path.Length - 1 ] == '/' )
					return path + filename;
				else return string.Format ( "{0}/{1}", path, filename );
			}
		}

		public T Load<T> ( string filename, params object [] args )
		{
			string temp;
			return Load<T> ( filename, out temp, args );
		}

		public T Load<T> ( string filename, out string key, params object [] args )
		{
			if ( FileSystem == null ) throw new ArgumentNullException ();

			Type type = typeof ( T );
			IContentReader loader = null;
			foreach ( IContentReader contentLoader in contentLoaders )
				if ( Utilities.IsSubtypeOf ( type, contentLoader.ContentType ) )
					loader = contentLoader;

			if ( loader == null )
			{
				if ( FileSystem.IsFileExist ( filename ) )
					return ( T ) loadedContent [ key = filename ];
				throw new ArgumentException ();
			}

			if ( !FileSystem.IsFileExist ( filename ) )
			{
				bool exist = false;

				if ( FileSystem.IsFileExist ( key = PathCombine ( Core.CurrentCulture.Name, filename ) ) ) exist = true;
				else if ( localeFileSystems.ContainsKey ( Core.CurrentCulture ) && localeFileSystems [ Core.CurrentCulture ].IsFileExist ( key = filename ) ) exist = true;
				else if ( FileSystem.IsFileExist ( key = PathCombine ( "unknown", filename ) ) ) exist = true;
				else
				{
					foreach ( string ext in loader.FileExtensions )
					{
						if ( exist = FileSystem.IsFileExist ( key = PathCombine ( Core.CurrentCulture.Name, string.Format ( "{0}.{1}", filename, ext.ToLower () ) ) ) ) break;
						else if ( localeFileSystems.ContainsKey ( Core.CurrentCulture ) &&
							( exist = localeFileSystems [ Core.CurrentCulture ].IsFileExist ( key = string.Format ( "{0}.{1}", filename, ext.ToLower () ) ) ) ) break;
						else if ( exist = FileSystem.IsFileExist ( key = PathCombine ( "unknown", string.Format ( "{0}.{1}", filename, ext.ToLower () ) ) ) ) break;
						else if ( exist = FileSystem.IsFileExist ( key = string.Format ( "{0}.{1}", filename, ext.ToLower () ) ) ) break;
					}
				}

				if ( !exist ) { key = null; throw new FileNotFoundException (); }
			}
			else key = filename;

			filename = key;
			key = MakeKey ( filename, type, args );

			if ( loadedContent.ContainsKey ( key ) ) return ( T ) loadedContent [ key ];
			else
			{
				Stream stream = FileSystem.OpenFile ( filename );
				object data = loader.Read ( stream, this, args );
				loadedContent.Add ( key, data );
				if ( !loader.AutoStreamDispose )
					stream.Dispose ();
				return ( T ) data;
			}
		}

		private string MakeKey ( string filename, Type type, params object [] args )
		{
			StringBuilder sb = new StringBuilder ( filename );
			foreach ( object o in args )
				sb.AppendFormat ( ".{0}", o );
			return sb.Append ( type ).ToString ();
		}

		public void Reset ()
		{
			foreach ( KeyValuePair<string, object> obj in loadedContent )
				if ( obj.Value is IDisposable )
					( obj.Value as IDisposable ).Dispose ();
			loadedContent.Clear ();
		}

		public void Reset<T> ()
		{
			List<string> removed = new List<string> ();
			foreach ( KeyValuePair<string, object> obj in loadedContent )
				if ( obj.Value is T )
				{
					if ( obj.Value is IDisposable )
						( obj.Value as IDisposable ).Dispose ();
					removed.Add ( obj.Key );
				}

			foreach ( string s in removed )
				loadedContent.Remove ( s );
		}

		public void Dispose ()
		{
			Reset ();
			loadedContent = null;
			contentLoaders.Clear ();
			contentLoaders = null;
		}

		public void AddCulture ( CultureInfo cultureInfo, IFileSystem fileSystem )
		{
			if ( localeFileSystems.ContainsKey ( cultureInfo ) )
				localeFileSystems [ cultureInfo ] = fileSystem;
			else localeFileSystems.Add ( cultureInfo, fileSystem );
		}

		public void RemoveCulture ( CultureInfo cultureInfo )
		{
			if ( localeFileSystems.ContainsKey ( cultureInfo ) )
				localeFileSystems.Remove ( cultureInfo );
		}

		[Obsolete ( "Not implemented method of ITable interface", true )]
		public bool Load ( Stream stream )
		{
			throw new NotImplementedException ();
		}

		[Obsolete ( "Not implemented method of ITable interface", true )]
		public bool Save ( Stream stream )
		{
			throw new NotImplementedException ();
		}
	}
}
