using HWeb.Entity.WebEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb
{
    /// <summary>
    /// 常量类
    /// </summary>
    public class ConstVal
    {
        /// <summary>
        /// 用户session的key
        /// </summary>
        public const string SessionUserStr = "user";
        /// <summary>
        /// ip
        /// </summary>
        public const string SessionIpStr = "Ip";


        /// <summary>
        /// 网站基本配置cache key
        /// </summary>
        public const string SysConfigCacheStr = "sysconfig";

        /// <summary>
        /// 用户菜单缓存时间
        /// </summary>
        public const int MenuCacheTime = 10 * 60 * 1000;

        /// <summary>
        /// 用户类型的菜单缓存key
        /// </summary>
        public const string UTMenuCacheStr = "usermenu";

        /// <summary>
        /// 默认时间
        /// </summary>
        public static DateTime DefaultDateTime = new DateTime(1900, 1, 1);

        /// <summary>
        /// 默认时间格式
        /// </summary>
        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 默认头像照片id
        /// </summary>

        public const int DefaultHeadImgId = 1;

        /// <summary>
        /// 系统配置
        /// </summary>
        public static SysConfig WebConfig = null;

    }
}