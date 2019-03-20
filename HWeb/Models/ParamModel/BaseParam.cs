using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ParamModel
{

	/// <summary>
    /// 传入参数基础类
    /// </summary>
	public class BaseParam
	{
		/// <summary>
        /// 请求时间
        /// </summary>
		public int DateTick { get; set; }

        /// <summary>
        /// token验证
        /// </summary>
        public string Token { get; set; }
	}
}