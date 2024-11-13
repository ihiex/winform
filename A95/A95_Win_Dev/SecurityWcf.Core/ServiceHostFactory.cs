using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Security.Cryptography.X509Certificates;

namespace SecurityWcf.Core {
    public class WcfServiceHostFactory : ServiceHostFactory {
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses) {
            ServiceHostBase host;
            Uri baseUri;
            if (!String.IsNullOrEmpty(WebConfig.ServiceUri) &&
                Uri.TryCreate(WebConfig.ServiceUri, UriKind.RelativeOrAbsolute, out baseUri)) {
                host = base.CreateServiceHost(constructorString, new Uri[] { baseUri });
            } else {
                host = base.CreateServiceHost(constructorString, baseAddresses);
            }
            if (WebConfig.EnableServiceCertificate) {
                string path = System.Web.Hosting.HostingEnvironment.MapPath(WebConfig.ServiceCertificate);
                if (!File.Exists(path)) {
                    throw new FileNotFoundException(WebConfig.ServiceCertificate);
                }
                host.Credentials.ServiceCertificate.Certificate =
                    new X509Certificate2(path, WebConfig.ServiceCertificatePassword, X509KeyStorageFlags.MachineKeySet);
            }
            return host;
        }
    }
}
