using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using Seagull.BarTender.Print;
using System.Windows.Forms;

namespace BartenderPrint
{
    public class SocketManager
    {
        #region 全局变量
        private int m_maxConnectNum;    //最大连接数  
        private int m_revBufferSize;    //最大接收字节数  
        BufferManager m_bufferManager;
        const int opsToAlloc = 2;
        Socket listenSocket;            //监听Socket  
        SocketEventPool m_pool;
        int m_clientCount;              //连接的客户端数量  
        Semaphore m_maxNumberAcceptedClients;
        Engine engine;

        public static List<AsyncUserToken> m_clients;       //客户端列表  
        public static AsyncUserToken BridgeUser;            //NG流出工位客户端信息(服务器模式)未使用
        public static SocketClientModel SocketClient = null;//NG流出工位客户端信息(客户端模式)
        #endregion

        #region 定义委托

        /// <summary>  
        /// 客户端连接数量变化时触发  
        /// </summary>  
        /// <param name="num">当前增加客户的个数(用户退出时为负数,增加时为正数,一般为1)</param>  
        /// <param name="token">增加用户的信息</param>  
        public delegate void OnClientNumberChange(int num, AsyncUserToken token);

        /// <summary>  
        /// 接收到客户端的数据  
        /// </summary>  
        /// <param name="token">客户端</param>  
        /// <param name="buff">客户端数据</param>  
        public delegate void OnReceiveData(AsyncUserToken token, byte[] buff);

        #endregion

        #region 定义事件
        /// <summary>  
        /// 客户端连接数量变化事件  
        /// </summary>  
        public event OnClientNumberChange ClientNumberChange;

        /// <summary>  
        /// 接收到客户端的数据事件  
        /// </summary>  
        public event OnReceiveData ReceiveClientData;


        #endregion

        #region 定义属性

        /// <summary>  
        /// 获取客户端列表  
        /// </summary>  
        public List<AsyncUserToken> ClientList { get { return m_clients; } }

        #endregion

        #region 构造函数
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="numConnections">最大连接数</param>  
        /// <param name="receiveBufferSize">缓存区大小</param>  
        public SocketManager()
        {
            int numConnections = 3;
            int receiveBufferSize = 10240;
            m_clientCount = 0;
            m_maxConnectNum = numConnections;
            m_revBufferSize = receiveBufferSize;
            m_bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToAlloc, receiveBufferSize);

            m_pool = new SocketEventPool(numConnections);
            m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
        }
        #endregion

        #region 服务器启动/停止
        /// <summary>  
        /// 初始化  
        /// </summary>  
        public void Init()
        {
            m_bufferManager.InitBuffer();
            m_clients = new List<AsyncUserToken>();
            SocketAsyncEventArgs readWriteEventArg;

            for (int i = 0; i < m_maxConnectNum; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.UserToken = new AsyncUserToken();

                m_bufferManager.SetBuffer(readWriteEventArg);
                m_pool.Push(readWriteEventArg);
            }
        }

        /// <summary>  
        /// 启动服务  
        /// </summary>  
        /// <param name="localEndPoint"></param>  
        public bool StartSocket()
        {
            try
            {
                Init();
                int port = int.Parse(ConfigurationManager.AppSettings["port"]);
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
                m_clients.Clear();
                listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listenSocket.Bind(localEndPoint);
                listenSocket.Listen(m_maxConnectNum);
                StartAccept(null);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>  
        /// 停止服务  
        /// </summary>  
        public void StopSocket()
        {
            foreach (AsyncUserToken token in m_clients)
            {
                try
                {
                    token.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception) { }
            }
            try
            {
                listenSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }

            listenSocket.Close();
            int c_count = m_clients.Count;
            lock (m_clients) { m_clients.Clear(); }

            if (ClientNumberChange != null)
                ClientNumberChange(-c_count, null);
        }

        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            try
            {
                if (acceptEventArg == null)
                {
                    acceptEventArg = new SocketAsyncEventArgs();
                    acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
                }
                else
                {
                    // socket must be cleared since the context object is being reused  
                    acceptEventArg.AcceptSocket = null;
                }

                m_maxNumberAcceptedClients.WaitOne();
                if (!listenSocket.AcceptAsync(acceptEventArg))
                {
                    ProcessAccept(acceptEventArg);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region 触发收发/连接断开
        /// <summary>
        /// 数据收发、连接断开触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {

            // determine which type of operation just completed and call the associated handler  
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    break;
            }

        }
        #endregion

        #region 数据接收
        /// <summary>
        /// 接收客户端发送过来的数据
        /// </summary>
        /// <param name="e"></param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            // check if the remote host closed the connection  
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                //读取数据  
                byte[] data = new byte[e.BytesTransferred];
                Array.Copy(e.Buffer, e.Offset, data, 0, e.BytesTransferred);
                try
                {
                    lock (token.Buffer)
                    {
                        token.Buffer.AddRange(data);
                    }
                    do
                    {
                        //判断包的长度  
                        byte[] lenBytes = token.Buffer.GetRange(0, 4).ToArray();
                        string lengthStr = Encoding.Default.GetString(lenBytes);
                        int leng = 0;
                        if (int.TryParse(lengthStr, out leng) == false)
                        {
                            token.Buffer.RemoveRange(0, data.Length);
                            SendMessage(token, Encoding.Default.GetBytes("ERRO"));
                        }
                        else
                        {
                            int packageLen = leng;
                            //BitConverter.ToInt32(lenBytes, 0);
                            if (packageLen > token.Buffer.Count - 4)
                            {   //长度不够时,退出循环,让程序继续接收  
                                break;
                            }

                            //包够长时,则提取出来,交给后面的程序去处理  
                            byte[] rev = token.Buffer.GetRange(4, packageLen).ToArray();
                            //从数据池中移除这组数据  
                            lock (token.Buffer)
                            {
                                token.Buffer.RemoveRange(0, packageLen + 4);
                            }
                            //将数据包交给后台处理,这里你也可以新开个线程来处理.加快速度.  
                            if (ReceiveClientData != null)
                                ReceiveClientData(token, rev);

                            string Tdata = Encoding.Default.GetString(rev);

                            string sendMsg = string.Empty;
                            string command = Tdata.Substring(0, 5);
                            if (command == "Print")
                            {
                                string pathLabel = Tdata.Substring(5, Tdata.Length - 5);
                                Print(token, pathLabel);
                            }
                        }
                    } while (token.Buffer.Count > 4);


                    //do
                    //{

                    //byte[] lenBytes = token.Buffer.GetRange(0, token.Buffer.Count).ToArray();
                    //string Tdata = Encoding.Default.GetString(lenBytes);

                    //string sendMsg = string.Empty;
                    //string command = Tdata.Substring(0, 5);
                    //if (command == "Print")
                    //{
                    //    string pathLabel = Tdata.Substring(5, Tdata.Length - 5);
                    //    Print(token, pathLabel);
                    //}
                    //} while (token.Buffer.Count > 5);
                    if (!token.Socket.ReceiveAsync(e))
                        this.ProcessReceive(e);
                }
                catch (Exception ex)
                {
                    SendMessage(token, Encoding.Default.GetBytes("ERRO"));
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }
        #endregion

        #region 数据发送
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                bool willRaiseEvent = token.Socket.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        /// <summary>  
        /// 对数据进行打包,然后再发送(异步发送)
        /// </summary>  
        /// <param name="token"></param>  
        /// <param name="message"></param>  
        /// <returns></returns>  
        public void SendMessage(AsyncUserToken token, byte[] message)
        {
            if (token == null || token.Socket == null || !token.Socket.Connected)
                return;
            try
            {
                //对要发送的消息,制定简单协议,头4字节指定包的大小,方便客户端接收(协议可以自己定)  
                byte[] buff = new byte[message.Length + 4];
                byte[] len = BitConverter.GetBytes(message.Length);
                Array.Copy(len, buff, 4);
                Array.Copy(message, 0, buff, 4, message.Length);
                //token.Socket.Send(buff);  //这句也可以发送, 可根据自己的需要来选择  
                //新建异步发送对象, 发送消息  
                SocketAsyncEventArgs sendArg = new SocketAsyncEventArgs();
                sendArg.UserToken = token;
                //sendArg.SetBuffer(buff, 0, buff.Length);  //将数据放置进去.  
                sendArg.SetBuffer(message, 0, message.Length);
                token.Socket.SendAsync(sendArg);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region 客户端连接
        /// <summary>
        /// 客户端连接触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                Interlocked.Increment(ref m_clientCount);
                SocketAsyncEventArgs readEventArgs = m_pool.Pop();
                AsyncUserToken userToken = (AsyncUserToken)readEventArgs.UserToken;
                userToken.Socket = e.AcceptSocket;
                userToken.ConnectTime = DateTime.Now;
                userToken.Remote = e.AcceptSocket.RemoteEndPoint;
                userToken.IPAddress = ((IPEndPoint)(e.AcceptSocket.RemoteEndPoint)).Address;
                userToken.Port = ((IPEndPoint)(e.AcceptSocket.RemoteEndPoint)).Port;

                lock (m_clients) { m_clients.Add(userToken); }

                if (ClientNumberChange != null)
                    ClientNumberChange(1, userToken);

                if (!e.AcceptSocket.ReceiveAsync(readEventArgs))
                {
                    ProcessReceive(readEventArgs);
                }
            }
            catch (Exception ex)
            {
            }
            if (e.SocketError == SocketError.OperationAborted) return;
            StartAccept(e);
        }

        /// <summary>
        /// 关闭客户端连接
        /// </summary>
        /// <param name="token"></param>
        public void CloseClient(AsyncUserToken token)
        {
            try
            {
                token.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex) { }
        }
        #endregion

        #region 客户端关闭
        /// <summary>
        /// 关闭客户端
        /// </summary>
        /// <param name="e"></param>
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            try
            {
                AsyncUserToken token = e.UserToken as AsyncUserToken;

                lock (m_clients) { m_clients.Remove(token); }
                //如果有事件,则调用事件,发送客户端数量变化通知  
                if (ClientNumberChange != null)
                    ClientNumberChange(-1, token);
                try
                {
                    token.Socket.Shutdown(SocketShutdown.Send);
                }
                catch (Exception) { }
                token.Socket.Close();
                Interlocked.Decrement(ref m_clientCount);
                m_maxNumberAcceptedClients.Release();
                e.UserToken = new AsyncUserToken();
                m_pool.Push(e);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion


        private void Print(AsyncUserToken token, string path)
        {
            try
            {
                if (engine == null)
                {
                    engine = new Engine(true);
                }
                engine.Start();
                string S_TmpPath = path;
                LabelFormatDocument LabFormat = engine.Documents.Open(S_TmpPath);
                LabFormat.PrintSetup.NumberOfSerializedLabels = 1;
                LabFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                LabFormat.Print();
               
                string sendMessage = "Delete";
                SendMessage(token, Encoding.Default.GetBytes(sendMessage));
            }
            catch (Exception ex)
            {
                engine.Stop();
                engine.Dispose();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
