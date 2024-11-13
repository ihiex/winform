using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Model;
using App.MyMES.PartSelectService;
using App.MyMES.mesUnitService;
using App.MyMES.mesSerialNumberService;
using App.MyMES.mesHistoryService;
using App.MyMES.mesProductionOrderService;
using App.MyMES.mesProductStructureService;
using App.MyMES.mesUnitComponentService;
using App.MyMES.mesUnitDefectService;
using App.MyMES.mesUnitDetailService;
using System.Text.RegularExpressions;
using App.MyMES.mesPartService;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using App.MyMES.mesRouteDetailService;
using App.MyMES.mesMachineService;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Threading;
using System.Diagnostics;

namespace App.MyMES
{
    public partial class RePrintFGSN_Form : FrmBase2
    {
        public RePrintFGSN_Form()
        {
            InitializeComponent();
        }
        LabelManager2.Application LabSN;
        DataTable DT_PrintSn;
        int I_PartID;

        [DllImport("User32.dll", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        private void PrintSNOffline_Form2_Load(object sender, EventArgs e)
        {
            try
            {
                LoadComBox();
                public_.OpenBartender(List_Login.StationID.ToString());
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {

                if (Com_PO.Text.Trim() == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20114", "NG", List_Login.Language);
                    return;
                }
                else
                {
                    if (Com_PartFamilyType.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyType.Text.ToString()))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20115", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_PartFamily.EditValue == null || string.IsNullOrEmpty(Com_PartFamily.Text.ToString()))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20116", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_Part.EditValue == null || string.IsNullOrEmpty(Com_Part.Text.ToString()))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20117", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_PO.EditValue == null || string.IsNullOrEmpty(Com_PO.Text.ToString()))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20118", "NG", List_Login.Language);
                        return;
                    }
                    if (!string.IsNullOrEmpty(Com_Part.EditValue.ToString()))
                    {
                        int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                        DataSet dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "SN_Pattern");
                        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            SN_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20181", "NG", List_Login.Language, new string[] { Com_Part.Text.ToString() });
                            return;
                        }
                    }

                    DataSet dsStructure = mesProductStructureSVC.GetBOMStructure(Com_Part.EditValue.ToString(), "", "");
                    if (dsStructure != null && dsStructure.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = dsStructure.Tables[0];
                        treeListColBom.Caption = dt.Rows[0]["ParentPartDescription"].ToString();
                        TreeKeyObj treeKeyObj = new TreeKeyObj();
                        treeKeyObj.FiterName = "ParentPartID";
                        treeKeyObj.FiterValue = Com_Part.EditValue.ToString();
                        treeKeyObj.KeyName1 = "PartID";
                        treeKeyObj.KeyName2 = "PartDescription";
                        treeKeyObj.KeyValue = "PartID";
                        SetDataSetToTree(TreeModule, null, treeKeyObj, dt);
                    }


                    DataSet DS_RouteData = PartSelectSVC.GetRouteData(List_Login.LineID.ToString(), Com_Part.EditValue.ToString(),
                            Com_PartFamily.EditValue.ToString(), Com_PO.EditValue.ToString());
                    string S_RouteType = DS_RouteData.Tables[0].Rows[0]["RouteType"].ToString();


                    listBoxRouteDetail.Items.Clear();
                    int index = 0;
                    int selectIndex = 0;
                    GrpControlRouteDetail.Text = DS_RouteData.Tables[0].Rows[0]["Description"].ToString();

                    if (S_RouteType == "1")
                    {
                        string result = string.Empty;
                        string routID = DS_RouteData.Tables[0].Rows[0]["ID"].ToString();
                        DataSet dsRoute = PartSelectSVC.uspCallProcedure("uspGetRouteData", routID, "", "",
                                                                "", "", List_Login.StationTypeID.ToString(), ref result);

                        if (dsRoute == null || dsRoute.Tables.Count <= 0)
                        {
                            return;
                        }
                        Grid_RouteData.Visible = true;
                        DataColumn keyColumn = dsRoute.Tables[0].Columns["ID"];         //主键
                        DataColumn foreignColumn = dsRoute.Tables[1].Columns["ParentID"];    //外键
                        dsRoute.Relations.Add("", keyColumn, foreignColumn);
                        Grid_RouteData.DataSource = dsRoute.Tables[0];
                        Grid_RouteDataView.OptionsDetail.ShowDetailTabs = false;
                        Grid_RouteData.UseEmbeddedNavigator = false;
                        Grid_RouteDataView.OptionsDetail.AllowExpandEmptyDetails = true;

                        int foundHandle = Grid_RouteDataView.LocateByDisplayText(0, Grid_RouteDataView.Columns["StationTypeID"],
                            List_Login.StationTypeID.ToString());
                        Grid_RouteDataView.FocusedRowHandle = foundHandle;
                        //Grid_RouteDataView.ExpandGroupLevel(0);
                    }
                    else
                    {
                        Grid_RouteData.Visible = false;
                        DataSet dsRouteDetail = mesRouteDetailSVC.GetRouteDetail(List_Login.LineID.ToString(),
                            Com_Part.EditValue.ToString(), Com_PartFamily.EditValue.ToString(), Com_PO.EditValue.ToString());
                        foreach (DataRow dr in dsRouteDetail.Tables[0].Rows)
                        {
                            string StationTypeName = string.Empty;
                            StationTypeName = dr["Sequence"].ToString() + "  " + dr["Description"].ToString();
                            if (List_Login.StationTypeID.ToString() == dr["StationTypeID"].ToString())
                            {
                                StationTypeName = "<color=Red>" + StationTypeName + "</color>";
                                selectIndex = index;
                            }
                            index++;
                            listBoxRouteDetail.Items.Add(StationTypeName);
                            listBoxRouteDetail.SelectedIndex = selectIndex + 3;
                            listBoxRouteDetail.SelectedIndex = selectIndex;
                        }
                    }

                    Com_PartFamilyType.Enabled = false;
                    Com_PartFamily.Enabled = false;
                    Com_Part.Enabled = false;
                    Com_PO.Enabled = false;
                    GrpControlInputData.Enabled = true;
                    Com_LineGroup.Enabled = false;
                    Btn_ConfirmPO.Enabled = false;
                    Edt_RePrintSN.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void LoadComBox()
        {
            List_Login = this.Tag as LoginList;
            ///////////////////////////////////////////////////////////////////////////////////////
            Get_TempletPath(true);
        }

        private void Get_TempletPath(Boolean IsMsg)
        {
            try
            {
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_ProductionOrderID = Com_PO.EditValue.ToString();
                string S_LoginLineID = List_Login.LineID.ToString();

                string S_LabelPath = public_.GetLabelPath(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
                if (S_LabelPath == "")
                {
                    if (IsMsg == true)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                    }
                }
                else
                {
                    Edt_Templet.Text = S_LabelPath;
                    if (S_LabelPath.Substring(0, 5) == "ERROR")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_LabelPath });
                        Edt_Templet.Text = "";
                    }
                }
            }
            catch
            { }
        }

        private void Com_PO_EditValueChanged(object sender, EventArgs e)
        {
            Get_TempletPath(false);
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_Part = Com_Part.Text.Trim();

                if (S_Part != "")
                {
                    I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());

                    Get_TempletPath(false);
                }
            }
            catch
            {
            }
        }

        private void Btn_Reprint_Click(object sender, EventArgs e)
        {
            string reList = Edt_RePrintSN.Text.Trim();    //Edt_SN.Text.Trim();
            if (string.IsNullOrEmpty(reList))
                return;
            RePrint(reList);
        }


        public void RePrint(string SNList)
        {
            DT_PrintSn = new DataTable();
            DT_PrintSn.Columns.Add("SN", typeof(string));
            DT_PrintSn.Columns.Add("CreateTime", typeof(string));
            DT_PrintSn.Columns.Add("PrintTime", typeof(string));
            string[] strList = SNList.Split(';');
            foreach (string str in strList)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    DataRow dr = DT_PrintSn.NewRow();
                    dr["SN"] = str;
                    dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DT_PrintSn.Rows.Add(dr);
                }
            }
            dataGridSN.DataSource = DT_PrintSn;
            for (int i = 0; i < this.dataGridSN.Columns.Count - 1; i++)
            {
                this.dataGridSN.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_PartID = Com_Part.EditValue.ToString();
            string S_ProductionOrderID = Com_PO.EditValue.ToString();
            string S_LoginLineID = List_Login.LineID.ToString();
            string S_LabelName = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
            string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, S_LabelName, DT_PrintSn, I_PartID);
            if (S_PrintResult != "OK")
            {
                string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_PrintResult);
            }
            else
            {
                foreach (string str in strList)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        DataSet dsSetPr = PartSelectSVC.GetmesSerialNumber(str);
                        string unit = dsSetPr.Tables[0].Rows[0]["UnitID"].ToString();
                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory.UnitID = Convert.ToInt32(unit);
                        v_mesHistory.UnitStateID = -200;
                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                        v_mesHistory.StationID = List_Login.StationID;
                        v_mesHistory.EnterTime = DateTime.Now;
                        v_mesHistory.ExitTime = DateTime.Now;
                        v_mesHistory.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                        v_mesHistory.PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                        v_mesHistory.LooperCount = 1;
                        v_mesHistory.StatusID = 1;
                        mesHistorySVC.Insert(v_mesHistory);
                    }
                }
                MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);
                Edt_RePrintSN.Text = "";
                Edt_SN.Text = "";
            }
        }

        public override void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_PartFamilyType_EditValueChanged(sender, e);
            int I_PartFamilyID = Convert.ToInt32(Com_PartFamilyType.EditValue.ToString());
            public_.AddLineGroup(Com_LineGroup, Grid_LineGroup, "M", I_PartFamilyID);
            S_DefectTypeID = public_.GetDefectTypeID(Com_PartFamilyType.Text.Trim());
        }

        private void Edt_RePrintSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string RePrintSNStr = Edt_RePrintSN.Text.Trim();
                if(string.IsNullOrEmpty(RePrintSNStr))
                {
                    return;
                }

                string[] RePrintSNList = RePrintSNStr.Split(';');
                foreach (string RePrintSN in RePrintSNList)
                {
                    //校验是否符合要求
                    DataSet daSetRePrint = PartSelectSVC.GetSerialNumber2(RePrintSN);
                    if (daSetRePrint != null && daSetRePrint.Tables.Count > 0 && daSetRePrint.Tables[0].Rows.Count > 0)
                    {
                        string productionOrderID = Com_PO.EditValue.ToString();
                        string partID = Com_Part.EditValue.ToString();
                        DataTable dtRePrint = daSetRePrint.Tables[0];
                        if (productionOrderID != dtRePrint.Rows[0]["ProductionOrderID"].ToString())
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                            return;
                        }
                        if (partID != dtRePrint.Rows[0]["PartID"].ToString())
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20043", "NG", List_Login.Language);
                            return;
                        }
                        Edt_SN.MaskBox.AppendText(RePrintSN + ";");
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { RePrintSN });
                    }

                    Edt_RePrintSN.Text = string.Empty;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Edt_RePrintSN.Text = string.Empty;
            Edt_SN.Text = string.Empty;
            Edt_RePrintSN.Focus();
        }
    }
}
