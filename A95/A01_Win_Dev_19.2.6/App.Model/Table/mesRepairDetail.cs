using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesRepairDetail
	{
		public int ID { get; set; }
		public int RepairID { get; set; }
		public int RepairDetailDefID { get; set; }
		public string Content { get; set; }
	}
}
