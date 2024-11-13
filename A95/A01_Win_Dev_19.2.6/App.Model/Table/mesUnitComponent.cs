using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesUnitComponent
	{
		public int ID { get; set; }
		public int UnitID { get; set; }
		public int UnitComponentTypeID { get; set; }
		public int ChildUnitID { get; set; }
		public string ChildSerialNumber { get; set; }
		public string ChildLotNumber { get; set; }
		public int ChildPartID { get; set; }
		public int ChildPartFamilyID { get; set; }
		public string Position { get; set; }
		public int InsertedEmployeeID { get; set; }
		public int InsertedStationID { get; set; }
		public DateTime? InsertedTime { get; set; }
		public int RemovedEmployeeID { get; set; }
		public int RemovedStationID { get; set; }
		public DateTime? RemovedTime { get; set; }
		public object StatusID { get; set; }
		public DateTime? LastUpdate { get; set; }
		public int PreviousLink { get; set; }
	}
}
