using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesPartDependent
	{
		public int ID { get; set; }
		public int ParentPartID { get; set; }
		public int PartID { get; set; }
		public string PartPosition { get; set; }
		public int DepPartID { get; set; }
		public string DepPartPosition { get; set; }
		public object Order { get; set; }
		public object Status { get; set; }
	}
}
