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

	public class StencilState
	{
		public StencilFunction Function { get; private set; }
		public int Reference { get; private set; }
		public int Mask { get; private set; }
		public StencilOperator ZFail { get; private set; }
		public StencilOperator Fail { get; private set; }
		public StencilOperator Pass { get; private set; }

		public StencilState ( StencilFunction func, int reference, int mask, StencilOperator zfail, StencilOperator fail, StencilOperator pass )
		{
			Function = func;
			Reference = reference;
			Mask = mask;
			ZFail = zfail;
			Fail = fail;
			Pass = pass;
		}
	}

	public struct DepthStencil
	{
		public bool DepthEnable { get; set; }
		public StencilState StencilState { get; set; }
	}
}
