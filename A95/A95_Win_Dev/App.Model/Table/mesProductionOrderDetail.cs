using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesProductionOrderDetail
	{
		public int ID { get; set; }
		public int ProductionOrderID { get; set; }
		public int ProductionOrderDetailDefID { get; set; }
		public string Content { get; set; }
	}
}
