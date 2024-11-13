using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class OctToDec : MESFuncObject
	{
		public OctToDec(string name) : base(name, 1)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			string result = "";
			if (Params.Count == this.ParamNum)
			{
				string oct = Conversions.ToString(0);
				try
				{
					oct = Conversions.ToString(Params[0]);
					result = this.OcttoDec(oct);
					return result;
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

		private string OcttoDec(string oct)
		{
			object left = oct.Length;
			int num = 0;
			int num2 = 0;
			checked
			{
				while (Operators.ConditionalCompareObjectGreater(left, 0, false))
				{
					char value = Conversions.ToChar(oct.Substring(Conversions.ToInteger(Operators.SubtractObject(left, 1)), 1));
					int num3 = 0;
					object left2 = num2;
					try
					{
						num3 = Conversions.ToInteger(Conversions.ToString(value));
						goto IL_80;
					}
					catch (Exception expr_4F)
					{
						ProjectData.SetProjectError(expr_4F);
						Exception ex = expr_4F;
						throw MESLBLException.CreateInstance(this.Name, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
					}
					goto IL_6B;
					IL_80:
					if (!Operators.ConditionalCompareObjectGreater(left2, 0, false))
					{
						num += num3;
						num2++;
						left = Operators.SubtractObject(left, 1);
						continue;
					}
					IL_6B:
					num3 *= 8;
					left2 = Operators.SubtractObject(left2, 1);
					goto IL_80;
				}
				return Conversions.ToString(num);
			}
		}
	}
}
