using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common.Json;
using Daramkun.Misty.Contents.Decoders.Images;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Tables;

namespace Daramkun.Misty.Contents
{
	public class PackageInfo
	{
		public bool IsSettingCompleted { get; private set; }

		public string PackageName { get; private set; }

		public string Author { get; private set; }
		public string Copyright { get; private set; }
		public string Description { get; private set; }

		public Guid PackageID { get; private set; }
		public Version Version { get; private set; }
		public DateTime ReleaseDate { get; private set; }

		public ImageInfo PackageCover { get; private set; }

		public bool IsSubPackage { get; private set; }
		public Guid [] MainPackageIDs { get; private set; }

		public StringTable StringTable { get; private set; }
		public ResourceTable ResourceTable { get; private set; }

		public PackageInfo () { }

		public PackageInfo ( string packageName, string author, string copyright, string description, Version version, DateTime releaseDate,
			bool isSubPackage, Guid [] mainPackageIds, StringTable stringTable, ResourceTable resourceTable, ImageInfo imageInfo = null, Guid? packageId = null )
		{
			PackageName = packageName;

			Author = author;
			Copyright = copyright;
			Description = description;

			PackageID = ( packageId == null ) ? Guid.NewGuid () : packageId.Value;
			Version = version;
			ReleaseDate = releaseDate;

			if ( imageInfo != null )
				PackageCover = imageInfo;

			IsSubPackage = isSubPackage;
			MainPackageIDs = mainPackageIds;

			StringTable = stringTable;
			ResourceTable = resourceTable;

			IsSettingCompleted = true;
		}

		public static PackageInfo LoadFromFileSystem ( IFileSystem fileSystem )
		{
			if ( !fileSystem.IsFileExist ( "packageInfo.json" ) )
				throw new ArgumentException ();

			PackageInfo packageInfo = new PackageInfo ();
			JsonEntry entry = JsonParser.Parse ( fileSystem.OpenFile ( "packageInfo.json" ) );

			packageInfo.PackageName = entry [ "title" ] as string;

			packageInfo.Author = entry [ "author" ] as string;
			packageInfo.Copyright = entry [ "copyright" ] as string;
			packageInfo.Description = entry [ "description" ] as string;

			packageInfo.PackageID = new Guid ( entry [ "packId" ] as string );
			packageInfo.Version = new Version ( entry [ "version" ] as string );
			packageInfo.ReleaseDate = DateTime.Parse ( entry [ "release_date" ] as string );

			if ( packageInfo.IsSubPackage = ( bool ) entry [ "issubpack" ] )
			{
				List<Guid> mainGuid = new List<Guid> ();
				JsonArray mainPackIds = entry [ "mainpacks" ] as JsonArray;
				foreach ( object item in mainPackIds )
					mainGuid.Add ( new Guid ( item as string ) );

				if ( !mainGuid.Contains ( Core.MainPackage.PackageID ) )
				{
					bool isContains = false;
					foreach ( PackageInfo subpack in Core.SubPackages )
						if ( mainGuid.Contains ( subpack.PackageID ) )
							isContains = true;
					if ( !isContains )
						throw new ArgumentException ( "This package is not allowed to this package." );
				}

				packageInfo.MainPackageIDs = mainGuid.ToArray ();
			}

			if ( fileSystem.IsFileExist ( "packageCover.png" ) )
			{
				ImageInfo imageInfo;
				new PngDecoder ().Decode ( fileSystem.OpenFile ( "packageCover.png" ), out imageInfo );
				packageInfo.PackageCover = imageInfo;
			}

			if ( fileSystem.IsFileExist ( "stringTable.stt" ) )
				packageInfo.StringTable = new StringTable ( fileSystem.OpenFile ( "stringTable.stt" ) );

			if ( fileSystem.IsFileExist ( "resourceTable.rst" ) )
				packageInfo.ResourceTable = new ResourceTable ( new ZipFileSystem ( fileSystem.OpenFile ( "resourceTable.rst" ) ) );

			return packageInfo;
		}
	}
}
