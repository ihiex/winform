using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesUnitOutputState
	{
		public int ID { get; set; }
		public int StationTypeID { get; set; }
		public int RouteID { get; set; }
		public int CurrStateID { get; set; }
		public int OutputStateID { get; set; }
		public int OutputStateDefID { get; set; }
	}
}
