using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;

namespace MESLabel
{
	public class MESDBFTable
	{
		private Hashtable m_ht;

		public MESDBFTable()
		{
			this.m_ht = new Hashtable();
		}

		public void AddDBField(string name, string value)
		{
			this.m_ht.Add(name, value);
		}

		public string GetDBField(string name)
		{
			return Conversions.ToString(this.m_ht[name]);
		}
	}
}
