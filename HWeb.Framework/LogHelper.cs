using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Framework
{
    public static class LogHelper
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="exception"></param>
        public static void WriteLog(string message)
        {
            try
            {
                logger.Info(message);
            }
            catch (Exception ex){
                throw new Exception(ex.Message);
            };
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="exception"></param>
        public static void WriteLog(Exception exception)
        {
            try
            {
                logger.Info(exception);
            }
            catch { };
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="exception"></param>
        public static void ErrorLog(string message) {
            try {
            logger.Error(message);
        }
            catch { };
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="exception"></param>
        public static void ErrorLog(Exception exception)
        {
            try
            {
                logger.Error(exception);
            }
            catch { };
        }
    }
}
