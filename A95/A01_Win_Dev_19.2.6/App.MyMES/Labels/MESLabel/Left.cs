using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Left : MESFuncObject
	{
		public Left(string name) : base(name, 2)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			if (Params.Count == this.ParamNum)
			{
				try
				{
					string text = Conversions.ToString(Params[0]);
					int num = Conversions.ToInteger(Params[1]);
					if (num <= text.Length)
					{
						return text.Substring(0, num);
					}
					goto IL_67;
				}
				catch (Exception expr_3E)
				{
					ProjectData.SetProjectError(expr_3E);
					Exception ex = expr_3E;
					throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
				}
				goto IL_59;
				IL_67:
				return "";
			}
			IL_59:
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
