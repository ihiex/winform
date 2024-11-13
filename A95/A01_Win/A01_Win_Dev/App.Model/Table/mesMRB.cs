using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesMRB
	{
		public int ID { get; set; }
		public int ComponentPartID { get; set; }
		public string ComponentSerialNumber { get; set; }
		public string ComponentLotNumber { get; set; }
		public object StatusID { get; set; }
		public int EmployeeID { get; set; }
		public int StationID { get; set; }
		public DateTime? Time { get; set; }
		public int UnitID { get; set; }
		public int LooperCount { get; set; }
		public string Position { get; set; }
	}
}
