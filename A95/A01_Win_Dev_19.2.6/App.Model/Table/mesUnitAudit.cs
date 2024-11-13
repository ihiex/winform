using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesUnitAudit
	{
		public int ID { get; set; }
		public int UnitID { get; set; }
		public int PackageID { get; set; }
		public int AuditLotID { get; set; }
		public object AuditStatusID { get; set; }
		public int FailureCategoryID { get; set; }
		public int ProductionOrderID { get; set; }
		public int PartID { get; set; }
		public int LooperCount { get; set; }
	}
}
