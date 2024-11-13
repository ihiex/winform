using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class luDefect
	{
		public int ID { get; set; }
        public int DefectTypeID { get; set; }
		public string DefectCode { get; set; }
		public string Description { get; set; }
        public int LocaltionID { get; set; }
		public int Status { get; set; }
	}
}
