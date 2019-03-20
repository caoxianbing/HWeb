using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ParamModel
{
    /// <summary>
    /// 上传图片参数
    /// </summary>
    public class UploadImgParam:BaseParam
    {
        /// <summary>
        /// 对象 id
        /// </summary>
        public int ObjId { get; set; }
        /// <summary>
        /// 图片类型 
        /// </summary>
        public ImgType Type { get; set; }

        public HttpPostedFileBase file { get; set; }

        /// <summary>
        /// 图片id
        /// </summary>
        public int Id { get; set;}
    }
}