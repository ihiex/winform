using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesInLineQAProcess
	{
		public int ID { get; set; }
		public int StationID { get; set; }
		public int PartID { get; set; }
		public int OutStateDefID { get; set; }
		public int Counter { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? LastUpdate { get; set; }
	}
}
