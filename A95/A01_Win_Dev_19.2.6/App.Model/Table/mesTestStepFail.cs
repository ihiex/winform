using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesTestStepFail
	{
		public int ID { get; set; }
		public int TestID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Comment { get; set; }
		public string Step { get; set; }
		public string TesterStartTime { get; set; }
		public string TesterEndTime { get; set; }
		public string TesterTestTime { get; set; }
		public string TesterModuleTime { get; set; }
		public bool IsPass { get; set; }
		public string Status { get; set; }
		public string Resource { get; set; }
		public string TestType { get; set; }
		public string GroupIndex { get; set; }
		public string LoopIndex { get; set; }
		public int ParentID { get; set; }
		public int DownLevel { get; set; }
	}
}
