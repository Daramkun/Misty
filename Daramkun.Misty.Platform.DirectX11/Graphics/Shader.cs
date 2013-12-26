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
		internal SharpDX.D3DCompiler.ShaderBytecode bytecode;

		public ShaderType ShaderType { get; private set; }
		public ShaderOption Option { get; set; }

		public object Handle { get; set; }

		public Shader ( IGraphicsDevice graphicsDevice, ShaderType shaderType, string shader )
			: this ( graphicsDevice, SharpDX.D3DCompiler.ShaderBytecode.Compile ( shader,
			GetShaderEntryPoint ( shaderType ), GetShaderProfile ( shaderType ) ), shaderType )
		{

		}

		private Shader ( IGraphicsDevice graphicsDevice, SharpDX.D3DCompiler.ShaderBytecode function, Graphics.ShaderType shaderType )
		{
			ShaderType = shaderType;
			this.bytecode = function;
			switch ( ShaderType )
			{
				case Graphics.ShaderType.VertexShader:
					Handle = new SharpDX.Direct3D11.VertexShader ( graphicsDevice.Handle as SharpDX.Direct3D11.Device, function );
					break;
				case Graphics.ShaderType.PixelShader:
					Handle = new SharpDX.Direct3D11.PixelShader ( graphicsDevice.Handle as SharpDX.Direct3D11.Device, function );
					break;
				case Graphics.ShaderType.GeometryShader:
					Handle = new SharpDX.Direct3D11.GeometryShader ( graphicsDevice.Handle as SharpDX.Direct3D11.Device, function );
					break;
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

		private static string GetShaderProfile ( ShaderType shaderType )
		{
			switch ( shaderType )
			{
				case ShaderType.VertexShader: return "vs_4_0";
				case ShaderType.PixelShader: return "ps_4_0";
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
	}
}
