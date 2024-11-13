using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class ApplyMap : MESFuncObject
	{
		public ApplyMap(string name) : base(name, 2)
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
						string text2 = Conversions.ToString(Params[1]);
						text.ToUpper();
						string text3 = Conversions.ToString(text2.Clone());
						text3.ToUpper();
						int num = text3.IndexOf(text);
						num = text3.IndexOf(":", num);
						int num2 = text3.IndexOf(";", num);
						num++;
						return text2.Substring(num, num2 - num);
					}
					catch (Exception expr_88)
					{
						ProjectData.SetProjectError(expr_88);
						Exception ex = expr_88;
						throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
					}
				}
				throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
			}
		}
	}
}
