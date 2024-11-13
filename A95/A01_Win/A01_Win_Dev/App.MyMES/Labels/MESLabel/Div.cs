using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Div : MESFuncObject
	{
		public Div(string Name) : base(Name, 2)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			if (Params.Count == this.ParamNum)
			{
				try
				{
					int num = Conversions.ToInteger(Params[0]);
					int num2 = Conversions.ToInteger(Params[1]);
					return Conversions.ToString((double)num / (double)num2);
				}
				catch (Exception expr_35)
				{
					ProjectData.SetProjectError(expr_35);
					MESLBLException.CreateInstance(this.Name, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR);
					ProjectData.ClearProjectError();
					goto IL_5E;
				}
				goto IL_50;
				IL_5E:
				return "";
			}
			IL_50:
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
