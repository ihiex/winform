using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesReworkActionStructure
	{
		public int ID { get; set; }
		public int ReworkActionID { get; set; }
		public int ParentID { get; set; }
		public int PartFamilyID { get; set; }
		public int PartID { get; set; }
		public int StationTypeID { get; set; }
	}
}
