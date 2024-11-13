using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Hosting;

namespace SecurityWcf.Core {
    public static class WebConfig {
        public static string ServiceCertificate {
            get {
                return ConfigurationManager.AppSettings["ServiceCertificate"];
            }
        }

        public static string ServiceCertificatePassword {
            get {
                return ConfigurationManager.AppSettings["ServiceCertificatePassword"];
            }
        }

        public static string ServiceUserName {
            get {
                return ConfigurationManager.AppSettings["ServiceUserName"];
            }
        }

        public static string ServicePassword {
            get {
                return ConfigurationManager.AppSettings["ServicePassword"];
            }
        }

        public static string ClientCertificate {
            get {
                return ConfigurationManager.AppSettings["ClientCertificate"];
            }
        }

        public static string ServiceUri {
            get {
                return ConfigurationManager.AppSettings["ServiceUri"];
            }
        }

        public static bool EnableServiceCertificate {
            get {
                bool result;
                if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableServiceCertificate"], out result)) {
                    result = false;
                }
                return result;
            }
        }
    }
}
