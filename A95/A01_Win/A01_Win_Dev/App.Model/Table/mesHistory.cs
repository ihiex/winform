using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesHistory
	{
		public int ID { get; set; }
		public int UnitID { get; set; }
		public int UnitStateID { get; set; }
		public int EmployeeID { get; set; }
		public int StationID { get; set; }
		public DateTime? EnterTime { get; set; }
		public DateTime? ExitTime { get; set; }
		public int ProductionOrderID { get; set; }
		public int PartID { get; set; }
		public int LooperCount { get; set; }
	}
}
