using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	public class MESScaner
	{
		public static ArrayList GetTopTokens(string formula)
		{
			ArrayList arrayList = new ArrayList();
			bool flag = true;
			int num = 0;
			int num2 = 0;
			int i = 0;
			int length = formula.Length;
			if (length == 0)
			{
				return arrayList;
			}
			checked
			{
				while (i <= length - 1)
				{
					char c = Conversions.ToChar(formula.Substring(i, 1));
					bool flag2 = false;
					switch (c)
					{
					case '"':
						flag = !flag;
						if (num == 0 & flag)
						{
							flag2 = true;
						}
						break;
					case '(':
						num++;
						break;
					case ')':
						num--;
						if (num < 0)
						{
							throw MESLBLException.CreateInstance(formula, MESLBLException.ErrCode.SYNX_PARENTHESES_ERROR);
						}
						if (num == 0 & flag)
						{
							flag2 = true;
						}
						break;
					}
					if (flag2)
					{
						arrayList.Add(formula.Substring(num2, i - num2 + 1));
						num2 = i + 1;
						i = num2;
					}
					else
					{
						i++;
					}
				}
				if (num2 != i)
				{
					arrayList.Add(formula.Substring(num2, i - num2));
				}
				if (num != 0)
				{
					throw MESLBLException.CreateInstance(formula, MESLBLException.ErrCode.SYNX_PARENTHESES_ERROR);
				}
				if (!flag)
				{
					throw MESLBLException.CreateInstance(formula, MESLBLException.ErrCode.SYNX_QUOTES_ERROR);
				}
				return arrayList;
			}
		}

		public static ArrayList GetTokens(string token, string seperator)
		{
			ArrayList arrayList = new ArrayList();
			int num = token.IndexOf("(");
			checked
			{
				if (num > 0)
				{
					arrayList.Add(token.Substring(0, num));
					if (token.Length - num - 2 > 0)
					{
						token = token.Substring(num + 1, token.Length - num - 2);
						MESScaner.GetSubTokens(token, seperator, arrayList);
					}
				}
				return arrayList;
			}
		}

		public static bool GetSubTokens(string token, string seperator, ArrayList al)
		{
			bool flag = true;
			int num = 0;
			int num2 = 0;
			int i = 0;
			int length = token.Length;
			if (length == 0)
			{
				return true;
			}
			checked
			{
				while (i <= length - 1)
				{
					char value = Conversions.ToChar(token.Substring(i, 1));
					object left = seperator.IndexOf(value);
					switch (value)
					{
					case '"':
						flag = !flag;
						break;
					case '(':
						num++;
						break;
					case ')':
						num--;
						break;
					}
					if (Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(Operators.CompareObjectGreater(left, -1, false), flag), num == 0)))
					{
						if (num2 < i)
						{
							al.Add(token.Substring(num2, i - num2));
						}
						num2 = i + 1;
						i = num2;
					}
					else
					{
						i++;
					}
				}
				al.Add(token.Substring(num2, i - num2));
				if (num != 0)
				{
					throw MESLBLException.CreateInstance(token, MESLBLException.ErrCode.SYNX_PARENTHESES_ERROR);
				}
				if (!flag)
				{
					throw MESLBLException.CreateInstance(token, MESLBLException.ErrCode.SYNX_QUOTES_ERROR);
				}
				return true;
			}
		}

		public static bool GetBoolTokens(string token, string seperator, ArrayList al)
		{
			bool flag = true;
			int num = 0;
			int num2 = 0;
			int i = 0;
			int length = token.Length;
			if (length == 0)
			{
				return true;
			}
			checked
			{
				while (i < length - 1)
				{
					char value = Conversions.ToChar(token.Substring(i, 1));
					object left = seperator.IndexOf(value);
					switch (value)
					{
					case '"':
						flag = !flag;
						break;
					case '(':
						num++;
						break;
					case ')':
						num--;
						break;
					}
					if (Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(Operators.CompareObjectGreater(left, -1, false), flag), num == 0)))
					{
						if (num2 < i)
						{
							al.Add(token.Substring(num2, i - num2));
						}
						left = seperator.IndexOf(token.Substring(i + 1, 1));
						if (Operators.ConditionalCompareObjectGreater(left, -1, false))
						{
							al.Add(token.Substring(i, 2));
							al.Add(token.Substring(i + 2, token.Length - i - 2));
							break;
						}
						al.Add(token.Substring(i, 1));
						al.Add(token.Substring(i + 1, token.Length - i - 1));
						break;
					}
					else
					{
						i++;
					}
				}
				if (num != 0)
				{
					throw MESLBLException.CreateInstance(token, MESLBLException.ErrCode.SYNX_PARENTHESES_ERROR);
				}
				if (!flag)
				{
					throw MESLBLException.CreateInstance(token, MESLBLException.ErrCode.SYNX_QUOTES_ERROR);
				}
				return true;
			}
		}
	}
}
