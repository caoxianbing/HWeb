using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ResultModel
{
    /// <summary>
    /// 系统公告返回
    /// </summary>
    public class NoticeResult : BaseResult
    {
        public string CreatedUserName { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string Created { get; set; }
    }
}