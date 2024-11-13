using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Len : MESFuncObject
	{
		public Len(string Name) : base(Name, 1)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			if (Params.Count == this.ParamNum)
			{
				try
				{
					string text = Conversions.ToString(Params[0]);
					return Conversions.ToString(text.Length);
				}
				catch (Exception expr_29)
				{
					ProjectData.SetProjectError(expr_29);
					MESLBLException.CreateInstance(this.Name, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR);
					ProjectData.ClearProjectError();
					goto IL_52;
				}
				goto IL_44;
				IL_52:
				return "";
			}
			IL_44:
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
