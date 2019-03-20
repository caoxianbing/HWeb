using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.APIModel
{
    public class RegisterModel : BaseModel
    {

        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 注册用户的信息
        /// </summary>
        public UserLoginInfo User { get; set; }
        /// <summary>
        /// 第三方登录信息
        /// </summary>
        public dynamic ThirdParty { get; set; }
    }

    public class UserLoginInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 时区
        /// </summary>
        public string Timezone { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 设备总数
        /// </summary>
        public int DeviceCount { get; set; }
        /// <summary>
        /// 1:用户 2：代理商
        /// </summary>
        public int UserType { get; set; }
      
        /// <summary>
        /// 电话号码
        /// </summary>
        public string CellPhone { get; set; }
    }
}
