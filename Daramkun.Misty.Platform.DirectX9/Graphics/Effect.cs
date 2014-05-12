using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Graphics
{
	class Effect : StandardDispose, IEffect
	{
		IGraphicsDevice graphicsDevice;
		IShader vertexShader;
		IShader pixelShader;
		Dictionary<string, SharpDX.Direct3D9.EffectHandle> handleCache;

		public object Handle { get { return new [] { vertexShader, pixelShader }; } }

		public IShader VertexShader { get { return vertexShader; } }
		public IShader PixelShader { get { return pixelShader; } }
		public IShader GeometryShader { get { return null; } }

		public Effect ( IGraphicsDevice graphicsDevice, IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName )
		{
			InitializeEffect ( graphicsDevice, vertexShader, pixelShader, geometryShader, attribName );
		}

		public Effect (IGraphicsDevice graphicsDevice, Stream stream)
		{
			string attribName;
			foreach ( var v in ShaderXmlParser.Parse ( stream, out attribName ) )
			{
				IShader shader = new Shader ( graphicsDevice, v.Key, v.Value );
				switch ( v.Key )
				{
					case ShaderType.VertexShader: vertexShader = shader; break;
					case ShaderType.PixelShader: pixelShader = shader; break;
				}
			}

			InitializeEffect ( graphicsDevice, VertexShader, PixelShader, GeometryShader );
		}

		private void InitializeEffect ( IGraphicsDevice graphicsDevice, IShader VertexShader, IShader PixelShader, IShader GeometryShader, params string [] attribName )
		{
			this.graphicsDevice = graphicsDevice;

			vertexShader = VertexShader;
			pixelShader = PixelShader;

			handleCache = new Dictionary<string, SharpDX.Direct3D9.EffectHandle> ();
		}

		protected override void Dispose ( bool isDisposing )
		{
			if ( isDisposing )
			{
				foreach ( SharpDX.Direct3D9.EffectHandle handle in handleCache.Values )
					handle.Dispose ();
				handleCache = null;

				foreach ( IShader shader in new IShader [] { VertexShader, PixelShader, GeometryShader } )
				{
					if ( shader == null ) continue;
					shader.Dispose ();
				}
			}
			base.Dispose ( isDisposing );
		}

		public void Use ( IGraphicsContext graphicsContext )
		{
			if ( graphicsContext.Owner != Thread.CurrentThread ) throw new Exception ( "This thread is not owner of Context." );

			SharpDX.Direct3D9.Device device = graphicsDevice.Handle as SharpDX.Direct3D9.Device;

			device.VertexShader = vertexShader.Handle as SharpDX.Direct3D9.VertexShader;
			device.PixelShader = pixelShader.Handle as SharpDX.Direct3D9.PixelShader;
		}

		public void SetUniform<T> ( string name, T value ) where T : struct
		{
			SetUniform<T> ( name, ref value );
		}

		public void SetUniform<T> ( string name, ref T value ) where T : struct
		{
			var device = graphicsDevice.Handle as SharpDX.Direct3D9.Device;
			var constantTable = ( vertexShader.Handle as SharpDX.Direct3D9.VertexShader ).Function.ConstantTable;
			var handle = ( handleCache.ContainsKey ( name ) ) ? handleCache [ name ] : constantTable.GetConstantByName ( null, name );
			constantTable.SetValue<T> ( device, handle, value );
			if ( !handleCache.ContainsKey ( name ) ) handleCache.Add ( name, handle );
		}

		public void SetUniform ( string name, params int [] value )
		{
			var device = graphicsDevice.Handle as SharpDX.Direct3D9.Device;
			var constantTable = ( vertexShader.Handle as SharpDX.Direct3D9.VertexShader ).Function.ConstantTable;
			var handle = ( handleCache.ContainsKey ( name ) ) ? handleCache [ name ] : constantTable.GetConstantByName ( null, name );
			constantTable.SetValue<int> ( device, handle, value );
			if ( !handleCache.ContainsKey ( name ) ) handleCache.Add ( name, handle );
		}

		public void SetUniform ( string name, params float [] value )
		{
			var device = graphicsDevice.Handle as SharpDX.Direct3D9.Device;
			var constantTable = ( vertexShader.Handle as SharpDX.Direct3D9.VertexShader ).Function.ConstantTable;
			var handle = ( handleCache.ContainsKey ( name ) ) ? handleCache [ name ] : constantTable.GetConstantByName ( null, name );
			constantTable.SetValue<float> ( device, handle, value );
			if ( !handleCache.ContainsKey ( name ) ) handleCache.Add ( name, handle );
		}
	}
}
