using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class DayOfYear : MESFuncObject
	{
		public DayOfYear(string name) : base(name, 0)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			if (Params.Count == this.ParamNum)
			{
				int num = DateAndTime.DatePart(DateInterval.DayOfYear, DateTime.Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
				string result;
				if (num < 10)
				{
					result = "0" + Conversions.ToString(num);
				}
				else
				{
					result = Conversions.ToString(num);
				}
				return result;
			}
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
