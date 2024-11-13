using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesRefDesignator
	{
		public int ID { get; set; }
		public int ParentPartID { get; set; }
		public int PartID { get; set; }
		public string Location { get; set; }
		public object Status { get; set; }
	}
}
