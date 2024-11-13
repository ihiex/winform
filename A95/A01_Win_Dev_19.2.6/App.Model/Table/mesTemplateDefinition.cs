using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesTemplateDefinition
	{
		public int ID { get; set; }
		public int TemplateID { get; set; }
		public int BasicFunctionID { get; set; }
		public object Order { get; set; }
		public bool IsTaskGroup { get; set; }
	}
}
