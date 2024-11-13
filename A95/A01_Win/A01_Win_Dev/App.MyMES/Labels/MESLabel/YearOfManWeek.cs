using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class YearOfManWeek : MESFuncObject
	{
		public YearOfManWeek(string name) : base(name, 0)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			string text = "";
			DateTime now = DateTime.Now;
			checked
			{
				if (Params.Count == this.ParamNum)
				{
					try
					{
						int num = DateAndTime.DatePart(DateInterval.Year, now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						if (DateAndTime.DatePart(DateInterval.DayOfYear, now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 4 && DateAndTime.DatePart(DateInterval.Weekday, now, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1) > 4)
						{
							num--;
						}
						text = num.ToString();
						text = text.Substring(2, 2);
						return text;
					}
					catch (Exception expr_53)
					{
						ProjectData.SetProjectError(expr_53);
						Exception ex = expr_53;
						throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
					}
				}
				throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
			}
		}
	}
}
