using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesPackage
	{
		public int ID { get; set; }
		public string SerialNumber { get; set; }
		public int StationID { get; set; }
		public int EmployeeID { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CurrentCount { get; set; }
		public object StatusID { get; set; }
		public DateTime? LastUpdate { get; set; }
		public int ParentID { get; set; }
		public int Stage { get; set; }
		public int CurrProductionOrderID { get; set; }
		public int CurrPartID { get; set; }
	}
}
