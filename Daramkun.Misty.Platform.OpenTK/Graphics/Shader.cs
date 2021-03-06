﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	class Shader : StandardDispose, IShader
	{
		int shaderId;

		public object Handle { get { return shaderId; } }
		public ShaderType ShaderType { get; private set; }

		public Shader ( IGraphicsDevice graphicsDevice, ShaderType shaderType, string shaderCode )
		{
			ShaderType = shaderType;

			shaderId = GL.CreateShader ( MistyValueToOriginal ( shaderType ) );

			if ( shaderType == ShaderType.PixelShader )
			{
				shaderCode = "#define getTexUV(texcoord) (vec2(texcoord.st.x, 1.0 - texcoord.st.y))\r\n" + shaderCode;
			}

			shaderCode = string.Format ( "#version {0}{1}0\r\n", graphicsDevice.Information.ShaderVersion.Major, graphicsDevice.Information.ShaderVersion.Minor ) + shaderCode;

			GL.ShaderSource ( shaderId, shaderCode );
			GL.CompileShader ( shaderId );

			int compileState;
			GL.GetShader ( shaderId, ShaderParameter.CompileStatus, out compileState );
			if ( compileState == 0 )
			{
				throw new ArgumentException ( string.Format ( "Compile failed: [Shader Type: {0}]\nLog:\n{1}\nOriginal code:\n{2}",
					ShaderType, GL.GetShaderInfoLog ( shaderId ), shaderCode ) );
			}
		}

		private OpenTK.Graphics.OpenGL.ShaderType MistyValueToOriginal ( ShaderType shaderType )
		{
			switch ( shaderType )
			{
				case ShaderType.VertexShader: return OpenTK.Graphics.OpenGL.ShaderType.VertexShader;
				case ShaderType.PixelShader: return OpenTK.Graphics.OpenGL.ShaderType.FragmentShader;
				case ShaderType.GeometryShader: return OpenTK.Graphics.OpenGL.ShaderType.GeometryShader;
				default: throw new ArgumentException ();
			}
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				GL.DeleteShader ( shaderId );
				shaderId = 0;
			}
			base.Dispose ( isDisposing );
		}
	}
}
