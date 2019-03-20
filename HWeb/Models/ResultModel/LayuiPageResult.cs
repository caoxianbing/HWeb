using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ResultModel
{
    /// <summary>
    /// layui分页返回格式参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LayuiPageResult<T>
    {
        public int code { get; set; }
        public string msg { get; set; }
        public List<T> data { get; set; }
        public int count { get; set; }
    }
}