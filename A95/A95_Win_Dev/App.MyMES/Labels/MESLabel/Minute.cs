using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Minute : MESFuncObject
	{
		public Minute(string name) : base(name, 0)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			string result = "";
			checked
			{
				if (Params.Count == this.ParamNum)
				{
					try
					{
						int num = DateAndTime.DatePart(DateInterval.Minute, DateTime.Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						num--;
						if (num < 10)
						{
							result = "0" + Conversions.ToString(num);
						}
						else
						{
							result = Conversions.ToString(num);
						}
					}
					catch (Exception expr_49)
					{
						ProjectData.SetProjectError(expr_49);
						Exception ex = expr_49;
						throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
					}
					return result;
				}
				throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
			}
		}
	}
}
