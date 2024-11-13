using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class PageNumber : MESFuncObject
	{
		public PageNumber(string name) : base(name, 0)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			if (Params.Count != this.ParamNum)
			{
				throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
			}
			if (MESLabel.LabelObj == null)
			{
				return "";
			}
			MESLabel labelObj = MESLabel.LabelObj;
			if (labelObj.CAP > 0)
			{
				return Conversions.ToString(Math.Ceiling((double)labelObj.LineRecordNumber / (double)labelObj.CAP));
			}
			return Conversions.ToString(1);
		}
	}
}
