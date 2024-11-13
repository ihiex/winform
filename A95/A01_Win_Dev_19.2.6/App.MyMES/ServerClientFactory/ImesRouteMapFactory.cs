using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace App.MyMES
{
    public static class  ImesRouteMapFactory
    {       
        public static mesRouteMapService.ImesRouteMapSVCClient  CreateServerClient()
        {
            Public_ public_ = new Public_();
            mesRouteMapService.ImesRouteMapSVCClient client = new mesRouteMapService.ImesRouteMapSVCClient();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string S_WebTimeout = "";
            int I_WebTimeout = 2;
            try
            {
                S_WebTimeout = config.AppSettings.Settings["WebTimeout"].Value.Trim();
                if (S_WebTimeout != "")
                {
                    I_WebTimeout = Convert.ToInt32(S_WebTimeout);
                }
            }
            catch
            {
                I_WebTimeout = 2;
            }
            TimeSpan TS = new TimeSpan(0, I_WebTimeout, 0);
            client.Endpoint.Binding.CloseTimeout = TS;
            client.Endpoint.Binding.OpenTimeout = TS;
            client.Endpoint.Binding.ReceiveTimeout = TS;
            client.Endpoint.Binding.SendTimeout = TS;

            string path = System.IO.Path.GetFullPath("config/COClient.pfx");
            client.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(path, public_.S_WCF_Password, X509KeyStorageFlags.MachineKeySet);            
            return client;
        }
    }
}