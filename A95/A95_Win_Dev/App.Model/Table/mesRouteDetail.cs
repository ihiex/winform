using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesRouteDetail
	{
		public int ID { get; set; }
		public int RouteID { get; set; }
		public int StationTypeID { get; set; }
		public int Sequence { get; set; }
		public string Description { get; set; }
	}
}
