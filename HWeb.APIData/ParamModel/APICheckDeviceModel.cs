using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    public class APICheckDeviceModel : APIBaseModel
    {
        /// <summary>
        /// 设备Imei
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
    }
}
