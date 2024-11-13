using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.MyMES.PartSelectService;
using DevExpress.XtraBars;
using System.Threading;

namespace App.MyMES
{
    public static class MessageInfo
    {
        static string S_Path_NG = Path.Combine(Application.StartupPath + "\\Sounds\\", "NG.wav");
        static string S_Path_OK = Path.Combine(Application.StartupPath + "\\Sounds\\", "OK.wav");
        static string S_Path_RE = Path.Combine(Application.StartupPath + "\\Sounds\\", "RE.wav");
        static string S_Path_QCNG = Path.Combine(Application.StartupPath + "\\Sounds\\", "QCNG.wav");
        static string S_Path_SCRAP = Path.Combine(Application.StartupPath + "\\Sounds\\", "SCRAP.wav");

        static SoundPlayer Sound_NG = new SoundPlayer(S_Path_NG);
        static SoundPlayer Sound_OK = new SoundPlayer(S_Path_OK);
        static SoundPlayer Sound_RE = new SoundPlayer(S_Path_RE);
        static SoundPlayer Sound_QCNG = new SoundPlayer(S_Path_QCNG);
        static SoundPlayer Sound_SCRAP = new SoundPlayer(S_Path_SCRAP);

        static string Lang = "EN";

        static PartSelectSVCClient PartSelectMsg = PartSelectFactory.CreateServerClient();
        static string S_Path = Directory.GetCurrentDirectory();
        public static DataTable dtHints = null;
        static string S_DayLog = "";

        public static Boolean IsQC = false;
        public static Boolean IsMessagePopup = false;
        public static int QCStatusID = 1;
        
        static QCNG_Form F_QCNG_Form = new QCNG_Form(); 

        static MessageInfo()
        {
            if (dtHints == null)
            {
                DataSet ds = PartSelectMsg.GetLanguage("Hints", "MSG");
                if (ds != null && ds.Tables.Count > 0)
                    dtHints = ds.Tables[0];
            }
        }

        public static string GetMsgByCode(string Code, string Language, string Type = "")
        {
            string msg = string.Empty;
            DataSet dsMsg = PartSelectMsg.GetLanguage(Type, "MSG");

            if (dsMsg != null && dsMsg.Tables.Count > 0)
            {
                DataTable dtdsMsg = dsMsg.Tables[0];
                Code = Code.Replace("'", "");
                DataRow[] drListmsg = dtdsMsg.Select("Code='" + Code + "' AND Language='" + Language + "'");
                if (drListmsg.Length > 0)
                {
                    msg = drListmsg[0]["Description"].ToString();
                }
            }

            if (string.IsNullOrEmpty(msg))
                msg = Code;
            return msg;
        }

        public static string GetMsgByFormName(string Code, string Language, string Type = "")
        {
            string msg = string.Empty;
            DataSet dsMsg = PartSelectMsg.GetLanguage(Type, "MSG");

            if (dsMsg != null && dsMsg.Tables.Count > 0)
            {
                DataTable dtdsMsg = dsMsg.Tables[0];
                Code = Code.Replace("'", "");
                DataRow[] drListmsg = dtdsMsg.Select("Code='" + Code + "' AND Language='" + Language + "'");
                if (drListmsg.Length > 0)
                {
                    msg = drListmsg[0]["FormName"].ToString();
                }
            }

            if (string.IsNullOrEmpty(msg))
                msg = Code;
            return msg;
        }


        /// <summary>
        /// strListPara传递提示信息中参数
        /// </summary>
        /// <param name="Edt_MSG"></param>
        /// <param name="Code"></param>
        /// <param name="S_Type"></param>
        /// <param name="Language"></param>
        public static void Add_Info_MSG(RichTextBox Edt_MSG, string Code, string S_Type, string Language, 
            string[] strListPara = null, string ShowCode = "")
        {
            string S_MSG = string.Empty;
            if (string.IsNullOrEmpty(Language))
                Language = Lang;
            if (string.IsNullOrEmpty(ShowCode))
                ShowCode = Code;
            if (S_Type == "OK")
            {
                if (dtHints != null && dtHints.Rows.Count > 0)
                {
                    DataRow[] drListHints = dtHints.Select("Code='" + Code + "' AND Language='" + Language + "'");
                    if (drListHints.Length > 0)
                    {
                        S_MSG = drListHints[0]["Description"].ToString();
                    }
                }
            }
            else
            {
                S_MSG = GetMsgByCode(Code, Language, "Error");

                if (strListPara!=null)
                {
                    if (strListPara.Count() > 2 && strListPara[2] == "Error_RE")
                    {
                        S_Type = "RE";
                    }
                }
            }

            //没有定义的提示信息
            if (string.IsNullOrEmpty(S_MSG))
            {

                DataRow[] drListError = dtHints.Select("Code='10000' AND Language='" + Language + "'");
                if (drListError.Length > 0)
                {
                    S_MSG = string.Format("MessageCode:{0},MessageDesc:{1}", ShowCode, drListError[0]["Description"].ToString());
                }

            }
            else
            {
                if (strListPara != null)
                {                    
                    S_MSG =  string.Format(S_MSG, strListPara);
                    try
                    {
                        if (strListPara[0].Substring(0, 3) == "SN:")
                        {
                            S_MSG = strListPara[0] + " " + string.Format(S_MSG, strListPara);
                        }
                    }
                    catch
                    { }

                }
                S_MSG = string.Format("MessageCode:{0},MessageDesc:{1}", ShowCode, S_MSG);
            }
            LoadMessageInfo(Edt_MSG, S_MSG, S_Type);
        }
        public static void Add_Info_MSG(RichTextBox Edt_MSG, string S_Type,
             string message)
        {
            LoadMessageInfo(Edt_MSG, message, S_Type);
        }

        public static void Add_Info_MSG(RichTextBox Edt_MSG, string Code, string S_Type, string Language,
            string[] strListPara, string ShowCode,string S_Tmp)
        {
            string S_MSG = string.Empty;
            if (string.IsNullOrEmpty(Language))
                Language = Lang;
            if (string.IsNullOrEmpty(ShowCode))
                ShowCode = Code;
            if (S_Type == "OK")
            {
                if (dtHints != null && dtHints.Rows.Count > 0)
                {
                    DataRow[] drListHints = dtHints.Select("Code='" + Code + "' AND Language='" + Language + "'");
                    if (drListHints.Length > 0)
                    {
                        S_MSG = drListHints[0]["Description"].ToString();
                    }
                }
            }
            else
            {
                S_MSG = GetMsgByCode(Code, Language, "Error");

                if (strListPara != null)
                {
                    if (strListPara.Count() > 2 && strListPara[2] == "Error_RE")
                    {
                        S_Type = "RE";
                    }
                }
            }

            //没有定义的提示信息
            if (string.IsNullOrEmpty(S_MSG))
            {

                DataRow[] drListError = dtHints.Select("Code='10000' AND Language='" + Language + "'");
                if (drListError.Length > 0)
                {
                    S_MSG = string.Format("MessageCode:{0},MessageDesc:{1}", ShowCode, drListError[0]["Description"].ToString());
                }

            }
            else
            {
                if (strListPara != null)
                {
                    S_MSG = string.Format(S_MSG, strListPara);
                    S_MSG = S_MSG.Replace("SN:", "").Replace("{}", "").Replace("{0}","");
                }
                S_MSG = string.Format("MessageCode:{0},MessageDesc:{1}", ShowCode, S_MSG);
            }
            LoadMessageInfo(Edt_MSG, S_MSG, S_Type);
        }


        public static void Add_Info_MSG(RichTextBox Edt_MSG, string Code, string S_Type, string Language,
            string[] strListPara, string ShowCode,Panel Panel_Data ,string S_IsFlashScreen)
        {
            string S_MSG = string.Empty;
            if (string.IsNullOrEmpty(Language))
                Language = Lang;
            if (string.IsNullOrEmpty(ShowCode))
                ShowCode = Code;
            if (S_Type == "OK")
            {
                if (dtHints != null && dtHints.Rows.Count > 0)
                {
                    DataRow[] drListHints = dtHints.Select("Code='" + Code + "' AND Language='" + Language + "'");
                    if (drListHints.Length > 0)
                    {
                        S_MSG = drListHints[0]["Description"].ToString();
                    }
                }

                if (S_IsFlashScreen == "1")
                {
                    Panel_Data.BackColor = Color.Green;
                    Edt_MSG.BackColor = Color.Green;
                }
            }
            else
            {
                S_MSG = GetMsgByCode(Code, Language, "Error");

                if (strListPara != null)
                {
                    if (strListPara.Count() > 2 && strListPara[2] == "Error_RE")
                    {
                        S_Type = "RE";
                    }
                }
                if (S_IsFlashScreen == "1")
                {
                    Panel_Data.BackColor = Color.Red;
                    Edt_MSG.BackColor = Color.Red;
                }
            }

            //没有定义的提示信息
            if (string.IsNullOrEmpty(S_MSG))
            {

                DataRow[] drListError = dtHints.Select("Code='10000' AND Language='" + Language + "'");
                if (drListError.Length > 0)
                {
                    S_MSG = string.Format("MessageCode:{0},MessageDesc:{1}", ShowCode, drListError[0]["Description"].ToString());
                }

            }
            else
            {
                if (strListPara != null)
                {
                    S_MSG = string.Format(S_MSG, strListPara);
                    if (strListPara[0].Substring(0, 3) == "SN:")
                    {
                        S_MSG = strListPara[0] + " " + string.Format(S_MSG, strListPara);
                    }

                }
                S_MSG = string.Format("MessageCode:{0},MessageDesc:{1}", ShowCode, S_MSG);
            }
            LoadMessageInfo(Edt_MSG, S_MSG, S_Type);
        }

        public static bool Add_Info_MessageBox(string Code, string Language, string[] strListPara = null)
        {
            string S_MSG = string.Empty;
            string Hints = string.Empty;
            if (string.IsNullOrEmpty(Language))
                Language = Lang;

            S_MSG = string.Format("MessageCode:{0},MessageDesc:{1}", Code, GetMsgByCode(Code, Language, "Hints"));

            //提示信息获取
            DataRow[] drListError = dtHints.Select("Code='10001' AND Language='" + Language + "'");
            if (drListError.Length > 0)
            {
                Hints = drListError[0]["Description"].ToString();
            }

            if (strListPara != null)
            {
                S_MSG = string.Format(S_MSG, strListPara);
            }
            
            return DevExpress.XtraEditors.XtraMessageBox.Show(S_MSG, Hints,
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        public static void Show_Info_MessageBox(string Code, string Language, MessageBoxIcon icon,string[] strListPara = null)
        {
            string S_MSG = string.Empty;
            if (string.IsNullOrEmpty(Language))
                Language = Lang;

            S_MSG = string.Format("MessageCode:{0},MessageDesc:{1}", Code, GetMsgByCode(Code, Language));

            if (strListPara != null)
            {
                S_MSG = string.Format(S_MSG, strListPara);
            }

            ShowMessageBox(S_MSG, Language, icon);
        }

        public static void ShowMessageBox(string StrMessage, string Language,MessageBoxIcon icon)
        {
            string Hints = string.Empty;
            //提示信息获取
            DataRow[] drListError = dtHints.Select("Code='10001' AND Language='" + Language + "'");
            if (drListError.Length > 0)
            {
                Hints = drListError[0]["Description"].ToString();
            }
            DevExpress.XtraEditors.XtraMessageBox.Show(StrMessage, Hints, MessageBoxButtons.OK, icon);
        }
        public static void ShowMessageBoxLoseFocus(string Code, string Language, MessageBoxIcon icon, string[] strListPara = null)
        {
            string S_MSG = string.Empty;
            if (string.IsNullOrEmpty(Language))
                Language = Lang;

            S_MSG = string.Format("MessageCode:{0},MessageDesc:{1}", Code, GetMsgByCode(Code, Language));

            if (strListPara != null)
            {
                S_MSG = string.Format(S_MSG, strListPara);
            }

            string Hints = string.Empty;
            //提示信息获取
            DataRow[] drListError = dtHints.Select("Code='10001' AND Language='" + Language + "'");
            if (drListError.Length > 0)
            {
                Hints = drListError[0]["Description"].ToString();
            }
            while (DevExpress.XtraEditors.XtraMessageBox.Show(S_MSG, Hints, MessageBoxButtons.OKCancel, icon, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                continue;
            }
            
        }
        static void LoadMessageInfo(RichTextBox Edt_MSG,string S_MSG, string S_Type)
        {
            S_MSG = string.Format("{0} ： {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), S_MSG);
            S_MSG += Environment.NewLine;
            Edt_MSG.SelectionStart = Edt_MSG.TextLength;
            Edt_MSG.SelectionLength = 0;

            try
            {
                CreateDIR();
                string S_QCStatusValue = "";

                if (S_Type == "NG")
                {
                    Sound_NG.Play();
                    Edt_MSG.SelectionColor = Color.Red;


                    if (QCStatusID == 1)
                    {
                        S_QCStatusValue = "NG";
                    }
                    if (QCStatusID == 2)
                    {                       
                        S_QCStatusValue = "NG";
                    }
                    else if (QCStatusID == 3)
                    {                       
                        S_QCStatusValue = "SCRAP";
                    }
                    else if (QCStatusID == 4)
                    {
                        S_QCStatusValue = "HOLD";
                    }
                }
                else if (S_Type == "OK")
                {
                    if (QCStatusID == 1)
                    {
                        Sound_OK.Play();
                    }
                    else if (QCStatusID == 2)
                    {
                        Sound_QCNG.Play();                        
                    }
                    else if (QCStatusID == 3)
                    {
                        Sound_SCRAP.Play();                       
                    }

                    Edt_MSG.SelectionColor = Color.Green;
                }
                else if (S_Type == "RE")
                {
                    Sound_RE.Play();
                    Edt_MSG.SelectionColor = Color.Orange;
                }


                Edt_MSG.AppendText(S_MSG);
                Edt_MSG.HideSelection = false;
                Edt_MSG.SelectionColor = Edt_MSG.ForeColor;

                if (S_Type == "Start") { S_Type = "OK"; }
                if (S_Type == "RE") { S_Type = "NG"; }


                //if (S_Type == "NG" && IsQC == true && IsMessagePopup==true && QCStatusID!=1)
                if (S_Type == "NG" && IsQC == true && IsMessagePopup == true )
                {
                    //MessageBox.Show(S_MSG, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    F_QCNG_Form.Edt_MSG.Text = S_MSG;
                    F_QCNG_Form.Lab_Status.Text = S_QCStatusValue;
                    F_QCNG_Form.ShowDialog();
                }

                File.AppendAllText(S_DayLog + S_Type + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                     DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "  " + S_MSG + "\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        /// <summary>
        /// 控件语言切换
        /// </summary>
        /// <param name="objControl"></param>
        /// <param name="dt"></param>
        /// <param name="language"></param>
        public static void LanguageForLableControl(object objControl, DataTable dt, string language)
        {
            string controlName = string.Empty;
            string captionName = string.Empty;
            DataRow[] drList = null;

            switch (objControl.GetType().Name)
            {
                case "LabelControl":
                    DevExpress.XtraEditors.LabelControl lblControl = objControl as DevExpress.XtraEditors.LabelControl;
                    lblControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    lblControl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
                    controlName = lblControl.Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as DevExpress.XtraEditors.LabelControl).Text = captionName;
                    }
                    break;
                case "Label":
                    controlName = (objControl as Label).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as Label).Text = captionName;
                    }
                    break;
                case "TextEdit":
                    controlName = (objControl as DevExpress.XtraEditors.TextEdit).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as DevExpress.XtraEditors.TextEdit).Text = captionName;
                    }
                    break;
                case "TextBox":
                    controlName = (objControl as TextBox).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as TextBox).Text = captionName;
                    }
                    break;
                case "SimpleButton":
                    controlName = (objControl as DevExpress.XtraEditors.SimpleButton).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as DevExpress.XtraEditors.SimpleButton).Text = captionName;
                    }
                    break;
                case "Button":
                    controlName = (objControl as Button).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as Button).Text = captionName;
                    }
                    break;
                case "BarButtonItem":
                    controlName = (objControl as BarButtonItem).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as BarButtonItem).Caption = captionName;
                    }
                    break;
                case "RadioButton":
                    controlName = (objControl as RadioButton).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as RadioButton).Text = captionName;
                    }
                    break;
                case "SearchLookUpEdit":
                    controlName = (objControl as DevExpress.XtraEditors.SearchLookUpEdit).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as DevExpress.XtraEditors.SearchLookUpEdit).Text = captionName;
                    }
                    break;
                case "SplitContainerControl":
                    DevExpress.XtraEditors.SplitContainerControl spcControl = objControl as DevExpress.XtraEditors.SplitContainerControl;
                    foreach (Control ctrl in spcControl.Panel1.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }

                    foreach (Control ctrl in spcControl.Panel2.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }
                    break;
                case "PanelControl":
                    DevExpress.XtraEditors.PanelControl plControl = objControl as DevExpress.XtraEditors.PanelControl;
                    foreach (Control ctrl in plControl.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }
                    break;
                case "Panel":
                    Panel pl = objControl as Panel;
                    foreach (Control ctrl in pl.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }
                    break;
                case "GroupControl":
                    DevExpress.XtraEditors.GroupControl gpControl = objControl as DevExpress.XtraEditors.GroupControl;
                    controlName = gpControl.Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        gpControl.Text = captionName;
                    }

                    if (gpControl.Name.ToString() == "GrpControlInputData")
                    {
                        string subFromName = gpControl.FindForm().Name;
                        PartSelectSVCClient PartSelectPanle = PartSelectFactory.CreateServerClient();
                        DataSet dsFrm = PartSelectPanle.GetLanguage(subFromName, "FRM");
                        PartSelectPanle.Close();
                        if (dsFrm != null && dsFrm.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtFrm = dsFrm.Tables[0];
                            foreach (object objPanles in gpControl.Controls)
                                LanguageForLableControl(objPanles, dtFrm, language);
                        }
                    }
                    else
                    {
                        foreach (Control ctrl in gpControl.Controls)
                        {
                            LanguageForLableControl(ctrl, dt, language);
                        }
                    }
                    break;
                case "BarStaticItem":
                    controlName = (objControl as DevExpress.XtraBars.BarStaticItem).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as BarStaticItem).Caption = captionName;
                    }
                    break;
                case "TablePanel":
                    DevExpress.Utils.Layout.TablePanel tablePanel = objControl as DevExpress.Utils.Layout.TablePanel;
                    foreach (Control ctrl in tablePanel.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }
                    break;
                case "GridControl":
                    foreach (DevExpress.XtraGrid.Views.Grid.GridView view in (objControl as DevExpress.XtraGrid.GridControl).Views)
                    {
                        if (view != null && view.Columns.Count > 0)
                        {
                            foreach (DevExpress.XtraGrid.Columns.GridColumn column in view.Columns)
                            {
                                controlName = column.Name;
                                drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                                if (drList.Length > 0)
                                {
                                    captionName = drList[0]["Description"].ToString();
                                    column.Caption = captionName;
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
