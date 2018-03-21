using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormWebApi.CommUtil
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
       

    }
        public class LogHelper
        {
        /// <summary>  
        /// 输出日志到Log4Net  
        /// </summary>  
        /// <param name="t"></param>  
        /// <param name="ex"></param>  
        #region static void WriteLog(Type t, Exception ex,LogLevel=LogLevel.Debug)  

        public static void WriteLog(Type t, Exception ex,LogLevel l=LogLevel.Debug)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(t);
            switch (l)
            {
                case LogLevel.Debug:
                    log.Debug("Debug", ex);
                    break;
                case LogLevel.Error:
                    log.Error("Error", ex);
                    break;
                case LogLevel.Fatal:
                    log.Fatal("Fatal", ex);
                    break;
                case LogLevel.Info:
                    log.Info("Info", ex);
                    break;
                case LogLevel.Warn:
                    log.Warn("Warn", ex);
                    break;
            }
               
            }

        #endregion

        /// <summary>  
        /// 输出日志到Log4Net  
        /// </summary>  
        /// <param name="t"></param>  
        /// <param name="msg"></param>  
        #region static void WriteLog(Type t, string msg,LogLevel l=LogLevel.Debug)  

        public static void WriteLog(Type t, string msg,LogLevel l=LogLevel.Debug)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(t);
            switch (l)
            {
                case LogLevel.Debug:
                    log.Debug(msg);
                    break;
                case LogLevel.Error:
                    log.Error(msg);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(msg);
                    break;
                case LogLevel.Info:
                    log.Info(msg);
                    break;
                case LogLevel.Warn:
                    log.Warn(msg);
                    break;
            }
        }

            #endregion

        }
    }
