using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;
using App.MyMES.PartSelectService;
using System.Configuration;
using App.MyMES.mesEmployeeService;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Management;
using Microsoft.VisualBasic.Devices;
using App.MyMES.mesRouteDetailService;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using App.MyMES.DataCommitService;

namespace App.MyMES
{
    public class Public_
    {
        static string S_Path_NG = Path.Combine(Application.StartupPath + "\\Sounds\\", "NG.wav");
        static string S_Path_OK = Path.Combine(Application.StartupPath + "\\Sounds\\", "OK.wav");
        static SoundPlayer Sound_NG = new SoundPlayer(S_Path_NG);
        static SoundPlayer Sound_OK = new SoundPlayer(S_Path_OK);


        //SoundPlayer Sound_NG = new SoundPlayer(Properties.Resources.ResourceManager.GetStream("NG"));
        //SoundPlayer Sound_OK = new SoundPlayer(Properties.Resources.ResourceManager.GetStream("OK"));

        static string S_DayLog = "";
        static string S_Path = Directory.GetCurrentDirectory();

        public string S_WCF_Password = "QSCwdvEFBrgnTHMyj,@2019";
        PrintDocument fPrintDocument = new PrintDocument();
        static string S_PrintName = "";

        public Public_()
        {
            try
            {
                S_PrintName = fPrintDocument.PrinterSettings.PrinterName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public DataSet P_DataSet(string S_Sql)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataSet DS = PartSelectSVC.P_DataSet(S_Sql);

            PartSelectSVC.Close();
            return DS;
        }

        //数值(包括整数和小数)正则表达式 
        private static Regex _numericregex = new Regex(@"^[-]?[0-9]+(\.[0-9]+)?$");

        /// <summary> 
        /// 是否是数值(包括整数和小数) 
        /// </summary> 
        public static bool IsNumeric(string numericStr)
        {
            return _numericregex.IsMatch(numericStr);
        }

        public string ExecSql(string S_Sql)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            string S_Result = PartSelectSVC.ExecSql(S_Sql);

            PartSelectSVC.Close();
            return S_Result;
        }
        public string ExecSql(string S_Sql, PartSelectSVCClient PartSelectSVC)
        {
            string S_Result = PartSelectSVC.ExecSql(S_Sql);
            return S_Result;
        }
        public DevExpress.XtraGrid.Columns.GridColumn DevColumnP(string Caption, string FieldName, int MaxWidth, Boolean vVisible)
        {
            DevExpress.XtraGrid.Columns.GridColumn v_Column = new DevExpress.XtraGrid.Columns.GridColumn();
            v_Column.Caption = Caption;
            v_Column.FieldName = FieldName;
            v_Column.MaxWidth = MaxWidth;
            v_Column.Name = "Column_" + FieldName;
            v_Column.Visible = vVisible;

            return v_Column;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddStationType(MultiColumnComboBoxEx Com_StationType)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetmesStationType().Tables[0];

                Com_StationType.DataSource = DT;
                Com_StationType.DisplayMember = "ID";
                Com_StationType.ValueMember = "Description";
                Com_StationType.DisplayColumnNames = "ID,Description";

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_StationType.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddStationType(DevExpress.XtraEditors.SearchLookUpEdit Com_StationType,
                                   DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetmesStationType().Tables[0];

                Com_StationType.Properties.DataSource = DT;
                Com_StationType.Properties.DisplayMember = "Description";
                Com_StationType.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_StationType.Properties.PopupFormSize = v_Size;

                //v_gridView.Columns.Clear();                 
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_StationType.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_StationType.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void AddLine(MultiColumnComboBoxEx Com_Line)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT_Line = PartSelectSVC.GetmesLine().Tables[0];
                Com_Line.DataSource = DT_Line;
                Com_Line.DisplayMember = "ID";
                Com_Line.ValueMember = "Description";
                Com_Line.DisplayColumnNames = "ID,Description";

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Line.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddLine(DevExpress.XtraEditors.SearchLookUpEdit Com_Line, DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT_Line = PartSelectSVC.GetmesLine().Tables[0];
                Com_Line.Properties.DataSource = DT_Line;
                Com_Line.Properties.DisplayMember = "Description";
                Com_Line.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_Line.Properties.PopupFormSize = v_Size;

                //v_gridView.Columns.Clear();
                // DevExpress.Utils.AppearanceObject.
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                    //Com_Line.ShowPopup();
                    //Com_Line.ClosePopup();
                }

                if (DT_Line.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT_Line.Rows[0]["ID"].ToString());
                    Com_Line.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Line.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddLineGroup(DevExpress.XtraEditors.SearchLookUpEdit Com_LineGroup, DevExpress.XtraGrid.Views.Grid.GridView v_gridView,
                                   string LineType, int PartFamilyTypeID)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT_Line = PartSelectSVC.mesLineGroup(LineType, PartFamilyTypeID).Tables[0];
                Com_LineGroup.Properties.DataSource = DT_Line;
                Com_LineGroup.Properties.DisplayMember = "LineName";
                Com_LineGroup.Properties.ValueMember = "LineID";

                Size v_Size = new Size(450, 350);
                Com_LineGroup.Properties.PopupFormSize = v_Size;

                //v_gridView.Columns.Clear();
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////   
                    DevExpress.XtraGrid.Columns.GridColumn Column_LineID = DevColumnP("LineID", "LineID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_LineNumber = DevColumnP("LineNumber", "LineNumber", 150, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_LineName = DevColumnP("LineName", "LineName", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_LineID,
                        Column_LineNumber,
                        Column_LineName
                        }
                    );
                }

                if (DT_Line.Rows.Count > 0)
                {
                    //int ID = Convert.ToInt32(DT_Line.Rows[0]["LineNumber"].ToString());
                    //Com_LineGroup.EditValue = ID;

                    //v_gridView.SelectRow(0);
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_LineGroup.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddStstus(MultiColumnComboBoxEx Com_Status)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT_Line = PartSelectSVC.GetsysStatus().Tables[0];
                Com_Status.DataSource = DT_Line;
                Com_Status.DisplayMember = "ID";
                Com_Status.ValueMember = "Description";
                Com_Status.DisplayColumnNames = "ID,Description";

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Status.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddStstus(DevExpress.XtraEditors.SearchLookUpEdit Com_Status, DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetsysStatus().Tables[0];
                Com_Status.Properties.DataSource = DT;
                Com_Status.Properties.DisplayMember = "Description";
                Com_Status.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_Status.Properties.PopupFormSize = v_Size;

                //v_gridView.Columns.Clear();
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                    //Com_Status.ShowPopup();
                    //Com_Status.ClosePopup();
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_Status.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Status.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void AddStation(MultiColumnComboBoxEx Com_Station, string S_LineID)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetmesStation(S_LineID).Tables[0];
                Com_Station.DataSource = DT;
                Com_Station.DisplayMember = "ID";
                Com_Station.ValueMember = "Description";
                Com_Station.DisplayColumnNames = "ID,Description,StationTypeID";

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Station.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddStation(DevExpress.XtraEditors.SearchLookUpEdit Com_Station, string S_LineID, DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetmesStation(S_LineID).Tables[0];
                Com_Station.Properties.DataSource = DT;
                Com_Station.Properties.DisplayMember = "Description";
                Com_Station.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_Station.Properties.PopupFormSize = v_Size;

                //v_gridView.Columns.Clear();
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 260, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_StationTypeID = DevColumnP("StationTypeID", "StationTypeID", 100, false);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description,
                        Column_StationTypeID
                        }
                    );
                    //Com_Station.ShowPopup();
                    //Com_Station.ClosePopup();
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_Station.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Station.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void AddPartFamilyType(MultiColumnComboBoxEx Com_PartFamilyType)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetluPartFamilyType().Tables[0];

                Com_PartFamilyType.DataSource = DT;
                Com_PartFamilyType.DisplayMember = "ID";
                Com_PartFamilyType.ValueMember = "Name";
                Com_PartFamilyType.DisplayColumnNames = "ID,Description";

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_PartFamilyType.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddPartFamilyType(DevExpress.XtraEditors.SearchLookUpEdit Com_PartFamilyType,
                                      DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetluPartFamilyType().Tables[0];

                Com_PartFamilyType.Properties.DataSource = DT;
                Com_PartFamilyType.Properties.DisplayMember = "Name";
                Com_PartFamilyType.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_PartFamilyType.Properties.PopupFormSize = v_Size;
                ////////////////////////////////////////////////////////////////////////////////////////////////////

                //v_gridView.Columns.Clear();
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Name = DevColumnP("Name", "Name", 300, true);
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Name
                        }
                    );

                    //Com_PartFamilyType.ShowPopup();
                    //Com_PartFamilyType.ClosePopup();
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_PartFamilyType.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_PartFamilyType.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void AddPartFamily(MultiColumnComboBoxEx Com_PartFamily, string S_PartFamilyTypeID)
        {
            try
            {
                Com_PartFamily.DataSource = null;
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetluPartFamily(S_PartFamilyTypeID).Tables[0];

                Com_PartFamily.DataSource = DT;
                Com_PartFamily.DisplayMember = "ID";
                Com_PartFamily.ValueMember = "Name";
                Com_PartFamily.DisplayColumnNames = "ID,Name";

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_PartFamily.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddPartFamily(DevExpress.XtraEditors.SearchLookUpEdit Com_PartFamily, string S_PartFamilyTypeID,
                                  DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                Com_PartFamily.Properties.DataSource = null;
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetluPartFamily(S_PartFamilyTypeID).Tables[0];

                Com_PartFamily.Properties.DataSource = DT;
                Com_PartFamily.Properties.DisplayMember = "Name";
                Com_PartFamily.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_PartFamily.Properties.PopupFormSize = v_Size;

                //v_gridView.Columns.Clear();
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Name = DevColumnP("Name", "Name", 300, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Name,
                        Column_Description
                        }
                    );
                    //Com_PartFamily.ShowPopup();
                    //Com_PartFamily.ClosePopup();
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_PartFamily.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_PartFamily.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void AddmesUnitState(MultiColumnComboBoxEx Com_UnitState, string S_PartID, string PartFamilyID,
            string S_RouteSequence, string LineID, int StationTypeID, string ProductionOrderID, string StatusID)
        {
            try
            {
                Com_UnitState.DataSource = null;
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetmesUnitState(S_PartID, PartFamilyID, S_RouteSequence,
                    LineID, StationTypeID, ProductionOrderID, StatusID).Tables[0];

                Com_UnitState.DataSource = DT;
                Com_UnitState.DisplayMember = "ID";
                Com_UnitState.ValueMember = "Description";

                Com_UnitState.DisplayColumnNames = "ID,Description";
                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_UnitState.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddmesUnitState(DevExpress.XtraEditors.SearchLookUpEdit Com_UnitState, string S_PartID, string PartFamilyID, string S_RouteSequence, string LineID, int StationTypeID,
            DevExpress.XtraGrid.Views.Grid.GridView v_gridView, string ProductionOrderID, string StatusID)
        {
            try
            {
                Com_UnitState.Properties.DataSource = null;
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetmesUnitState(S_PartID, PartFamilyID, S_RouteSequence,
                    LineID, StationTypeID, ProductionOrderID, StatusID).Tables[0];
                Com_UnitState.Properties.DataSource = DT;
                Com_UnitState.Properties.DisplayMember = "Description";
                Com_UnitState.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_UnitState.Properties.PopupFormSize = v_Size;

                //v_gridView.Columns.Clear();
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                    //Com_UnitState.ShowPopup();
                    //Com_UnitState.ClosePopup();
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_UnitState.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_UnitState.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddPart(MultiColumnComboBoxEx Com_Part, string S_PartFamilyID)
        {
            try
            {
                Com_Part.DataSource = null;
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetmesPart(S_PartFamilyID).Tables[0];

                Com_Part.DataSource = DT;
                Com_Part.DisplayMember = "ID";
                Com_Part.ValueMember = "PartNumber";

                Com_Part.DisplayColumnNames = "ID,PartNumber,Description";
                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Part.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddPart(DevExpress.XtraEditors.SearchLookUpEdit Com_Part, string S_PartFamilyID,
                            DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                Com_Part.Properties.DataSource = null;
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetmesPart(S_PartFamilyID).Tables[0];

                Com_Part.Properties.DataSource = DT;
                Com_Part.Properties.DisplayMember = "PartNumber";
                Com_Part.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_Part.Properties.PopupFormSize = v_Size;

                //v_gridView.Columns.Clear(); 
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_PartNumber = DevColumnP("PartNumber", "PartNumber", 300, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_PartNumber,
                        Column_Description
                        }
                    );
                    //Com_Part.ShowPopup();
                    //Com_Part.ClosePopup();
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_Part.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Part.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void AddPartPrint(MultiColumnComboBoxEx Com_Part)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetmesPartPrint().Tables[0];

                Com_Part.DataSource = DT;
                Com_Part.DisplayMember = "ID";
                Com_Part.ValueMember = "PartNumber";

                Com_Part.DisplayColumnNames = "ID,PartNumber,Description,SNFormatID,SNFormatName";
                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Part.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddPOAll(MultiColumnComboBoxEx Com_PO, string S_PartID, string S_LineID)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetPOAll(S_PartID, S_LineID).Tables[0];

                Com_PO.DataSource = DT;
                Com_PO.DisplayMember = "ID";
                Com_PO.ValueMember = "ProductionOrderNumber";

                Com_PO.DisplayColumnNames = "ID,ProductionOrderNumber";
                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_PO.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddPOAll(DevExpress.XtraEditors.SearchLookUpEdit Com_PO, string S_PartID, string S_LineID,
                             DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetPOAll(S_PartID, S_LineID).Tables[0];

                Com_PO.Properties.DataSource = DT;
                Com_PO.Properties.DisplayMember = "ProductionOrderNumber";
                Com_PO.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_PO.Properties.PopupFormSize = v_Size;
                /////////////////////////////////////////////////////////////////////////////////////////////     

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_ProductionOrderNumber = DevColumnP("ProductionOrderNumber", "ProductionOrderNumber", 200, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_ProductionOrderNumber,
                        Column_Description
                        }
                    );
                    //Com_PO.ShowPopup();
                    //Com_PO.ClosePopup();
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_PO.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_PO.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void AddApplicationType(MultiColumnComboBoxEx Com_ApplicationType, string StationTypeID)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetApplicationType(StationTypeID).Tables[0];
                Com_ApplicationType.DataSource = DT;
                Com_ApplicationType.DisplayMember = "ID";
                Com_ApplicationType.ValueMember = "Description";
                Com_ApplicationType.DisplayColumnNames = "ID,Description";

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_ApplicationType.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddApplicationType(DevExpress.XtraEditors.SearchLookUpEdit Com_ApplicationType, string StationTypeID,
                                        DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetApplicationType(StationTypeID).Tables[0];

                Com_ApplicationType.Properties.DataSource = DT;
                Com_ApplicationType.Properties.DisplayMember = "ApplicationType";
                Com_ApplicationType.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_ApplicationType.Properties.PopupFormSize = v_Size;
                /////////////////////////////////////////////////////////////////////////////////////////////     

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_ApplicationType = DevColumnP("ApplicationType", "ApplicationType", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_ApplicationType
                        }
                    );
                    //Com_ApplicationType.ShowPopup();
                    //Com_ApplicationType.ClosePopup();
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_ApplicationType.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_ApplicationType.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddDefect(MultiColumnComboBoxEx Com_Defect, int DefectTypeID)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetluDefect(DefectTypeID).Tables[0];
                Com_Defect.DataSource = DT;
                Com_Defect.DisplayMember = "ID";
                Com_Defect.ValueMember = "不良代码";
                Com_Defect.DisplayColumnNames = "ID,不良代码,不良名称,位置";

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Defect.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddluUnitStatus(MultiColumnComboBoxEx Com_luUnitStatus)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetluUnitStatus().Tables[0];
                Com_luUnitStatus.DataSource = DT;
                Com_luUnitStatus.DisplayMember = "ID";
                Com_luUnitStatus.ValueMember = "Description";
                Com_luUnitStatus.DisplayColumnNames = "ID,Description";

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_luUnitStatus.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddluUnitStatus(DevExpress.XtraEditors.SearchLookUpEdit Com_luUnitStatus,
                                    DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetluUnitStatus().Tables[0];

                DataTable DT2 = new DataTable();
                DT2.Columns.Add("ID");
                DT2.Columns.Add("Description");

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    string S_ID = DT.Rows[i]["ID"].ToString();
                    string S_Description = DT.Rows[i]["Description"].ToString();

                    if (S_Description.IndexOf("ONHOLD") < 0)
                    {
                        DataRow DR = DT2.NewRow();
                        DR["ID"] = S_ID;
                        DR["Description"] = S_Description;

                        DT2.Rows.Add(DR);
                    }
                }


                Com_luUnitStatus.Properties.DataSource = DT2;
                Com_luUnitStatus.Properties.DisplayMember = "Description";
                Com_luUnitStatus.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_luUnitStatus.Properties.PopupFormSize = v_Size;

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                    //Com_luUnitStatus.ShowPopup();
                    //Com_luUnitStatus.ClosePopup();
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_luUnitStatus.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_luUnitStatus.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void AddluSerialNumberType(DevExpress.XtraEditors.SearchLookUpEdit Com_luUnitStatus,
                                    DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetluSerialNumberType().Tables[0];

                Com_luUnitStatus.Properties.DataSource = DT;
                Com_luUnitStatus.Properties.DisplayMember = "Description";
                Com_luUnitStatus.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_luUnitStatus.Properties.PopupFormSize = v_Size;

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_luUnitStatus.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_luUnitStatus.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void AddRouteDetail(DevExpress.XtraEditors.SearchLookUpEdit Com_RouteDetail,
                                    DevExpress.XtraGrid.Views.Grid.GridView v_gridView,
                                    ImesRouteDetailSVCClient mesRouteDetailSVC,
                                    string LineID,
                                    string PartID,
                                    string PartFamilyID,
                                    string ProductionOrderID
                                    )
        {
            try
            {


                DataTable DT = mesRouteDetailSVC.GetRouteDetail2(LineID,
                                            PartID, PartFamilyID, ProductionOrderID).Tables[0];

                //DataTable DT = PartSelectSVC.GetluSerialNumberType().Tables[0];

                Com_RouteDetail.Properties.DataSource = DT;
                Com_RouteDetail.Properties.DisplayMember = "StationType";
                Com_RouteDetail.Properties.ValueMember = "Sequence";

                Size v_Size = new Size(550, 350);
                Com_RouteDetail.Properties.PopupFormSize = v_Size;

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("Sequence", "Sequence", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_RouteName = DevColumnP("StationType", "StationType", 300, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_UnitStateID = DevColumnP("UnitStateID", "UnitStateID", 80, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_RouteName,
                        Column_UnitStateID
                        }
                    );
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_RouteDetail.EditValue = ID;
                }

                // PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_RouteDetail.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddStation(DevExpress.XtraEditors.SearchLookUpEdit Com_Station,
                                    DevExpress.XtraGrid.Views.Grid.GridView v_gridView,
                                    string StationTypeID,
                                    string LineID
                                    )
        {
            DevExpress.XtraEditors.SearchLookUpEdit v_Com_Station = new DevExpress.XtraEditors.SearchLookUpEdit();
            DevExpress.XtraGrid.Views.Grid.GridView F_gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            v_Com_Station = Com_Station;
            F_gridView = v_gridView;
            v_Com_Station.Properties.PopupView = F_gridView;
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetmesStation2(StationTypeID, LineID).Tables[0];

                Com_Station.Properties.DataSource = DT;
                Com_Station.Properties.DisplayMember = "Description";
                Com_Station.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_Station.Properties.PopupFormSize = v_Size;

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description

                        }
                    );
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_Station.EditValue = ID;
                }

                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                Com_Station = v_Com_Station;
                //Com_Station.Properties.DataSource = null;
                //MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void AddComProt(DevExpress.XtraEditors.SearchLookUpEdit Com_ComProt,
                                    DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                DataTable DT = new DataTable();
                DT.Columns.Add("ID");
                DT.Columns.Add("Description");

                int ID = 0;
                Computer v_Computer = new Computer();
                foreach (var item in v_Computer.Ports.SerialPortNames)
                {
                    DataRow DR = DT.NewRow();
                    DR["ID"] = ID;
                    ID += 1;
                    DR["Description"] = item;

                    DT.Rows.Add(DR);
                }

                Com_ComProt.Properties.DataSource = DT;
                Com_ComProt.Properties.DisplayMember = "Description";
                Com_ComProt.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_ComProt.Properties.PopupFormSize = v_Size;

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                }

                if (DT.Rows.Count > 0)
                {
                    ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_ComProt.EditValue = ID;
                }
            }
            catch (Exception ex)
            {
                Com_ComProt.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        /// <summary>
        /// 获取当前工序顺序
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="StationTypeID"></param>
        /// <returns></returns>
        public int RouteSequence(string PartID, string PartFamilyID, int StationTypeID, string LineID, string ProductionOrderID)
        {
            int I_Sequence = 0;
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                int RouteID = PartSelectSVC.GetRouteID(LineID, PartID, PartFamilyID, ProductionOrderID);
                DataTable DT_Route = PartSelectSVC.GetRoute("", RouteID).Tables[0];

                var v_Route = from c in DT_Route.AsEnumerable()
                              where c.Field<int>("StationTypeID") == StationTypeID
                              select c;
                //I_Sequence = v_Route.ToList()[0].Field<int>("Sequence");
                I_Sequence = Convert.ToInt32(v_Route.ToList()[0]["Sequence"].ToString());
                PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                I_Sequence = -9;
            }
            return I_Sequence;
        }
        /// <summary>
        /// 第一站是否  打印
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="StationTypeID"></param>
        /// <returns></returns>
        public Boolean IsOneStationPrint(string PartID, string PartFamilyID, int StationTypeID, string LineID, string ProductionOrderID)
        {
            Boolean B_Result = false;
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            try
            {
                int RouteID = PartSelectSVC.GetRouteID(LineID, PartID, PartFamilyID, ProductionOrderID);
                DataTable DT_Route = PartSelectSVC.GetRoute("", RouteID).Tables[0];
                PartSelectSVC.Close();

                //第一站是打印的情况 确定以后
                var v_RouteSequence = from c in DT_Route.AsEnumerable()
                                      where c.Field<int>("Sequence") == 1 && c.Field<string>("ApplicationType") == "Print"
                                      select c;

                if (v_RouteSequence.Count() > 0)
                {
                    B_Result = true;
                }
            }
            catch
            {
                PartSelectSVC.Close();
            }
            return B_Result;
        }

        public string GetluUnitStatusID(string S_PartID, string PartFamilyID, int StationTypeID,
            string LineID, string ProductionOrderID, string StatusID)
        {
            string S_UnitStateID = "";
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            try
            {
                DataSet ds = PartSelectSVC.GetmesUnitState(S_PartID, PartFamilyID, "",
                    LineID, StationTypeID, ProductionOrderID, StatusID);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable DT = PartSelectSVC.GetmesUnitState(S_PartID, PartFamilyID, "",
                        LineID, StationTypeID, ProductionOrderID, StatusID).Tables[0];
                    S_UnitStateID = DT.Rows[0]["ID"].ToString().Trim();
                }
                PartSelectSVC.Close();
            }
            catch
            {
                PartSelectSVC.Close();
            }
            return S_UnitStateID;
        }

        //public string GetluUnitStatusID_Diagram(string S_PartID, string PartFamilyID, int StationTypeID, string LineID,string ProductionOrderID)
        //{
        //    string S_UnitStateID = "";
        //    PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
        //    try
        //    {
        //        DataTable DT = PartSelectSVC.GetmesUnitState_Diagram(S_PartID, PartFamilyID.ToString(), LineID, StationTypeID, ProductionOrderID).Tables[0];
        //        if (DT != null && DT.Rows.Count > 0)
        //        {
        //            S_UnitStateID = DT.Rows[0]["OutputStateID"].ToString().Trim();
        //        }

        //        PartSelectSVC.Close();
        //    }
        //    catch
        //    {
        //        PartSelectSVC.Close();
        //    }
        //    return S_UnitStateID;
        //}

        public List<string> SnToPOID(string SN)
        {
            List<string> List_Result = new List<string>();
            try
            {
                string S_Sql = @"select A.Value SN,B.ProductionOrderID,B.PartID
                                from mesSerialNumber A JOIN mesUnit B on A.UnitID=B.ID
                            where A.Value='" + SN + "'";
                DataTable DT = P_DataSet(S_Sql).Tables[0];
                if (DT.Rows.Count > 0)
                {
                    List_Result.Add(DT.Rows[0]["ProductionOrderID"].ToString());
                    List_Result.Add(DT.Rows[0]["PartID"].ToString());
                }
                else
                {
                    List_Result.Add("ERROR20012");
                }
            }
            catch (Exception ex)
            {
                List_Result.Clear();

                List_Result.Add("ERROR20013" + ex.Message);
                List_Result.Add("");
            }
            return List_Result;
        }

        public string POID_ToPO(string POID)
        {
            string S_Result = "";
            try
            {
                string S_Sql = @"select id,ProductionOrderNumber  from mesProductionOrder 
                            where id='" + POID + "'";
                DataTable DT = P_DataSet(S_Sql).Tables[0];
                if (DT.Rows.Count > 0)
                {
                    S_Result = DT.Rows[0]["ProductionOrderNumber"].ToString();
                }
            }
            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.Message;
            }
            return S_Result;
        }

        public string GetServerIP()
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            string S_IP = PartSelectSVC.GetServerIP();
            string[] List_IP = S_IP.Split(',');

            PartSelectSVC.Close();
            return List_IP[0];
        }

        public string GetDbName()
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            string S_Result = PartSelectSVC.GetDBName();

            PartSelectSVC.Close();
            return S_Result;
        }
        public string GetDefectTypeID(string Description)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            string S_Result = PartSelectSVC.GetluDefectType(Description);
            PartSelectSVC.Close();

            return S_Result;
        }


        private static void CreateDIR()
        {
            try
            {
                if (Directory.Exists(S_Path + "\\Log") == false)
                {
                    Directory.CreateDirectory(S_Path + "\\Log");
                }

                S_DayLog = S_Path + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                if (Directory.Exists(S_DayLog) == false)
                {
                    Directory.CreateDirectory(S_DayLog);
                }

                if (Directory.Exists(S_DayLog + "OK\\") == false)
                {
                    Directory.CreateDirectory(S_DayLog + "OK\\");
                }

                if (Directory.Exists(S_DayLog + "NG\\") == false)
                {
                    Directory.CreateDirectory(S_DayLog + "NG\\");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Add_Info_MSG(RichTextBox Edt_MSG, string S_MSG, string S_Type)
        {
            S_MSG = string.Format("{0} ： {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), S_MSG);
            S_MSG += Environment.NewLine;
            Edt_MSG.SelectionStart = Edt_MSG.TextLength;
            Edt_MSG.SelectionLength = 0;

            try
            {
                CreateDIR();

                if (S_Type == "NG")
                {
                    Sound_NG.Play();
                    Edt_MSG.SelectionColor = Color.Red;
                }
                else if (S_Type == "OK")
                {
                    Sound_OK.Play();
                    Edt_MSG.SelectionColor = Color.Green;
                }

                Edt_MSG.AppendText(S_MSG);
                Edt_MSG.HideSelection = false;
                Edt_MSG.SelectionColor = Edt_MSG.ForeColor;

                if (S_Type == "Start") { S_Type = "OK"; }
                File.AppendAllText(S_DayLog + S_Type + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                     DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "  " + S_MSG + "\r\n");
                //Edt_MSG.Select(Edt_MSG.Text.Length, 0);
                //Edt_MSG.ScrollToCaret();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public DataTable BoxSnToBatch(string S_BoxSN, out string S_Result)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            S_Result = "";
            DataTable DT_Result = new DataTable();
            DataSet DS_Result = PartSelectSVC.BoxSnToBatch(S_BoxSN, out S_Result);

            if (DS_Result.Tables.Count > 0)
            {
                DT_Result = DS_Result.Tables[0];
            }

            PartSelectSVC.Close();
            return DT_Result;
        }



        public void Grid_NGColor(DataGridView v_DataGridView)
        {
            for (int i = 0; i < v_DataGridView.Rows.Count; i++)
            {
                string S_UnitState = v_DataGridView.Rows[i].Cells["UnitState"].Value.ToString();
                if (S_UnitState == "NG" || S_UnitState == "Scrap")
                {
                    v_DataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }


        public string GridToExcel(DataGridView dgvData)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "Export Excel File To";
            DialogResult dr = saveFileDialog.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return "导出取消";
            }

            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            //StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("gb2312"));
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
            string str = "";
            try
            {
                //写标题
                for (int i = 0; i < dgvData.ColumnCount; i++)
                {
                    if (i > 0)
                    {
                        str += "\t";
                    }
                    str += dgvData.Columns[i].HeaderText;
                }
                sw.WriteLine(str);
                //写内容
                for (int j = 0; j < dgvData.Rows.Count; j++)
                {
                    string tempStr = "";
                    for (int k = 0; k < dgvData.Columns.Count; k++)
                    {
                        if (k > 0)
                        {
                            tempStr += "\t";
                        }
                        string cellValue = dgvData.Rows[j].Cells[k].Value.ToString();
                        cellValue = cellValue.Replace(" ", "");
                        cellValue = cellValue.Replace("\r", "");
                        cellValue = cellValue.Replace("\n", "");
                        cellValue = cellValue.Replace("\r\n", "");
                        tempStr += cellValue;
                        // tempStr += dgvData.Rows[j].Cells[k].Value.ToString();
                    }

                    sw.WriteLine(tempStr);
                }
                sw.Close();
                myStream.Close();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }

            return "OK";
        }


        /// <summary>
        /// 获取指定目录中的匹配项（文件或目录）
        /// </summary>
        /// <param name="dir">要搜索的目录</param>
        /// <param name="regexPattern">项名模式（正则）。null表示忽略模式匹配，返回所有项</param>
        /// <param name="recurse">是否搜索子目录</param>
        /// <param name="throwEx">是否抛异常</param>
        /// <returns></returns>
        private string[] GetFileSystemEntries(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
        {
            List<string> lst = new List<string>();

            try
            {
                foreach (string item in Directory.GetFileSystemEntries(dir))
                {
                    try
                    {
                        if (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        { lst.Add(item); }

                        //递归
                        if (recurse && (File.GetAttributes(item) & FileAttributes.Directory) == FileAttributes.Directory)
                        { lst.AddRange(GetFileSystemEntries(item, regexPattern, true)); }
                    }
                    catch { if (throwEx) { throw; } }
                }
            }
            catch { if (throwEx) { throw; } }

            return lst.ToArray();
        }

        /// <summary>
        /// 获取指定目录中的匹配文件
        /// </summary>
        /// <param name="dir">要搜索的目录</param>
        /// <param name="regexPattern">文件名模式（正则）。null表示忽略模式匹配，返回所有文件</param>
        /// <param name="recurse">是否搜索子目录</param>
        /// <param name="throwEx">是否抛异常</param>
        /// <returns></returns>
        private string[] GetFiles(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
        {
            List<string> lst = new List<string>();

            try
            {
                foreach (string item in Directory.GetFileSystemEntries(dir))
                {
                    try
                    {
                        bool isFile = (File.GetAttributes(item) & FileAttributes.Directory) != FileAttributes.Directory;

                        if (isFile && (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline)))
                        { lst.Add(item); }

                        //递归
                        if (recurse && !isFile) { lst.AddRange(GetFiles(item, regexPattern, true)); }
                    }
                    catch { if (throwEx) { throw; } }
                }
            }
            catch { if (throwEx) { throw; } }

            return lst.ToArray();
        }

        /// <summary>
        /// 获取指定目录中的匹配目录
        /// </summary>
        /// <param name="dir">要搜索的目录</param>
        /// <param name="regexPattern">目录名模式（正则）。null表示忽略模式匹配，返回所有目录</param>
        /// <param name="recurse">是否搜索子目录</param>
        /// <param name="throwEx">是否抛异常</param>
        /// <returns></returns>
        private string[] GetDirectories(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
        {
            List<string> lst = new List<string>();

            try
            {
                foreach (string item in Directory.GetDirectories(dir))
                {
                    try
                    {
                        if (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        { lst.Add(item); }

                        //递归
                        if (recurse) { lst.AddRange(GetDirectories(item, regexPattern, true)); }
                    }
                    catch { if (throwEx) { throw; } }
                }
            }
            catch { if (throwEx) { throw; } }

            return lst.ToArray();
        }


        public DataSet GetLabelCMD(PartSelectSVCClient PartSelectSVC, string StationTypeID,
            string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            DataSet DS = PartSelectSVC.GetLabelCMD(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
            return DS;
        }


        public string GetLabelName(PartSelectSVCClient PartSelectSVC, string StationTypeID,
            string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            string S_LabelName = "";
            try
            {
                DataTable DT_LabelName = PartSelectSVC.GetLabelName(StationTypeID, PartFamilyID,
                                         PartID, ProductionOrderID, LineID).Tables[0];
                int lengthPa = 0;
                string outPut = string.Empty;
                string suffix = string.Empty;
                string targetPath = string.Empty;
                string PageCapacity = string.Empty;
                for (int i = 0; i < DT_LabelName.Rows.Count; i++)
                {
                    S_Path = DT_LabelName.Rows[i]["LabelPath"].ToString();
                    outPut = DT_LabelName.Rows[i]["OutPutType"].ToString();
                    targetPath = DT_LabelName.Rows[i]["TargetPath"].ToString();
                    PageCapacity = DT_LabelName.Rows[i]["PageCapacity"].ToString();

                    lengthPa = S_Path.LastIndexOf('.');
                    suffix = S_Path.Substring(lengthPa + 1, S_Path.Length - lengthPa - 1);

                    if (string.IsNullOrEmpty(outPut) || (outPut != "5" && outPut != "6" && outPut != "7" && outPut != "1" && outPut != "2"))
                    {
                        return "ERROR20097";
                    }

                    if (outPut == "5" && suffix.ToUpper() != "LAB")
                    {
                        return "ERROR20102";
                    }
                    else if (outPut == "6" && suffix.ToUpper() != "BTW")
                    {
                        return "ERROR20103";
                    }
                    else if (outPut == "7" && suffix.ToUpper() != "BTW")
                    {
                        return "ERROR20103";
                    }
                    else if (outPut == "2" && suffix.ToUpper() != "PRN")
                    {
                        return "ERROR20104";
                    }

                    S_LabelName = (string.IsNullOrEmpty(S_LabelName) ? "" : S_LabelName + ";") + DT_LabelName.Rows[i]["LabelName"].ToString() +
                        "," + DT_LabelName.Rows[i]["LabelPath"].ToString() + "," + outPut + "," + targetPath + "," + PageCapacity;
                }
            }
            catch
            { }
            return S_LabelName;
        }

        static LabelManager2.Application LabSN;
        public static string PrintCodeSoftSN(PartSelectSVCClient PartSelectSVC, string S_LabelName,
                                                DataTable DT_PrintSn, int I_PartID)
        {
            try
            {
                string S_Result = "OK";
                if (DT_PrintSn == null || DT_PrintSn.Rows.Count == 0)
                {
                    S_Result = "20107";
                    return S_Result;
                }

                string LabelName = string.Empty;
                string LabelPath = string.Empty;
                string OutPut = string.Empty;
                string TargetPath = string.Empty;
                string[] List_Label = S_LabelName.Split(';');
                int PageCapacity = 1;
                int I_LabelQTY = 0;

                foreach (string item in List_Label)
                {
                    I_LabelQTY += 1;
                    string[] ListItem = item.Split(',');
                    LabelName = ListItem[0].ToString();
                    LabelPath = ListItem[1].ToString();
                    OutPut = ListItem[2].ToString();
                    TargetPath = ListItem[3].ToString();
                    if (!string.IsNullOrEmpty(ListItem[4].ToString()))
                    {
                        //int number = Convert.ToInt32(ListItem[4].ToString());
                        //if (number > 0)
                        //{
                        //    PageCapacity = number;
                        //}

                        int number = Convert.ToInt32(ListItem[4].ToString());
                        if (I_LabelQTY > 1 && DT_PrintSn.Rows.Count > 1)
                        {
                            DataTable DT_New = new DataTable();
                            for (int k = 0; k < DT_PrintSn.Columns.Count; k++)
                            {
                                DT_New.Columns.Add(DT_PrintSn.Columns[k].ToString());
                            }

                            DataRow DR_New = DT_New.NewRow();
                            DT_New.Rows.Add(DR_New);

                            for (int k = 0; k < DT_PrintSn.Columns.Count; k++)
                            {
                                DT_New.Rows[0][DT_PrintSn.Columns[k].Caption] = DT_PrintSn.Rows[0][k].ToString();
                            }
                            DT_New.Rows[0]["tmpPath"] = LabelPath;

                            DT_PrintSn = DT_New;
                        }

                        if (number > 1 && DT_PrintSn.Rows.Count == 1)
                        {
                            for (int i = 1; i < number; i++)
                            {
                                DataRow DR = DT_PrintSn.NewRow();
                                DR[0] = DT_PrintSn.Rows[0][0].ToString();
                                DR[1] = DT_PrintSn.Rows[0][1].ToString();

                                DT_PrintSn.Rows.Add(DR);
                            }
                            PageCapacity = 1;
                        }
                        else
                        {
                            if (number > 0)
                            {
                                PageCapacity = number;
                            }
                        }

                    }

                    if (!DT_PrintSn.Columns.Contains("TmpPath"))
                    {
                        DT_PrintSn.Columns.Add("TmpPath");
                    }
                    if (!DT_PrintSn.Columns.Contains("PartID"))
                    {
                        DT_PrintSn.Columns.Add("PartID");
                    }
                    if (!DT_PrintSn.Columns.Contains("TargetPath"))
                    {
                        DT_PrintSn.Columns.Add("TargetPath");
                    }
                    DT_PrintSn.Rows[0]["PartID"] = I_PartID;
                    DT_PrintSn.Rows[0]["TmpPath"] = LabelPath;
                    DT_PrintSn.Rows[0]["TargetPath"] = TargetPath;

                    if (OutPut != "5" && OutPut != "6" && OutPut != "7" && OutPut != "1" && OutPut != "2")
                    {
                        S_Result = "20108";
                    }
                    if (OutPut == "5" && LabSN == null)
                    {
                        LabSN = new LabelManager2.Application();
                    }

                    while (PageCapacity > 0)
                    {
                        MESLabel.MESLabel.PrintLabel("", PartSelectSVC, LabSN, DT_PrintSn, null,
                            LabelName, false, false, S_PrintName, "", false, "");
                        PageCapacity--;
                    }
                }
                return S_Result;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        //public static string PrintCodeSoftSN(PartSelectSVCClient PartSelectSVC, string S_LabelName,
        //                                        DataTable DT_PrintSn, int I_PartID,DataTable DR_LabelCMD)
        //{
        //    try
        //    {
        //        string S_Result = "OK";
        //        if (DT_PrintSn == null || DT_PrintSn.Rows.Count == 0)
        //        {
        //            S_Result = "20107";
        //            return S_Result;
        //        }

        //        string LabelName = string.Empty;
        //        string LabelPath = string.Empty;
        //        string OutPut = string.Empty;
        //        string TargetPath = string.Empty;
        //        string[] List_Label = S_LabelName.Split(';');
        //        int PageCapacity = 1;
        //        foreach (string item in List_Label)
        //        {
        //            string[] ListItem = item.Split(',');
        //            LabelName = ListItem[0].ToString();
        //            LabelPath = ListItem[1].ToString();
        //            OutPut = ListItem[2].ToString();
        //            TargetPath = ListItem[3].ToString();
        //            if (!string.IsNullOrEmpty(ListItem[4].ToString()))
        //            {
        //                int number = Convert.ToInt32(ListItem[4].ToString());
        //                if (number > 0)
        //                {
        //                    PageCapacity = number;
        //                }
        //            }

        //            if (!DT_PrintSn.Columns.Contains("TmpPath"))
        //            {
        //                DT_PrintSn.Columns.Add("TmpPath");
        //            }
        //            if (!DT_PrintSn.Columns.Contains("PartID"))
        //            {
        //                DT_PrintSn.Columns.Add("PartID");
        //            }
        //            if (!DT_PrintSn.Columns.Contains("TargetPath"))
        //            {
        //                DT_PrintSn.Columns.Add("TargetPath");
        //            }
        //            DT_PrintSn.Rows[0]["PartID"] = I_PartID;
        //            DT_PrintSn.Rows[0]["TmpPath"] = LabelPath;
        //            DT_PrintSn.Rows[0]["TargetPath"] = TargetPath;

        //            if (OutPut != "5" && OutPut != "6" && OutPut != "1" && OutPut != "2")
        //            {
        //                S_Result = "20108";
        //            }
        //            if (OutPut == "5" && LabSN == null)
        //            {
        //                LabSN = new LabelManager2.Application();
        //            }

        //            while (PageCapacity > 0)
        //            {
        //                MESLabel.MESLabel.PrintLabel("", PartSelectSVC, LabSN, DT_PrintSn, null,
        //                    LabelName, false, false, S_PrintName, "", false, "", DR_LabelCMD);
        //                PageCapacity--;
        //            }
        //        }
        //        return S_Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }

        //}

        public string GetLabelPath(PartSelectSVCClient PartSelectSVC, string StationTypeID,
            string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            string S_LabelPath = "";
            DataTable DT_LabelName = null;
            try
            {
                DataSet DtsPath = PartSelectSVC.GetLabelName(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
                if (DtsPath != null && DtsPath.Tables.Count > 0)
                {
                    DT_LabelName = DtsPath.Tables[0];

                    for (int i = 0; i < DT_LabelName.Rows.Count; i++)
                    {
                        S_LabelPath += DT_LabelName.Rows[i]["LabelPath"].ToString() + ";";
                    }
                }
            }
            catch (Exception ex)
            {
                S_LabelPath = "ERROR:" + ex.ToString();
            }

            //if (DT_LabelName == null || DT_LabelName.Rows.Count == 0)
            //{
            //    S_LabelPath = "";
            //}
            return S_LabelPath;
        }


        public string GetWinVer()
        {
            string _operatingSystem = "";
            string _osArchitecture = "";
            using (ManagementObjectSearcher win32OperatingSystem = new ManagementObjectSearcher("select * from Win32_OperatingSystem"))
            {
                foreach (ManagementObject obj in win32OperatingSystem.Get())
                {
                    _operatingSystem = obj["Caption"].ToString();
                    _osArchitecture = obj["OSArchitecture"].ToString();
                    break;
                }
            }
            return _osArchitecture;
        }

        public int ASC(String Data) //获取ASC码
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(Data);
            int p = 0;

            if (b.Length == 1) //如果为英文字符直接返回
                return (int)b[0];

            for (int i = 0; i < b.Length; i += 2)
            {
                p = (int)b[i];
                p = p * 256 + b[i + 1] - 65536;
            }
            return p;
        }

        public void AddLanguage(DevExpress.XtraEditors.SearchLookUpEdit v_Com, DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                DataTable DT_Language = new DataTable();
                DT_Language.Columns.Add("ID");
                DT_Language.Columns.Add("Description");

                DataRow DR = DT_Language.NewRow();
                DR["ID"] = 1;
                DR["Description"] = "CN";
                DT_Language.Rows.Add(DR);

                DR = DT_Language.NewRow();
                DR["ID"] = 2;
                DR["Description"] = "EN";
                DT_Language.Rows.Add(DR);

                v_Com.Properties.DataSource = DT_Language;
                v_Com.Properties.DisplayMember = "Description";
                v_Com.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                v_Com.Properties.PopupFormSize = v_Size;

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                }

                if (DT_Language.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT_Language.Rows[0]["ID"].ToString());
                    v_Com.EditValue = ID;
                }
            }
            catch (Exception ex)
            {
                v_Com.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddSkin(DevExpress.XtraEditors.SearchLookUpEdit v_Com, DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                DataTable DT_Skin = new DataTable();
                DT_Skin.Columns.Add("ID");
                DT_Skin.Columns.Add("Description");

                int I_ID = 1;
                foreach (DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins)
                {
                    DataRow DR = DT_Skin.NewRow();
                    DR["ID"] = I_ID;
                    DR["Description"] = skin.SkinName;
                    DT_Skin.Rows.Add(DR);

                    I_ID += 1;
                }

                v_Com.Properties.DataSource = DT_Skin;
                v_Com.Properties.DisplayMember = "Description";
                v_Com.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                v_Com.Properties.PopupFormSize = v_Size;

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                }

                if (DT_Skin.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT_Skin.Rows[0]["ID"].ToString());
                    v_Com.EditValue = ID;
                }
            }
            catch (Exception ex)
            {
                v_Com.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string GetVer()
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            string S_Ver = PartSelectSVC.GetVer();
            PartSelectSVC.Close();
            return S_Ver;
        }

        public string IsUpdate()
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            string S_Result = PartSelectSVC.P_DataSet("select * from sysVersion").Tables[0].Rows[0]["VersionName"].ToString();


            PartSelectSVC.Close();
            return S_Result;
        }

        public DataTable GetmesStationConfig(string Name, string StationID)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetmesStationConfig(Name, StationID).Tables[0];
            PartSelectSVC.Close();
            return DT;
        }

        public void AddWHType(DevExpress.XtraEditors.SearchLookUpEdit v_Com, DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                DataTable DT_Language = new DataTable();
                DT_Language.Columns.Add("ID");
                DT_Language.Columns.Add("Description");

                DataRow DR = DT_Language.NewRow();
                DR["ID"] = 0;
                DR["Description"] = "暂未确认NoConfirm";
                DT_Language.Rows.Add(DR);

                DR = DT_Language.NewRow();
                DR["ID"] = 1;
                DR["Description"] = "已确认栈板纸ConfirmedOK";
                DT_Language.Rows.Add(DR);

                DR = DT_Language.NewRow();
                DR["ID"] = 2;
                DR["Description"] = "已扫描出库ShipScanOK";
                DT_Language.Rows.Add(DR);

                DR = DT_Language.NewRow();
                DR["ID"] = 3;
                DR["Description"] = "已确认出货Shipped";
                DT_Language.Rows.Add(DR);

                DR = DT_Language.NewRow();
                DR["ID"] = 4;
                DR["Description"] = "已生成EDI";
                DT_Language.Rows.Add(DR);

                DR = DT_Language.NewRow();
                DR["ID"] = 5;
                DR["Description"] = "已发送SAP_Recieved";
                DT_Language.Rows.Add(DR);

                DR = DT_Language.NewRow();
                DR["ID"] = 6;
                DR["Description"] = "已关闭Closed";
                DT_Language.Rows.Add(DR);

                DR = DT_Language.NewRow();
                DR["ID"] = 999;
                DR["Description"] = "ALL";
                DT_Language.Rows.Add(DR);

                v_Com.Properties.DataSource = DT_Language;
                v_Com.Properties.DisplayMember = "Description";
                v_Com.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                v_Com.Properties.PopupFormSize = v_Size;

                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////                
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Description = DevColumnP("Description", "Description", 300, true);
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Description
                        }
                    );
                }

                if (DT_Language.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT_Language.Rows[0]["ID"].ToString());
                    v_Com.EditValue = ID;
                }
            }
            catch (Exception ex)
            {
                v_Com.Properties.DataSource = null;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public string GetWebIP()
        {
            string S_Result = "";
            try
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                S_Result = config.AppSettings.Settings["WebIP"].Value;
            }
            catch
            {
            }
            return S_Result;
        }

        public void ShowTimeSpan(string name, ref DateTime dateTimeStart)
        {
            LogNet.LogDug("test", name + " :" + (DateTime.Now - dateTimeStart).TotalMilliseconds.ToString());
            dateTimeStart = DateTime.Now;
        }
        /// <summary>
        /// UPC站选择的是区域工单，只检查料号
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="POID"></param>
        /// <param name="PartID"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public DataSet GetAndCheckUnitInfoUPC(string barcode, string PartID, PartSelectSVCClient PartSelectSVC)
        {
            try
            {
                string sql = @"SELECT b.*,d.PartFamilyID
                            FROM dbo.mesSerialNumber a
                            JOIN dbo.mesUnit b ON b.ID = a.UnitID
                            JOIN dbo.mesProductionOrder c ON b.ProductionOrderID = c.ID
                            JOIN dbo.mesPart d ON  d.ID = c.PartID
                            WHERE a.Value = '" + barcode + "' AND d.ID = '" + PartID + "'";
                return PartSelectSVC.P_DataSet(sql);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetmesUnitDetail(string S_UnitID, PartSelectSVCClient PartSelectSVC)
        {
            string S_Sql = "UPDATE mesUnitDetail SET reserved_04 = '2' where reserved_04 = '1' and  UnitID='" + S_UnitID + "'";
            PartSelectSVC.P_DataSet(S_Sql);
        }

        public DataSet GetTTBindBoxStatus(string S_PartID, string S_ProductionOrderID, string S_TTBoxType,
            string S_IsCheckPO, LoginList List_Login, PartSelectSVCClient PartSelectSVC)
        {
            string S_Sql = "";
            string S_SerialNumberTypeID = "0";
            if (S_TTBoxType == "1")
            {
                S_SerialNumberTypeID = "10";
            }
            else if (S_TTBoxType == "2")
            {
                S_SerialNumberTypeID = "8";
            }
            else if (S_TTBoxType == "3")
            {
                S_SerialNumberTypeID = "9";
            }

            if (S_IsCheckPO == "1")
            {
                S_Sql = @"SELECT A.ID ValStr1,C.Value ValStr2 
                        FROM mesUnit A JOIN mesUnitDetail B ON A.ID=B.UnitID 
                            JOIN mesSerialNumber C ON A.ID=C.UnitID  and C.SerialNumberTypeID='" + S_SerialNumberTypeID + @"' 
                        WHERE A.ProductionOrderID ='" + S_ProductionOrderID + @"' 
                            AND B.reserved_04 = 1 AND A.ID =
                            (
                                SELECT max(A.ID)
                                FROM mesUnit A JOIN mesUnitDetail B ON A.ID = B.UnitID                                             
                                WHERE A.ProductionOrderID ='" + S_ProductionOrderID + @"' AND B.reserved_04 = 1
                                and A.StationID='" + S_IsCheckPO + @"'
   	                    )";

            }
            else
            {
                S_Sql = @"SELECT A.ID ValStr1,C.Value ValStr2 
                        FROM mesUnit A JOIN mesUnitDetail B ON A.ID=B.UnitID 
                            JOIN mesSerialNumber C ON A.ID=C.UnitID and C.SerialNumberTypeID='" + S_SerialNumberTypeID + @"'  
                        WHERE A.PartID ='" + S_PartID + @"' AND B.reserved_04 = 1 AND A.ID =
                            (
                                SELECT max(A.ID)
                                FROM mesUnit A JOIN mesUnitDetail B ON A.ID = B.UnitID                                             
                                WHERE A.PartID ='" + S_PartID + @"' AND B.reserved_04 = 1
                                and A.StationID='" + List_Login.StationID.ToString() + @"'
   	                    )";
            }
            return P_DataSet(S_Sql);
        }
        public DataSet SetNullBoxMachineStatus(string S_mesMachineSN)
        {
            string S_Sql = "UPDATE mesMachine SET StatusID = 1 WHERE SN='" + S_mesMachineSN + "'";
            return P_DataSet(S_Sql);

        }


        public DataSet GetmesLineOrder(string S_LineID, string S_ProductionOrderID)
        {
            S_LineID = S_LineID ?? "";
            S_ProductionOrderID = S_ProductionOrderID ?? "";

            if (S_LineID == "")
            {
                return null;
            }
            if (S_LineID == "" && S_ProductionOrderID == "")
            {
                return null;
            }

            string S_Sql = "select * FROM mesLineOrder WHERE LineID='" + S_LineID +
                "' AND ProductionOrderID='" + S_ProductionOrderID + "'";
            return P_DataSet(S_Sql);

        }


        public string GetTTBindBoxChildSN(string S_BoxSN, PartSelectSVCClient PartSelectSVC)
        {
            string S_Sql =
                @"SELECT B.[Value] ChildSN FROM mesUnit A 
	                JOIN mesSerialNumber B ON A.ID=B.UnitID
                WHERE A.PanelID IN 
                (
	                SELECT UnitID FROM mesSerialNumber WHERE [Value]='" + S_BoxSN + @"'
                )";
            string S_Result = "";
            DataSet DS= PartSelectSVC.P_DataSet(S_Sql);
            if (DS.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    S_Result += DS.Tables[0].Rows[i]["ChildSN"].ToString() + "\r\n";
                } 
            }
            return S_Result;
        }


        public DataSet Get_PalletProductData(string S_SN, PartSelectSVCClient PartSelectSVC)
        {
            string S_Sql = @"SELECT ROW_NUMBER() OVER(ORDER BY A.ID) SEQNO, B.SerialNumber KITSN,B.LastUpdate TIME,b.CurrPartID,b.CurrProductionOrderID,
                 d.UnitStateID,d.PartID,d.ProductionOrderID,d.StatusID,e.Value
                FROM mesPackage A 
                INNER JOIN mesPackage B ON A.ID=B.ParentID 
                JOIN dbo.mesUnitDetail c ON c.InmostPackageID = b.ID
                JOIN dbo.mesUnit d ON d.ID = c.UnitID
                JOIN dbo.mesSerialNumber e ON e.UnitID = d.ID
                WHERE A.Stage=2 AND A.SerialNumber= '" + S_SN + "'";
            return PartSelectSVC.P_DataSet(S_Sql);
        }

        /// <summary>
        /// 通过设备SN获取对应实际的料号
        /// </summary>
        /// <param name="S_SN"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public DataSet GetPartIdByMachineSN(string S_SN, PartSelectSVCClient PartSelectSVC)
        {
            string S_Sql = $@"SELECT TOP 1 b.PartID
                            FROM dbo.mesMachine a
                            LEFT JOIN dbo.mesRouteMachineMap b ON  a.ID = b.MachineID or 
								                            a.PartID =b.MachinePartID or 
								                            a.MachineFamilyID =b.MachineFamilyID
                            WHERE a.SN ='{S_SN}'
                            ORDER BY b.MachineID DESC, b.MachineFamilyID DESC,b.MachinePartID DESC";
            return PartSelectSVC.P_DataSet(S_Sql);
        }

        /// <summary>
        /// 根据物料条码获取料号
        /// </summary>
        /// <param name="S_SN">批次切割后条码</param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public DataSet GetPartIdByMaterialUnitSN(string S_SN, PartSelectSVCClient PartSelectSVC)
        {
            string S_Sql = $@"SELECT  PartID
                                FROM dbo.mesMaterialUnit
                                WHERE SerialNumber= '{S_SN}'";
            return PartSelectSVC.P_DataSet(S_Sql);
        }

        /// <summary>
        /// 画图类型的工艺路线，是否是最后一站
        /// </summary>
        /// <param name="stationTypeId"></param>
        /// <param name="routeId"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public bool IsDiagramSFCLastStation(string stationTypeId, string routeId, PartSelectSVCClient PartSelectSVC)
        {
            bool isSFCLastStation = false;
            string S_Sql = $@"SELECT 1 FROM mesUnitoUTputState A
						LEFT JOIN V_StationTypeInfo B ON A.StationTypeID=B.StationTypeID
						WHERE A.RouteID={routeId} and A.StationTypeID='{stationTypeId}' 
						AND ISNULL(B.Content,'')<>'TT' AND CurrStateID=0
						 AND A.OutputStateDefID=1";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);
            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
            {
               string res = ds.Tables[0].Rows[0][0]?.ToString();
               isSFCLastStation = res == "1";
            }
            return isSFCLastStation;
        }
        /// <summary>
        /// 获取工艺路线类型
        /// </summary>
        /// <param name="I_RouteID"></param>
        /// <param name="partSelectSvc"></param>
        /// <returns></returns>
        public DataSet GetRouteType(int I_RouteID,PartSelectSVCClient partSelectSvc)
        {
            DataSet dsRoute = null;
            string routeID = I_RouteID.ToString();

            string S_Sql = string.Format(@"select * from mesRoute where ID={0}", I_RouteID);

            dsRoute = partSelectSvc.P_DataSet(S_Sql);

            return dsRoute;
        }
        /// <summary>
        /// 表格类工艺路线，判断是否是最后一站
        /// </summary>
        /// <param name="stationTypeId"></param>
        /// <param name="routeId"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public bool IsTableFCLastStation(string stationTypeId, string routeId, string UnitStateID, PartSelectSVCClient PartSelectSVC)
        {
            bool isSFCLastStation = false;
            string S_Sql = $@"SELECT 1
                            FROM dbo.mesRouteDetail a
                            WHERE a.RouteID = {routeId} AND a.StationTypeID = {stationTypeId} AND a.UnitStateID = {UnitStateID} AND a.Sequence = (SELECT MAX(b.Sequence) FROM dbo.mesRouteDetail b WHERE b.RouteID = a.RouteID)";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);
            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
            {
                string res = ds.Tables[0].Rows[0][0]?.ToString();
                isSFCLastStation = res == "1";
            }
            return isSFCLastStation;
        }
        /// <summary>
        /// 是否存在子料绑定关系
        /// true 则存在绑定关系，反之亦然
        /// </summary>
        /// <param name="mainUnitID"></param>
        /// <param name="childUnitID"></param>
        /// <param name="stationTypeId"></param>
        /// <param name="BatchNumber"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public bool IsExistBindChildPart(int mainUnitID, int childUnitID, string stationTypeId,string BatchNumber, string childPartId,PartSelectSVCClient PartSelectSVC)
        {
            bool isSFCLastStation = false;
            string S_Sql = $@"SELECT 1
            FROM dbo.mesUnitComponent a
            JOIN dbo.mesStation b ON b.ID = a.InsertedStationID
            WHERE b.StationTypeID = {stationTypeId} AND a.UnitID = {mainUnitID}  AND a.StatusID = 1  AND a.ChildPartID = {childPartId}";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);
            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                string res = ds.Tables[0].Rows[0][0]?.ToString();
                isSFCLastStation = res == "1";
            }
            return isSFCLastStation;
        }

        /// <summary>
        /// 查找父料料号
        /// </summary>
        /// <param name="currentPartId">子料</param>
        /// <param name="mainPartId">FG主码的料号</param>
        /// <param name="stationTypeId"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public string CheckBindParentPart(int currentPartId, int mainPartId,int stationTypeId, PartSelectSVCClient PartSelectSVC)
        {
            string parentPartID = "0";
            string S_Sql = $@"DECLARE @StationTypeID INT = {stationTypeId},@tmpPartId INT = {currentPartId},@tmpMainPartId INT = {mainPartId},@parentPartId INT = 0
                            --当扫入的条码是子料时，才需要检查主料是否已经扫描
                            SELECT TOP 1 @parentPartId = a.ParentPartID
                            FROM dbo.mesProductStructure a
                            WHERE a.StationTypeID = @StationTypeID
                            AND a.PartID = @tmpPartId AND a.ParentPartID <> @tmpMainPartId  AND a.Status  = 1 
                            ORDER BY a.ID DESC

                            SELECT @parentPartId";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);
            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                parentPartID = ds.Tables[0].Rows[0][0]?.ToString();
            }
            return parentPartID;
        }

        /// <summary>
        /// 判断导入的批次码是否存在
        /// --》 true    不存在条码记录
        /// --》 false   存在条码记录
        /// </summary>
        /// <param name="SNs"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns> </returns>
        public bool CheckBatchNumberExist(string SNs, PartSelectSVCClient PartSelectSVC)
        {
            string S_Sql = $@"
                            DECLARE @tmpSNS VARCHAR(MAX) = '{SNs}'
                            DECLARE @tmpTab TABLE(
	                            SN VARCHAR(200)
                            )
                            INSERT INTO @tmpTab(SN)
                            SELECT value FROM dbo.F_Split(@tmpSNS,',')

                            --SELECT *
                            --FROM @tmpTab a
                            --WHERE EXISTS(SELECT 1 FROM dbo.mesSerialNumber b WHERE b.Value = a.SN)
                            --UNION ALL
                            SELECT *
                            FROM @tmpTab c 
                            WHERE EXISTS(SELECT 1 FROM dbo.mesMaterialUnit d WHERE d.SerialNumber = c.SN)";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);

            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取批次对应的Bin信息
        /// </summary>
        /// <param name="batchNumber"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public string GetBatchBin(string batchNumber, PartSelectSVCClient PartSelectSVC)
        {
            string result = "";
            string S_Sql = $@"
                            SELECT b.SerialNumber BinInfo
                            FROM dbo.mesMaterialUnit a
                            JOIN dbo.mesMaterialUnit b ON b.ID = a.ParentID
                            WHERE a.SerialNumber = '{batchNumber}'";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);

            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0][0].ToString();
            }
            return result;
        }



        /// <summary>
        /// 上传条码及相关信息
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="StationID"></param>
        /// <param name="LineID"></param>
        /// <param name="UnitStateID"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public string UploadSNList( string EmployeeID, string StationID, string LineID,string PartID, PartSelectSVCClient PartSelectSVC)
        {
            string result = "";
            string S_Sql = $@"DECLARE @EmployeeID INT = {EmployeeID}, @PartID INT={PartID}, @StationID INT = {StationID}, @LineID INT = {LineID}
                            SELECT a.* 
                            INTO #tmpExistTab
                            FROM tmpBinSNList a
                            JOIN dbo.mesMaterialUnit b  WITH(NOLOCK) ON b.SerialNumber = a.ChildSN
                            IF EXISTS(SELECT * FROM #tmpExistTab)
                            BEGIN
                                SELECT 'many sn had exist. please check it' ErrorMessage
	                            RETURN
                            END

                            BEGIN TRAN
                            BEGIN TRY
                                SELECT MainSN
                                INTO #tmpMainSNs 
	                            FROM tmpBinSNList
	                            GROUP BY MainSN

                                INSERT INTO dbo.mesMaterialUnit(SerialNumber, PartID, CreationTime, ParentID, Quantity)
                                SELECT a.MainSN,0,GETDATE(),0,0
                                FROM #tmpMainSNs a
                                WHERE NOT EXISTS(SELECT * FROM dbo.mesMaterialUnit b WHERE b.SerialNumber = a.MainSN)

	                            INSERT INTO dbo.mesMaterialUnit(SerialNumber, PartID, StatusID, StationID, EmployeeID, CreationTime, LineID, ParentID, Quantity)
                                SELECT a.ChildSN,@PartID,1,@StationID,@EmployeeID,GETDATE(),@LineID,b.ID,1
                                FROM tmpBinSNList a
	                            JOIN dbo.mesMaterialUnit b WITH(NOLOCK) ON b.SerialNumber = a.MainSN

                                SELECT '1' as ErrorMessage
	                            TRUNCATE TABLE tmpBinSNList
                                COMMIT TRAN
                            END TRY
                            BEGIN CATCH
                                ROLLBACK TRAN
                                SELECT ERROR_MESSAGE() as ErrorMessage
                            END CATCH
                            ";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);

            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0][0].ToString();
            }
            return result;
        }
        /// <summary>
        /// 获取BinID
        /// </summary>
        /// <param name="BinNumber"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public string UploadSingleBin(string BinNumber, PartSelectSVCClient PartSelectSVC)
        {
            string result = "";
            string S_Sql = $@"DECLARE @BinSN VARCHAR(200) ='{BinNumber}'
                            IF	EXISTS(SELECT * FROM dbo.mesMaterialUnit WHERE SerialNumber = @BinSN)
                            BEGIN
                                SELECT TOP 1 ID FROM dbo.mesMaterialUnit WHERE SerialNumber = @BinSN
                            END
                            ELSE
                            BEGIN
                                INSERT INTO dbo.mesMaterialUnit(SerialNumber, PartID,  CreationTime,  ParentID, Quantity)
                                VALUES(@BinSN, -- SerialNumber - varchar(200)
                                0   , -- PartID - int
                                GETDATE(), -- CreationTime - datetime
                                0,   -- ParentID - int
                                  0  )

	                            SELECT  TOP 1  ID FROM dbo.mesMaterialUnit WHERE SerialNumber = @BinSN
                            END";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);

            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0][0].ToString();
            }
            return result;
        }
        
        public bool CheckTabExist(PartSelectSVCClient PartSelectSVC)
        {
            bool result = false;
            string S_Sql = $@"
                            IF NOT EXISTS(SELECT * FROM sys.sysobjects WHERE type = 'U' AND name = 'tmpBinSNList')
                            BEGIN
	                            CREATE TABLE tmpBinSNList(
		                            MainSN NVARCHAR(400),
		                            ChildSN NVARCHAR(400)
	                            )
                            END
                            ELSE
                            BEGIN
                                TRUNCATE TABLE tmpBinSNList
                            END
                            ";
            var res = PartSelectSVC.ExecSql(S_Sql);
            return result = (res == "OK");
        }


        public string UploadSingleSN(string SN, string BinID, string PartID, PartSelectSVCClient PartSelectSVC)
        {
            string result = "";
            string S_Sql = $@"DECLARE  @SN VARCHAR(200) ='{SN}',@PartID INT = {PartID}, @BinID INT ={BinID}
                            IF	EXISTS(SELECT * FROM dbo.mesMaterialUnit WHERE SerialNumber = @SN)
                            BEGIN
                                SELECT TOP 1 ID FROM dbo.mesMaterialUnit WHERE SerialNumber = @SN
                            END
                            ELSE
                            BEGIN
                                INSERT INTO dbo.mesMaterialUnit(SerialNumber, PartID,  CreationTime,  ParentID, Quantity)
                                VALUES(@SN, -- SerialNumber - varchar(200)
                                @PartID   , -- PartID - int
                                GETDATE(), -- CreationTime - datetime
                                @BinID,   -- ParentID - int
                                  1  )

	                            SELECT '1'
                            END";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);

            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0][0].ToString();
            }
            return result;
        }

        /// <summary>
        /// string parentId, string parentPartId, string parentSN
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="PartSelectSVC"></param>
        /// <returns></returns>
        public Tuple<string , string , string> QueryParentTooling(string SN,  PartSelectSVCClient PartSelectSVC)
        {

            string S_Sql = $@"DECLARE  @SN VARCHAR(200) = '{SN}', @parentPartId INT,@parentId INT,@parentSN VARCHAR(200)
SELECT @parentId = ISNULL(a.ParentID, 0)
FROM dbo.mesMachine a
WHERE a.SN = @SN

IF @parentId IS NOT NULL AND @parentId <> 0
BEGIN
    SELECT @parentPartId = ISNULL(PartID, 0), @parentSN = SN FROM dbo.mesMachine WHERE ID = @parentId
END

SELECT @parentId parentId, @parentPartId parentPartId, @parentSN parentSN
";
            DataSet ds = PartSelectSVC.P_DataSet(S_Sql);

            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                return new Tuple<string, string, string>(ds.Tables[0].Rows[0][0].ToString(), ds.Tables[0].Rows[0][1]?.ToString(), ds.Tables[0].Rows[0][2]?.ToString());
            }
            else
            {
                return  new Tuple<string, string, string>("","","");
            }
        }

        [DllImport("user32.dll")]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        public void OpenBartender(string S_StationID)
        {
            string S_Result = string.Empty;
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                try
                {
                    DataSet DS = PartSelectSVC.GetPLCSeting("Bartender_Ver", S_StationID);
                    if (DS != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                    {
                        DataTable DT = DS.Tables[0];

                        if (DT.Rows.Count > 0)
                        {
                            string S_Value = DT.Rows[0]["Value"].ToString().Trim();

                            if (S_Value != "")
                            {
                                File.Delete(S_Path + "\\Seagull.BarTender.Print.dll");
                                File.Delete(S_Path + "\\BartenderPrint.exe");
                                File.Delete(S_Path + "\\BartenderPrint_X86.exe");
                                File.Delete(S_Path + "\\BartenderPrint.exe.config");
                                File.Delete(S_Path + "\\BartenderPrint_X86.exe.config");


                                File.Copy(S_Path + "\\Bartender\\" + S_Value + "\\Seagull.BarTender.Print.dll", S_Path + "\\Seagull.BarTender.Print.dll");
                                File.Copy(S_Path + "\\Bartender\\" + S_Value + "\\BartenderPrint.exe", S_Path + "\\BartenderPrint.exe");
                                File.Copy(S_Path + "\\Bartender\\" + S_Value + "\\BartenderPrint.exe.config", S_Path + "\\BartenderPrint.exe.config");
                                File.Copy(S_Path + "\\Bartender\\" + S_Value + "\\BartenderPrint_X86.exe", S_Path + "\\BartenderPrint_X86.exe");
                                File.Copy(S_Path + "\\Bartender\\" + S_Value + "\\BartenderPrint_X86.exe.config", S_Path + "\\BartenderPrint_X86.exe.config");
                            }
                        }
                    }
                }
                catch
                { }


                DataSet ds = PartSelectSVC.uspCallProcedure("uspGetCheckBartender", S_StationID, "", "",
                                                                "", "", "", ref S_Result);
                if (S_Result == "1")
                {
                    string S_BarPrint = "";
                    string S_WindowsVer = GetWinVer().Substring(0, 2);

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
                            //SetParent(pp.MainWindowHandle, Panel_Bar.Handle);
                            //ShowWindow(pp.MainWindowHandle, (int)ProcessWindowStyle.Maximized);
                        }
                    }
                }
            }
            catch
            { }

            if (S_Result == "1")
            {
                int I_Port = 6899;
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    I_Port = Convert.ToInt32(config.AppSettings.Settings["Port"].Value.Trim());
                }
                catch
                {

                }

                try
                {
                    if (SocketTCPClient._clientSocket == null || !SocketTCPClient._clientSocket.Connected)
                    {
                        SocketTCPClient.Close();
                        SocketTCPClient.Server = "127.0.0.1";
                        SocketTCPClient.Port = I_Port;
                        SocketTCPClient.Connect();
                    }
                }
                catch
                { }
            }
        }


        //用于双击截图
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, // 目标 DC的句柄
                                          int nXDest,
                                          int nYDest,
                                          int nWidth,
                                          int nHeight,
                                          IntPtr hdcSrc,  // 源DC的句柄
                                          int nXSrc,
                                          int nYSrc,
                                          System.Int32 dwRop  // 光栅的处理数值
                                          );



        public string GetAppSet(PartSelectSVCClient PartSelectSVC,string S_SetName)
        {
            string S_SetValue = "";
            try
            {
                S_SetValue = PartSelectSVC.GetAppSet(S_SetName);                
            }
            catch(Exception ex)
            { }
            return S_SetValue;
        }


    }
}
