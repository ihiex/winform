using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Right : MESFuncObject
	{
		public Right(string name) : base(name, 2)
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
						return text.Substring(checked(text.Length - num));
					}
					goto IL_6D;
				}
				catch (Exception expr_44)
				{
					ProjectData.SetProjectError(expr_44);
					Exception ex = expr_44;
					throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
				}
				goto IL_5F;
				IL_6D:
				return "";
			}
			IL_5F:
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
