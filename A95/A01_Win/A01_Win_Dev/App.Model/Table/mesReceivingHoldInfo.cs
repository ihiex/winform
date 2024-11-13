using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesReceivingHoldInfo
	{
		public int ID { get; set; }
		public string SerialNumber { get; set; }
		public string ProductionOrderNumber { get; set; }
		public object StatusID { get; set; }
		public string Accessories { get; set; }
		public string ToteNumber { get; set; }
		public string Comment { get; set; }
		public int HoldEmployeeID { get; set; }
		public int HoldStationID { get; set; }
		public DateTime? HoldTime { get; set; }
		public int ReleasedEmployeeID { get; set; }
		public int RelesedStationID { get; set; }
		public DateTime? ReleasedTime { get; set; }
	}
}
