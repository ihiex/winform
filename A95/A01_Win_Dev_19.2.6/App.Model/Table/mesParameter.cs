using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesParameter
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int ApplicationTypeID { get; set; }
		public string Value { get; set; }
		public int Counter { get; set; }
		public string InputType { get; set; }
		public string ValueList { get; set; }
		public int ParentID { get; set; }
		public string Condition { get; set; }
		public int HelpCode { get; set; }
		public object Order { get; set; }
		public object Status { get; set; }
	}
}
