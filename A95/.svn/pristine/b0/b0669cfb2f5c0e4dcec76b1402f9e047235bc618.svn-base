using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Centry : MESFuncObject
	{
		public Centry(string name) : base(name, 0)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			string text = "";
			if (Params.Count == this.ParamNum)
			{
				try
				{
					text = Conversions.ToString(DateAndTime.DatePart(DateInterval.Year, DateTime.Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1));
					text = text.Substring(0, 2);
					return text;
				}
				catch (Exception expr_32)
				{
					ProjectData.SetProjectError(expr_32);
					Exception ex = expr_32;
					throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
				}
			}
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
