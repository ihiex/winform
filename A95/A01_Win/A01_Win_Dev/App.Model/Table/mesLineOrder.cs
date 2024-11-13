using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesLineOrder
	{
		public int ID { get; set; }
		public string Description { get; set; }
		public int LineQuantity { get; set; }
		public DateTime? CreationTime { get; set; }
		public int StartedQuantity { get; set; }
		public int ReadyQuantity { get; set; }
		public int AllowOverBuild { get; set; }
		public int LineID { get; set; }
		public int ProductionOrderID { get; set; }
		public int Priority { get; set; }
	}
}
