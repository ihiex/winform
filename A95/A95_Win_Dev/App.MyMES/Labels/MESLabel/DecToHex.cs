using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class DecToHex : MESFuncObject
	{
		public DecToHex(string name) : base(name, 1)
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
					result = this.DectoHex(dec);
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

		private string DectoHex(int dec)
		{
			if (dec < 10)
			{
				return Conversions.ToString(dec);
			}
			checked
			{
				if (dec < 16)
				{
					char value = Strings.Chr(55 + dec);
					return Conversions.ToString(value);
				}
				int num = dec % 16;
				int dec2 = (int)Math.Round(Math.Floor((double)(dec - num) / 16.0));
				return this.DectoHex(dec2) + this.DectoHex(num);
			}
		}
	}
}
