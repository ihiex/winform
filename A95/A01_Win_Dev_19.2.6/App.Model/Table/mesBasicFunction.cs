using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesBasicFunction
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string AssemblyName { get; set; }
		public object BasicFunctionTypeID { get; set; }
		public bool IsUserDefined { get; set; }
		public string ConfigClassName { get; set; }
		public string ConfigAssemblyName { get; set; }
	}
}
