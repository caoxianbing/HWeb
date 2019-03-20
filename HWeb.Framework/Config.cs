using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Framework
{
    public static class Config
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        public static string ApiUrl = ConfigurationManager.AppSettings["APIUrl"];
        /// <summary>
        /// 对应接口的参数的appid
        /// </summary>
        public static string APPId = ConfigurationManager.AppSettings["APPId"];

    }
}
