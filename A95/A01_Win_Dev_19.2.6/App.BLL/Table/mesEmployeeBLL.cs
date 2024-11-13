using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesEmployeeBLL
	{
		public int Insert(mesEmployee model)
		{
			return new mesEmployeeDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesEmployeeDAL().Delete(id);
		}

		public bool Update(mesEmployee model)
		{
			return new mesEmployeeDAL().Update(model);
		}

		public mesEmployee Get(int id)
		{
			return new mesEmployeeDAL().Get(id);
		}

		public IEnumerable<mesEmployee> ListAll(string S_Where)
		{
			return new mesEmployeeDAL().ListAll(S_Where);
		}

        public bool PermissionCheck(int EmployeeID, int StationTypeID)
        {
            return new mesEmployeeDAL().PermissionCheck(EmployeeID, StationTypeID);
        }


        public string ValidateSecond(string UserID, string PWD)
        {
            return new mesEmployeeDAL().ValidateSecond(UserID, PWD);
        }

        public string ChangePWD(string UserID, string OldPWD, string NewPWD)
        {
            return new mesEmployeeDAL().ChangePWD( UserID,  OldPWD,  NewPWD);
        }
    }
}
