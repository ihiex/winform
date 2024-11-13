using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesProductStructure
	{
		public int ID { get; set; }
		public int ParentPartID { get; set; }
		public int PartID { get; set; }
		public string PartPosition { get; set; }
		public bool IsCritical { get; set; }
		public object Status { get; set; }
		public int StationTypeID { get; set; }
	}
}
