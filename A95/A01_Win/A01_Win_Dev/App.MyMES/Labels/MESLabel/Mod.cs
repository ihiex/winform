using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Mod : MESFuncObject
	{
		public Mod(string Name) : base(Name, 2)
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
					return Conversions.ToString(num % num2);
				}
				catch (Exception expr_33)
				{
					ProjectData.SetProjectError(expr_33);
					MESLBLException.CreateInstance(this.Name, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR);
					ProjectData.ClearProjectError();
					goto IL_5C;
				}
				goto IL_4E;
				IL_5C:
				return "";
			}
			IL_4E:
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
