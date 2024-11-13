using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class Bool : MESFuncObject
	{
		public Bool(string name) : base(name, 3)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			if (Params.Count != this.ParamNum)
			{
				return "";
			}
			object left = Params[1];
			if (Operators.ConditionalCompareObjectEqual(left, ">", false))
			{
				return Conversions.ToString(Operators.CompareString(Conversions.ToString(Params[0]), Conversions.ToString(Params[2]), false) > 0);
			}
			if (Operators.ConditionalCompareObjectEqual(left, "<", false))
			{
				return Conversions.ToString(Operators.CompareString(Conversions.ToString(Params[0]), Conversions.ToString(Params[2]), false) < 0);
			}
			if (Operators.ConditionalCompareObjectEqual(left, "<>", false))
			{
				return Conversions.ToString(Operators.CompareString(Conversions.ToString(Params[0]), Conversions.ToString(Params[2]), false) != 0);
			}
			if (Operators.ConditionalCompareObjectEqual(left, "=", false))
			{
				return Conversions.ToString(Operators.CompareString(Conversions.ToString(Params[0]), Conversions.ToString(Params[2]), false) == 0);
			}
			if (Operators.ConditionalCompareObjectEqual(left, ">=", false))
			{
				return Conversions.ToString(Operators.CompareString(Conversions.ToString(Params[0]), Conversions.ToString(Params[2]), false) >= 0);
			}
			if (Operators.ConditionalCompareObjectEqual(left, "<=", false))
			{
				return Conversions.ToString(Operators.CompareString(Conversions.ToString(Params[0]), Conversions.ToString(Params[2]), false) <= 0);
			}
			throw MESLBLException.CreateInstance(this.Name, Params, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
		}
	}
}
