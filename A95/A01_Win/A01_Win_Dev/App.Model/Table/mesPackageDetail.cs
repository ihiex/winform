using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesPackageDetail
	{
		public int ID { get; set; }
		public int PackageID { get; set; }
		public int PackageDetailDefID { get; set; }
		public string Content { get; set; }
	}
}
