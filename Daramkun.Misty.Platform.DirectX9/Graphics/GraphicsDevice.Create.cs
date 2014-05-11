using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;

namespace Daramkun.Misty.Graphics
{
	public partial class GraphicsDevice : StandardDispose, IGraphicsDevice
	{
		public IGraphicsContext CreateDeferredContext () { throw new NotImplementedException (); }

		public IRenderBuffer CreateRenderBuffer ( int width, int height )
		{
			return new RenderBuffer ( this, width, height );
		}

		public ITexture1D CreateTexture1D ( int width, int mipmapLevel = 1 ) { throw new NotImplementedException (); }
		public ITexture1D CreateTexture1D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 ) { throw new NotImplementedException (); }
		public ITexture2D CreateTexture2D ( int width, int height, int mipmapLevel = 1 )
		{ return new Texture2D ( this, width, height, mipmapLevel ); }
		public ITexture2D CreateTexture2D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{ return new Texture2D ( this, imageInfo, colorKey, mipmapLevel ); }
		public ITexture3D CreateTexture3D ( int width, int height, int depth, int mipmapLevel = 1 ) { throw new NotImplementedException (); }
		public ITexture3D CreateTexture3D ( ImageInfo [] imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{ throw new NotImplementedException (); }

		public IVertexDeclaration CreateVertexDeclaration ( params VertexElement [] elements ) { return new VertexDeclaration ( this, elements ); }

		public IBuffer CreateBuffer ( BufferType bufferType, Type vertexType, int length ) { return new Buffer ( this, vertexType, length, bufferType ); }
		public IBuffer CreateBuffer<T> ( BufferType bufferType, T [] vertices ) where T : struct
		{
			IBuffer buffer = new Buffer ( this, typeof ( T ), vertices.Length, bufferType );
			buffer.SetBufferDatas<T> ( vertices );
			return buffer;
		}

		public IShader CreateShader ( ShaderType shaderType, string shader ) { return new Shader ( this, shaderType, shader ); }

		public IEffect CreateEffect ( IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName )
		{
			return new Effect ( this, vertexShader, pixelShader, geometryShader, attribName );
		}
		public IEffect CreateEffect ( Stream xmlStream ) { return new Effect ( this, xmlStream ); }
	}
}
