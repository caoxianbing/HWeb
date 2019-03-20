using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ParamModel
{
    /// <summary>
    /// 通用layui 分页参数类
    /// </summary>
    public class LayUIPageParam : BaseParam
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 每页的数量
        /// </summary>
        public int limit { get; set; }
        /// <summary>
        /// 根据用户类型获取用户列表的类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 根据日期获取日志的日期
        /// </summary>
        public string date { get; set; }
    }
}