using System;
using System.Collections;

namespace MESLabel
{
	internal class PrinterName : MESFuncObject
	{
		public PrinterName(string name) : base(name, 0)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			if (Params.Count == this.ParamNum)
			{
				return MESLabel.PrinterName;
			}
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
