using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ParamModel
{
    /// <summary>
    /// 注册提交参数
    /// </summary>
    public class RegisterParam : BaseParam
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 设备IMEI
        /// </summary>
        public string IMEI { get; set; }
        
    }
}