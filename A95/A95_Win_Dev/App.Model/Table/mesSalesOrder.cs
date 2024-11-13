using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesSalesOrder
	{
		public int ID { get; set; }
		public string SalesOrderNumber { get; set; }
		public string SalesOrderPosition { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public int EmployeeID { get; set; }
		public DateTime? CreationTime { get; set; }
		public DateTime? LastUpdate { get; set; }
		public string CustomerName { get; set; }
		public string CustomerOrderNumber { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }
		public string PackingType { get; set; }
		public string UOM { get; set; }
		public string PackingText { get; set; }
		public string ShipmentText { get; set; }
		public bool IsCombineShipment { get; set; }
	}
}
