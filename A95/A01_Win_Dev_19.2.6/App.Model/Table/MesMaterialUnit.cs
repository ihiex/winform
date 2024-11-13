using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class MesMaterialUnit
    {
        private int? iD;
        private string serialNumber;
        private int? partID;
        private int statusID;
        private int materialTypeID;
        private int? stationID;
        private int? employeeID;
        private string rollCode;
        private string lotCode;
        private string materialDateCode;
        private string dateCode;
        private string traceCode;
        private string mPN;
        private int? vendorID;
        private int? quantity;
        private int? balanceQty;
        private int? remainQTY;
        private int? looperCount;
        private DateTime? creationTime;
        private DateTime? finishTime;
        private int? lineID;
        private DateTime? lastUpdate;
        private int? processNameID;
        private DateTime? expiringTime;
        private int? parentID;

        public int? ID { get => iD; set => iD = value; }
        public string SerialNumber { get => serialNumber; set => serialNumber = value; }
        public int? PartID { get => partID; set => partID = value; }
        public int StatusID { get => statusID; set => statusID = value; }
        public int MaterialTypeID { get => materialTypeID; set => materialTypeID = value; }
        public int? StationID { get => stationID; set => stationID = value; }
        public int? EmployeeID { get => employeeID; set => employeeID = value; }
        public string LotCode { get => lotCode; set => lotCode = value; }
        public string MaterialDateCode { get => materialDateCode; set => materialDateCode = value; }
        public string DateCode { get => dateCode; set => dateCode = value; }
        public string TraceCode { get => traceCode; set => traceCode = value; }
        public string MPN { get => mPN; set => mPN = value; }
        public int? VendorID { get => vendorID; set => vendorID = value; }
        public int? Quantity { get => quantity; set => quantity = value; }
        public int? BalanceQty { get => balanceQty; set => balanceQty = value; }
        public int? LooperCount { get => looperCount; set => looperCount = value; }
        public DateTime? CreationTime { get => creationTime; set => creationTime = value; }
        public DateTime? FinishTime { get => finishTime; set => finishTime = value; }
        public int? LineID { get => lineID; set => lineID = value; }
        public DateTime? LastUpdate { get => lastUpdate; set => lastUpdate = value; }
        public int? ProcessNameID { get => processNameID; set => processNameID = value; }
        public DateTime? ExpiringTime { get => expiringTime; set => expiringTime = value; }
        public int? ParentID { get => parentID; set => parentID = value; }
        public string RollCode { get => rollCode; set => rollCode = value; }
        public int? RemainQTY { get => remainQTY; set => remainQTY = value; }
    }
}
