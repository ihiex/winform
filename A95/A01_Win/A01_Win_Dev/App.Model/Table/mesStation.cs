using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesStation
	{
		public int ID { get; set; }
		public string Description { get; set; }
		public int StationTypeID { get; set; }
		public int LineID { get; set; }
		public object Status { get; set; }
	}
}
