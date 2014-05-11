using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Common
{
	public static class ShaderXmlParser
	{
		public static IEnumerable<KeyValuePair<ShaderType, string>> Parse ( Stream stream, out string option )
		{
			XmlReader reader = XmlReader.Create ( stream );
			List<KeyValuePair<ShaderType, string>> shaders = new List<KeyValuePair<ShaderType, string>> ();
			option = null;
			while ( reader.Read () )
			{
				if ( reader.NodeType == XmlNodeType.Element )
				{
					XmlReader innerReader = reader.ReadSubtree ();
					while ( innerReader.Read () )
					{
						if ( innerReader.NodeType == XmlNodeType.Element && innerReader.Name == "language" )
						{
							if ( Core.GraphicsDevice.Information.ShaderLanguage == ConvertToShaderLanguage ( innerReader.GetAttribute ( "type" ) ) &&
								Core.GraphicsDevice.Information.ShaderVersion >= new Version ( innerReader.GetAttribute ( "version" ) ) )
							{
								option = innerReader.GetAttribute ( "option" );

								KeyValuePair<ShaderType, string>? v;
								while ( ( v = GetShader ( innerReader ) ) != null )
									shaders.Add ( v.Value );
								return shaders;
							}
						}
					}
				}
			}
			return null;
		}

		private static KeyValuePair<ShaderType, string>? GetShader ( XmlReader reader )
		{
			while ( reader.Read () )
			{
				if ( reader.NodeType == XmlNodeType.Element )
					if ( reader.Name == "shader" ) return new KeyValuePair<ShaderType, string> ( ConvertToShaderType ( reader.GetAttribute ( "type" ) ),
						reader.ReadElementContentAsString () );
					else break;
			}
			return null;
		}

		private static ShaderLanguage ConvertToShaderLanguage ( string p )
		{
			switch ( p )
			{
				case "hlsl": return ShaderLanguage.HLSL;
				case "glsl": return ShaderLanguage.GLSL;
				case "glsles": return ShaderLanguage.GLSLES;
				case "cgfx": return ShaderLanguage.CgFX;
				case "mgfx": return ShaderLanguage.MgFX;
				case "psm": return ShaderLanguage.PSM;
				case "shaderlab": return ShaderLanguage.ShaderLab;

				default: return ShaderLanguage.Unknown;
			}
		}

		private static ShaderType ConvertToShaderType ( string p )
		{
			switch(p)
			{
				case "vertex": return ShaderType.VertexShader;
				case "pixel": return ShaderType.PixelShader;
				case "geometry": return ShaderType.GeometryShader;

				case "mixed": return ShaderType.MixedShader;

				default: throw new Exception ( "Unknown Shader type" );
			}
		}
	}
}
