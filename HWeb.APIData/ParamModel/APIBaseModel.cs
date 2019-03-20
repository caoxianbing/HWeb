using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    public class APIBaseModel
    {
        /// <summary>
        /// 登录后获取到的 Token
        /// </summary>
        public string Token { get { return UserData.Token; } }


        private string _language = "zh-cn";
        /// <summary>
        /// 语言选项 默认 zh-cn
        /// </summary>
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        /// <summary>
        /// APP应用标识
        /// </summary>
        public string AppId { get; set; }
    }
}
