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
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Management;
using Microsoft.VisualBasic.Devices;

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

        public void AddmesUnitState(MultiColumnComboBoxEx Com_UnitState, string S_PartID, string S_RouteSequence, string LineID, int StationTypeID)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetmesUnitState(S_PartID, S_RouteSequence, LineID, StationTypeID).Tables[0];

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

        public void AddmesUnitState(DevExpress.XtraEditors.SearchLookUpEdit Com_UnitState, string S_PartID, string S_RouteSequence, string LineID, int StationTypeID,
            DevExpress.XtraGrid.Views.Grid.GridView v_gridView)
        {
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                DataTable DT = PartSelectSVC.GetmesUnitState(S_PartID, S_RouteSequence, LineID, StationTypeID).Tables[0];
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
        public int RouteSequence(string PartID, int StationTypeID, string LineID)
        {
            int I_Sequence = 0;
            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT_Route = PartSelectSVC.GetRoute(PartID, "", LineID, StationTypeID).Tables[0];

                var v_Route = from c in DT_Route.AsEnumerable()
                              where c.Field<int>("StationTypeID") == StationTypeID
                              select c;
                I_Sequence = v_Route.ToList()[0].Field<int>("Sequence");
                PartSelectSVC.Close();
            }
            catch
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
        public Boolean IsOneStationPrint(string PartID, int StationTypeID, string LineID)
        {
            Boolean B_Result = false;
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            try
            {
                DataTable DT_Route = PartSelectSVC.GetRoute(PartID, "", LineID, StationTypeID).Tables[0];
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

        public string GetluUnitStatusID(string S_PartID, int StationTypeID, string LineID)
        {
            string S_UnitStateID = "";
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            try
            {
                int I_RouteSequence = RouteSequence(S_PartID, StationTypeID, LineID);
                DataTable DT = PartSelectSVC.GetmesUnitState(S_PartID, I_RouteSequence.ToString(), LineID, StationTypeID).Tables[0];
                S_UnitStateID = DT.Rows[0]["ID"].ToString().Trim();

                PartSelectSVC.Close();
            }
            catch
            {
                PartSelectSVC.Close();
            }
            return S_UnitStateID;
        }

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
                    List_Result.Add("ERROR:SN不存在！");
                }
            }
            catch (Exception ex)
            {
                List_Result.Clear();

                List_Result.Add("ERROR: 数据库连接失败 " + ex.Message);
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
            string S_IP = "Server:" + PartSelectSVC.GetServerIP();
            string[] List_IP = S_IP.Split(',');

            PartSelectSVC.Close();
            return List_IP[0];
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


        public string GetLabelName(PartSelectSVCClient PartSelectSVC, string StationTypeID, 
            string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            string S_LabelName = "";
            try
            {
                DataTable DT_LabelName = PartSelectSVC.GetLabelName(StationTypeID, PartFamilyID,
                                         PartID, ProductionOrderID, LineID).Tables[0];

                for (int i = 0; i < DT_LabelName.Rows.Count; i++)
                {
                    S_LabelName += DT_LabelName.Rows[0]["LabelName"].ToString()+";";
                }
            }
            catch
            { }
            return S_LabelName;
        }

        public string GetLabelPath(PartSelectSVCClient PartSelectSVC, string StationTypeID, 
            string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            string S_LabelPath = "";
            try
            {
                DataTable DT_LabelName = PartSelectSVC.GetLabelName(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID).Tables[0];

                for (int i = 0; i < DT_LabelName.Rows.Count; i++)
                {
                    S_LabelPath += DT_LabelName.Rows[0]["LabelPath"].ToString()+";";
                }
            }
            catch (Exception ex)
            {
                S_LabelPath = "ERROR:" + ex.ToString();
            }
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
    }
}
