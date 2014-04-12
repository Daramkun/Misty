using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Daramkun.Misty.Graphics
{
	class GraphicsDeviceInformation : IGraphicsDeviceInformation
	{
		Version rendererVersion, shaderVersion;

		public BaseRenderer BaseRenderer { get { return Graphics.BaseRenderer.OpenGL; } }
		public Version RendererVersion
		{
			get
			{
				if ( rendererVersion != null ) return rendererVersion;
				string versionString = GL.GetString ( StringName.Version );
				int index = versionString.IndexOf ( ' ' );
				if ( index <= -1 ) index = versionString.IndexOf ( '-' );
				if ( index <= -1 ) index = versionString.Length;
				string [] v = versionString.Substring ( 0, index ).Trim ().Split ( '.' );
				return rendererVersion = new Version ( int.Parse ( v [ 0 ] ), int.Parse ( v [ 1 ] ) );
			}
		}
		public ScreenResolution [] AvailableScreenResolutions
		{
			get
			{
				List<ScreenResolution> screenSizes = new List<ScreenResolution> ();
				foreach ( OpenTK.DisplayResolution resolution in OpenTK.DisplayDevice.Default.AvailableResolutions )
					screenSizes.Add ( new ScreenResolution () { ScreenSize = new Vector2 ( resolution.Width, resolution.Height ), RefreshRate = resolution.RefreshRate } );
				return screenSizes.ToArray ();
			}
		}
		public int MaximumAnisotropicLevel
		{
			get
			{
				int level;
				GL.GetInteger ( ( GetPName ) ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out level );
				return level;
			}
		}
		public bool IsSupportTexture1D { get { return true; } }
		public bool IsSupportTexture3D { get { return true; } }
		public bool IsSupportGeometryShader { get { return RendererVersion >= new Version ( 3, 2 ); } }
		public Version ShaderVersion
		{
			get
			{
				if ( shaderVersion != null ) return shaderVersion;
				if ( RendererVersion == new Version ( 4, 4 ) ) return ( shaderVersion = new Version ( 4, 4 ) );
				else if ( RendererVersion == new Version ( 4, 3 ) ) return ( shaderVersion = new Version ( 4, 3 ) );
				else if ( RendererVersion == new Version ( 4, 2 ) ) return ( shaderVersion = new Version ( 4, 2 ) );
				else if ( RendererVersion == new Version ( 4, 1 ) ) return ( shaderVersion = new Version ( 4, 1 ) );
				else if ( RendererVersion == new Version ( 4, 0 ) ) return ( shaderVersion = new Version ( 4, 0 ) );
				else if ( RendererVersion == new Version ( 3, 3 ) ) return ( shaderVersion = new Version ( 3, 3 ) );
				else if ( RendererVersion == new Version ( 3, 2 ) ) return ( shaderVersion = new Version ( 1, 5 ) );
				else if ( RendererVersion == new Version ( 3, 1 ) ) return ( shaderVersion = new Version ( 1, 4 ) );
				else if ( RendererVersion == new Version ( 3, 0 ) ) return ( shaderVersion = new Version ( 1, 3 ) );
				else if ( RendererVersion == new Version ( 2, 1 ) ) return ( shaderVersion = new Version ( 1, 2 ) );
				else if ( RendererVersion == new Version ( 2, 0 ) ) return ( shaderVersion = new Version ( 1, 1 ) );
				else throw new PlatformNotSupportedException ();
			}
		}
		public string DeviceVendor { get { return GL.GetString ( StringName.Vendor ); } }
	}
}
