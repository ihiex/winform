using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	internal class HexToDec : MESFuncObject
	{
		public HexToDec(string name) : base(name, 1)
		{
		}

		public override string Evaluate(ArrayList Params)
		{
			string result = "";
			if (Params.Count == this.ParamNum)
			{
				string hex = Conversions.ToString(0);
				try
				{
					hex = Conversions.ToString(Params[0]);
					result = this.HextoDec(hex);
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

		private string HextoDec(string hex)
		{
			object left = hex.Length;
			int num = 0;
			int num2 = 0;
			checked
			{
				while (Operators.ConditionalCompareObjectGreater(left, 0, false))
				{
					char value = Conversions.ToChar(hex.Substring(Conversions.ToInteger(Operators.SubtractObject(left, 1)), 1));
					int num3 = 0;
					object left2 = num2;
					switch (value)
					{
					case 'A':
						num3 = 10;
						break;
					case 'B':
						num3 = 11;
						break;
					case 'C':
						num3 = 12;
						break;
					case 'D':
						num3 = 13;
						break;
					case 'E':
						num3 = 14;
						break;
					case 'F':
						num3 = 15;
						break;
					default:
						try
						{
							num3 = Conversions.ToInteger(Conversions.ToString(value));
						}
						catch (Exception expr_97)
						{
							ProjectData.SetProjectError(expr_97);
							Exception ex = expr_97;
							throw MESLBLException.CreateInstance(this.Name, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
						}
						break;
					}
					while (Operators.ConditionalCompareObjectGreater(left2, 0, false))
					{
						num3 *= 16;
						left2 = Operators.SubtractObject(left2, 1);
					}
					num += num3;
					num2++;
					left = Operators.SubtractObject(left, 1);
				}
				return Conversions.ToString(num);
			}
		}
	}
}
