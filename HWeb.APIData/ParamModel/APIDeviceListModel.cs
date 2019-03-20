using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    public class APIDeviceListModel:APIBaseModel
    {
        /// <summary>
        /// 传 1
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 在线状态，0 是全部。1 为在线 , 2为离线
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 查询的页数
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 登录类型 传0
        /// </summary>
        public int LoginType { get; set; }
        /// <summary>
        /// 地图类型
        /// </summary>
        public string MapType { get; set; }
        /// <summary>
        /// 上次获取时间
        /// </summary>
        public DateTime LastTime { get; set; }

        /// <summary>
        /// 获取设备的id
        /// </summary>
        public string AllIds { get; set; }
    }
}
