using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesMeasurementConfig
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int TestTypeID { get; set; }
		public int PartID { get; set; }
		public string Location { get; set; }
		public string Symbol { get; set; }
		public string UOM { get; set; }
		public string UpperLimit { get; set; }
		public string LowerLimit { get; set; }
		public string Sample { get; set; }
		public string UpperRange { get; set; }
		public string LowerRange { get; set; }
		public DateTime? CreationTime { get; set; }
	}
}
