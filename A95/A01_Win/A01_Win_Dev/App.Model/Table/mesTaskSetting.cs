using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesTaskSetting
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int TaskID { get; set; }
		public string Value { get; set; }
		public int Counter { get; set; }
		public int PartID { get; set; }
		public int PartFamilyID { get; set; }
	}
}
