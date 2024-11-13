using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesMeasurementGroupDetail
	{
		public int ID { get; set; }
		public int MeasurementGroupID { get; set; }
		public int MeasurementGroupDetailDefID { get; set; }
		public string Content { get; set; }
	}
}
