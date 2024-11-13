using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesUnitPartOnHold
	{
		public int ID { get; set; }
		public int UnitID { get; set; }
		public string PartNumber { get; set; }
		public string Revision { get; set; }
		public object StatusID { get; set; }
		public int HoldEmployeeID { get; set; }
		public int HoldStationID { get; set; }
		public DateTime? HoldTime { get; set; }
		public DateTime? PartReceivedTime { get; set; }
		public int ReleasedEmployeeID { get; set; }
		public int ReleasedStationID { get; set; }
		public DateTime? ReleasedTime { get; set; }
		public string Comment { get; set; }
		public int ProductionOrderID { get; set; }
		public int LooperCount { get; set; }
	}
}
