using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public enum ShaderType : byte
	{
		VertexShader = 1,
		PixelShader = 2,
		GeometryShader = 3,
		
		MixedShader = 7,
	}

	public interface IShader : IDisposable
	{
		ShaderType ShaderType { get; }
		object Handle { get; }
	}

	public interface IEffect : IDisposable
	{
		object Handle { get; }

		IShader VertexShader { get; }
		IShader PixelShader { get; }
		IShader GeometryShader { get; }

		void Use ( IGraphicsContext graphicsContext );

		void SetUniform<T> ( string name, T value ) where T : struct;
		void SetUniform<T> ( string name, ref T value ) where T : struct;
		void SetUniform ( string name, params int [] value );
		void SetUniform ( string name, params float [] value );

		//void SetTextures ( params TextureArgument [] args );
	}
}
