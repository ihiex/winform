using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MyMES
{
    public class LogNet
    {
        static string OpenLogFile;

        static LogNet()
        {
            if (string.IsNullOrEmpty(OpenLogFile))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                OpenLogFile = config.AppSettings.Settings["OpenLogFile"].Value.Trim();
            }
        }
        public static void WriteLog(Type t, Exception ex)
        {
            if (!string.IsNullOrEmpty(OpenLogFile) && OpenLogFile == "Y")
            {
                log4net.ILog log = log4net.LogManager.GetLogger(t);
                log.Error("Error", ex);
            }
        }

        public static void LogEor(string name, string msg)
        {
            if (!string.IsNullOrEmpty(OpenLogFile) && OpenLogFile == "Y")
            {
                log4net.ILog log = log4net.LogManager.GetLogger(name);
                log.Error(msg);
            }
        }

        public static void LogDug(string name, string msg)
        {
            if (!string.IsNullOrEmpty(OpenLogFile) && OpenLogFile == "Y")
            {
                log4net.ILog log = log4net.LogManager.GetLogger(name);
                log.Debug(msg);
            }
        }
    }
}
