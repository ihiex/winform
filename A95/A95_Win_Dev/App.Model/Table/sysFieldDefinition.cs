using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class sysFieldDefinition
	{
		public int ID { get; set; }
		public string TableName { get; set; }
		public string FieldName { get; set; }
		public string Definition { get; set; }
		public string Description { get; set; }
	}
}
