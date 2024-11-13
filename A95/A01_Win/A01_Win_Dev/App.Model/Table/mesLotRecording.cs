using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesLotRecording
	{
		public int ID { get; set; }
		public int UnitID { get; set; }
		public int LotControlID { get; set; }
		public int StationID { get; set; }
		public int EmployeeID { get; set; }
		public DateTime? Time { get; set; }
	}
}
