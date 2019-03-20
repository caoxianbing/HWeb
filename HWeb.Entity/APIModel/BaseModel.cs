using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.APIModel
{
    public class BaseModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string Message { get; set; }
    }
}
