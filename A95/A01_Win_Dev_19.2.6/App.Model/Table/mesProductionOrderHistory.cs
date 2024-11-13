using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesProductionOrderHistory
	{
		public int ID { get; set; }
		public int ProductionOrderID { get; set; }
		public object ProductionOrderStatusID { get; set; }
		public DateTime? Time { get; set; }
	}
}
