using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesMeasurementSection
	{
		public int ID { get; set; }
		public int MeasurementGroupID { get; set; }
		public int ControlChartID { get; set; }
		public int ControlParameterID { get; set; }
		public string Value { get; set; }
	}
}
