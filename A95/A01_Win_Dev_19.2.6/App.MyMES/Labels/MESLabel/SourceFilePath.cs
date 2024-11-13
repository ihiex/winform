using System;
using System.Collections;

namespace MESLabel
{
	internal class SourceFilePath : MESFuncObject
	{
		public SourceFilePath(string name) : base(name, 0)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			if (Params.Count != this.ParamNum)
			{
				throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
			}
			if (MESLabel.LabelObj != null)
			{
				return MESLabel.LabelObj.SFP;
			}
			return "";
		}
	}
}
