using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesMeasurementSubGroup
	{
		public int ID { get; set; }
		public int MeasurementGroupID { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public int PointCount { get; set; }
		public bool IsActive { get; set; }
		public bool IsPass { get; set; }
		public string Comment { get; set; }
	}
}
