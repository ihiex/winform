using Microsoft.VisualBasic;
using System;
using System.Collections;

namespace MESLabel
{
	public abstract class MESFuncObject
	{
		private string m_name;

		private int m_paramNum;

		protected const FirstDayOfWeek FIRST_DAY_OF_WEEK = FirstDayOfWeek.Monday;

		protected const FirstWeekOfYear FIRST_WEEK_OF_YEAR = FirstWeekOfYear.FirstFourDays;

		public string Name
		{
			get
			{
				return this.m_name;
			}
		}

		public int ParamNum
		{
			get
			{
				return this.m_paramNum;
			}
		}

		public MESFuncObject(string name, int @params)
		{
			this.m_name = name;
			this.m_paramNum = @params;
		}

		public abstract string Evaluate(ArrayList Params);
	}
}
