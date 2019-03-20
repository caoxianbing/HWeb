using HWeb.Entity.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HWeb
{
    /// <summary>
    /// 输出报表数据
    /// </summary>
    public class PrintReport
    {
        /// <summary>
        /// 打印健康报表
        /// </summary>
        /// <returns></returns>
        public static string PrintHealtReport(HealthReportModel info)
        {
            
            //命名导出表格的StringBuilder变量
            StringBuilder sHtml = new StringBuilder(string.Empty);
            sHtml.Append("<tr height=\"20\" align=\"center\" style='background-color:#009688;'><td>测量时间</td><td>心率</td><td>收缩压</td><td>舒张压</td><td>步数</td><td>运动距离(KM)</td><td>卡里路(千卡)</td></tr>");
            //打印列名
            foreach (var v in info.Items)
            {
                sHtml.Append("<tr height=\"20\" align=\"center\"><td>" + v.LastUpdate + "</td><td>" + v.Heartbeat + "</td><td>" + v.Shrink + "</td><td>" + v.Diastolic + "</td><td>" + v.Steps + "</td><td>" + WebHelper.GetDistance(v.Steps) + "</td><td>" + WebHelper.GetCariello(v.Steps) + "</td></tr>");
            }

            if(info.Items.Count==0)
                sHtml.Append("<tr height=\"30\"><td colspan=\"7\"  align=\"center\">暂无数据</td></tr>");

            //打印表尾
            // sHtml.Append("<tr height=\"40\"><td align=\"center\" colspan=\"5\" style='background-color:#CD0000;font-size:24px'><b>XXXXXXXX</a> </b></td></tr>");



            return sHtml.ToString();
            //调用输出Excel表的方法
            //ExportToExcel("application/ms-excel", "XXXXXX报价表.xls", sHtml.ToString());
        }
    }
}