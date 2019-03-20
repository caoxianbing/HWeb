using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.APIModel
{
    /// <summary>
    /// 历史轨迹
    /// </summary>
    public class HistoryModel:BaseModel
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 最后id
        /// </summary>
        public int LastLocationId { get; set; }
        /// <summary>
        /// 最后时间
        /// </summary>
        public string LastDeviceUtcDate { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<HistoryInfo> Items { get; set; }

        /// <summary>
        /// 去除停留点之前的数量
        /// </summary>
        public int ItemsCount { get; set; }
    }
   /// <summary>
   /// 轨迹详细信息
   /// </summary>
    public class HistoryInfo
    {
        public int LocationId { get; set; }

        public string Time { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

        public decimal Speed { get; set; }

        public decimal Course { get; set; }

        public int IsStop { get; set; }

        public int StopTime { get; set; }

        public string Icon { get; set; }

        public int DataType { get; set; }
        public string StopTimeStr { get; set; }

        public string EndTime { get; set; }

        public int Battery { get; set; }


    }
}
