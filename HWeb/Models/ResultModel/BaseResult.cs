using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ResultModel
{
    /// <summary>
    /// 返回结果基础类
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
    }
}