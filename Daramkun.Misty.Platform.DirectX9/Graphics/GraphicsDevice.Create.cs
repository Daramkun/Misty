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
		public IRenderBuffer CreateRenderBuffer ( int width, int height )
		{
			return new RenderBuffer ( this, width, height );
		}

		public ITexture1D CreateTexture1D ( int width, int mipmapLevel = 1 ) { throw new NotImplementedException (); }
		public ITexture1D CreateTexture1D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 ) { throw new NotImplementedException (); }
		public ITexture2D CreateTexture2D ( int width, int height, int mipmapLevel = 1 )
		{
			Texture2D texture = null;
			Core.Dispatch ( () => { texture = new Texture2D ( this, width, height, mipmapLevel ); } );
			return texture;
		}
		public ITexture2D CreateTexture2D ( ImageInfo imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{
			Texture2D texture = null;
			Core.Dispatch ( () => { texture = new Texture2D ( this, imageInfo, colorKey, mipmapLevel ); } );
			return texture;
		}
		public ITexture3D CreateTexture3D ( int width, int height, int depth, int mipmapLevel = 1 ) { throw new NotImplementedException (); }
		public ITexture3D CreateTexture3D ( ImageInfo [] imageInfo, Color? colorKey = null, int mipmapLevel = 1 )
		{ throw new NotImplementedException (); }

		public IVertexDeclaration CreateVertexDeclaration ( params VertexElement [] elements ) { return new VertexDeclaration ( this, elements ); }

		public IVertexBuffer CreateVertexBuffer ( Type vertexType, int length ) { return new VertexBuffer ( this, vertexType, length ); }
		public IVertexBuffer CreateVertexBuffer<T> ( T [] vertices ) where T : struct
		{
			IVertexBuffer buffer = new VertexBuffer ( this, typeof ( T ), vertices.Length );
			buffer.SetBufferDatas<T> ( vertices );
			return buffer;
		}

		public IIndexBuffer CreateIndexBuffer ( Type indexType, int length, bool is16bit = false ) { return new IndexBuffer ( this, indexType, length, is16bit ); }
		public IIndexBuffer CreateIndexBuffer<T> ( T [] indices, bool is16bit = false ) where T : struct
		{
			IIndexBuffer buffer = new IndexBuffer ( this, typeof ( T ), indices.Length, is16bit );
			buffer.SetBufferDatas<T> ( indices );
			return buffer;
		}

		public IShader CreateShader ( ShaderType shaderType, string shader ) { return new Shader ( this, shaderType, shader ); }

		public IEffect CreateEffect ( IShader vertexShader, IShader pixelShader, IShader geometryShader = null, params string [] attribName )
		{
			return new Effect ( this, vertexShader, pixelShader, geometryShader, attribName );
		}
		public IEffect CreateEffect ( Stream xmlStream )
		{
			TextReader reader = new StreamReader ( xmlStream );
			XmlDocument doc = new XmlDocument ();
			doc.LoadXml ( reader.ReadToEnd () );
			return CreateEffect ( doc );
		}
		public IEffect CreateEffect ( XmlDocument xmlDoc ) { return new Effect ( this, xmlDoc ); }
	}
}
