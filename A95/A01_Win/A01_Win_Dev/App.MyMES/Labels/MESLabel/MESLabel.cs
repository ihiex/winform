using App.Model;
using App.MyMES.PartSelectService;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WdProtocolNet;
using App.MyMES;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace MESLabel
{
	public class MESLabel
	{
        static LabelManager2.Application LabSN;
        static Panel Panel_Bar;
        static string S_LabelName;
        static Public_ public_ = new Public_();  
        string S_WindowsVer = "64";
        int I_Port = 6800;
        Socket clientSocket;

        [DllImport("User32.dll", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessageA(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private extern static IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


        public enum PrinterType
		{
			ZPLDirect,
			TmpZPL,
			LWCSV,
			LMCSV,
            CodeSoft,
            Bartender
		}

		public enum LabelType
		{
			UnitInfo,
			MasterInfo,
			PalletInfo,
			MaterialUnitInfo,
			MachineInfo,
			MachineTableInfo,
			MachineTableSlotInfo,
			CartInfo
		}

		public enum FormatPos
		{
			Header,
			Body,
			Footer
		}

		public class LabelFormat : IComparable
		{
			private ArrayList m_fields;
			private ArrayList m_data;
			private string m_name;
			private MESLabel.FormatPos m_pos;

			public ArrayList Fields
			{
				get
				{
					return this.m_fields;
				}
			}

			public ArrayList Dats
			{
				get
				{
					return this.m_data;
				}
			}

			public string Name
			{
				get
				{
					return this.m_name;
				}
			}

			public int Pos
			{
				get
				{
					return (int)this.m_pos;
				}
			}

			public LabelFormat(string name, int pos)
			{
				this.m_name = name;
				this.m_pos = (MESLabel.FormatPos)pos;
				this.m_fields = new ArrayList();
				this.m_data = new ArrayList();
			}

			public void AddField(MESLabel.LabelField field)
			{
				this.m_fields.Add(field);
			}

			public void AddData(MESDBFTable table)
			{
				this.m_data.Add(table);
			}

			public int CompareTo(object obj)
			{
				return this.m_pos.CompareTo(((MESLabel.LabelFormat)obj).m_pos);
			}
		}

        
        public class LabelField
		{
			private string m_name;
			private string m_formula;
            //private FFDBFTable m_table ;

            public object Name
			{
				get
				{
					return this.m_name;
				}
			}

			public object Formala
			{
				get
				{
					return this.m_formula;
				}
			}
            public MESDBFTable table;
            public object Value
			{
				get
				{
					return MESParser.GetValue(MESScaner.GetTopTokens(this.m_formula), table);
				}
			}

			public LabelField(string name, string formula)
			{
				this.m_name = name;
				this.m_formula = formula;
			}
		}

		protected const string cstrLBL = "LBL";

		protected const string cstrName = "NAM";

		protected const string cstrType = "TYP";

		protected const string cstrOut = "OUT";

		protected const string cstrTFP = "TFP";

		protected const string cstrCMD = "CMD";

		protected const string cstrSFP = "SFP";

		protected const string cstrCAP = "CAP";

		protected const string cstrFMT = "FMT";

		protected const string cstrPOS = "POS";

		protected const string cstrDAT = "DAT";

		protected const string cstrFLD = "FLD";

		protected const string cstrELM = "ELM";

		protected const string cstrALS = "ALS";

		protected const string ZPL_START = "^XA";

		protected const string ZPL_END = "^XZ";

		protected const string ZPL_FORMATNAME = "^XF";

		protected const string ZPL_ENDOFLINE = "^FS";

		protected const string ZPL_FIELDNAME = "^FN";

		protected const string ZPL_FIELDVALUE = "^FD";

		protected const string ZPL_CARET = "^";

		protected const string CSV_SPEPERATOR = ",";

		protected const string CSV_QUOTE = "\"";

		protected const string CSV_FORMAT = "Format";

		protected const string CSV_QUANTITY = "LLMQTY";

		protected static bool m_csvAddCounter = false;

		protected static bool m_moreCaret = false;

		protected static string m_printerName;

		protected static MESLabel m_meslabel = null;

		protected static string m_stationName;

		protected string m_name;

		protected MESLabel.LabelType m_type;

		protected MESLabel.PrinterType m_out;

		protected string m_cmd;

		protected string m_tfp;

		protected string m_sfp;

		protected int m_cap;

		protected int m_LineRecordNumber;

		protected int m_TotalLineRecord;

		public static ArrayList m_formats=new ArrayList();


        public string CMD
		{
			get
			{
				return this.m_cmd;
			}
			set
			{
				this.m_cmd = value;
			}
		}

		public string TFP
		{
			get
			{
				return this.m_tfp;
			}
			set
			{
				this.m_tfp = value;
			}
		}

		public string SFP
		{
			get
			{
				return this.m_sfp;
			}
			set
			{
				this.m_sfp = value;
			}
		}

		public int CAP
		{
			get
			{
				return this.m_cap;
			}
			set
			{
				this.m_cap = value;
			}
		}

		public MESLabel.PrinterType OutputType
		{
			get
			{
				return this.m_out;
			}
		}

		public static string PrinterName
		{
			get
			{
				return MESLabel.m_printerName;
			}
		}

		public static MESLabel LabelObj
		{
			get
			{
				return MESLabel.m_meslabel;
			}
		}

		public int LineRecordNumber
		{
			get
			{
				return this.m_LineRecordNumber;
			}
		}

		public int TotalLineRecord
		{
			get
			{
				return this.m_TotalLineRecord;
			}
		}

		public MESLabel(string name, MESLabel.LabelType type, MESLabel.PrinterType @out)
		{
			this.m_LineRecordNumber = 1;
			this.m_TotalLineRecord = 1;
			this.m_name = name;
			this.m_type = type;
			this.m_out = @out;
            //m_formats = new ArrayList();

            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                I_Port =Convert.ToInt32(config.AppSettings.Settings["Port"].Value.Trim());
            }
            catch
            {

            }


            //  2019-12-24  改为自动识别
            //try
            //{
            //    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //    S_WindowsVer = config.AppSettings.Settings["WindowsVer"].Value.Trim();
            //}
            //catch
            //{

            //}
        }

		public  void AddFormat(MESLabel.LabelFormat lbl)
		{
            m_formats.Clear();
            m_formats.Add(lbl);
		}

		public virtual void Print(DataTable DT_Print, PartSelectSVCClient PartSelectSVC)
		{
			switch (this.m_out)
			{
			    case MESLabel.PrinterType.ZPLDirect:
				    this.ZPLDirectPrint();
				    break;
			    case MESLabel.PrinterType.TmpZPL:
				    this.ZPLTemplate();
				    break;
			    case MESLabel.PrinterType.LWCSV:
				    this.LWCSVPrint(true);
				    break;
			    case MESLabel.PrinterType.LMCSV:
				    this.LMCSVPrint();
				    break;
                case MESLabel.PrinterType.CodeSoft:
                    CodeSoftPrint(DT_Print, PartSelectSVC);
                    break;
                case MESLabel.PrinterType.Bartender:
                    BartenderPrint(DT_Print, PartSelectSVC);
                    break;
            }
		}

		protected virtual void ZPLDirectPrint()
		{
			StringBuilder sb = new StringBuilder();
			m_formats.Sort();
			checked
			{
				int num = m_formats.Count - 1;
				for (int i = num; i >= 0; i += -1)
				{
					MESLabel.LabelFormat labelFormat = (MESLabel.LabelFormat)m_formats[i];
					if (labelFormat.Pos - 1 == 1)
					{
						this.m_TotalLineRecord = (int)Math.Round((double)labelFormat.Dats.Count / (double)labelFormat.Fields.Count);
					}
					this.GetZPLFormat(labelFormat, sb);
				}
				try
				{
					this.GenerateLabel(sb, null, false);
				}
				catch (Exception expr_8B)
				{
					ProjectData.SetProjectError(expr_8B);
					Exception ex = expr_8B;
					throw MESLBLException.CreateInstance(MESLBLException.ErrCode.FILE_OP_ERROR, ex.Message);
				}
				finally
				{                    
				}
			}
		}

		protected virtual void MoreCaret(StringBuilder sb)
		{
			if (MESLabel.m_moreCaret)
			{
				sb.Append("^");
			}
		}

		protected virtual string GetZPLFormat(MESLabel.LabelFormat format, StringBuilder sb)
		{
			checked
			{
				int num = format.Dats.Count - 1;
				for (int i = num; i >= 0; i += -1)
				{
					MESDBFTable dat = (MESDBFTable)format.Dats[i];
					this.MoreCaret(sb);
					sb.Append("^XA");
					sb.Append("^XF");
					sb.Append(format.Name);
					sb.Append("^FS" + Environment.NewLine);
					this.GetZPLField(format.Fields, dat, sb);
				}
				string result="";
				return result;
			}
		}

		protected virtual object GetZPLField(ArrayList fields, MESDBFTable dat, StringBuilder sb)
		{
			try
			{
				IEnumerator enumerator = fields.GetEnumerator();
				while (enumerator.MoveNext())
				{
					MESLabel.LabelField labelField = (MESLabel.LabelField)enumerator.Current;
                    labelField.table = dat; 


                    this.MoreCaret(sb);
					sb.Append("^FN");
					sb.Append(RuntimeHelpers.GetObjectValue(labelField.Name));
					sb.Append("^FD");
					sb.Append(RuntimeHelpers.GetObjectValue(dat));
					sb.Append("^FS" + Environment.NewLine);
				}
			}
			finally
			{
			}
			this.MoreCaret(sb);
			sb.Append("^XZ" + Environment.NewLine);
			sb.Append(Environment.NewLine);
			object result="";
			return result;
		}

        protected static string DTtoXML(DataTable DT_LBLGenLabel)
        {
            string S_Result = "";
            for (int i = 0; i < DT_LBLGenLabel.Rows.Count; i++)
            {
                string S_Content = DT_LBLGenLabel.Rows[i]["Content"].ToString();
                S_Result += S_Content;
            }
            return S_Result;
        }

        protected virtual void CodeSoftPrint(DataTable DT_Print, PartSelectSVCClient PartSelectSVC)
        {
            LabelManager2.Document doc = null;
            try
            {                
                string S_Path_Lab = this.SFP;                
                LabSN.Documents.Open(S_Path_Lab, false);
                doc = LabSN.ActiveDocument;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i< DT_Print.Rows.Count; i++)
            {
                DataTable DTxml = PartSelectSVC.GetLBLGenLabel(DT_Print.Rows[i]["SN"].ToString(), S_LabelName).Tables[0];
                string S_xml = DTtoXML(DTxml);

                MESLabel.m_meslabel = null;
                MESLabel.m_meslabel = MESLabel.ParseXML(S_xml, false , "");
               
                if (m_formats.Count == 1)
                {
                    try
                    {
                        foreach (var item in m_formats)
                        {
                            MESLabel.LabelFormat labelFormat = (MESLabel.LabelFormat)item;
                            if (labelFormat.Pos - 1 == 1)
                            {
                                this.m_TotalLineRecord = (int)Math.Round((double)labelFormat.Dats.Count / (double)labelFormat.Fields.Count);
                            }
                            MESDBFTable table = (MESDBFTable)labelFormat.Dats[0];

                            try
                            {
                                foreach (var item2 in labelFormat.Fields)
                                {
                                    MESLabel.LabelField labelField = (MESLabel.LabelField)item2;
                                    labelField.table = table;
                                    doc.Variables.Item(labelField.Name).Value = labelField.Value.ToString().Replace("#","");
                                }
                                DT_Print.Rows[i]["PrintTime"]= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                doc.PrintLabel(1);
                            }
                            catch (Exception ex)
                            {
                                doc.Close();
                                //LabSN.Quit();
                                MessageBox.Show("先检查模板变量和标签设置是否一致  "+ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        doc.FormFeed();
                        doc.Close();
                        //LabSN.Quit();
                        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            doc.FormFeed();
            doc.Close();
            //LabSN.Quit();
        }

        App.MyMES.MyINI INI;
        protected virtual void BartenderPrint(DataTable DT_Print, PartSelectSVCClient PartSelectSVC)
        {
            string S_TmpPath = DT_Print.Rows[0]["TmpPath"].ToString();
            //S_TmpPath = S_TmpPath.Replace(";", "");
            string[] List_TmpPath = S_TmpPath.Split(';'); 

            int I_RowCount = DT_Print.Rows.Count;
            string S_DBPath = Path.GetDirectoryName(S_TmpPath) + "\\BarTender.txt";
            File.Delete(S_DBPath);
            int I_Start = 0;

            if (INI == null)
            {
                INI = new App.MyMES.MyINI(Application.StartupPath + "\\BarTender.ini");
            }
            INI.WriteValue("Path", "Path", S_TmpPath);
            INI.WriteValue("Port", "Port", I_Port.ToString());
            

            for (int i = 0; i< DT_Print.Rows.Count; i++)
            {
                DataTable DTxml = PartSelectSVC.GetLBLGenLabel(DT_Print.Rows[i]["SN"].ToString(), S_LabelName).Tables[0];
                string S_xml = DTtoXML(DTxml);
                
                MESLabel.m_meslabel = null;
                MESLabel.m_meslabel = MESLabel.ParseXML(S_xml, false , "");
               
                if (m_formats.Count == 1)
                {
                    try
                    {
                        foreach (var item in m_formats)
                        {
                            MESLabel.LabelFormat labelFormat = (MESLabel.LabelFormat)item;
                            if (labelFormat.Pos - 1 == 1)
                            {
                                this.m_TotalLineRecord = (int)Math.Round((double)labelFormat.Dats.Count / (double)labelFormat.Fields.Count);
                            }
                            MESDBFTable table = (MESDBFTable)labelFormat.Dats[0];

                            try
                            {
                                if (I_Start == 0)
                                {
                                    string S_Title = "";
                                    foreach (var item2 in labelFormat.Fields)
                                    {
                                        MESLabel.LabelField labelField = (MESLabel.LabelField)item2;
                                        labelField.table = table;
                                        S_Title += "\"" + labelField.Name + "\""+",";                                        
                                    }
                                    File.AppendAllText(S_DBPath, S_Title + "\r\n");
                                }

                                string S_Value = "";
                                foreach (var item2 in labelFormat.Fields)
                                {
                                    MESLabel.LabelField labelField = (MESLabel.LabelField)item2;
                                    labelField.table = table;
                                    S_Value+= "\"" + labelField.Value + "\"" + ",";
                                }
                                File.AppendAllText(S_DBPath, S_Value + "\r\n");

                                DT_Print.Rows[i]["PrintTime"]= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("先检查模板变量和标签设置是否一致  "+ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                I_Start = 1;
            }

            string S_BarPrint = "";
            S_WindowsVer = public_.GetWinVer().Substring(0, 2);

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

            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            if (clientSocket == null)
            {
                 clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            try
            {
                if (clientSocket.Connected == false)
                {
                    clientSocket.Connect(new IPEndPoint(ip, I_Port)); //配置服务器IP与端口  
                }
            }
            catch
            {
                MessageBox.Show("连接Bartender打印服务失败", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);               
                return;
            }

            try
            {
                Thread.Sleep(200);   
                string sendMessage = "Print";
                clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));                
            }
            catch
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();                
            }
            

            //Thread.Sleep(300);
            //const int BM_CLICK = 0xF5;
            //Task task = new Task(() =>
            //{
            //    while (true)
            //    {
            //        //测试警告框
            //        IntPtr maindHwnd = FindWindow(null, "BartenderPrint");//主窗口标题
            //        if (maindHwnd != IntPtr.Zero)
            //        {
            //            IntPtr childHwnd = FindWindowEx(maindHwnd, IntPtr.Zero, null, "BartenderPrint");//按钮控件标题
            //            if (childHwnd != IntPtr.Zero)
            //            {
            //                SendMessage(childHwnd, BM_CLICK, 0, 0);
            //                return;
            //            }
            //        }
            //    }
            //});
            //task.Start();

        }

        protected virtual void ZPLTemplate()
		{
			//checked
			{
                FileStream stream = null;
                StreamReader streamReader = null;

                //try
				{
					 stream = File.Open(this.SFP, FileMode.Open);
					 streamReader = new StreamReader(stream, Encoding.UTF8);
					StringBuilder stringBuilder = new StringBuilder(streamReader.ReadToEnd());

                    if (m_formats.Count == 1)
					{
						try
						{
                            foreach (var item in m_formats)
                            {
                                MESLabel.LabelFormat labelFormat = (MESLabel.LabelFormat)item;
                                if (labelFormat.Pos - 1 == 1)
                                {
                                    this.m_TotalLineRecord = (int)Math.Round((double)labelFormat.Dats.Count / (double)labelFormat.Fields.Count);
                                }
                                MESDBFTable table = (MESDBFTable)labelFormat.Dats[0];
                                
                                
                                try
                                {
                                    foreach (var item2 in labelFormat.Fields)
                                    {
                                        MESLabel.LabelField labelField = (MESLabel.LabelField)item2;
                                        labelField.table = table;

                                        stringBuilder.Replace(Conversions.ToString(Operators.AddObject(Operators.AddObject("%", labelField.Name), "%")), 
                                            Conversions.ToString(labelField.Value));   

                                    }
                                }
                                catch
                                {
                                }
                            }
							goto IL_141;
						}
						finally
						{
						}
						goto IL_139;
						IL_141:

                        stream.Close();
                        streamReader.Close();
                        this.GenerateLabel(stringBuilder, null, false);
						return;
					}

                 
                IL_139:
					throw MESLBLException.CreateInstance(MESLBLException.ErrCode.ZPL_TEM_ERROR);
				}
				//catch (Exception expr_14D)
				//{
				//	ProjectData.SetProjectError(expr_14D);
				//	Exception ex = expr_14D;
				//	throw FFLBLException.CreateInstance(FFLBLException.ErrCode.FILE_OP_ERROR, ex.Message);
				//}
				//finally
				//{
    //                //stream.Close();
    //                //streamReader.Close();
    //            }
			}
		}

		protected virtual void LWCSVPrint(bool renameFile = true)
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			ArrayList arrayList4 = new ArrayList();
			ArrayList arrayList5 = new ArrayList();
			ArrayList arrayList6 = new ArrayList();
			int num = 1;
			int num2 = 0;
			int num3 = -1;
			int num4 = -1;
			int arg_43_0 = 0;
			checked
			{
				int num5 = m_formats.Count - 1;
				for (int i = arg_43_0; i <= num5; i++)
				{
					MESLabel.LabelFormat labelFormat = (MESLabel.LabelFormat)m_formats[i];
					switch (labelFormat.Pos - 1)
					{
					case 0:
						num3 = i;
						break;
					case 1:
						this.GetCSVFormat(arrayList3, arrayList4, labelFormat, ref num, MESLabel.m_csvAddCounter);
						num2 = labelFormat.Fields.Count;
						if (this.m_cap == 0)
						{
							this.m_cap = labelFormat.Dats.Count;
						}
						break;
					case 2:
						num4 = i;
						break;
					}
				}
				this.m_LineRecordNumber = 1;
				ArrayList arrayList7 = new ArrayList();
				int num6 = 0;
				int num7 = 0;
				int j = 0;
				bool flag = false;
				if (arrayList3.Count > 0 & this.m_cap == 0)
				{
					throw MESLBLException.CreateInstance(MESLBLException.ErrCode.NO_DATA_ERROR);
				}
				if (this.m_cap == 0)
				{
					j = -1;
				}
				while (j < this.m_cap)
				{
					StringBuilder stringBuilder = new StringBuilder();
					StringBuilder stringBuilder2 = new StringBuilder();
					arrayList.Clear();
					arrayList2.Clear();
					if (num3 >= 0 && m_formats.Count > num3)
					{
						ArrayList arg_162_1 = arrayList;
						ArrayList arg_162_2 = arrayList2;
						MESLabel.LabelFormat arg_162_3 = (MESLabel.LabelFormat)m_formats[num3];
						int num8 = 0;
						this.GetCSVFormat(arg_162_1, arg_162_2, arg_162_3, ref num8, false);
					}
					int arg_173_0 = 0;
					int num9 = arrayList.Count - 1;
					for (int k = arg_173_0; k <= num9; k++)
					{
						stringBuilder.Append(Conversions.ToString(arrayList[k]));
						stringBuilder2.Append(Conversions.ToString(arrayList2[k]));
					}
					bool flag2 = false;
					if (arrayList3.Count > 0)
					{
						int arg_1CA_0 = num7;
						int num10 = arrayList3.Count - 1;
						for (int l = arg_1CA_0; l <= num10; l++)
						{
							stringBuilder.Append(Conversions.ToString(arrayList3[l]));
							stringBuilder2.Append(Conversions.ToString(arrayList4[l]));
							num6++;
							if (l == arrayList3.Count - 1)
							{
								flag2 = true;
								if (this.m_cap > 0)
								{
									int num11;
									if (arrayList3.Count > this.m_cap * num2)
									{
										num11 = this.m_cap * num2 - (l - num7 + 1);
									}
									else
									{
										num11 = arrayList3.Count - (l - num7 + 1);
									}
									int arg_252_0 = 1;
									int num12 = num11;
									for (int m = arg_252_0; m <= num12; m++)
									{
										stringBuilder2.Append(",");
									}
								}
							}
							if (num6 == num2)
							{
								j++;
								num6 = 0;
								this.m_LineRecordNumber++;
							}
							if (j == this.m_cap)
							{
								num7 = l + 1;
								j = 0;
								break;
							}
						}
					}
					else
					{
						j++;
					}
					arrayList5.Clear();
					arrayList6.Clear();
					this.m_LineRecordNumber--;
					if (num4 >= 0 && m_formats.Count > num4)
					{
						ArrayList arg_304_1 = arrayList5;
						ArrayList arg_304_2 = arrayList6;
						MESLabel.LabelFormat arg_304_3 = (MESLabel.LabelFormat)m_formats[num4];
						int num8 = 0;
						this.GetCSVFormat(arg_304_1, arg_304_2, arg_304_3, ref num8, false);
					}
					this.m_LineRecordNumber++;
					int arg_323_0 = 0;
					int num13 = arrayList5.Count - 1;
					for (int n = arg_323_0; n <= num13; n++)
					{
						stringBuilder.Append(Conversions.ToString(arrayList5[n]));
						stringBuilder2.Append(Conversions.ToString(arrayList6[n]));
					}
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Remove(stringBuilder.Length - 1, 1);
					}
					if (stringBuilder2.Length > 0)
					{
						stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
					}
					if (!flag)
					{
						arrayList7.Add(stringBuilder);
						flag = true;
					}
					arrayList7.Add(stringBuilder2);
					if (flag2)
					{
						break;
					}
				}
				this.GenerateLabel(null, arrayList7, renameFile);
			}
		}

		protected virtual string GenerateUniqueFileName(string fileName)
		{
			char[] separator = new char[]
			{
				'.'
			};
			string[] array = fileName.Split(separator);
			string str = Conversions.ToString(DateTime.Now.Ticks);
			checked
			{
				string text="";
				if (array.Length > 0)
				{
					array[0] = array[0] + MESLabel.m_stationName + str;
					int arg_50_0 = 0;
					int num = array.Length - 1;
					for (int i = arg_50_0; i <= num; i++)
					{
						if (i > 0)
						{
							text += ".";
						}
						text += array[i];
					}
				}
				else
				{
					text = fileName + MESLabel.m_stationName + str;
				}
				return text;
			}
		}

		protected virtual void LMCSVPrint()
		{
			this.LWCSVPrint(false);
			try
			{
				Process.Start(this.CMD);
			}
			catch (Exception expr_15)
			{
				ProjectData.SetProjectError(expr_15);
				Exception ex = expr_15;
				throw MESLBLException.CreateInstance(MESLBLException.ErrCode.BATCH_OP_ERROR, ex.Message);
			}
		}

		protected virtual void GetCSVFormat(ArrayList names, ArrayList values, MESLabel.LabelFormat format, ref int counter, bool addCounter = false)
		{
			checked
			{
				if (format.Pos - 1 == 1)
				{
					this.m_TotalLineRecord = format.Dats.Count;
				}
				try
				{
					IEnumerator enumerator = format.Dats.GetEnumerator();
					while (enumerator.MoveNext())
					{
						MESDBFTable table = (MESDBFTable)enumerator.Current;
						try
						{
							IEnumerator enumerator2 = format.Fields.GetEnumerator();
							while (enumerator2.MoveNext())
							{
								MESLabel.LabelField labelField = (MESLabel.LabelField)enumerator2.Current;
                                labelField.table = table;


                                if (addCounter)
								{
									names.Add(Operators.AddObject(Operators.AddObject(Operators.AddObject(Operators.AddObject("\"", labelField.Name), Conversions.ToString(counter)), "\""), ","));
								}
								else
								{
									names.Add(Operators.AddObject(Operators.AddObject(Operators.AddObject("\"", labelField.Name), "\""), ","));
								}
								values.Add(Operators.AddObject(Operators.AddObject(Operators.AddObject("\"", labelField.Value), "\""), ","));
								int num = format.Pos - 1;
								if (num == 1)
								{
									int num2=0;
									num2++;
									if (num2 % format.Fields.Count == 0)
									{
										this.m_LineRecordNumber++;
									}
								}
							}
						}
						finally
						{
							//IEnumerator enumerator2;
							//if (enumerator2 is IDisposable)
							//{
							//	(enumerator2 as IDisposable).Dispose();
							//}
						}
						if (addCounter)
						{
							counter++;
						}
					}
				}
				finally
				{
					//IEnumerator enumerator;
					//if (enumerator is IDisposable)
					//{
					//	(enumerator as IDisposable).Dispose();
					//}
				}
			}
		}

		protected virtual void GenerateLabel(StringBuilder sb = null, ArrayList content = null, bool renameFile = false)
		{
            FileStream fileStream=null;
            StreamWriter streamWriter=null;

            try
			{
                if (!this.IsIPAddress(this.TFP))
				{
					if (renameFile)
					{
						this.TFP = this.GenerateUniqueFileName(this.m_tfp);
					}
					 fileStream = File.Open(this.TFP, FileMode.Create);
					 streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
					if (sb.ToString()  != "" & sb != null)
					{
						streamWriter.Write(sb.ToString());
                        streamWriter.Flush();

                    }
					else
					{
						try
						{
							IEnumerator enumerator = content.GetEnumerator();
							while (enumerator.MoveNext())
							{
								StringBuilder stringBuilder = (StringBuilder)enumerator.Current;
								streamWriter.WriteLine(stringBuilder.ToString());
                                streamWriter.Flush();
                            }
						}
						finally
						{
                            //IEnumerator enumerator;
                            //if (enumerator is IDisposable)
                            //{
                            //	(enumerator as IDisposable).Dispose();
                            //}
                        }
					}
                }
				else
				{
					LPSClient lPSClient = new LPSClient();
					string text="";
					if (sb.ToString()  != "" & sb != null)
					{
						text = sb.ToString() + "\r\n";
					}
					else
					{
						try
						{
							IEnumerator enumerator2 = content.GetEnumerator();
							while (enumerator2.MoveNext())
							{
								StringBuilder stringBuilder2 = (StringBuilder)enumerator2.Current;
								text = text + stringBuilder2.ToString() + "\r\n";
							}                           
                        }
						finally
						{
							//IEnumerator enumerator2;
							//if (enumerator2 is IDisposable)
							//{
							//	(enumerator2 as IDisposable).Dispose();
							//}
						}
					}
					int num = lPSClient.PrintJob(this.TFP, Conversions.ToString(DateAndTime.Now.Ticks), text, MESLabel.PrinterName);
					if (num > 0)
					{
						throw new Exception(lPSClient.JobStatusText);
					}
				}
			}
			catch (Exception expr_167)
			{
				ProjectData.SetProjectError(expr_167);
				Exception ex = expr_167;
				throw MESLBLException.CreateInstance(MESLBLException.ErrCode.FILE_OP_ERROR, ex.Message);
			}
			finally
			{
                //fileStream.Close();
                streamWriter.Close();
            }
		}

		private bool IsIPAddress(string sString)
		{
			checked
			{
				bool result;
				try
				{
					string[] array = sString.Split(new char[]
					{
						'.'
					});
					if (array.Length != 4)
					{
						result = false;
					}
					else
					{
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string expression = array2[i];
							if (!Versioned.IsNumeric(expression))
							{
								result = false;
								return result;
							}
						}
						result = true;
					}
				}
				catch (Exception expr_4D)
				{
					ProjectData.SetProjectError(expr_4D);
					result = false;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}

        public static bool PrintLabel(
                    string xml,

                    PartSelectSVCClient PartSelectSVC,
                    LabelManager2.Application v_LabSN = null,
                    DataTable DT_Print = null,
                    Panel BarPanel = null,
                    string v_LabelName="",

                    bool csvAddCounter = false, 
                    bool moreCaret = false, 
                    string printerName = "", 
                    string stationName = "", 
                    bool useCustomLabelDLL = false, 
                    string customLabelDLL = ""                    
                    )
		{
            LabSN = v_LabSN;
            Panel_Bar = BarPanel;
            S_LabelName = v_LabelName;


            MESLabel.m_meslabel = null;
			MESLabel.m_csvAddCounter = csvAddCounter;
			MESLabel.m_moreCaret = moreCaret;
			MESLabel.m_stationName = stationName;
			MESLabel.m_printerName = printerName;

            if (xml == "")
            {                
                DataTable DTxml = PartSelectSVC.GetLBLGenLabel(DT_Print.Rows[0]["SN"].ToString(), S_LabelName).Tables[0];
                xml = MESLabel.DTtoXML(DTxml);
            }

            MESLabel.m_meslabel = MESLabel.ParseXML(xml, useCustomLabelDLL, customLabelDLL);
            MESLabel.m_meslabel.Print(DT_Print, PartSelectSVC);
			return true;
		}

        private static MESLabel ParseXML(string xml, bool useCustomLabelDLL = false, string customLabelDLL = "")
		{
			XmlDocument xmlDocument = new XmlDocument();
			//checked
			{
				MESLabel v_Label;
				try
				{
					xmlDocument.LoadXml(xml);
					XmlNode xmlNode = xmlDocument.SelectSingleNode("//LBL");
					string value = xmlNode.Attributes["NAM"].Value;
					MESLabel.LabelType labelType = (MESLabel.LabelType)(Conversions.ToInteger(xmlNode.Attributes["TYP"].Value) - 1);
					MESLabel.PrinterType printerType = (MESLabel.PrinterType)(Conversions.ToInteger(xmlNode.Attributes["OUT"].Value) - 1);
					if (!useCustomLabelDLL)
					{
						v_Label = new MESLabel(value, labelType, printerType);
					}
					else
					{
						object[] args = new object[]
						{
							value,
							labelType,
							printerType
						};
						Assembly assembly = Assembly.LoadFrom(customLabelDLL + ".dll");
						v_Label = (MESLabel)assembly.CreateInstance(customLabelDLL + "." + customLabelDLL, true, BindingFlags.Default, null, args, null, null);
					}
					v_Label.TFP = MESLabel.GetElement(xmlDocument, "//LBL/TFP");
					v_Label.CMD = MESLabel.GetElement(xmlDocument, "//LBL/CMD");
					v_Label.SFP = MESLabel.GetElement(xmlDocument, "//LBL/SFP");
					string element = MESLabel.GetElement(xmlDocument, "//LBL/CAP");
					if (Operators.CompareString(element, "", false) == 0)
					{
						v_Label.CAP = 0;
					}
					else
					{
						v_Label.CAP = Conversions.ToInteger(element);
					}
					XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//LBL/FMT");
					try
					{						
                        for (int i = 0; i < xmlNodeList.Count; i++)
                        {
                            XmlNode xnd = (XmlNode)xmlNodeList[i];
                            MESLabel.LabelFormat format = MESLabel.GetFormat(xnd);
                            v_Label.AddFormat(format);
                        }

                        //IEnumerator enumerator = xmlNodeList.GetEnumerator();
                        //while (enumerator.MoveNext())
						//{
						//	XmlNode xnd = (XmlNode)enumerator.Current;
						//	MESLabel.LabelFormat format = MESLabel.GetFormat(xnd);
						//	v_Label.AddFormat(format);
						//}
					}
					finally
					{
					}
				}
				catch (Exception expr_194)
				{
					ProjectData.SetProjectError(expr_194);
					Exception ex = expr_194;
					throw MESLBLException.CreateInstance(MESLBLException.ErrCode.INVALID_XML_FORMAT, ex.Message);
				}
				return v_Label;
			}
		}

		private static string GetElement(XmlDocument dom, string path)
		{
			XmlNode xmlNode = dom.SelectSingleNode(path);
			if (xmlNode == null)
			{
				return "";
			}
			return xmlNode.InnerText;
		}

		private static MESLabel.LabelFormat GetFormat(XmlNode xnd)
		{
			string value = xnd.Attributes["NAM"].Value;
			int pos = Conversions.ToInteger(xnd.Attributes["POS"].Value);
			MESLabel.LabelFormat labelFormat = new MESLabel.LabelFormat(value, pos);
			XmlNodeList xmlNodeList = xnd.SelectNodes("descendant::FLD");
			try
			{
				IEnumerator enumerator = xmlNodeList.GetEnumerator();
				while (enumerator.MoveNext())
				{
					XmlNode xnd2 = (XmlNode)enumerator.Current;
					MESLabel.LabelField field = MESLabel.GetField(xnd2);

                    //FFDBFTable v_table = new FFDBFTable();
                    //v_table.AddDBField()

                    //field.table = v_table;

                    labelFormat.AddField(field);
				}
			}
			finally
			{
			}
			XmlNodeList xmlNodeList2 = xnd.SelectNodes("descendant::DAT");
			try
			{
				IEnumerator enumerator2 = xmlNodeList2.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					XmlNode xnd3 = (XmlNode)enumerator2.Current;
					MESDBFTable dBTable = MESLabel.GetDBTable(xnd3);


					labelFormat.AddData(dBTable);
				}
			}
			finally
			{
			}
			return labelFormat;
		}

		private static MESLabel.LabelField GetField(XmlNode xnd)
		{
			XmlAttribute xmlAttribute = xnd.Attributes["NAM"];
			string innerText = xnd.InnerText;
			return new MESLabel.LabelField(xmlAttribute.InnerText, innerText);
		}

		private static MESDBFTable GetDBTable(XmlNode xnd)
		{
			MESDBFTable mesDBFTable = new MESDBFTable();
			XmlNodeList xmlNodeList = xnd.SelectNodes("descendant::ELM");
			try
			{
				IEnumerator enumerator = xmlNodeList.GetEnumerator();
				while (enumerator.MoveNext())
				{
					XmlNode xmlNode = (XmlNode)enumerator.Current;
					string value = xmlNode.Attributes["ALS"].Value;
					string innerText = xmlNode.InnerText;
					mesDBFTable.AddDBField(value, innerText);
				}
			}
			finally
			{
			}
			return mesDBFTable;
		}

    }
}
