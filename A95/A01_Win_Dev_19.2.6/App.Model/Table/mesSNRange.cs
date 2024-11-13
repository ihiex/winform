using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesSNRange
	{
		public int ID { get; set; }
		public int SNSectionID { get; set; }
		public string Start { get; set; }
		public string End { get; set; }
		public object Order { get; set; }
		public object StatusID { get; set; }
	}
}
