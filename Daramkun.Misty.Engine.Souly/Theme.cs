using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;

namespace Daramkun.Misty.Souly
{
	public abstract class Theme
	{
		public abstract ITexture2D Window { get; }
		public abstract Font WindowTitleFont { get; }

		public abstract ITexture2D Button { get; }
		public abstract ITexture2D TextBox { get; }
		public abstract ITexture2D ScrollBar { get; }
		public abstract ITexture2D CheckBox { get; }
		public abstract ITexture2D RadioButton { get; }
		public abstract ITexture2D ComboBox { get; }
		public abstract ITexture2D ProgressBar { get; }
		public abstract ITexture2D TabPage { get; }
		public abstract ITexture2D GroupBox { get; }
		public abstract ITexture2D TrackBar { get; }
		public abstract ITexture2D ListViewHeader { get; }

		public abstract Font DefaultFont { get; }
	}
}
