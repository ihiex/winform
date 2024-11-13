using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesPartFamilyStructure
	{
		public int ID { get; set; }
		public int ParentPartFamilyID { get; set; }
		public int PartFamilyID { get; set; }
		public object Status { get; set; }
	}
}
