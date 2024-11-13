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
        public string Line { get; set; }
        public int StationID { get; set; }
        public string Station { get; set; }
        public int EmployeeID { get; set; }
        public int SkinID { get; set; }
        public string SkinName { get; set; }
        public string Language { get; set; }
        public string ServerIP { get; set; }
        public string Ver { get; set; }


        public Boolean IsCheckPO { get; set; }
        public Boolean IsCheckNG { get; set; }

    }
}
