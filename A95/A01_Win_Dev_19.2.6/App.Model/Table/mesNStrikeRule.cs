using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesNStrikeRule
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int NStrikeGroupID { get; set; }
		public int NStrikeTypeID { get; set; }
		public int PartID { get; set; }
		public int PartFamilyID { get; set; }
	}
}
