using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class sysErrorLog
	{
		public int ID { get; set; }
		public string Operation { get; set; }
		public DateTime? CreationTime { get; set; }
		public int ErrorNo { get; set; }
		public string ErrorSource { get; set; }
		public string Description { get; set; }
	}
}
