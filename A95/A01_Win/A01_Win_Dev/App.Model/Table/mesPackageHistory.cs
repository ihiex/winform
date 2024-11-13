using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesPackageHistory
	{
		public int ID { get; set; }
		public int PackageID { get; set; }
		public object PackageStatusID { get; set; }
		public int StationID { get; set; }
		public int EmployeeID { get; set; }
		public DateTime? Time { get; set; }
	}
}
