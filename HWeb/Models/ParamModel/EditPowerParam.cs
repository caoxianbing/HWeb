using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ParamModel
{
    public class EditPowerParam : BaseParam
    {
        /// <summary>
        /// 1新增，2修改
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 权限id
        /// </summary>
        public int[] PIds { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 要修改的所有权限名称
        /// </summary>
        public string PowerStr { get; set; }
    }
}