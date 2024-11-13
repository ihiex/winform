using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace App.MyMES
{
    public static class ImesPackageFactory
    {
        public static mesPackageService.ImesPackageSVCClient CreateServerClient()
        {
            Public_ public_ = new Public_();
            mesPackageService.ImesPackageSVCClient client = new mesPackageService.ImesPackageSVCClient();
            string path = System.IO.Path.GetFullPath("config/COClient.pfx");
            client.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(path, public_.S_WCF_Password, X509KeyStorageFlags.MachineKeySet);
            return client;
        }
    }

}
