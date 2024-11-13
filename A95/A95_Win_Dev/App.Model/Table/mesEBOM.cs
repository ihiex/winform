using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesEBOM
	{
		public int ID { get; set; }
		public int ProductionOrderID { get; set; }
		public int PartID { get; set; }
		public string PartPosition { get; set; }
		public object Scannable { get; set; }
		public int ParentID { get; set; }
		public DateTime? CreationTime { get; set; }
	}
}
