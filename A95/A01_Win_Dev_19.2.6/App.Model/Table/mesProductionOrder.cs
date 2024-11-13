using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesProductionOrder
	{
		public int ID { get; set; }
		public string ProductionOrderNumber { get; set; }
		public string Description { get; set; }
		public int PartID { get; set; }
		public int OrderQuantity { get; set; }
		public int EmployeeID { get; set; }
		public DateTime? CreationTime { get; set; }
		public DateTime? LastUpdate { get; set; }
		public int StatusID { get; set; }
		public DateTime? RequestedStartTime { get; set; }
		public DateTime? ActualStartTime { get; set; }
		public DateTime? RequestedFinishTime { get; set; }
		public DateTime? ActualFinishTime { get; set; }
		public DateTime? ShippedTime { get; set; }
		public string UOM { get; set; }
		public int PriorityByERP { get; set; }
		public bool IsLotAuditCompleted { get; set; }
	}
}
