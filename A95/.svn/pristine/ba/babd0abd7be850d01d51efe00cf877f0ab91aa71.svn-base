using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Hour24 : MESFuncObject
	{
		public Hour24(string name) : base(name, 0)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			string result = "";
			if (Params.Count == this.ParamNum)
			{
				try
				{
					int num = DateAndTime.DatePart(DateInterval.Hour, DateTime.Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
					if (num < 10)
					{
						result = "0" + Conversions.ToString(num);
						return result;
					}
					result = Conversions.ToString(num);
					return result;
				}
				catch (Exception expr_45)
				{
					ProjectData.SetProjectError(expr_45);
					Exception ex = expr_45;
					throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
				}
			}
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
