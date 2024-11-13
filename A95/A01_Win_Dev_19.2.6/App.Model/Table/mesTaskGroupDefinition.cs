using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesTaskGroupDefinition
	{
		public int ID { get; set; }
		public int TaskGroupID { get; set; }
		public int TaskID { get; set; }
		public int PartID { get; set; }
		public int PartFamilyID { get; set; }
		public object Order { get; set; }
	}
}
