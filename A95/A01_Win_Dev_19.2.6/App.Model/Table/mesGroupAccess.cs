using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesGroupAccess
	{
		public int ID { get; set; }
		public int ApplicationTypeID { get; set; }
		public int ApplicationSectionID { get; set; }
		public int ApplicationOperationID { get; set; }
		public int EmployeeGroupID { get; set; }
		public object Status { get; set; }
	}
}
