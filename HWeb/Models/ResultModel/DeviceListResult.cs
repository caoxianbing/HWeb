using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ResultModel
{
    public class DeviceListResult:BaseResult
    {
        public List<DeviceInfo> Items;
    }
    public class DeviceInfo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 所属用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 所属用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 设备方id
        /// </summary>
        public int APIId { get; set; }
        /// <summary>
        /// 设备 imei
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public int Icon { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 设备型号 文本
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 设备型号 值
        /// </summary>
        public int TypeValue { get; set; }
        /// <summary>
        /// 设备SIM卡号
        /// </summary>
        public string Sim { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public int Model { get; set; }
        /// <summary>
        /// 电量
        /// </summary>
        public int Battery { get; set; }
        /// <summary>
        /// 通讯时间
        /// </summary>
        public DateTime ServerUtcDate { get; set; }
        /// <summary>
        /// 定位时间
        /// </summary>
        public DateTime DeviceUtcDate { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
    }
}