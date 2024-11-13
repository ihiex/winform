using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesSNFormat
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int SNFamilyID { get; set; }
		public string Format { get; set; }
		public bool IsPool { get; set; }
	}
}
