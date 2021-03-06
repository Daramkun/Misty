﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	class GraphicsDeviceInformation : IGraphicsDeviceInformation
	{
		SharpDX.Direct3D9.Direct3D d3d;
		SharpDX.Direct3D9.Capabilities d3dCaps;

		public BaseRenderer BaseRenderer { get { return BaseRenderer.DirectX; } }
		public ShaderLanguage ShaderLanguage { get { return ShaderLanguage.HLSL; } }
		public Version RendererVersion { get { return new Version ( 9, 0 ); } }
		public Version ShaderVersion { get { return new Version ( 3, 0 ); } }

		public ScreenResolution [] AvailableScreenResolutions
		{
			get
			{
				List<ScreenResolution> sizes = new List<ScreenResolution> ();
				int count = d3d.GetAdapterModeCount ( 0, SharpDX.Direct3D9.Format.X8R8G8B8 );
				for ( int i = 0; i < count; i++ )
				{
					SharpDX.Direct3D9.DisplayMode mode = d3d.EnumAdapterModes ( 0, SharpDX.Direct3D9.Format.X8R8G8B8, i );
					sizes.Add ( new ScreenResolution () { ScreenSize = new Vector2 ( mode.Width, mode.Height ), RefreshRate = mode.RefreshRate } );
				}
				return sizes.ToArray ();
			}
		}

		public int MaximumAnisotropicLevel { get { return d3dCaps.MaxAnisotropy; } }

		public bool IsSupportTexture1D { get { return false; } }
		public bool IsSupportTexture3D { get { return false; } }
		public bool IsSupportGeometryShader { get { return false; } }
		public bool IsSupportMultiContext { get { return false; } }
		
		public string DeviceVendor
		{
			get
			{
				switch ( d3d.GetAdapterIdentifier ( 0 ).VendorId )
				{
					case 0x1002: return "ATI Technologies Inc.";
					case 0x10DE: return "NVIDIA";
					case 0x8086: return "Intel";
					case 0x15AD: return "VMWare Inc.";
					case 0x80EE: return "Oracle Corporation";
					case 0x1AB8: return "Parallels";
					default: return string.Format ( "Unknown({0:x4})", d3d.GetAdapterIdentifier ( 0 ).VendorId );
				}
			}
		}

		public GraphicsDeviceInformation ( SharpDX.Direct3D9.Direct3D d3d )
		{
			this.d3d = d3d;
			d3dCaps = d3d.GetDeviceCaps ( 0, SharpDX.Direct3D9.DeviceType.Hardware );
		}
	}
}
