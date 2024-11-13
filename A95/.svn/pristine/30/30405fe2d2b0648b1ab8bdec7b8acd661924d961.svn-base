using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class If : MESFuncObject
	{
		public If(string name) : base(name, 3)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			if (Params.Count == this.ParamNum)
			{
				try
				{
					bool flag = Conversions.ToBoolean(Params[0]);
					string result;
					if (flag)
					{
						result = Conversions.ToString(Params[1]);
						return result;
					}
					result = Conversions.ToString(Params[2]);
					return result;
				}
				catch (Exception expr_3C)
				{
					ProjectData.SetProjectError(expr_3C);
					Exception ex = expr_3C;
					throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
				}
			}
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
