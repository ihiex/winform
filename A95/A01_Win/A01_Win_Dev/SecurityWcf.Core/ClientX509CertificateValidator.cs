using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace SecurityWcf.Core {
    public class ClientX509CertificateValidator : X509CertificateValidator {
        public override void Validate(X509Certificate2 certificate) {
            //throw new NotImplementedException();
            if (certificate == null) {
                throw new ArgumentNullException("certificate");
            }
            string path = Path.GetFullPath(WebConfig.ServiceCertificate);
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
