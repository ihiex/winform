using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class sysVersion
	{
		public int ID { get; set; }
		public int ApplicationTypeID { get; set; }
		public string VersionNumber { get; set; }
		public string VersionName { get; set; }
	}
}
