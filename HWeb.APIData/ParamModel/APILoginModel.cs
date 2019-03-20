using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    /// <summary>
    /// 用户登录接口的model 
    /// </summary>
    class APILoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pass { get; set; }

        /// <summary>
        /// APP应用标识
        /// </summary>
        public string AppId { get; set; }

        private string _language = "zh-cn";
        /// <summary>
        /// 用户语方言
        /// </summary>
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }
        /// <summary>
        /// 登录类型
        /// 0. 表示用户登录
        /// </summary>
        public int LoginType { get; set; }

    }
}
