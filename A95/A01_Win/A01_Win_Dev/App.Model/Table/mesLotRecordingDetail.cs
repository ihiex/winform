using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesLotRecordingDetail
	{
		public int ID { get; set; }
		public int LotRecordingID { get; set; }
		public int LotRecordingDetailDefID { get; set; }
		public string Content { get; set; }
	}
}
