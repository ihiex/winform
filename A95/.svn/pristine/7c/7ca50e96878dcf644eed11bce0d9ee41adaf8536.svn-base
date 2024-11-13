using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace BartenderPrint
{
    public class Log4net
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogError(string msg)
        {
            log.Error(msg);
        }

        public static void LogDug(string msg)
        {
            log.Debug(msg);
        }
    }
}
