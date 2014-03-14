using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Blockar.Json;

namespace Daramkun.Misty.Contents.Tables
{
	public sealed class StringTable : ITable, ISubCulture<JsonContainer>
	{
		JsonContainer stringTable;
		Version tableVersion;

		public StringTable ( Stream stream )
			: this ( new JsonContainer ( stream ) )
		{ }

		public StringTable ( JsonContainer jsonEntry )
		{
			stringTable = jsonEntry;
			tableVersion = new Version ( stringTable [ "tableversion" ] as string );
			if ( tableVersion.Major == 1 )
			{
				if ( tableVersion.Minor <= 1 )
				{
					if ( tableVersion.Minor >= 1 )
					{
						List<string> temp = new List<string> ();
						foreach ( KeyValuePair<object, object> locale in stringTable.GetDictionaryEnumerable () )
							if ( locale.Value is string && locale.Key as string != "tableversion" )
								temp.Add ( locale.Key as string );//stringTable [ locale.Key ] = stringTable [ locale.Value ];
						foreach ( string t in temp )
							stringTable [ t ] = stringTable [ stringTable [ t ] ];
					}
					return;
				}
			}
			throw new ArgumentException ( "Table version is not 1.1 or higher" );
		}

		public string this [ string key ]
		{
			get
			{
				if ( stringTable.Contains ( Core.CurrentCulture.Name ) )
				{
					JsonContainer innerEntry = stringTable [ Core.CurrentCulture.Name ] as JsonContainer;
					if ( innerEntry.Contains ( key ) ) return innerEntry [ key ] as string;
				}
				else
				{
					JsonContainer innerEntry = stringTable [ "unknown" ] as JsonContainer;
					if ( innerEntry.Contains ( key ) ) return innerEntry [ key ] as string;
				}
				if ( !stringTable.Contains ( key ) ) return null;
				return stringTable [ key ] as string;
			}
		}

		public bool Contains ( string key )
		{
			if ( stringTable.Contains ( CultureInfo.CurrentCulture.Name ) )
			{
				if ( ( stringTable [ Core.CurrentCulture.Name ] as JsonContainer ).Contains ( key ) )
					return true;
			}
			else if ( ( stringTable [ "unknown" ] as JsonContainer ).Contains ( key ) ) return true;
			return stringTable.Contains ( key );
		}

		public void AddCulture ( CultureInfo cultureInfo, JsonContainer localeStringTable )
		{
			if ( stringTable.Contains ( cultureInfo.Name ) )
				stringTable [ cultureInfo.Name ] = localeStringTable;
			else stringTable.Add ( localeStringTable, cultureInfo.Name );
		}

		public void RemoveCulture ( CultureInfo cultureInfo )
		{
			if ( stringTable.Contains ( cultureInfo.Name ) )
				stringTable.Remove ( cultureInfo.Name );
		}

		public bool Load ( Stream stream )
		{
			stringTable = new JsonContainer ( stream );
			return true;
		}

		public bool Save ( Stream stream )
		{
			byte [] buffer = Encoding.UTF8.GetBytes ( stringTable.ToString () );
			stream.Write ( buffer, 0, buffer.Length );
			return true;
		}
	}
}
