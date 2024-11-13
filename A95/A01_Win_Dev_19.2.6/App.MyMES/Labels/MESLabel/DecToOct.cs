using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class DecToOct : MESFuncObject
	{
		public DecToOct(string name) : base(name, 1)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			string result = "";
			if (Params.Count == this.ParamNum)
			{
				try
				{
					int dec = Conversions.ToInteger(Params[0]);
					result = this.DectoOct(dec);
					return result;
				}
				catch (Exception expr_2D)
				{
					ProjectData.SetProjectError(expr_2D);
					Exception ex = expr_2D;
					throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
				}
			}
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}

		private string DectoOct(int dec)
		{
			if (dec < 8)
			{
				return Conversions.ToString(dec);
			}
			int num = dec % 8;
			int dec2 = checked((int)Math.Round(Math.Floor((double)(dec - num) / 8.0)));
			return this.DectoOct(dec2) + this.DectoOct(num);
		}
	}
}
