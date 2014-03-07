﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Contents.Tables
{
	public sealed class ResourceTable : ITable, IDisposable
	{
		public static List<Assembly> ContentLoaderAssemblies { get; private set; }

		List<IContentLoader> contentLoaders = new List<IContentLoader> ();
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

		public void AddContentLoader ( IContentLoader contentLoader )
		{
			if ( contentLoaders.Contains ( contentLoader ) ) return;
			contentLoaders.Add ( contentLoader );
		}

		public void RemoveContentLoader ( IContentLoader contentLoader )
		{
			contentLoaders.Remove ( contentLoader );
		}

		public void AddDefaultContentLoader ()
		{
			foreach ( Assembly assembly in ContentLoaderAssemblies )
			{
				foreach ( Type type in assembly.GetTypes () )
				{
					if ( Utilities.IsSubtypeOf ( type, typeof ( IContentLoader ) ) && type != typeof ( IContentLoader )
						&& !type.IsAbstract && !type.IsInterface && type.IsPublic )
						AddContentLoader ( Activator.CreateInstance ( type ) as IContentLoader );
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
			IContentLoader loader = null;
			foreach ( IContentLoader contentLoader in contentLoaders )
			{
				if ( Utilities.IsSubtypeOf ( type, contentLoader.ContentType ) )
					loader = contentLoader;
			}

			key = null;

			if ( loader == null )
			{
				if ( FileSystem.IsFileExist ( filename ) )
				{
					key = filename;
					return ( T ) loadedContent [ filename ];
				}
				throw new ArgumentException ();
			}

			if ( !FileSystem.IsFileExist ( filename ) )
			{
				if ( FileSystem.IsFileExist ( PathCombine ( Core.CurrentCulture.Name, filename ) ) )
					key = PathCombine ( Core.CurrentCulture.Name, filename );
				else if ( localeFileSystems [ Core.CurrentCulture ].IsFileExist ( filename ) )
					key = filename;
				else if ( FileSystem.IsFileExist ( PathCombine ( "unknown", filename ) ) )
					key = PathCombine ( "unknown", filename );
				else
				{
					bool exist = false;
					foreach ( string ext in loader.FileExtensions )
					{
						if ( exist = FileSystem.IsFileExist ( key = PathCombine ( Core.CurrentCulture.Name, string.Format ( "{0}.{1}", filename, ext.ToLower () ) ) ) )
							break;
						else if ( exist = localeFileSystems [ Core.CurrentCulture ].IsFileExist ( key = string.Format ( "{0}.{1}", filename, ext.ToLower () ) ) )
							break;
						else if ( exist = FileSystem.IsFileExist ( key = PathCombine ( "unknown", string.Format ( "{0}.{1}", filename, ext.ToLower () ) ) ) )
							break;
						else if ( exist = FileSystem.IsFileExist ( key = string.Format ( "{0}.{1}", filename, ext.ToLower () ) ) )
							break;
					}

					if ( !exist ) { key = null; throw new FileNotFoundException (); }
				}
			}
			else key = filename;

			filename = key;
			key = MakeKey ( filename, type, args );

			if ( loadedContent.ContainsKey ( key ) )
			{
				return ( T ) loadedContent [ key ];
			}
			else
			{
				Stream stream = FileSystem.OpenFile ( filename );
				object data = loader.Load ( stream, this, args );
				loadedContent.Add ( key, data );
				if ( !loader.IsSelfStreamDispose )
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
