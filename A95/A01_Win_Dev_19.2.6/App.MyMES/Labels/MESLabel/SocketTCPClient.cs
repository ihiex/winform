using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.MyMES
{
    public delegate void DelegateMsg(object msg);

    public class SocketTCPClient
    {
        public static Socket _clientSocket;

        private static string _server;

        public static string Server
        {
            get { return SocketTCPClient._server; }
            set { SocketTCPClient._server = value; }
        }
        private static int _port;

        public static int Port
        {
            get { return SocketTCPClient._port; }
            set { SocketTCPClient._port = value; }
        }

        public static void Connect()
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(_server), _port);
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _clientSocket.ReceiveTimeout = 15000;
                _clientSocket.SendTimeout = 3000;
                _clientSocket.BeginConnect(ip, new AsyncCallback(ConnectCallBack), _clientSocket);

                Thread.Sleep(100);
            }
            catch (Exception e)
            {
            }
        }

        private static void ConnectCallBack(IAsyncResult iar)
        {
            Socket client = (Socket)iar.AsyncState;
            try
            {
                client.EndConnect(iar);
            }
            catch (SocketException e)
            {
            }
        }

        public static void Send(string msg)
        {
            if (_clientSocket == null || msg == string.Empty) return;

            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(msg);
            try
            {
                _clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    int length = _clientSocket.EndSend(asyncResult);
                }, null);
            }
            catch (Exception e)
            {
            }
        }

        public static string Recive()
        {
            string msg = string.Empty;
            byte[] data = new byte[1024];
            try
            {
                _clientSocket.BeginReceive(data, 0, data.Length, SocketFlags.None,
                asyncResult =>
                {
                    try
                    {
                        if (_clientSocket != null && _clientSocket.Connected)
                        {
                            int length = _clientSocket.EndReceive(asyncResult);
                            msg = Encoding.GetEncoding("GB2312").GetString(data, 0, length);
                            Thread.Sleep(100);
                            Recive();
                        }
                    }
                    catch (SocketException e)
                    {
                    }
                }, null);
            }
            catch (Exception ex)
            {
                //OnMsgRed(ex.Message);
            }
            return msg;
        }

        public static bool Close()
        {
            try
            {
                if (_clientSocket != null)
                {
                    if (_clientSocket.Connected)
                        _clientSocket.Shutdown(SocketShutdown.Both);
                    _clientSocket.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
