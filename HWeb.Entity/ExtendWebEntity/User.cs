using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.WebEntity
{
    public partial class User
    {
        /// <summary>
        /// 医生账号
        /// </summary>
        [NotMapped]
        public string BindDoctorName { get; set; }

        /// <summary>
        /// 设备imei
        /// </summary>
        [NotMapped]
        public string IMEI { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        [NotMapped]
        public int DeviceId { get; set; }

        #region 医生信息
        /// <summary>
        /// 医生擅长的项目
        /// </summary>
        [NotMapped]
        public string Subject { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [NotMapped]
        public string Position { get; set; }
        /// <summary>
        /// 任职医院
        /// </summary>
        [NotMapped]
        public string Hospital { get; set; }

        /// <summary>
        /// 详细介绍
        /// </summary>
        [NotMapped]
        public string DoctorInfo { get; set; }
        #endregion

        public decimal Heart { get; set; }

        public decimal Shrink { get; set; }

        public decimal Diastolic { get; set; }

        public int Step { get; set; }
        /// <summary>
        /// 心率阈值配置
        /// </summary>
        public string HeartValue { get; set; }
        /// <summary>
        /// 收缩压阈值配置
        /// </summary>
        public string BloodValue { get; set; }
        /// <summary>
        /// 舒张压阈值配置
        /// </summary>
        public string BloodValue2 { get; set; }
        /// <summary>
        /// 最后测量时间
        /// </summary>
        public string LastCheckTime { get; set; }
    }
}
