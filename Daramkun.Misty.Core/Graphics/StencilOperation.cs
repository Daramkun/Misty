using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Graphics
{
	public enum StencilFunction
	{
		Never,
		Less,
		Equal,
		LessEqual,
		Greater,
		NotEqual,
		GreaterEqual,
		Always,
	}

	public enum StencilOperator
	{
		Keep,
		Zero,
		Invert,
		Replace,
		Increase,
		Decrease,
		IncreaseWrap,
		DecreaseWrap,
	}

	public struct StencilOperation
	{
		public StencilFunction Function { get; set; }
		public int Reference { get; set; }
		public int Mask { get; set; }
		public StencilOperator ZFail { get; set; }
		public StencilOperator Fail { get; set; }
		public StencilOperator Pass { get; set; }

		public StencilOperation ( StencilFunction func, int reference, int mask, StencilOperator zfail, StencilOperator fail, StencilOperator pass )
			: this ()
		{
			Function = func;
			Reference = reference;
			Mask = mask;
			ZFail = zfail;
			Fail = fail;
			Pass = pass;
		}
	}
}
