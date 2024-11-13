using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesRMA
	{
		public int ID { get; set; }
		public string RMANumber { get; set; }
		public int Quantity { get; set; }
		public DateTime? ReceivedTime { get; set; }
		public object StatusID { get; set; }
	}
}
