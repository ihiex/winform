using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesSNSection
	{
		public int ID { get; set; }
		public int SNFormatID { get; set; }
		public object SectionType { get; set; }
		public string SectionParam { get; set; }
		public object Increment { get; set; }
		public string InvalidChar { get; set; }
		public string LastUsed { get; set; }
		public object Order { get; set; }
	}
}
