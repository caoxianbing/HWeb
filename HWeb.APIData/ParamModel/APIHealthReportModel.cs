using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    public class APIHealthReportModel:APIBaseModel
    {
        /// <summary>
        /// IMEI
        /// </summary>
        public string Imei { get; set; }
        /// <summary>
        /// 开始时间 UTC
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// 结束时间 UTC
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// 查询类型1:心率 2:血压,3全部
        /// </summary>
        public int TypeId { get; set; }


        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 查询数量
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// 手机时区
        /// </summary>
        public double TimeOffset { get; set; }
    }
}
