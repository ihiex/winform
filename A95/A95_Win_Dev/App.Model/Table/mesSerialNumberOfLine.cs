using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class mesSerialNumberOfLine
    {
        public int ID { get; set; }
        public string SerialNumber { get; set; }
        public string SNCategory { get; set; }
        public int PrintCount { get; set; }
        public DateTime FirstPrintTime { get; set; }
        public DateTime LastPrintTime { get; set; }
    }
}
