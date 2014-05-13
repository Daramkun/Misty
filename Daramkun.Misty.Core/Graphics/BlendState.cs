using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public enum BlendParameter : byte
	{
		Zero = 0,
		One,

		SourceColor,
		InvertSourceColor,
		SourceAlpha,
		InvertSourceAlpha,

		DestinationAlpha,
		InvertDestinationAlpha,
		DestinationColor,
		InvertDestinationColor,
	}

	public enum BlendOperator : byte
	{
		Add,
		Subtract,
		ReverseSubtract,
		Minimum,
		Maximum,
	}

	public class BlendState
	{
		public static BlendState None { get { return new BlendState ( BlendOperator.Add, BlendParameter.One, BlendParameter.Zero ); } }
		public static BlendState AlphaBlend { get { return new BlendState ( BlendOperator.Add, BlendParameter.SourceAlpha, BlendParameter.InvertSourceAlpha ); } }
		public static BlendState AdditiveBlend { get { return new BlendState ( BlendOperator.Add, BlendParameter.SourceAlpha, BlendParameter.One ); } }
		public static BlendState SubtractBlend { get { return new BlendState ( BlendOperator.ReverseSubtract, BlendParameter.SourceAlpha, BlendParameter.One ); } }
		public static BlendState MultiplyBlend { get { return new BlendState ( BlendOperator.Add, BlendParameter.DestinationColor, BlendParameter.Zero ); } }

		public BlendParameter SourceParameter { get; private set; }
		public BlendParameter DestinationParameter { get; private set; }
		public BlendOperator Operator { get; private set; }

		public BlendState ( BlendOperator op, BlendParameter sourceParam, BlendParameter destParam )
		{
			Operator = op;
			SourceParameter = sourceParam;
			DestinationParameter = destParam;
		}
	}
}
