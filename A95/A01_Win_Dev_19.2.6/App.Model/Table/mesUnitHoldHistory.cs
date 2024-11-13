using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesUnitHoldHistory
	{
		public int ID { get; set; }
		public int UnitID { get; set; }
		public string HoldType { get; set; }
		public int HoldEmployeeID { get; set; }
		public int HoldStationID { get; set; }
		public DateTime? HoldTime { get; set; }
		public int ReleasedEmployeeID { get; set; }
		public int ReleasedStationID { get; set; }
		public DateTime? ReleasedTime { get; set; }
		public int ProductionOrderID { get; set; }
		public int LooperCount { get; set; }
	}
}
