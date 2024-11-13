using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesDebug
	{
		public int ID { get; set; }
		public int UnitID { get; set; }
		public int TestID { get; set; }
		public int PartID { get; set; }
		public string Location { get; set; }
		public int DefectID { get; set; }
		public string Comment { get; set; }
		public int StationID { get; set; }
		public int EmployeeID { get; set; }
		public DateTime? Time { get; set; }
	}
}
