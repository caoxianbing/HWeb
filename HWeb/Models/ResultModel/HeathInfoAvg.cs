using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ResultModel
{
    public class HeathInfoAvg
    {
        /// <summary>
        /// 天数或者时间段
        /// </summary>
        public string DateKey { get; set; }
        /// <summary>
        /// 和
        /// </summary>
        public int Sum { get; set; }

        public int Sum2 { get; set; }

        public int Sum3 { get; set; }

        public DateTime LastDate { get; set; }

        public Decimal Avg { get {
                if (HCount == 0 || Sum == 0) {
                    return 0;
                }
                return Sum / HCount;
            } }
        public Decimal Avg2 {
            get
            {
                if (SCount == 0 || Sum2 == 0)
                {
                    return 0;
                }
                return Sum2 / SCount;
            }
        }
        public Decimal Avg3 {
            get
            {
                if (SCount == 0 || Sum3 == 0)
                {
                    return 0;
                }
                return Sum3 / SCount;
            }
        }

        /// <summary>
        /// 类型 1:心率 2:血压,4步数
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 心率数量
        /// </summary>
        public int HCount { get; set; }
        /// <summary>
        /// 血压数量
        /// </summary>
       public int SCount { get; set; }
    }
}