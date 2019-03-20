using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.APIModel
{
    /// <summary>
    /// 健康数据报表
    /// </summary>
    public class HealthReportModel : BaseModel
    {

        public HealthReportModel()
        {
            Items = new List<HealthInfo>();
            HeartRate = "";
            Blood = "";
        }
        /// <summary>
        /// 数据
        /// </summary>
        public List<HealthInfo> Items { get; set; }

        /// <summary>
        /// 心率最大最小值
        /// </summary>
        public string HeartRate { get; set; }

        /// <summary>
        /// 血压最大最小值
        /// </summary>
        public string Blood { get; set; }
    }

    /// <summary>
    /// 健康信息
    /// </summary>
    public class HealthInfo
    {
        /// <summary>
        /// IMEI
        /// </summary>
        public string IMEI { get; set; }
        //public int Roll { get; set; }
        /// <summary>
        /// 步数
        /// </summary>
        public int Steps { get; set; }
        /// <summary>
        /// 心率
        /// </summary>
        public int Heartbeat { get; set; }
        //public decimal BloodPressure { get; set; }
        //public decimal BodyTemperature { get; set; }
        /// <summary>
        /// 舒张压
        /// </summary>
        public decimal Diastolic { get; set; }
        /// <summary>
        /// 收缩压
        /// </summary>
        public decimal Shrink { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime LastUpdate { get; set; }
        /// <summary>
        /// 血糖
        /// </summary>
        public decimal BloodSugar { get; set; }

        /// <summary>
        /// 距离
        /// </summary>
        public decimal Distance { get; set; }
        /// <summary>
        /// 卡里路
        /// </summary>
        public decimal Calorie { get; set; }

    }
}
