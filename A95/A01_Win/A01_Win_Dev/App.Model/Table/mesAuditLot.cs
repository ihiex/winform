using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesAuditLot
	{
		public int ID { get; set; }
		public string Description { get; set; }
		public int LotSize { get; set; }
		public int SampleSize { get; set; }
		public object AuditStatusID { get; set; }
		public int EmployeeID { get; set; }
		public int StationID { get; set; }
	}
}
