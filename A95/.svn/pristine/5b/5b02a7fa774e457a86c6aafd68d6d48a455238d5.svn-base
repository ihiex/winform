using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	public class MESLBLException : ApplicationException
	{
		public enum ErrCode
		{
			Unknown,
			EXCEPTION_EXCEPTION,
			SYNX_PARENTHESES_ERROR,
			SYNX_QUOTES_ERROR,
			FUNC_PARAM_NUM_ERROR,
			FUNC_PARAM_TYPE_ERROR,
			FUNC_NOTFOUND_ERROR,
			DBFIELD_OP_ERROR,
			DBFIELD_NOTFOUND_ERROR,
			DB_OP_ERROR,
			INVALID_XML_FORMAT,
			FILE_OP_ERROR,
			BATCH_OP_ERROR,
			NO_DATA_ERROR,
			ZPL_TEM_ERROR
		}

		private const string m_seperator = ":";

		private static string[] m_ErrMsgs = new string[]
		{
			"Unknown exception occurs",
			"Exception occurs while generating exception",
			"Parentheses are not matched",
			"Quotes are not matched",
			"Function doesn't take this number of parameters",
			"Function doesn't take such type(s) of parameters",
			"No such function is defined",
			"Invalid database field operation",
			"No such database field is defined",
			"Errors in Database operation",
			"Invalid XML format",
			"Error occured in file operation",
			"Error occured in triggering the batch file",
			"Not all data available for this Label",
			"Data for ZPL Template is invalid."
		};

		public static MESLBLException CreateInstance(string preFix, MESLBLException.ErrCode errCode)
		{
			return new MESLBLException(preFix, errCode);
		}

		public static MESLBLException CreateInstance(string func, ArrayList @params, MESLBLException.ErrCode errCode, string detail)
		{
			detail = Conversions.ToString(Operators.AddObject(Operators.AddObject(MESLBLException.GetFunc(func, @params), ":"), detail));
			return new MESLBLException(errCode, detail);
		}

		public static MESLBLException CreateInstance(string func, MESLBLException.ErrCode errCode, string detail)
		{
			return new MESLBLException(func, errCode, detail);
		}

		public static MESLBLException CreateInstance(string func, ArrayList @params, MESLBLException.ErrCode errCode)
		{
			if (@params != null)
			{
				string detail = Conversions.ToString(MESLBLException.GetFunc(func, @params));
				return new MESLBLException(errCode, detail);
			}
			throw new MESLBLException(MESLBLException.ErrCode.Unknown);
		}

		public static MESLBLException CreateInstance(MESLBLException.ErrCode errCode, string detail)
		{
			return new MESLBLException(errCode, detail);
		}

		public static MESLBLException CreateInstance(MESLBLException.ErrCode errcode)
		{
			return new MESLBLException(errcode);
		}

		private static object GetFunc(string func, ArrayList @params)
		{
			string text = "";
			text = func + "(";
			checked
			{
				if (@params != null)
				{
					try
					{
						if (@params.Count > 0)
						{
							int arg_29_0 = 0;
							int num = @params.Count - 1;
							for (int i = arg_29_0; i <= num; i++)
							{
								text += Conversions.ToString(@params[i]);
								if (i != @params.Count - 1)
								{
									text += ",";
								}
							}
						}
					}
					catch (Exception expr_61)
					{
						ProjectData.SetProjectError(expr_61);
						Exception ex = expr_61;
						throw new MESLBLException(MESLBLException.ErrCode.EXCEPTION_EXCEPTION, ex.Message);
					}
				}
				text += ")";
				return text;
			}
		}

		private MESLBLException(string ErrMsg) : base(ErrMsg)
		{
		}

		private MESLBLException(MESLBLException.ErrCode errCode) : base(MESLBLException.GetErrorMsg(errCode))
		{
		}

		private MESLBLException(string preFix, MESLBLException.ErrCode errCode) : base(preFix + ":" + MESLBLException.GetErrorMsg(errCode))
		{
		}

		private MESLBLException(MESLBLException.ErrCode errCode, string detail) : base(MESLBLException.GetErrorMsg(errCode) + ":" + detail)
		{
		}

		private MESLBLException(string preFix, MESLBLException.ErrCode errCode, string detail) : base(string.Concat(new string[]
		{
			MESLBLException.GetErrorMsg(errCode),
			":",
			preFix,
			":",
			detail
		}))
		{
		}

		private static string GetErrorMsg(MESLBLException.ErrCode errCode)
		{
			return MESLBLException.m_ErrMsgs[(int)errCode];
		}
	}
}
