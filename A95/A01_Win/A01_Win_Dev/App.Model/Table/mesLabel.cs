using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesLabel
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int LabelFamilyID { get; set; }
		public object LabelType { get; set; }
		public string TargetPath { get; set; }
		public object OutputType { get; set; }
		public string PrintCMD { get; set; }
		public string SourcePath { get; set; }
		public int PageCapacity { get; set; }
	}
}
