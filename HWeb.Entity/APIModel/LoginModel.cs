using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.APIModel
{
    /// <summary>
    /// 登陆接口返回
    /// </summary>
   public class LoginModel:BaseModel
    { /// <summary>
      /// 登录类型
      /// 0. 表示用户登录 <br/>
      /// 1. 表示设备登录 <br />
      /// </summary>
        public int LoginType { get; set; }
        /// <summary>
        /// 本次访问Token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 相应实体，用户登录时返回用户信息，设备登录时返回设备信息
        /// </summary>
        public UserProfile Item { get; set; }
    }

    /// <summary>
    /// 接口返回的用户信息表
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// 用户 id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// 步数目标
        /// </summary>
        public int Steps { get; set; }
        /// <summary>
        /// 目标里程
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// 目标时长
        /// </summary>
        public int SportTime { get; set; }
        /// <summary>
        /// 目标卡路里
        /// </summary>
        public int Calorie { get; set; }
    }
}
