using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
	public class mesEmployee
	{
		public int ID { get; set; }
		public string UserID { get; set; }
		public string Password { get; set; }
		public string Lastname { get; set; }
		public string Firstname { get; set; }
		public string Description { get; set; }
		public string EmailAddress { get; set; }
		public int LoginAttempt { get; set; }
		public int EmployeeGroupID { get; set; }
		public int StatusID { get; set; }
        public int PermissionId { get; set; }
	}
}
