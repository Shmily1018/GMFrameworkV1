using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;

namespace GoldMantis.Log
{
    public class LogHelper
    {
        private static readonly log4net.ILog loginDB = log4net.LogManager.GetLogger("fileAppender");

        public string ConnectionString
        {
            get { return GetLog4NetConnection(); }
        }

        static LogHelper()
        {
            //从Web.config log4Net中读取配置节
            log4net.Config.XmlConfigurator.Configure();

            //从其他文件中读取配置节
            //SetConfig(new System.IO.FileInfo(string.Format("{0}/Config/Log4Net.config", AppDomain.CurrentDomain.BaseDirectory)));
        }

        private static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        public string GetLog4NetConnection()
        {
            Hierarchy log4NetHierarchy = loginDB.Logger.Repository as Hierarchy;
            AdoNetAppender appender = log4NetHierarchy.GetAppenders().First(x => x.Name == "ADONetAppender") as AdoNetAppender;
            if (appender != null && !string.IsNullOrEmpty(appender.ConnectionString))
            {
                return appender.ConnectionString;
            }
            else
            {
                throw new Exception("log4net不是ADONetAppender或者没有连接信息");
            }
        }


        public static void WriteLog(string acount, string userName, string projectName, string action, string message, params object[] list)
        {
            try
            {
                ThreadContext.Properties["UserName"] = string.Format("{0}({1})", userName, acount);

                if (ThreadContext.Properties["UserName"].ToString() == "()")
                    ThreadContext.Properties["UserName"] = String.Empty;

                ThreadContext.Properties["OperateTime"] = DateTime.Now;
                ThreadContext.Properties["UserIP"] = HttpContext.Current.Request.UserHostAddress;
                ThreadContext.Properties["PCName"] = Environment.MachineName;
                ThreadContext.Properties["ProjectName"] = projectName;
                ThreadContext.Properties["Operation"] = action;
                ThreadContext.Properties["UserId"] = acount;
                //ThreadContext.Properties["Memo"] = Serialize(list);
                loginDB.Info(message);
            }
            catch
            {
                // ignored
            }
        }


    }
}
