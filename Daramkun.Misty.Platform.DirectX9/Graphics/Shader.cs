using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class Shader : StandardDispose, IShader
	{
		public ShaderType ShaderType { get; private set; }
		public ShaderOption Option { get; set; }
		public object Handle { get; private set; }

		public Shader ( IGraphicsDevice graphicsDevice, ShaderType shaderType, string shader )
			: this ( graphicsDevice, SharpDX.Direct3D9.ShaderBytecode.Compile ( shader,
			GetShaderEntryPoint ( shaderType ), GetShaderProfile ( shaderType ), SharpDX.Direct3D9.ShaderFlags.None ),
			shaderType )
		{

		}

		private Shader ( IGraphicsDevice graphicsDevice, SharpDX.Direct3D9.ShaderBytecode function, Graphics.ShaderType shaderType )
		{
			ShaderType = shaderType;
			switch ( ShaderType )
			{
				case Graphics.ShaderType.VertexShader:
					Handle = new SharpDX.Direct3D9.VertexShader ( graphicsDevice.Handle as SharpDX.Direct3D9.Device, function );
					break;
				case Graphics.ShaderType.PixelShader:
					Handle = new SharpDX.Direct3D9.PixelShader ( graphicsDevice.Handle as SharpDX.Direct3D9.Device, function );
					break;
			}
		}

		private static string GetShaderProfile ( ShaderType shaderType )
		{
			switch ( shaderType )
			{
				case ShaderType.VertexShader: return "vs_2_0";
				case ShaderType.PixelShader: return "ps_2_0";
				default: throw new ArgumentException ();
			}
		}

		private static string GetShaderEntryPoint ( ShaderType shaderType )
		{
			switch ( shaderType )
			{
				case ShaderType.VertexShader: return "vs_main";
				case ShaderType.PixelShader: return "ps_main";
				default: throw new ArgumentException ();
			}
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				( Handle as IDisposable ).Dispose ();
			}
			base.Dispose ( isDisposing );
		}
	}
}
