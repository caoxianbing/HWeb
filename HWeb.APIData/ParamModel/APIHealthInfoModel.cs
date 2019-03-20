using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    public class APIHealthInfoModel : APIBaseModel
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public int DeviceId { get; set; }

        /// <summary>
        /// 手机时区
        /// </summary>
        public double TimeOffset { get; set; }
    }
}
