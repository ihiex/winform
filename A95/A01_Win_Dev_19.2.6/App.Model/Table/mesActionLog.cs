using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesActionLog
	{
		public int ID { get; set; }
		public string Description { get; set; }
		public int ActionID { get; set; }
		public int ApproverID { get; set; }
		public int EmployeeID { get; set; }
		public int StationID { get; set; }
		public DateTime? Time { get; set; }
		public bool SendStatus { get; set; }
		public bool SendNotification { get; set; }
	}
}
