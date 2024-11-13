using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesNStrikeDefaultSettingRule
	{
		public int ID { get; set; }
		public int NStrikeDefaultSettingID { get; set; }
		public string Rule { get; set; }
		public string Action { get; set; }
		public string ActionParameter { get; set; }
		public object Priority { get; set; }
	}
}
