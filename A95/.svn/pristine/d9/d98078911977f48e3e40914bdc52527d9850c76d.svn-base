using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BartenderPrint
{
    public class SocketClientModel
    {
        IPAddress _IP;
        IPEndPoint _ipEnd;
        public Socket socket;

        public SocketClientModel()
        { }
        public SocketClientModel(string IP, int Port)
        {
            InitSocket(IP, Port);
        }

        public bool InitSocket(string IP, int Port)
        {
            try
            {
                _IP = IPAddress.Parse(IP);
                _ipEnd = new IPEndPoint(_IP, Port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.ReceiveTimeout = 800;
                socket.SendTimeout = 800;
                if (ConnectServer())
                    return true;
                else
                    return false;

            }
            catch
            {
                return false;
            }
        }

        public bool Connected
        {
            get
            {
                if (socket == null)
                    return false;
                return socket.Connected;
            }
        }
        public bool ConnectServer()
        {
            try
            {
                socket.Connect(_ipEnd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                if (socket.Connected)
                    socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SendCommand(byte[] cmd)
        {
            try
            {
                if (socket != null && socket.Connected)
                {
                    socket.Send(cmd);
                }
            }
            catch (Exception e)
            {

                System.Threading.Thread.Sleep(1000);
            }

        }
    }
}
