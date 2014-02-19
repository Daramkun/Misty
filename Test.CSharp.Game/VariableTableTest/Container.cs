using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Nodes;

namespace Test.CSharp.Game.VariableTableTest
{
	[MainNode]
	public class Container : Node
	{
		IFileSystem fileSystem;
		VariableTable variableTable;
		Guid myGuid = new Guid ( "6FFCD743-B9AE-4183-B163-79EE52BC4220" );

		public override void Intro ( params object [] args )
		{
			variableTable = new VariableTable ();

			fileSystem = FileSystemManager.GetFileSystem ( "LocalFileSystem" );
			if ( fileSystem.IsFileExist ( "vartab.dat" ) )
				variableTable.Load ( fileSystem.OpenFile ( "vartab.dat" ) );
			else
			{
				variableTable.AddPackageVariableTable ( myGuid, 3 );
				variableTable.SetVariable ( myGuid, 0, 0 );
				variableTable.SetVariable ( myGuid, 1, 0.0 );
				variableTable.SetVariable ( myGuid, 2, false );
			}

			base.Intro ( args );
		}

		public override void Outro ()
		{
			if ( !fileSystem.IsFileExist ( "vartab.dat" ) )
				( fileSystem as IWritableFileSystem ).CreateFile ( "vartab.dat" );
			variableTable.Save ( fileSystem.OpenFile ( "vartab.dat" ) );
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			base.Draw ( gameTime );
		}

		public override string ToString () { return "Variable Table Test"; }
	}
}
