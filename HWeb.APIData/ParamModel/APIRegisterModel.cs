using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    public class APIRegisterModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// IMEI
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { get; set; }
      

        private string _Language = "zh-cn";
        /// <summary>
        /// 语言
        /// </summary>
        public string Language
        {
            get { return _Language; }
            set
            {
                _Language = value;
            }
        }

        /// <summary>
        /// Appid
        /// </summary>
        public string AppId { get; set; }
       
    }
}
