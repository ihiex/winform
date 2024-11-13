using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesMachine
	{
		public int ID { get; set; }
		public int StationID { get; set; }
		public string SN { get; set; }
		public int RuningQuantity { get; set; }
		public int MaxUseQuantity { get; set; }
		public DateTime? StartRuningTime { get; set; }
		public DateTime? LastRuningTime { get; set; }
		public int MachineFamilyID { get; set; }
		public int PartID { get; set; }
		public int WarningStatus { get; set; }
		public string Discription { get; set; }
		public int Status { get; set; }
		public string ValidFrom { get; set; }
		public string ValidTo { get; set; }
		public string PartNO { get; set; }
		public string PartDesc { get; set; }
		public string PartGroup { get; set; }
		public string PartGroupDesc { get; set; }
		public DateTime? LastMaintenance { get; set; }
		public DateTime? NextMaintenance { get; set; }
		public string Store { get; set; }
		public string Attributes { get; set; }
	}
}
