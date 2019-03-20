using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ResultModel
{
    /// <summary>
    /// 上传图片返回
    /// </summary>
    public class UploadImgResult:BaseResult
    {
        /// <summary>
        /// 图片id
        /// </summary>
        public int ImgId { get; set; }
    }
}