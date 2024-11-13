using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Month : MESFuncObject
	{
		public Month(string name) : base(name, 0)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			string result = "";
			if (Params.Count == this.ParamNum)
			{
				try
				{
					int num = Conversions.ToInteger(Conversions.ToString(DateAndTime.DatePart(DateInterval.Month, DateTime.Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)));
					if (num < 10)
					{
						result = "0" + Conversions.ToString(num);
						return result;
					}
					result = Conversions.ToString(num);
					return result;
				}
				catch (Exception expr_4F)
				{
					ProjectData.SetProjectError(expr_4F);
					Exception ex = expr_4F;
					throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
				}
			}
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
