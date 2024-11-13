using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesLotControl
	{
		public int ID { get; set; }
		public int PartID { get; set; }
		public string LotCode { get; set; }
		public string DateCode { get; set; }
		public string Supplier { get; set; }
		public int MachineID { get; set; }
		public int SlotID { get; set; }
		public string Position { get; set; }
		public int Quantity { get; set; }
		public DateTime? StartedTime { get; set; }
		public DateTime? FinishTime { get; set; }
		public int StationID { get; set; }
		public int EmployeeID { get; set; }
	}
}
