using App.BLL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SFCAPI”。
public class SFCAPISVC : ISFCAPISVC
{
    SFCAPIBLL F_BLL = new SFCAPIBLL();

    public string MainSN_Check(string S_SN, LoginList List_Login)
    {
        return F_BLL.MainSN_Check(S_SN, List_Login);
    }

    public string MainSN_OutFGSN(string S_SN, LoginList List_Login)
    {
        return F_BLL.MainSN_OutFGSN(S_SN, List_Login);
    }

    public string MainSN_Check_OutCCCode(string S_SN, LoginList List_Login, out string S_CCCode)
    {
        return F_BLL.MainSN_Check_OutCCCode(S_SN, List_Login, out S_CCCode);
    }
    public string MainSN_Check_OutCCCodeV2(string S_SN, LoginList List_Login)
    {
        return F_BLL.MainSN_Check_OutCCCodeV2(S_SN, List_Login);
    }

    public string Update_SFC(string S_SN, int I_StatusID, LoginList List_Login, string S_DefectCode)
    {
        return  F_BLL.Update_SFC(S_SN, I_StatusID, List_Login, S_DefectCode);
    }

    public DataSet GetEmployee(string S_UserName)
    {
        return  F_BLL.GetEmployee(S_UserName);
    }



    public int GetUser(string UserID, string Password)
    {
        return F_BLL.GetUser(UserID, Password);
    }

    public DataSet GetmesLine()
    {
        return F_BLL.GetmesLine();
    }

    public DataSet GetluPartFamilyType()
    {
        return F_BLL.GetluPartFamilyType();
    }

    public DataSet GetluPartFamily(string PartFamilyTypeID)
    {
        return F_BLL.GetluPartFamily(PartFamilyTypeID);
    }

    public DataSet GetmesPart(string PartFamilyID)
    {
        return F_BLL.GetmesPart(PartFamilyID);
    }

    public DataSet GetmesPartOne(string ID)
    {
        return F_BLL.GetmesPartOne(ID);
    }

    public DataSet GetmesStationType()
    {
        return F_BLL.GetmesStationType();
    }

    public DataSet GetmesStation(string LineID)
    {
        return F_BLL.GetmesStation(LineID);
    }

    public DataSet GetPOAll(string S_PartID, string S_LineID)
    {
        return F_BLL.GetPOAll(S_PartID, S_LineID);
    }

    public string SaveStationSet(string StationID, string Value)
    {
        return F_BLL.SaveStationSet(StationID, Value);
    }

}
