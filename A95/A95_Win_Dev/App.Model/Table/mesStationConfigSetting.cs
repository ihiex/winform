using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesStationConfigSetting
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int StationID { get; set; }
		public string Value { get; set; }
		public int Counter { get; set; }
	}
}