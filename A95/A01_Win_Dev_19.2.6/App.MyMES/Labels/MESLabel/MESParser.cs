using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace MESLabel
{
	public class MESParser
	{
		private static MESDBFTable m_table;

		public static string Evaluate(ArrayList topTokens)
		{
			string text = "";
			try
			{
				IEnumerator enumerator = topTokens.GetEnumerator();
				while (enumerator.MoveNext())
				{
					object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
					text += MESParser.Evaluate(Conversions.ToString(objectValue));
				}
			}
			finally
			{
				//IEnumerator enumerator;
				//if (enumerator is IDisposable)
				//{
				//	(enumerator as IDisposable).Dispose();
				//}
			}
			return text;
		}

		public static string Evaluate(string token)
		{
			string result = "";
			if (token.Length > 0)
			{
				char value = Conversions.ToChar(token.Substring(0, 1));
				if (Operators.CompareString(Conversions.ToString(value), "\"", false) == 0)
				{
					result = token.Substring(1, checked(token.Length - 2));
				}
				else
				{
					if (token.IndexOf(".") > -1 & token.IndexOf(".") < token.IndexOf("("))
					{
						return MESParser.GetDBField(token);
					}
					if (token.IndexOf("(") <= -1)
					{
						return token;
					}
					result = MESParser.GetFunction(token);
				}
			}
			return result;
		}

		private static string GetDBField(string token)
		{
			ArrayList tokens = MESScaner.GetTokens(token, "-");
			if (tokens.Count > 3)
			{
				throw MESLBLException.CreateInstance(token, MESLBLException.ErrCode.DBFIELD_OP_ERROR);
			}
			string dBField = MESParser.m_table.GetDBField(Conversions.ToString(tokens[0]));
			if (dBField == null)
			{
				throw MESLBLException.CreateInstance(token, MESLBLException.ErrCode.DBFIELD_NOTFOUND_ERROR);
			}
			string result="";
			try
			{
				switch (tokens.Count)
				{
				case 1:
					result = dBField;
					break;
				case 2:
					result = dBField.Substring(Conversions.ToInteger(tokens[1]));
					break;
				case 3:
					result = dBField.Substring(Conversions.ToInteger(tokens[1]), checked(Conversions.ToInteger(tokens[2]) - Conversions.ToInteger(tokens[1]) + 1));
					break;
				}
			}
			catch (Exception expr_A3)
			{
				ProjectData.SetProjectError(expr_A3);
				Exception ex = expr_A3;
				throw MESLBLException.CreateInstance(token, MESLBLException.ErrCode.DBFIELD_OP_ERROR, ex.Message);
			}
			return result;
		}

		private static string GetFunction(string token)
		{
			ArrayList tokens = MESScaner.GetTokens(token, ",");
			MESFuncObjFactory fFFuncObjFactory = MESFuncObjFactory.CreateInstance();
			if (tokens.Count <= 0)
			{
				throw MESLBLException.CreateInstance(token, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
			}
			string text = "";
			try
			{
				text = Conversions.ToString(tokens[0]);
			}
			catch (Exception expr_35)
			{
				ProjectData.SetProjectError(expr_35);
				throw MESLBLException.CreateInstance(MESLBLException.ErrCode.Unknown);
			}
			MESFuncObject fFFuncObject;
			if (Operators.CompareString(text.ToUpper(), "IF", false) == 0)
			{
				if (tokens.Count == 4)
				{
					string text2 = "";
					try
					{
						text2 = Conversions.ToString(tokens[1]);
					}
					catch (Exception expr_7F)
					{
						ProjectData.SetProjectError(expr_7F);
						throw MESLBLException.CreateInstance(MESLBLException.ErrCode.Unknown);
					}
					ArrayList arrayList = new ArrayList();
					MESScaner.GetBoolTokens(text2, "<>=", arrayList);
					if (arrayList.Count == 3)
					{
						try
						{
							string text3 = Conversions.ToString(arrayList[0]);
							string value = Conversions.ToString(arrayList[1]);
							string text4 = Conversions.ToString(arrayList[2]);
							text3 = MESParser.Evaluate(text3);
							text4 = MESParser.Evaluate(text4);
							arrayList.Clear();
							arrayList.Add(text3);
							arrayList.Add(value);
							arrayList.Add(text4);
							fFFuncObject = fFFuncObjFactory.BoolFuncObj;
							if (fFFuncObject == null)
							{
								throw MESLBLException.CreateInstance("Bool", MESLBLException.ErrCode.FUNC_NOTFOUND_ERROR);
							}
							tokens[1] = fFFuncObject.Evaluate(arrayList);
							goto IL_165;
						}
						catch (Exception expr_13C)
						{
							ProjectData.SetProjectError(expr_13C);
							Exception ex = expr_13C;
							throw MESLBLException.CreateInstance(text2, MESLBLException.ErrCode.FUNC_PARAM_TYPE_ERROR, ex.Message);
						}
					}
					throw MESLBLException.CreateInstance(text2, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
				}
				throw MESLBLException.CreateInstance(token, MESLBLException.ErrCode.FUNC_PARAM_NUM_ERROR);
			}
			IL_165:
			fFFuncObject = fFFuncObjFactory.GetFuncObject(text);
			if (fFFuncObject == null)
			{
				throw MESLBLException.CreateInstance(text, MESLBLException.ErrCode.FUNC_NOTFOUND_ERROR);
			}
			tokens.RemoveAt(0);
			checked
			{
				if (tokens.Count > 0)
				{
					int arg_195_0 = 0;
					int num = tokens.Count - 1;
					for (int i = arg_195_0; i <= num; i++)
					{
						tokens[i] = MESParser.Evaluate(Conversions.ToString(tokens[i]));
					}
				}
				return fFFuncObject.Evaluate(tokens);
			}
		}

		public static string GetValue(ArrayList tokens, MESDBFTable table)
		{
			MESParser.m_table = table;
			return MESParser.Evaluate(tokens);
		}
	}
}
