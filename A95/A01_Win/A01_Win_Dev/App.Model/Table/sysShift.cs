using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class sysShift
	{
		public int ID { get; set; }
		public string Description { get; set; }
		public string ShiftStartTime { get; set; }
		public string ShiftEndTime { get; set; }
		public object StartStatus { get; set; }
		public object EndStatus { get; set; }
	}
}
