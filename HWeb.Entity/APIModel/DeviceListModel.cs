using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.APIModel
{
    public class DeviceListModel : BaseModel
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 设备信息集合
        /// </summary>
        public List<DeviceListInfo> Items { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DeviceListInfo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 设备 imei
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 设备名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分组Id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNo { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 设备图片
        /// </summary>
        public string AvatarImage { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 原始纬度
        /// </summary>
        public decimal OLat { get; set; }
        /// <summary>
        /// 原始经度
        /// </summary>
        public decimal OLng { get; set; }
        /// <summary>
        /// ACC状态
        /// </summary>
        public int Acc { get; set; }
        /// <summary>
        /// 方位角
        /// </summary>
        public decimal Course { get; set; }
        /// <summary>
        /// 是否显示ACC
        /// </summary>
        public int IsShowAcc { get; set; }
        /// <summary>
        /// 设备型号 文本
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 设备型号 值
        /// </summary>
        public int TypeValue { get; set; }
        /// <summary>
        /// 是否车载设备 1 为车载 0 为个人
        /// </summary>
        public int IsCarDevice { get; set; }
        /// <summary>
        /// 车标
        /// </summary>
        public string Logo { get; set; }
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
        /// 里程
        /// </summary>
        public decimal Distance { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public decimal Speed { get; set; }

        /// <summary>
        /// IccId obd用 其他无效
        /// </summary>
        public string IccId { get; set; }

        /// <summary>
        /// 紧急联系人号码1
        /// </summary>
        public string SOSPhone1 { get; set; }

        /// <summary>
        /// 紧急联系人号码2
        /// </summary>
        public string SOSPhone2 { get; set; }
    }

}
