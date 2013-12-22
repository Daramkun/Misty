using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.Decoders.Images;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.IO.Compression;

namespace Daramkun.Misty.Contents.Decoders.Packs
{
	[FileFormat ( "wlnt" )]
	public class PackageDecoder : IDecoder<PackageInfo>
	{
		public bool Decode ( Stream stream, out PackageInfo to, params object [] args )
		{
			BinaryReader reader = new BinaryReader ( stream );
			to = new PackageInfo ();
			if ( Encoding.UTF8.GetString ( reader.ReadBytes ( 4 ), 0, 4 ) != "WLNT" )
				return false;

			DeflateStream deflate = new DeflateStream ( stream, CompressionMode.Decompress );
			reader = new BinaryReader ( deflate );

			PackageInfo packInfo = new PackageInfo ();

			string packageName = Encoding.UTF8.GetString ( reader.ReadBytes ( 32 ), 0, 32 ).Trim ( '\0', ' ', '\t', '\n', '　' );
			string author = Encoding.UTF8.GetString ( reader.ReadBytes ( 32 ), 0, 32 ).Trim ( '\0', ' ', '\t', '\n', '　' );
			string copyright = Encoding.UTF8.GetString ( reader.ReadBytes ( 128 ), 0, 128 ).Trim ( '\0', ' ', '\t', '\n', '　' );
			string description = Encoding.UTF8.GetString ( reader.ReadBytes ( 128 ), 0, 128 ).Trim ( '\0', ' ', '\t', '\n', '　' );

			Guid packageID = new Guid ( reader.ReadBytes ( 16 ) );
			Version version = new Version ( reader.ReadByte (), reader.ReadByte (), reader.ReadByte (), reader.ReadUInt16 () );
			DateTime releaseDate = new DateTime ( reader.ReadInt16 (), reader.ReadByte (), reader.ReadByte () );

			int imageSize = reader.ReadInt32 ();
			ImageInfo packageCover = new ImageInfo ();
			if ( imageSize > 0 )
			{
				new PngDecoder ().Decode ( new MemoryStream ( reader.ReadBytes ( imageSize ) ), out packageCover );
			}

			bool isSubPackage = reader.ReadBoolean ();

			List<Guid> mainGuid = new List<Guid> ();
			if ( packInfo.IsSubPackage )
			{
				int mainPackCount = reader.ReadByte ();
				if ( mainPackCount > 0 )
				{
					for ( int i = 0; i < mainPackCount; i++ )
						mainGuid.Add ( new Guid ( reader.ReadBytes ( 16 ) ) );

					if ( !mainGuid.Contains ( Core.MainPackage.PackageID ) )
					{
						bool isContains = false;
						foreach ( PackageInfo subpack in Core.SubPackages )
							if ( mainGuid.Contains ( subpack.PackageID ) )
								isContains = true;
						if ( !isContains )
							return false;
					}
				}
			}

			int stringTableSize = reader.ReadInt32 ();
			StringTable stringTable = null;
			if ( stringTableSize > 0 )
				stringTable = new StringTable ( new MemoryStream ( reader.ReadBytes ( stringTableSize ) ) );

			int resourceTableSize = reader.ReadInt32 ();
			ResourceTable resourceTable = null;
			if ( resourceTableSize > 0 )
				resourceTable = new ResourceTable ( new ZipFileSystem ( new MemoryStream ( reader.ReadBytes ( resourceTableSize ) ) ) );

			to = new PackageInfo ( packageName, author, copyright, description, version,
				releaseDate, isSubPackage, mainGuid.ToArray (), stringTable, resourceTable,
				( imageSize == 0 ) ? null : ( ImageInfo? ) packageCover, packageID );

			return true;
		}
	}
}
