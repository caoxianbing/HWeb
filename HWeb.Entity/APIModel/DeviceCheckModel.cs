using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.APIModel
{
    public class DeviceCheckModel:BaseModel
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public int Model
        {
            get;
            set;

        }
        /// <summary>
        /// 是否需要用户号码
        /// </summary>
        public bool NeedPhone
        {
            get;
            set;
        }

        /// <summary>
        /// 是否实名认证[专用]
        /// </summary>
        public bool IsActivation { get; set; }
    }
}
