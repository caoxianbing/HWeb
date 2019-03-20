using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.APIModel
{
    /// <summary>
    /// 健康数据
    /// </summary>
    public class HealthInfoModel:BaseModel
    {
        public HealthInfoModel()
        {
            DeviceId = 0;
            SerialNumber = "";
            Step = 0;
            Distance = 0;
            Energy = 0;
            HeartRate = 0;
            BloodMax = 0;
            BloodMin = 0;
            SleepAll = 0;
            DeepSleep = 0;
            LightSleep = 0;
            Note = "";
            LastUpdateTime = "";
        }
        public int DeviceId { get; set; }

        public string SerialNumber { get; set; }
        /// <summary>
        /// 步数
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 距离
        /// </summary>
        public decimal Distance { get; set; }

        /// <summary>
        /// 卡里路
        /// </summary>
        public decimal Energy { get; set; }

        /// <summary>
        /// 心率
        /// </summary>
        public decimal HeartRate { get; set; }

        /// <summary>
        /// 血压最大值
        /// </summary>
        public decimal BloodMax { get; set; }

        /// <summary>
        /// 血压最低值
        /// </summary>
        public decimal BloodMin { get; set; }

        /// <summary>
        /// 睡眠
        /// </summary>
        public double SleepAll { get; set; }
        /// <summary>
        /// 深睡眠
        /// </summary>
        public double DeepSleep { get; set; }

        /// <summary>
        /// 浅睡眠
        /// </summary>
        public double LightSleep { get; set; }

        /// <summary>
        /// 闹钟提醒
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 数据更新时间
        /// </summary>
        public string LastUpdateTime { get; set; }
    }
}
