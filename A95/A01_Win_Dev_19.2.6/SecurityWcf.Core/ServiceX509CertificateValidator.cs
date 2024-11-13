using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ServiceModel.Security;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;

namespace SecurityWcf.Core {
    public class ServiceX509CertificateValidator : X509CertificateValidator {
        public override void Validate(X509Certificate2 certificate) {
            string path = HostingEnvironment.MapPath(WebConfig.ClientCertificate);
            if (!File.Exists(path)) {
                throw new FileNotFoundException(Path.GetFileName(path));
            }
            X509Certificate2 clientCertificate = new X509Certificate2(path);
            //This is the Client  Certificate Thumbprint,In Production,We can validate the Certificate With CA
            if (!certificate.Thumbprint.Equals(clientCertificate.Thumbprint, StringComparison.CurrentCultureIgnoreCase)) {
                throw new SecurityTokenException("Unknown Certificate");
            }
        }
    }
}
