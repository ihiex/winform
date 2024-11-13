using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesTemplate
	{
		public int ID { get; set; }
		public string Description { get; set; }
		public object TemplateCategoryID { get; set; }
		public bool IsUserDefined { get; set; }
		public bool ShowDefault { get; set; }
	}
}
