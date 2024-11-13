using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Substr : MESFuncObject
	{
		public Substr(string name) : base(name, 3)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			checked
			{
				if (Params.Count == this.ParamNum)
				{
					try
					{
						string text = Conversions.ToString(Params[0]);
						int num = Conversions.ToInteger(Params[1]);
						int num2 = Conversions.ToInteger(Params[2]);
						if (num2 > 0 & num > -1 & num2 + num - 1 <= text.Length)
						{
							return text.Substring(num - 1, num2);
						}
						goto IL_8B;
					}
					catch (Exception expr_60)
					{
						ProjectData.SetProjectError(expr_60);
						Exception ex = expr_60;
						throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
					}
					goto IL_7D;
					IL_8B:
					return "";
				}
				IL_7D:
				throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
			}
		}
	}
}
