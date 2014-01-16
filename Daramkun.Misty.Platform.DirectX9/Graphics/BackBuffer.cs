using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Graphics
{
	class BackBuffer : IRenderBuffer
	{
		GraphicsDevice graphicsDevice;

		public int Width { get { return graphicsDevice.d3dpp.BackBufferWidth; } }
		public int Height { get { return graphicsDevice.d3dpp.BackBufferHeight; } }
		public Vector2 Size { get { return new Vector2 ( Width, Height ); } }
		public object Handle { get { return ( graphicsDevice.Handle as SharpDX.Direct3D9.Device ).GetBackBuffer ( 0, 0 ); } }
		public Color [] Buffer
		{
			get
			{
				SharpDX.Direct3D9.Surface texture = Handle as SharpDX.Direct3D9.Surface;
				SharpDX.DataRectangle dr = texture.LockRectangle ( new SharpDX.Rectangle ( 0, 0, Width, Height ), SharpDX.Direct3D9.LockFlags.None );
				SharpDX.DataStream stream = new SharpDX.DataStream ( dr.DataPointer, Width * Height * 4, true, false );
				Color [] colours = new Color [ stream.Length / 4 ];
				for ( int i = 0; i < colours.Length; ++i )
					colours [ i ] = new Color ( stream.Read<SharpDX.Color> ().ToBgra (), true );
				texture.UnlockRectangle ();
				return colours;
			}
			set
			{
				SharpDX.Direct3D9.Surface texture = Handle as SharpDX.Direct3D9.Surface;
				SharpDX.DataRectangle dr = texture.LockRectangle ( new SharpDX.Rectangle ( 0, 0, Width, Height ), SharpDX.Direct3D9.LockFlags.None );
				SharpDX.DataStream stream = new SharpDX.DataStream ( dr.DataPointer, Width * Height * 4, false, true );
				SharpDX.Color [] colours = new SharpDX.Color [ stream.Length / 4 ];
				for ( int i = 0; i < value.Length; ++i )
					colours [ i ] = new SharpDX.Color ( value [ i ].ARGBValue );
				stream.WriteRange<SharpDX.Color> ( colours );
				texture.UnlockRectangle ();
			}
		}

		public BackBuffer ( GraphicsDevice graphicsDevice ) { this.graphicsDevice = graphicsDevice; }
		public void Dispose () { }
	}
}
