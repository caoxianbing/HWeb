using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    public class APISendCmdModel:APIBaseModel
    {
        /// <summary>
        /// 设备 id
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceModel { get; set; }
        /// <summary>
        /// 指令编码
        /// </summary>
        public string CmdCode { get; set; }
        /// <summary>
        /// 指令参数
        /// </summary>
        public string Params { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get { return 1; } }
        /// <summary>
        /// 未使用
        /// </summary>
        public int Type { get { return 0; } }
    }
}
