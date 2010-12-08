using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace FbTracker.Facebook
{
    public static class CommonUtils
    {
        public static bool ConfigureLogger()
        {
            bool result = true;
            if (!LogManager.GetRepository().Configured)
            {
                //string tmpFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string tmpFile = AppDomain.CurrentDomain.BaseDirectory;
                tmpFile = Path.Combine(tmpFile, "log4net.xml");
                if (System.IO.File.Exists(tmpFile))
                {
                    XmlConfigurator.Configure(new System.IO.FileInfo(tmpFile));
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }

    public enum ClientType
    {
        Unkown = 0,
        InsideCanvas = 10,
        InsideFBL = 20,
        InsideIFrame = 30,
        Outside = 40
    }
}
