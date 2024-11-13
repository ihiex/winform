using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesStationAvailability
	{
		public int ID { get; set; }
		public int StationID { get; set; }
		public DateTime? StopTime { get; set; }
		public int StopEmployeeID { get; set; }
		public int StopReasonID { get; set; }
		public DateTime? RestartTime { get; set; }
		public int RestartEmployeeID { get; set; }
	}
}
