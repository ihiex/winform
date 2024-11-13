using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Seagull.BarTender.Print;
using Seagull.BarTender.Print.Database;
using Seagull.BarTender.PrintServer;
using Seagull.BarTender.PrintServer.Tasks;
using BarTender;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace BarPrint
{
    public partial class BarTender_Form : Form
    {
        Engine engine;
        //BarTender.Application btApp;
        //BarTender.Format btFormat; 
        MyINI INI;
        string PathLable = string.Empty;


        Thread th = null;
        Thread ths = null;
        Socket socketWatch = null;
        Dictionary<string, Socket> dict = new Dictionary<string, Socket>();//存放 客户端对应服务端套字节
        Dictionary<string, Thread> dictTh = new Dictionary<string, Thread>();//存放 客户端对应接受字节的线程
        int I_Port = 6800;


        //string F_Value;
        public BarTender_Form()
        {
            //F_Value = S_Value;            
            InitializeComponent();
        }

        private void Print()
        {
            try
            {
                if (INI == null)
                {
                    //INI = new MyINI(System.Windows.Forms.Application.StartupPath + "\\BarTender.ini");
                    INI = new MyINI("D:\\Labels\\BarTender.ini");
                }
                string S_TmpPath = INI.ReadValue("Path", "Path");
                I_Port = Convert.ToInt32(INI.ReadValue("Port", "Port"));

                //string S_DBPath = Path.GetDirectoryName(S_TmpPath) + "\\BarTender.txt";
                string S_DBPath = "D:\\Labels\\BarTender.txt";
                if (File.Exists(S_DBPath) == false)
                {
                    MessageBox.Show("在主程序先生成SN后再打印！", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //if (btApp == null)
                //{
                //    btApp = new BarTender.Application();
                //}
                //btFormat = btApp.Formats.Open(S_TmpPath, false, "");
                //btFormat.PrintOut(false,false ); 

                if (engine == null)
                {
                    engine = new Engine(true);
                }
                engine.Start();
                LabelFormatDocument LabFormat = engine.Documents.Open(S_TmpPath);
                LabFormat.PrintSetup.NumberOfSerializedLabels = 1;
                LabFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                LabFormat.Print();


                this.WindowState = FormWindowState.Minimized;                
                File.Delete(S_DBPath);

                Thread.Sleep(200);
                string sendMessage = "Delete";
                socketWatch.Send(Encoding.ASCII.GetBytes(sendMessage));
                
                //this.Close();
            }
            catch (Exception ex)
            {
                engine.Stop();
                engine.Dispose();
                //btApp.Quit();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }


        private void Btn_Print_Click(object sender, EventArgs e)
        {
            Print();            
        }

        private void BarTender_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                engine.Stop();
                engine.Dispose();
                //btApp.Quit();
                socketWatch.Close();
            }
            catch
            { }
        }

        private void MyListen()
        {
            try
            {
                if (INI == null)
                {
                    //INI = new MyINI(System.Windows.Forms.Application.StartupPath + "\\BarTender.ini");
                    INI = new MyINI("D:\\Labels\\BarTender.ini");
                }
                I_Port = Convert.ToInt32(INI.ReadValue("Port", "Port"));

                //监听
                //创建监听套字节 参数（选择寻址，选择传输类型，选择指定协议）
                socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //创建IP地址
                IPAddress address = IPAddress.Parse("127.0.0.1");
                //创建一个IP地址和端口号的组合
                IPEndPoint endpoint = new IPEndPoint(address, I_Port);
                //绑定 监听套字节
                socketWatch.Bind(endpoint);
                //侦听该套字节一次性最多侦听的客户端的个数
                socketWatch.Listen(1);

                th = new Thread(LianJie);
                th.IsBackground = true;//让线程在后台运行，当前台程序关闭后，后台线程也关闭了。
                th.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LianJie()
        {
            try
            {
                while (true)
                {
                    //Accept方法开始监听 绑定的IP端口，该方法会阻断线程，一旦接受到客户端的请求后，会返回一个新的专门负责与这个客户端通讯的套字节对象
                    //说简单点Accept方法是 前台接待的小MM，一旦有客户来咨询，她就把客户带的一个 套字节对象 面前，让这个套字节对象 专门与客户交流，
                    //然后小MM又得前台等待下一个客户
                    Socket socketLianJie = socketWatch.Accept();
                    //获取 服务端的套字节
                    string fuwuduan = socketLianJie.LocalEndPoint.ToString();
                    //获取 客户端的套字节
                    string benji = socketLianJie.RemoteEndPoint.ToString();
                    //存放  专门与 该客户端通讯的 服务端套字节对象
                    dict.Add(benji, socketLianJie);
                    //把用户列表拼接成字符串

                    //GengXin();//更新连接的客户端
                    ths = new Thread(JieShou);
                    ths.IsBackground = true;
                    ths.Start(socketLianJie);
                    dictTh.Add(benji, ths);//存放 接受数据线程
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void JieShou(object socketLianJies)
        {
            try
            {
                while (true)
                {
                    System.Windows.Forms.Application.DoEvents();
                    //多线程传参都是 object类型
                    Socket socketLianJie = socketLianJies as Socket;
                    //获取传过来的 与该服务端套字节通讯的客户端
                    string S_ClientIP = socketLianJie.RemoteEndPoint.ToString();
                    byte[] bt = new byte[1024 * 1024 * 2];
                    //接受客户端发送过来的 消息 byte类型

                    int length = -1;
                    //这个是对客户端进行异常处理
                    try
                    {
                        length = socketLianJie.Receive(bt);
                    }
                    catch (SocketException se)
                    {
                        //报错说明客户端已下线
                        try
                        {
                            dict.Remove(S_ClientIP);
                            dictTh.Remove(S_ClientIP);
                            //listBox1.Items.Clear(); 
   
                            //GengXin();
                            string msg = S_ClientIP + "已下线！";
                            byte[] BTMsg = System.Text.Encoding.UTF8.GetBytes(msg);
                            foreach (Socket s in dict.Values)
                            {
                                s.Send(BTMsg);
                            }
                        }
                        finally
                        {
                        }
                        break;
                    }

                    if (bt[0] == 0)//客户端向另一个客户端发送消息
                    {
                    }
                    else if (bt[0] == 4)//客户端向另一个客户端发送文件
                    {

                    }
                    else //客户端向服务端发送数据
                    {
                        string strMsg = System.Text.Encoding.UTF8.GetString(bt, 0, length);
                        byte[] btMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);

                        if (strMsg == "Print")
                        {
                            Print();
                        }
                        ////收到消息后 回复消息给客户端 
                        //try
                        //{
                        //    string S_ReClient = "FACNO:" + Msg[3] + " IP:" + S_ClientIP + " Received data";
                        //    bt = System.Text.Encoding.UTF8.GetBytes(S_ReClient);
                        //    //向指定的客户端发送消息
                        //    dict[S_ClientIP].Send(bt);
                        //}
                        //catch
                        //{
                        //}                   
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BarTender_Form_Load(object sender, EventArgs e)
        {            
            MyListen();
        }
    }
}
