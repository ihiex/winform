using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesAuditThreshold
	{
		public int ID { get; set; }
		public int AuditLotID { get; set; }
		public int FailureCategoryID { get; set; }
		public int Threshold { get; set; }
	}
}
