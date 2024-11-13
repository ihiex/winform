using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class luMachineFamily
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int MachineFamilyTypeID { get; set; }
		public object Status { get; set; }
	}
}
