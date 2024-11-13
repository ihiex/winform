using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesLabelField
	{
		public int LabelID { get; set; }
		public object LabelFormatPos { get; set; }
		public int LabelFieldID { get; set; }
		public string Description { get; set; }
		public int FieldDefinitionID { get; set; }
		public object Order { get; set; }
	}
}
