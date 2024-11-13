using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesImportProduct
	{
		public int ID { get; set; }
		public string SerialNumber { get; set; }
		public string SalesOrderNumber { get; set; }
		public string SalesOrderPosition { get; set; }
		public string ProductionOrderNumber { get; set; }
		public string RMANumber { get; set; }
		public string ItemType { get; set; }
		public string PartNumber { get; set; }
		public string Revision { get; set; }
		public string UOM { get; set; }
		public int Quantity { get; set; }
		public DateTime? CreationTime { get; set; }
		public bool IsImported { get; set; }
		public DateTime? ImportedTime { get; set; }
		public string reserved_01 { get; set; }
		public string reserved_02 { get; set; }
		public string reserved_03 { get; set; }
		public string reserved_04 { get; set; }
		public string reserved_05 { get; set; }
		public string reserved_06 { get; set; }
		public string reserved_07 { get; set; }
		public string reserved_08 { get; set; }
		public string reserved_09 { get; set; }
		public string reserved_10 { get; set; }
		public string reserved_11 { get; set; }
		public string reserved_12 { get; set; }
		public string reserved_13 { get; set; }
		public string reserved_14 { get; set; }
		public string reserved_15 { get; set; }
		public string reserved_16 { get; set; }
		public string reserved_17 { get; set; }
		public string reserved_18 { get; set; }
		public string reserved_19 { get; set; }
		public string reserved_20 { get; set; }
	}
}
