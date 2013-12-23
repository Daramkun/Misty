using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Daramkun.Misty.Graphics
{
	public partial class GraphicsDevice
	{
		private CullingMode ConvertNativeValue ( Microsoft.Xna.Framework.Graphics.CullMode cullMode )
		{
			switch ( cullMode )
			{
				case Microsoft.Xna.Framework.Graphics.CullMode.None: return CullingMode.None;
				case Microsoft.Xna.Framework.Graphics.CullMode.CullClockwiseFace: return CullingMode.ClockWise;
				case Microsoft.Xna.Framework.Graphics.CullMode.CullCounterClockwiseFace: return CullingMode.CounterClockWise;
				default: throw new ArgumentException ();
			}
		}

		private Microsoft.Xna.Framework.Graphics.CullMode ConvertMistyValue ( CullingMode value )
		{
			switch ( value )
			{
				case CullingMode.None: return Microsoft.Xna.Framework.Graphics.CullMode.None;
				case CullingMode.ClockWise: return Microsoft.Xna.Framework.Graphics.CullMode.CullClockwiseFace;
				case CullingMode.CounterClockWise: return Microsoft.Xna.Framework.Graphics.CullMode.CullCounterClockwiseFace;
				default: throw new ArgumentException ();
			}
		}

		private Graphics.FillMode ConvertNativeValue ( Microsoft.Xna.Framework.Graphics.FillMode fillMode )
		{
			switch ( fillMode )
			{
				case Microsoft.Xna.Framework.Graphics.FillMode.Solid: return Graphics.FillMode.Solid;
				case Microsoft.Xna.Framework.Graphics.FillMode.WireFrame: return Graphics.FillMode.Wireframe;
				default: throw new ArgumentException ();
			}
		}

		private Microsoft.Xna.Framework.Graphics.FillMode ConvertMistyValue ( Graphics.FillMode value )
		{
			switch ( value )
			{
				case Graphics.FillMode.Solid: return Microsoft.Xna.Framework.Graphics.FillMode.Solid;
				case Graphics.FillMode.Wireframe: return Microsoft.Xna.Framework.Graphics.FillMode.WireFrame;
				default: throw new ArgumentException ();
			}
		}
	}
}
