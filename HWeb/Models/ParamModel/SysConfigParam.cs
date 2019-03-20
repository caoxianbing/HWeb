using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ParamModel
{
    /// <summary>
    /// 站点配置参数
    /// </summary>
    public class SysConfigParam : BaseParam
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string SysName { get; set; }

        /// <summary>
        /// js版本
        /// </summary>
        public string JsVersion { get; set; }

        /// <summary>
        /// 备案号
        /// </summary>
        public string ICP { get; set; }
    }
}