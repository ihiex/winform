using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace App.MyMES
{
    public class WebP
    {


        public static NetTcpBinding GetNetTcpBinding(bool TransactionFlow)
        {
            NetTcpBinding tcp = new NetTcpBinding();
            tcp.TransactionFlow = TransactionFlow;
            tcp.TransactionProtocol = System.ServiceModel.TransactionProtocol.WSAtomicTransactionOctober2004;
            tcp.CloseTimeout = new TimeSpan(0, 1, 0);
            tcp.OpenTimeout = new TimeSpan(0, 1, 0);
            tcp.ReceiveTimeout = new TimeSpan(0, 20, 0);
            tcp.SendTimeout = new TimeSpan(0, 1, 0);
            tcp.TransferMode = TransferMode.Buffered;
            tcp.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            tcp.MaxBufferPoolSize = int.MaxValue;
            tcp.MaxBufferSize = int.MaxValue;
            tcp.MaxReceivedMessageSize = int.MaxValue;
            tcp.ReaderQuotas.MaxArrayLength = int.MaxValue;
            tcp.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            tcp.ReaderQuotas.MaxDepth = 32;
            tcp.ReaderQuotas.MaxNameTableCharCount = 16384;
            tcp.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            tcp.MaxConnections = 10; //最大连接数
            tcp.ListenBacklog = 10;  //最大挂起
            
            return tcp;
        }

        //public void aa<T_Client>()
        //{
        //    //定义一个终结点与地址:服务名需要根据发布的hex确定
        //    EndpointAddress endadd = new EndpointAddress("net.tcp://服务器址址:端口/服务名");

        //    //回调对象根据实际,有些没有
        //    T_Client cilent = new TaskService.TaskServiceClient(回调对象, GetNetTcpBinding(true), endadd);

        //    //若需要身份验证
        //    NetworkCredential credential = cilent.ChannelFactory.Credentials.Windows.ClientCredential;
        //    credential.UserName = "用户名";
        //    credential.Password = "密码";

        //    cilent.方法()
        //}

    }
}
