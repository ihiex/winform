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
    public class WcfInvokeFactory
    {
        public static T CreateServiceByUrl<T>(string url)
        {
            return CreateServiceByUrl<T>(url, "basicHttpBinding");
        }

        public static T CreateServiceByUrl<T>(string url, string bing)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) throw new NotSupportedException("URL地址错误");
                EndpointAddress address = new EndpointAddress(url);
                Binding binding = CreateBinding(bing);
                ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);
                
                return factory.CreateChannel();
            }
            catch (Exception ex)
            {
                throw new Exception("创建服务工厂出现异常. "+ex.ToString());
            }
        }

        /// <summary>
        /// 创建传输协议
        /// </summary>
        /// <param name="binding">传输协议名称</param>
        /// <returns></returns>
        private static Binding CreateBinding(string binding)
        {
            Binding bindinginstance = null;
            if (binding.ToLower() == "basichttpbinding" || binding.ToLower() == "custombinding")
            {
                var ws = new BasicHttpBinding
                {
                    MaxBufferSize = 2147483647,
                    MaxBufferPoolSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647,
                    ReaderQuotas = { MaxStringContentLength = 2147483647 },
                    CloseTimeout = new TimeSpan(0, 10, 0),
                    OpenTimeout = new TimeSpan(0, 10, 0),
                    ReceiveTimeout = new TimeSpan(0, 10, 0),
                    SendTimeout = new TimeSpan(0, 10, 0)
                };

                bindinginstance = ws;
            }
            else if (binding.ToLower() == "netnamedpipebinding")
            {
                var ws = new NetNamedPipeBinding { MaxReceivedMessageSize = 65535000 };
                bindinginstance = ws;
            }
            //else if (binding.ToLower() == "netpeertcpbinding")
            //{
            //    var ws = new NetPeerTcpBinding { MaxReceivedMessageSize = 65535000 };
            //    bindinginstance = ws;
            //}
            else if (binding.ToLower() == "nettcpbinding")
            {
                var ws = new NetTcpBinding { MaxReceivedMessageSize = 65535000, Security = { Mode = SecurityMode.None } };
                bindinginstance = ws;
            }
            else if (binding.ToLower() == "wsdualhttpbinding")
            {
                var ws = new WSDualHttpBinding { MaxReceivedMessageSize = 65535000 };

                bindinginstance = ws;
            }
            //else if (binding.ToLower() == "webhttpbinding")
            //{
            //    var ws = new WebHttpBinding { MaxReceivedMessageSize = 65535000 };
            //    bindinginstance = ws;
            //}
            else if (binding.ToLower() == "wsfederationhttpbinding")
            {
                var ws = new WSFederationHttpBinding { MaxReceivedMessageSize = 65535000 };
                bindinginstance = ws;
            }
            else if (binding.ToLower() == "wshttpbinding")
            {
                var ws = new WSHttpBinding(SecurityMode.None) { MaxReceivedMessageSize = 65535000 };
                ws.Security.Message.ClientCredentialType = System.ServiceModel.MessageCredentialType.Windows;
                ws.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                bindinginstance = ws;
            }
            return bindinginstance;

        }

    }
}
