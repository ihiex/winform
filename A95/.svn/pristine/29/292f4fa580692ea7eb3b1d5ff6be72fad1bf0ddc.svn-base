using App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.MyMES.PartSelectService;
using App.MyMES.mesPackageService;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using App.MyMES.DataCommitService;

namespace App.MyMES
{
    public partial class ShipMentV2_Form : DevExpress.XtraEditors.XtraForm
    {
        public Public_ public_ = new Public_();
        public LoginList List_Login = new LoginList();
        public PartSelectSVCClient PartSelectSVC;
        public DataCommitSVCClient DataCommitSVC;
        public ImesPackageSVCClient mesPackageSVC;
        public string GS1Name = string.Empty;                   //情况3打印模板名称
        public string GS2Name = string.Empty;                   //情况4打印模板名称
        string MupltipackPalletFormat = string.Empty;           //打印格式
        string MupltipackPalletLabelPath = string.Empty;        //模板路径
        string PritType = "1";                                  //条码打印类型1:扫描完毕打印  2:每扫描一次打印一张
        DataTable dtMupltiPack = null;                          //扫描MultipackSN
        DataTable dtShipmentDetail;                             //Pallet出货明细         
        int number = 1;
        DataTable DT_PrintSn;

        string S_MultipackPalletSN = string.Empty;

        private bool IsScan2DSN = false;
        private string MultiSN_2D = string.Empty;
        private string SN_Pattern = string.Empty;
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        public ShipMentV2_Form()
        {
            InitializeComponent();
        }

        private void GetMultipackDetail()
        {
            string S_BillNo = txtBillNo.Text.Trim();
            string xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
            string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
            string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
            string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

            string Result = "1";
            DataSet dsShipment = PartSelectSVC.uspCallProcedure("uspGetShipMentData", S_BillNo,
                                                xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null, ref Result);

            if (dsShipment != null && dsShipment.Tables.Count > 0 && dsShipment.Tables[0].Rows.Count > 0)
            {
                dtShipmentDetail = dsShipment.Tables[0];
                grdControlMultipack.DataSource = dtShipmentDetail;
            }

            //刷新数量列表
            DataSet dsDetail = PartSelectSVC.uspCallProcedure("uspGetShipMentDetail", S_BillNo,
                                            null, null, null, null, null, ref Result);
            if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
            {
                dtMupltiPack = dsDetail.Tables[0];
                trListMultipack.DataSource = dtMupltiPack;
            }
        }


        /// <summary>
        /// BillNo扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBillNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string BillNo = txtBillNo.Text.Trim();
                if (string.IsNullOrEmpty(BillNo))
                {
                    // 条码不能为空.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    grdControlMultipack.DataSource = null;
                    txtBillNo.Focus();
                    return;
                }

                string Result = "1";
                string xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                DataSet dsShipment = PartSelectSVC.uspCallProcedure("uspGetShipMentData", BillNo,
                                                    xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null, ref Result);
                if (Result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    //条码:{0} 错误信息:{1}.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { BillNo, ProMsg }, Result);
                    grdControlMultipack.DataSource = null;
                    txtBillNo.Text = string.Empty;
                    txtBillNo.Focus();
                    return;
                }


                if (dsShipment != null && dsShipment.Tables.Count > 0 && dsShipment.Tables[0].Rows.Count > 0)
                {
                    dtShipmentDetail = dsShipment.Tables[0];
                    grdControlMultipack.DataSource = dtShipmentDetail;

                    DataSet dsDetail = PartSelectSVC.uspCallProcedure("uspGetShipMentDetail", BillNo,
                                                    null, null, null, null, null, ref Result);
                    if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                    {
                        dtMupltiPack = dsDetail.Tables[0];
                        trListMultipack.DataSource = dtMupltiPack;
                        number = dtMupltiPack.Rows.Count + 1;
                    }

                    //判断条码打印类型

                    PartSelectSVC.uspCallProcedure("uspGetShipmentPrintType", BillNo,
                                                   null, null, null, null, null, ref PritType);
                    //PritType 1 第一次扫描 BillNO 生成MultipackPalletSN,二次扫找出
                    if (PritType == "1")
                    {
                        S_MultipackPalletSN = DataCommitSVC.GetShipmentPalletSN(BillNo);

                        if (S_MultipackPalletSN == "")
                        {
                            mesUnit mesUnit = new mesUnit();
                            mesUnit.StationID = List_Login.StationID;
                            mesUnit.EmployeeID = List_Login.EmployeeID;
                            mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                            mesUnit.PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                            string result = string.Empty;

                            //string MultipackPalletSN = string.Empty;

                            result = PartSelectSVC.Get_CreatePackageSN(MupltipackPalletFormat, xmlProdOrder, xmlPart,
                                xmlStation, xmlExtraData, mesUnit, ref S_MultipackPalletSN, 3);
                            if (result != "1")
                            {
                                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                                //错误信息:{0}.
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                                return;
                            }
                        }
                    }

                    txtBillNo.Enabled = false;
                    if (IsScan2DSN)
                    {
                        txtMultipack2DSN.Enabled = true;
                        txtMultipack2DSN.Text = string.Empty;
                        txtMultipack2DSN.Focus();
                    }
                    else
                    {
                        txtMultipackSN.Enabled = true;
                        txtMultipackSN.Text = string.Empty;
                        txtMultipackSN.Focus();
                    }
                }
                else
                {
                    //未查询到出货关联数据,请确认.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20197", "NG", List_Login.Language);
                    grdControlMultipack.DataSource = null;
                    txtBillNo.Text = string.Empty;
                    txtBillNo.Focus();
                    return;
                }
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShipMent_Form_Load(object sender, EventArgs e)
        {
            try
            {
                Btn_Refresh_Click(null, null);

            }
            catch (Exception ex)
            {
                //未查询到出货关联数据,请确认.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        /// <summary>
        /// txtMultipackSN扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMultipackSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool ScanOver = false;
                string Result = string.Empty;
                string S_BillNo = txtBillNo.Text.Trim();
                string S_MultipackSN = txtMultipackSN.Text.Trim().ToUpper();
                if (string.IsNullOrEmpty(S_MultipackSN))
                {
                    //条码不能为空.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtMultipackSN.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(S_BillNo))
                {
                    //条码不能为空.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtBillNo.Enabled = true;
                    txtMultipackSN.Enabled = false;
                    txtMultipackSN.Text = string.Empty;
                    txtBillNo.Focus();
                    return;
                }

                if (IsScan2DSN)
                {
                    if (MultiSN_2D != S_MultipackSN)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG,"NG","Error: 2D barcode and multi-pack SN mismatch.");
                        txtMultipackSN.Enabled = true;
                        txtMultipackSN.Text = string.Empty;
                        txtMultipackSN.Focus();
                        return;
                    }
                }


                string xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlExtraData = "<ExtraData BillNO=\"" + txtBillNo.Text.Trim() + "\"> </ExtraData>";
                string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                //包装校验
                PartSelectSVC.uspCallProcedure("uspPackageRouteCheck", S_MultipackSN, xmlProdOrder, xmlPart,
                                                            xmlStation, xmlExtraData, "", ref Result);

                if (Result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    //条码:{0} 错误信息:{1}.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_MultipackSN, ProMsg }, Result);
                    txtMultipackSN.SelectAll();
                    return;
                }


                /////////////////////////////////////////////

                string DetailID = string.Empty;
                //DataSet dsFDetailID = PartSelectSVC.uspCallProcedure("uspSetShipmentMultipack", MultipackSN,
                //                                    null, null, null, null, txtBillNo.Text.Trim(), ref Result);

                DataSet dsFDetailID=new DataSet();
                if (PritType == "1")
                {
                    dsFDetailID = DataCommitSVC.SetShipmentMultipack(S_BillNo, S_MultipackSN, S_MultipackPalletSN);
                }
                else
                {
                    dsFDetailID = DataCommitSVC.SetShipmentMultipackPrintOne(S_BillNo, S_MultipackSN);
                }

                Result = dsFDetailID.Tables[0].Rows[0]["OutResult"].ToString();

                if (Result != "1" && Result != "0")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    //条码:{0} 错误信息:{1}.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_MultipackSN, ProMsg }, Result);
                    txtMultipackSN.Text = string.Empty;
                    txtMultipackSN.Focus();
                    return;
                }
                if (dsFDetailID != null && dsFDetailID.Tables.Count > 0 && dsFDetailID.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dtMupltiPack.NewRow();
                    dr["ID"] = number;
                    dr["MultiPackSN"] = txtMultipackSN.Text.Trim();
                    dtMupltiPack.Rows.Add(dr);
                    trListMultipack.DataSource = dtMupltiPack;
                    DetailID = dsFDetailID.Tables[0].Rows[0]["FDetailID"].ToString();
                    int counts = dtShipmentDetail.Rows.Count;
                    int j = 0;
                    foreach (DataRow drDetail in dtShipmentDetail.Rows)
                    {
                        if (drDetail["FDetailID"].ToString().ToUpper() == DetailID)
                        {
                            int outSN = Convert.ToInt32(drDetail["FOutSN"].ToString());
                            drDetail["FOutSN"] = outSN + 1;
                        }

                        if (drDetail["FOutSN"].ToString() == drDetail["FCTN"].ToString())
                        {
                            j++;
                        }
                    }
                    number++;
                    if (j == counts)
                    {
                        ScanOver = true;
                    }
                }
                else
                {
                    //更新包装信息失败.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20121", "NG", List_Login.Language);
                    txtMultipackSN.Text = string.Empty;
                    txtMultipackSN.Focus();

                    DataSet dsDetail = PartSelectSVC.uspCallProcedure("uspGetShipMentDetail", S_BillNo,
                                                    null, null, null, null, null, ref Result);
                    if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                    {
                        dtMupltiPack = dsDetail.Tables[0];
                        trListMultipack.DataSource = dtMupltiPack;                        
                    }


                    return;
                }
                // PritType 1  扫描一箱处理一箱
                string S_IsOutCountComplete = "";
                if (PritType == "1")
                {
                    string S_Result = DataCommitSVC.uspPalletPackaging(Com_Part.EditValue.ToString(),
                        Com_PO.EditValue.ToString(), S_MultipackSN, S_MultipackPalletSN, List_Login, S_BillNo, 0);
                    if (S_Result != "OK")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_Result, List_Login.Language);
                        //错误信息:{0}.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_Result);
                        txtMultipackSN.Text = string.Empty;
                        txtMultipackSN.Focus();

                        // MoveShipment(S_BillNo, S_MultipackSN, Result);

                        //刷新数量列表
                        GetMultipackDetail();

                        return;
                    }

                    //SN{0}扫进成功. 2022-05-10 加入
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10004", "OK", List_Login.Language, new string[] { S_MultipackSN });
                    S_IsOutCountComplete = DataCommitSVC.GetIsOutCountComplete(S_BillNo, S_MultipackPalletSN, DetailID, ScanOver);
                }

                

                // PritType 2 生成MultipackPalletSN，PritType 1 满板处理
                if (PritType == "2" || (S_IsOutCountComplete == "OK" && PritType == "1"))
                //if (PritType == "2" || (Result == "0" && PritType=="1"))
                {
                    string result = string.Empty;
                    if (PritType == "2")
                    {
                        mesUnit mesUnit = new mesUnit();
                        mesUnit.StationID = List_Login.StationID;
                        mesUnit.EmployeeID = List_Login.EmployeeID;
                        mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                        mesUnit.PartID = Convert.ToInt32(Com_Part.EditValue.ToString());

                        string MultipackPalletSN = string.Empty;

                        result = PartSelectSVC.Get_CreatePackageSN(MupltipackPalletFormat, xmlProdOrder, xmlPart,
                            xmlStation, xmlExtraData, mesUnit, ref MultipackPalletSN, 3);
                        if (result != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            //错误信息:{0}.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                            return;
                        }
                        S_MultipackPalletSN = MultipackPalletSN;
                    }

                    string S_SetMesPackageShipmennt = "";
                    if (PritType == "1")
                    {
                        S_SetMesPackageShipmennt = DataCommitSVC.SetMesPackageShipment(DetailID, S_MultipackPalletSN, Convert.ToInt32(PritType));
                    }
                    else
                    {
                        S_SetMesPackageShipmennt = DataCommitSVC.SetMesPackageShipmentPrintOne(DetailID, S_MultipackPalletSN, Convert.ToInt32(PritType));

                        //if (S_SetMesPackageShipmennt != "OK")
                        //{
                        //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_SetMesPackageShipmennt });

                        //    string S_Result_Roll = DataCommitSVC.SetMesPackageShipmentRollPrintOne(S_BillNo, S_MultipackSN);
                        //    DataRow[] DR_List = dtMupltiPack.Select("MultiPackSN='" + S_MultipackSN + "'");
                        //    if (DR_List.Length > 0)
                        //    {
                        //        dtMupltiPack.Rows.Remove(DR_List[0]);
                        //        trListMultipack.DataSource = dtMupltiPack;
                        //    }
                        //    return;
                        //}
                    }

                    if (S_SetMesPackageShipmennt != "OK")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_SetMesPackageShipmennt });

                        string S_Result_Roll = DataCommitSVC.SetMesPackageShipmentRoll(S_BillNo, S_MultipackPalletSN, S_MultipackSN, DetailID);

                        DataSet dsDetail = PartSelectSVC.uspCallProcedure("uspGetShipMentDetail", S_BillNo,
                                                        null, null, null, null, null, ref Result);
                        if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                        {
                            dtMupltiPack = dsDetail.Tables[0];
                            trListMultipack.DataSource = dtMupltiPack;
                        }
                        return;
                    }


                    //绑定关系
                    if (PritType == "2")
                    {
                        result = DataCommitSVC.uspPalletPackagingPrintOne(Com_Part.EditValue.ToString(),
                            Com_PO.EditValue.ToString(), S_MultipackSN, S_MultipackPalletSN, List_Login,  0);
                        if (result != "OK")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            //错误信息:{0}.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                            txtMultipackSN.Text = string.Empty;
                            txtMultipackSN.Focus();

                            MoveShipment(S_BillNo, S_MultipackSN, Result);
                            return;
                        }

                        result = DataCommitSVC.uspPalletPackagingPrintOne(Com_Part.EditValue.ToString(),
                                        Com_PO.EditValue.ToString(), S_MultipackSN, S_MultipackPalletSN, List_Login,  1);
                        if (result != "OK")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            //卡板:{0}包装失败 {1}.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20096", "NG", List_Login.Language, new string[] { S_MultipackSN, ProMsg }, result);
                            txtMultipackSN.Text = string.Empty;
                            txtMultipackSN.Focus();

                            MoveShipment(S_BillNo, S_MultipackSN, Result);
                            return;
                        }

                        //打印情况2
                        if (DT_PrintSn != null)
                        {
                            DT_PrintSn.Columns.Clear();
                            DT_PrintSn.Rows.Clear();
                        }
                        else
                        {
                            DT_PrintSn = new DataTable();
                        }
                        DT_PrintSn.Columns.Add("SN");
                        DT_PrintSn.Columns.Add("CreateTime");
                        DT_PrintSn.Columns.Add("PrintTime");
                        DataRow DR = DT_PrintSn.NewRow();
                        DR["SN"] = S_MultipackSN;
                        DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        DT_PrintSn.Rows.Add(DR);

                        string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, MupltipackPalletLabelPath,
                                            DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                        Thread.Sleep(1000);
                        if (PrintResult != "OK")
                        {
                            //卡板:{0}包装完成,打印失败 {1}.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", List_Login.Language, new string[] { S_MultipackPalletSN, PrintResult });

                        }
                        else
                        {
                            //卡板:{0}包装完成并发送至打印机.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10018", "OK", List_Login.Language, new string[] { S_MultipackPalletSN });
                        }
                    }
                    else
                    {
                        //foreach(DataRow dr in dtMupltiPack.Rows)
                        //{
                        //    string MulitiPackSNList = dr["MultiPackSN"].ToString();
                        //    result = DataCommitSVC.uspPalletPackaging(Com_Part.EditValue.ToString(),
                        //        Com_PO.EditValue.ToString(), MulitiPackSNList, MultipackPalletSN, List_Login, 0);
                        //    if (result != "OK")
                        //    {
                        //        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        //        //错误信息:{0}.
                        //        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                        //        txtMultipackSN.Text = string.Empty;
                        //        txtMultipackSN.Focus();

                        //        MoveShipment(S_BillNo, S_MultipackSN, Result);
                        //        return;
                        //    }
                        //}

                        int count = dtMupltiPack.Rows.Count;
                        result = DataCommitSVC.uspPalletPackaging(Com_Part.EditValue.ToString(),
                                        Com_PO.EditValue.ToString(), S_MultipackSN, S_MultipackPalletSN, List_Login, S_BillNo, count);
                        if (result != "OK")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            //卡板:{0}包装失败 {1}.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20096", "NG", List_Login.Language, new string[] { S_MultipackSN, ProMsg }, result);
                            txtMultipackSN.Text = string.Empty;
                            txtMultipackSN.Focus();

                            MoveShipment(S_BillNo, S_MultipackSN, Result);
                            return;
                        }

                        #region 打印情况1
                        DataSet dsPrintData = PartSelectSVC.uspCallProcedure("uspGetShipMentPrintData", S_MultipackPalletSN,
                                                    null, null, null, null, null, ref Result);

                        if (Result != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            //条码:{0} 错误信息:{1}.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_MultipackSN, ProMsg }, result);
                            txtMultipackSN.Text = string.Empty;
                            txtMultipackSN.Focus();
                            return;
                        }
                        DataTable dtPrintData = dsPrintData.Tables[0];
                        if (DT_PrintSn != null)
                        {
                            DT_PrintSn.Columns.Clear();
                            DT_PrintSn.Rows.Clear();
                        }
                        else
                        {
                            DT_PrintSn = new DataTable();
                        }
                        DT_PrintSn.Columns.Add("SN");
                        DT_PrintSn.Columns.Add("CreateTime");
                        DT_PrintSn.Columns.Add("PrintTime");

                        foreach (DataRow drPrint in dtPrintData.Rows)
                        {
                            DataRow DR = DT_PrintSn.NewRow();
                            DR["SN"] = drPrint["SerialNumber"].ToString();
                            DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            DT_PrintSn.Rows.Add(DR);
                        }

                        string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, MupltipackPalletLabelPath,
                                            DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));

                        Thread.Sleep(1000);
                        if (PrintResult != "OK")
                        {
                            //卡板:{0}包装完成,打印失败 {1}.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", List_Login.Language, new string[] { S_MultipackPalletSN, PrintResult });

                        }

                        string LabelPathData = string.Empty;
                        //打印情况3/4

                        //jimzhou,2021.10.13,处理同一张单MPN相同,LineItem不相同的情况
                        Boolean isNormalSKU = true;
                        if (dtShipmentDetail != null && dtShipmentDetail.Rows.Count > 1)
                        {
                            string mpn = dtShipmentDetail.Rows[0]["FMPNNO"].ToString();
                            for (int i = 1; i < dtShipmentDetail.Rows.Count; i++)
                            {
                                string mpn2 = dtShipmentDetail.Rows[i]["FMPNNO"].ToString();
                                if (string.Compare(mpn.Trim(), mpn2.Trim()) != 0)
                                {
                                    isNormalSKU = false;
                                    break;
                                }
                            }
                        }

                        //if (dtShipmentDetail.Rows.Count == 1)
                        if (isNormalSKU)
                        {
                            if (DT_PrintSn != null)
                            {
                                DT_PrintSn.Columns.Clear();
                                DT_PrintSn.Rows.Clear();
                            }
                            else
                            {
                                DT_PrintSn = new DataTable();
                            }
                            DT_PrintSn.Columns.Add("SN");
                            DT_PrintSn.Columns.Add("CreateTime");
                            DT_PrintSn.Columns.Add("PrintTime");
                            DataRow DR = DT_PrintSn.NewRow();
                            DR["SN"] = dtPrintData.Rows[0]["SerialNumber"].ToString();
                            DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            DT_PrintSn.Rows.Add(DR);

                            DataSet GS1LabelDs = PartSelectSVC.GetMesLabelData(GS1Name);
                            if (GS1LabelDs == null || GS1LabelDs.Tables.Count == 0 || GS1LabelDs.Tables[0].Rows.Count == 0)
                            {
                                //此工序没有配置打印标签.
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", List_Login.Language);
                                txtMultipackSN.Text = string.Empty;
                                txtMultipackSN.Focus();
                                return;
                            }
                            LabelPathData = GS1LabelDs.Tables[0].Rows[0]["LablePath"].ToString();
                        }
                        else
                        {
                            if (DT_PrintSn != null)
                            {
                                DT_PrintSn.Columns.Clear();
                                DT_PrintSn.Rows.Clear();
                            }
                            else
                            {
                                DT_PrintSn = new DataTable();
                            }
                            DT_PrintSn.Columns.Add("SN");
                            DT_PrintSn.Columns.Add("CreateTime");
                            DT_PrintSn.Columns.Add("PrintTime");
                            DataRow DR = DT_PrintSn.NewRow();
                            DR["SN"] = S_MultipackPalletSN;
                            DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            DT_PrintSn.Rows.Add(DR);

                            DataSet GS2LabelDs = PartSelectSVC.GetMesLabelData(GS2Name);
                            if (GS2LabelDs == null || GS2LabelDs.Tables.Count == 0 || GS2LabelDs.Tables[0].Rows.Count == 0)
                            {
                                //此工序没有配置打印标签.
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", List_Login.Language);
                                txtMultipackSN.Text = string.Empty;
                                txtMultipackSN.Focus();
                                return;
                            }
                            LabelPathData = GS2LabelDs.Tables[0].Rows[0]["LablePath"].ToString();
                        }



                        PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, LabelPathData,
                                            DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));

                        Thread.Sleep(1000);
                        if (PrintResult != "OK")
                        {
                            //卡板:{0}包装完成,打印失败 {1}.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", List_Login.Language, new string[] { S_MultipackPalletSN, PrintResult });

                        }
                        else
                        {
                            //卡板:{0}包装完成并发送至打印机.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10018", "OK", List_Login.Language, new string[] { S_MultipackPalletSN });
                        }

                        #endregion
                    }

                }

                if (ScanOver)
                {
                    txtBillNo.Text = string.Empty;
                    txtMultipackSN.Text = string.Empty;
                    txtBillNo.Enabled = true;
                    txtMultipackSN.Enabled = false;
                    grdControlMultipack.DataSource = null;
                    trListMultipack.DataSource = null;
                    dtMupltiPack.Rows.Clear();
                    dtShipmentDetail.Rows.Clear();
                    txtBillNo.Focus();
                    number = 1;

                    txtMultipack2DSN.Text = string.Empty;
                    txtMultipack2DSN.Enabled = false;
                }
                else
                {
                    if (IsScan2DSN)
                    {
                        txtMultipackSN.Text = string.Empty;
                        txtMultipackSN.Enabled = false;
                        txtMultipack2DSN.Text = string.Empty;
                        txtMultipack2DSN.Enabled = true;
                        txtMultipack2DSN.Focus();
                    }
                    else
                    {
                        txtMultipackSN.Text = string.Empty;
                        txtMultipackSN.Focus();
                    }

                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtBillNo.Text = string.Empty;
            txtBillNo.Enabled = true;
            txtMultipackSN.Text = string.Empty;
            txtMultipackSN.Enabled = false;

            txtMultipack2DSN.Text = string.Empty;
            if (IsScan2DSN)
            {
                lbl_Mult2D.Show();
                txtMultipack2DSN.Show();
                txtMultipack2DSN.Enabled = false;
            }
            else
            {
                lbl_Mult2D.Hide();
                txtMultipack2DSN.Hide();
            }

            grdControlMultipack.DataSource = null;
            trListMultipack.DataSource = null;
            number = 1;
            if (dtMupltiPack != null && dtMupltiPack.Rows.Count > 0)
            {
                dtMupltiPack.Rows.Clear();
            }
            if (dtShipmentDetail != null && dtShipmentDetail.Rows.Count > 0)
            {
                dtShipmentDetail.Rows.Clear();
            }
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            List_Login = this.Tag as LoginList;

            PartSelectSVC = PartSelectFactory.CreateServerClient();
            mesPackageSVC = ImesPackageFactory.CreateServerClient();
            DataCommitSVC = DataCommitFactory.CreateServerClient();

            if (dtMupltiPack == null)
            {
                dtMupltiPack = new DataTable();
                dtMupltiPack.Columns.Add("ID", typeof(int));
                dtMupltiPack.Columns.Add("MultiPackSN", typeof(string));
            }

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
            panelCtrMain.Enabled = false;
            txtBillNo.Enabled = true;
            txtBillNo.Text = string.Empty;
            txtMultipackSN.Text = string.Empty;
            txtMultipackSN.Enabled = false;
            this.Com_PartFamily.Enabled = true;
            this.Com_PartFamilyType.Enabled = true;
            this.Com_Part.Enabled = true;
            this.Com_PO.Enabled = true;
            this.Btn_ConfirmPO.Enabled = true;
            this.Btn_Refresh.Enabled = false;
            this.btnRePrint.Enabled = false;
            Btn_ReplaceBill.Enabled = true;

            public_.OpenBartender(List_Login.StationID.ToString());

            DataSet DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drIsScan2D = DS_StationTypeDef.Tables[0].Select("Description='IsScan2DSN'");
            if (drIsScan2D.Length > 0)
            {
                string tmpIsScan2DSN = drIsScan2D[0]["Content"].ToString();

                IsScan2DSN = string.IsNullOrEmpty(tmpIsScan2DSN) || tmpIsScan2DSN == "1";
            }
            else
            {
                IsScan2DSN = true;
            }

            if (IsScan2DSN)
            {
                //dataSet = PartSelectSVC.GetmesPartDetail(PartID, "SN_Pattern");
                //if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                //{
                //    SN_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
                //}
                //else
                //{
                //    MessageInfo.Add_Info_MSG(Edt_MSG, "20181", "NG", List_Login.Language, new string[] { Com_Part.Text.ToString() });
                //    return;
                //}

                DataRow[] drDataRows = DS_StationTypeDef.Tables[0].Select("Description='Pattern_2DSN'");
                if (drDataRows != null && drDataRows.Length > 0)
                {
                    SN_Pattern = drDataRows[0]["Content"].ToString();
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "Pattern_2DSN" });
                    return;
                }
            }

            btnReset_Click(null, null);

            string S_BarPrint = "";
            string S_WindowsVer = public_.GetWinVer().Substring(0, 2);

            if (S_WindowsVer == "64")
            {
                S_BarPrint = "BartenderPrint.exe";
            }
            else if (S_WindowsVer == "32")
            {
                S_BarPrint = "BartenderPrint_X86.exe";
            }

            Process[] arrayProcess = Process.GetProcessesByName(S_BarPrint.Replace(".exe", ""));
            if (arrayProcess.Length == 0)
            {
                Process p = Process.Start(S_BarPrint);
                Thread.Sleep(500);
            }
            else
            {
                foreach (Process pp in arrayProcess)
                {
                    IntPtr handle = pp.MainWindowHandle;
                    SwitchToThisWindow(handle, true);
                }
            }


        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {
                if (Com_PO.Text.Trim() == "")
                {
                    //工单不能为空,请确认.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20114", "NG", List_Login.Language);
                    return;
                }
                else
                {
                    if (Com_PartFamilyType.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyType.Text.ToString()))
                    {
                        //未选择料号类别,请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20115", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_PartFamily.EditValue == null || string.IsNullOrEmpty(Com_PartFamily.Text.ToString()))
                    {
                        //未选择料号群,请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20116", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_Part.EditValue == null || string.IsNullOrEmpty(Com_Part.Text.ToString()))
                    {
                        //未选择料号,请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20117", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_PO.EditValue == null || string.IsNullOrEmpty(Com_PO.Text.ToString()))
                    {
                        //未选择工单，请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20118", "NG", List_Login.Language);
                        return;
                    }

                    int PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                    DataSet dataSet = PartSelectSVC.GetmesPartDetail(PartID, "GS1_PalletLabelName");
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        GS1Name = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    }
                    else
                    {
                        //料号未配置参数:[{0}],请检查.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20198", "NG", List_Login.Language, new string[] { "GS1_PalletLabelName" });
                        return;
                    }

                    dataSet = PartSelectSVC.GetmesPartDetail(PartID, "GS2_PalletLabelName");
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        GS2Name = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    }
                    else
                    {
                        //料号未配置参数:[{0}],请检查.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20198", "NG", List_Login.Language, new string[] { "GS2_PalletLabelName" });
                        return;
                    }




                    // 查询模板
                    string S_StationTypeID = List_Login.StationTypeID.ToString();
                    string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_PartID = Com_Part.EditValue.ToString();
                    string S_ProductionOrderID = Com_PO.EditValue.ToString();
                    string S_LoginLineID = List_Login.LineID.ToString();


                    MupltipackPalletFormat = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID, S_LoginLineID,
                         Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
                    if (string.IsNullOrEmpty(MupltipackPalletFormat))
                    {
                        //工单未关联生成SN的格式,请检查
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20075", "NG", List_Login.Language);
                        return;
                    }

                    MupltipackPalletLabelPath = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID,
                                            S_PartID, S_ProductionOrderID, S_LoginLineID);

                    if (string.IsNullOrEmpty(MupltipackPalletLabelPath))
                    {
                        //工单未配置条码打印文件路径(LabelTemplatePath)参数,请检查.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20076", "NG", List_Login.Language);
                        return;
                    }

                    this.Com_PartFamily.Enabled = false;
                    this.Com_PartFamilyType.Enabled = false;
                    this.Com_Part.Enabled = false;
                    this.Com_PO.Enabled = false;
                    this.Btn_ConfirmPO.Enabled = false;
                    this.Btn_Refresh.Enabled = true;
                    this.panelCtrMain.Enabled = true;
                    this.btnRePrint.Enabled = true;

                    txtMultipack2DSN.Text = string.Empty;
                    if (IsScan2DSN)
                    {
                        lbl_Mult2D.Show();
                        txtMultipack2DSN.Show();
                        txtMultipack2DSN.Enabled = false;
                    }
                    else
                    {
                        lbl_Mult2D.Hide();
                        txtMultipack2DSN.Hide();
                    }
                    txtBillNo.Focus();
                }
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void ShipMent_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            PartSelectSVC.Close();
            mesPackageSVC.Close();
            DataCommitSVC.Close();
        }

        private void btnRePrint_Click(object sender, EventArgs e)
        {
            ShipMentRePrint shipMentRePrint = new ShipMentRePrint(List_Login, MupltipackPalletLabelPath, GS1Name, GS2Name);
            shipMentRePrint.ShowDialog();
        }


        private void MoveShipment(string S_BillNo, string S_MultipackSN, string S_Result)
        {
            try
            {
                string S_MoveShipment = DataCommitSVC.MoveShipment(S_BillNo, S_MultipackSN, Com_PO.EditValue.ToString().Trim(),
                   Com_Part.EditValue.ToString().Trim(), List_Login.StationID.ToString(), List_Login.EmployeeID.ToString());

                if (S_MoveShipment != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(S_MoveShipment, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_MultipackSN, ProMsg }, S_Result);
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10033", "OK", List_Login.Language, new string[] { S_MultipackSN });
                }

                DataRow[] DR_List = dtMupltiPack.Select("MultiPackSN='" + S_MultipackSN + "'");
                if (DR_List.Length > 0)
                {
                    dtMupltiPack.Rows.Remove(DR_List[0]);
                    trListMultipack.DataSource = dtMupltiPack;
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.ToString() });
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    trListMultipack.ExportToXlsx(saveFileDialog1.FileName);
                }
                catch
                {
                }
            }
        }

        private void Btn_ReplaceBill_Click(object sender, EventArgs e)
        {
            ReplaceBill_Form v_ReplaceBill_Form = new ReplaceBill_Form();
            v_ReplaceBill_Form.Show_ReplaceBill_Form(v_ReplaceBill_Form, List_Login, PartSelectSVC,
                    MupltipackPalletLabelPath, GS1Name, GS2Name);
        }

        private void txtMultipack2DSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            string mult2DSN = txtMultipack2DSN.Text.Trim();
            if (string.IsNullOrEmpty(mult2DSN))
            {
                // 条码不能为空.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                txtMultipack2DSN.Focus();
                return;
            }

            if (!Regex.IsMatch(mult2DSN, SN_Pattern))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { mult2DSN });
                txtMultipack2DSN.Text = string.Empty;
                txtMultipack2DSN.Focus();
                return;
            }


            string Result = "1";
            string xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
            string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
            string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
            string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

            DataSet multBoxSN = PartSelectSVC.uspCallProcedure("uspShipping2DBoxCheck", mult2DSN,
                xmlProdOrder, xmlPart, xmlStation, xmlExtraData, txtBillNo.Text.Trim(), ref Result);
            if (Result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { mult2DSN, ProMsg }, Result);
                txtMultipack2DSN.Text = string.Empty;
                txtMultipack2DSN.Focus();
                return;
            }

            if (multBoxSN != null && multBoxSN.Tables.Count == 1 && multBoxSN.Tables[0].Rows.Count == 1)
            {
                MultiSN_2D = multBoxSN.Tables[0].Rows[0][0].ToString();
            }

            txtMultipack2DSN.Enabled = false;
            txtMultipackSN.Enabled = true;
            txtMultipackSN.Text = string.Empty;
            txtMultipackSN.Focus();

        }
    }
}
