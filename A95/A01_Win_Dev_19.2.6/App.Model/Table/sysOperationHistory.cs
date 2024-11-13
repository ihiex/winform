using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class sysOperationHistory
	{
		public int ID { get; set; }
		public string Category { get; set; }
		public DateTime? Time { get; set; }
		public int EmployeeID { get; set; }
		public string Operation { get; set; }
	}
}
