using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class LoginList
    {
        public int StationTypeID { get; set; }
        public string StationType { get; set; }
        public int ApplicationTypeID { get; set; }
        public string ApplicationType { get; set; }
        public int LineID { get; set; }
        public int StationID { get; set; }
        public int EmployeeID { get; set; }
        //public int POID { get; set; }    
        
        public string Ver { get; set; }
    }
}
