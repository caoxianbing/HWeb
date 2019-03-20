using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    /// <summary>
    /// 历史轨迹接口 参数
    /// </summary>
    public class APIHistoryModel:APIBaseModel
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 开始时间(时间为 UTC0 时间)
        /// 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间(时间为 UTC0 时间)
        /// 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 1:显示lbs 
        /// 2:显示wifi
        /// </summary>
        public int ShowLbs { get; set; }
        /// <summary>
        /// 地图类型
        /// baidu 百度坐标系
        /// google 谷歌坐标系（包括高德）
        /// </summary>
        public string MapType { get; set; }
        /// <summary>
        /// 返回记录的最大数量
        /// </summary>
        public int SelectCount { get; set; }

        /// <summary>
        /// 定位类型
        /// 0: GPS,1: LBS,2: Wifi,3: GPS+LBS,4: GPS+Wifi,5: LBS+Wifi,6: GPS+LBS+Wifi
        /// </summary>
        public int PositionType { get; set; }
    }
}
