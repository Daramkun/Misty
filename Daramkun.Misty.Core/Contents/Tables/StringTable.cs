using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common.Json;

namespace Daramkun.Misty.Contents.Tables
{
	public sealed class StringTable : ITable
	{
		JsonEntry stringTable;

		public bool IsCultureMode { get; set; }

		public StringTable ( Stream stream )
		{
			stringTable = JsonParser.Parse ( stream );
			if ( stringTable [ "tableversion" ] as string != "1.0" )
				throw new ArgumentException ( "Table version is not 1.0 or lesser" );
		}

		public StringTable ( JsonEntry jsonEntry )
		{
			stringTable = jsonEntry;
			if ( stringTable [ "tableversion" ] as string != "1.0" )
				throw new ArgumentException ( "Table version is not 1.0 or lesser" );
		}

		public string this [ string key ]
		{
			get
			{
				if ( IsCultureMode )
				{
					if ( stringTable.Contains ( Core.CurrentCulture.Name ) )
					{
						JsonEntry innerEntry = stringTable [ Core.CurrentCulture.Name ] as JsonEntry;
						if ( innerEntry.Contains ( key ) ) return innerEntry [ key ] as string;
					}
				}
				if ( !stringTable.Contains ( key ) ) return null;
				return stringTable [ key ] as string;
			}
		}

		public bool Contains ( string key )
		{
			if ( IsCultureMode )
			{
				if ( stringTable.Contains ( CultureInfo.CurrentCulture.Name ) )
				{
					bool result = ( stringTable [ Core.CurrentCulture.Name ] as JsonEntry ).Contains ( key );
					if ( result ) return result;
				}
			}
			return stringTable.Contains ( key );
		}

		public bool Load ( Stream stream )
		{
			stringTable = JsonParser.Parse ( stream );
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
