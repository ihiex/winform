using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesUnit
	{
		public int ID { get; set; }
		public int UnitStateID { get; set; }
		public int StatusID { get; set; }
		public int StationID { get; set; }
		public int EmployeeID { get; set; }
		public DateTime? CreationTime { get; set; }
		public DateTime? LastUpdate { get; set; }
		public int PanelID { get; set; }
		public int? LineID { get; set; }
        public int? PartFamilyID { get; set; }
        public int ProductionOrderID { get; set; }
		public int RMAID { get; set; }
		public int? PartID { get; set; }
		public int LooperCount { get; set; } 

        //如下做类别区分，不是实际表要保存的字段
        public int? SNFamilyID { get; set; }
        public int SerialNumberType { get; set; }

    }
}
