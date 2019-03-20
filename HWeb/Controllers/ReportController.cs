using HWeb.APIData;
using HWeb.Entity.APIModel;
using HWeb.Entity.WebEntity;
using HWeb.Framework;
using HWeb.Logic;
using HWeb.Models.ParamModel;
using HWeb.Models.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HWeb.Controllers
{
    [HWebAuth]
    public class ReportController : BaseController
    {
        /// <summary>
        /// 血压统计
        /// </summary>
        /// <returns></returns>
        public ActionResult BloodReport()
        {
            return View();
        }
        /// <summary>
        /// 步数统计
        /// </summary>
        /// <returns></returns>
        public ActionResult StepReport()
        {
            return View();
        }
        /// <summary>
        /// 综合统计 展示用户一段时间以来的平均心率、平均步数和平均移动距离（其中心率为7：00-22:00,22:00-次日7:00平均心率，其余均为日平均）
        /// </summary>
        /// <returns></returns>
        public ActionResult AvgReport()
        {
            return View();
        }

        /// <summary>
        /// 心率统计
        /// </summary>
        /// <returns></returns>
        public ActionResult HeartReport()
        {
            return View();
        }

        /// <summary>
        /// 获取监控报表
        /// </summary>
        /// <param name="type">1:心率 2:血压,3全部,4步数</param>
        /// <param name="date">时间范围，用~隔开</param>
        /// <param name="deviceId">设备id</param>
        /// <returns></returns>
        public JsonResult AjaxGetHealthList(int type, string date, int deviceId)
        {
            Device d = DeviceLogic.GetDeviceById(deviceId);
            if (d != null)
            {
                DateTime start = Convert.ToDateTime(date.Split('~')[0]);
                DateTime end = Convert.ToDateTime(date.Split('~')[1]);
                int space = (end - start).Days + 1;
                //取报表
                HealthReportModel model = getReport(type, date, d);
                List<HeathInfoAvg> avgHbs = new List<HeathInfoAvg>();
                model.Items.ForEach(p =>
                {
                    p.Distance = WebHelper.GetDistance(p.Steps);
                    p.Calorie = WebHelper.GetDistance(p.Steps);
                    p.LastUpdate = p.LastUpdate.AddHours(8);
                    //按时间段统计
                    if (type != 4 && (p.Heartbeat > 0 || p.Diastolic > 0))
                    {
                        string dateKey = p.LastUpdate.ToString("yyyy-MM-dd 7:00") + "~" + p.LastUpdate.ToString("yyyy-MM-dd 22:00");
                        //DateTime _dt = p.LastUpdate.AddDays(-1);
                        if (p.LastUpdate.Hour < 7)
                        {
                            dateKey = p.LastUpdate.AddDays(-1).ToString("yyyy-MM-dd 22:00") + "~" + p.LastUpdate.ToString("yyyy-MM-dd 7:00");
                        }
                        else if (p.LastUpdate.Hour > 22)
                        {
                            dateKey = p.LastUpdate.ToString("yyyy-MM-dd 22:00") + "~" + p.LastUpdate.AddDays(1).ToString("yyyy-MM-dd 7:00");
                        }
                        HeathInfoAvg agv = avgHbs.FirstOrDefault(t => t.DateKey == dateKey);
                        if (agv == null)
                        {
                            agv = new HeathInfoAvg
                            {
                                DateKey = dateKey,
                                Sum = p.Heartbeat,
                                Sum2 = Convert.ToInt32(p.Shrink),
                                Sum3 = Convert.ToInt32(p.Diastolic),
                                SCount = p.Shrink > 0 ? 1 : 0,
                                HCount = p.Heartbeat > 0 ? 1 : 0,
                                Type = 3,
                                LastDate = p.LastUpdate
                            };
                            avgHbs.Add(agv);
                        }
                        else
                        {
                            if (p.Heartbeat > 0)
                                agv.HCount++;
                            if (p.Shrink > 0)
                                agv.SCount++;
                            agv.Sum += p.Heartbeat;
                            agv.Sum2 += Convert.ToInt32(p.Shrink);
                            agv.Sum3 += Convert.ToInt32(p.Diastolic);
                        }

                    }
                });

                if (type == 4)
                {
                    //按天统计
                    var _list = model.Items.GroupBy(p => p.LastUpdate.ToString("yyyy-MM-dd")).Select(g => new
                    {
                        Steps = g.Select(t => t.Steps).Max(),
                        Distance = g.Select(t => t.Distance).Max(),
                        Calorie = g.Select(t => t.Calorie).Max(),
                        DateKey = g.Last().LastUpdate.ToString("yyyy-MM-dd")
                    }).ToList();
                    if (_list.Count < space)
                    {
                        for (int i = 0; i < space; i++)
                        {
                            string _date = start.AddDays(i).ToString("yyyy-MM-dd");
                            if (!_list.Exists(p => p.DateKey == _date))
                            {
                                _list.Add(new
                                {
                                    Steps = 0,
                                    Distance = 0.0M,
                                    Calorie = 0.0M,
                                    DateKey = _date
                                });
                            }
                        }
                    }
                    return Json(new
                    {
                        code = 0,
                        data = model.Items.Where(p => p.Steps > 0).ToList(),
                        extend = _list.OrderByDescending(p => p.DateKey)
                    });
                }
                else
                {
                    avgHbs = avgHbs.OrderByDescending(p => p.LastDate).ToList();
                    ////最小时间替换成开始时间
                    if (avgHbs.Count > 0)// avgHbs[0].DateKey.Split('~')[0]
                    {
                        avgHbs[0].DateKey = avgHbs[0].DateKey.Split('~')[0] + "~" + end.ToString("MM-dd HH:mm");

                        avgHbs[avgHbs.Count - 1].DateKey = start.ToString("yyyy-MM-dd HH:mm") + "~" + avgHbs[avgHbs.Count - 1].DateKey.Split('~')[1];
                    }
                    ////最大时间替换成结束时间
                    //if (avgHbs.Count > 1)
                    //    avgHbs[avgHbs.Count - 1].DateKey = start.ToString("yyyy-MM-dd HH:mm") + "~" + avgHbs[avgHbs.Count - 1].DateKey.Split('~')[1];
                    return Json(new
                    {
                        code = 0,
                        data = model.Items.Where(p => (p.Heartbeat > 0 || p.Diastolic > 0)).ToList(),
                        extend = avgHbs
                    });
                }
            }
            else
            {
                return Json(new { code = State.NotFind, msg = "设备不存在！" });
            }
        }

        /// <summary>
        /// 批量导出数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="deviceIds"></param>
        [HttpGet]
        public void BatchExportReport(int type, string date, string deviceIds = "")
        {
            //居民不能批量倒
            if (AuthUser.UserTypeId == 4)
            {
                return;
            }

            var dAndU = DeviceLogic.GetDeviceList(AuthUser.UserTypeId.Value, AuthUser.Id, 1, 10000);
            if (string.IsNullOrWhiteSpace(deviceIds))
            {
                foreach (var v in dAndU)
                    deviceIds += v.DeviceId + ",";

                deviceIds.TrimEnd(',');
            }
            string[] strs = deviceIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sHtml = new StringBuilder();
            //打印表头
            sHtml.Append("<table border=\"1\" width=\"90%\">");
            sHtml.Append("<tr><td colspan=\"7\" style='font-size:16px;'>时间段：<b>" + date + "</b></td></tr>");
            foreach (string str in strs)
            {
                int _dId = Convert.ToInt32(str);
                var _u = dAndU.FirstOrDefault(p => p.DeviceId == _dId);
                //不在名下不能倒
                if (_u == null) continue;
                var _d = DeviceLogic.GetDeviceById(_dId);
                if (_d == null) continue;
                HealthReportModel _model = getReport(type, date, _d);

                sHtml.Append("<tr height=\"30\"><td colspan=\"7\" align=\"left\" style='font-size:16px;font-weight:600;'><b>" + (!string.IsNullOrWhiteSpace(_u.UserName) ? _u.UserName : "")+ "&nbsp;&nbsp;&nbsp;设备号:" + _u.Imei + "</b></td></tr>");
                sHtml.Append(PrintReport.PrintHealtReport(_model));

            }
            sHtml.Append("</table>");


            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + AuthUser.UserName + " 管辖居民健康报表.xls");

            System.IO.StringWriter tw = new System.IO.StringWriter();
            // Response.Output.Write(sHtml.ToString());
            Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" + sHtml.ToString());
            /*乱码BUG修改 20140505*/
            //如果采用以上代码导出时出现内容乱码，可将以下所注释的代码覆盖掉上面【System.Web.HttpContext.Current.Response.Output.Write(ExcelContent.ToString());】即可实现。
            //System.Web.HttpContext.Current.Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" + ExcelContent.ToString());
            Response.Flush();
            Response.End();
        }



        /// <summary>
        /// 取报表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        HealthReportModel getReport(int type, string date, Device d)
        {
            DateTime start = Convert.ToDateTime(date.Split('~')[0]);
            DateTime end = Convert.ToDateTime(date.Split('~')[1]);
            int space = (end - start).Days + 1;

            HealthReportModel model = null;

            HealthReportModel model1;
            HealthReportModel model2;
            //心率
            if (type == 1 || (type == 4 || type == 3))
            {
                model1 = HealthData.GetHealthReport(d.Imei, start, end, 1);
                model = model1;
            }
            //血压
            if (type == 2 || (type == 4 || type == 3))
            {
                model2 = HealthData.GetHealthReport(d.Imei, start, end, 2);
                if (model == null)
                    model = model2;
                else
                    model.Items.AddRange(model2.Items);
                model.Items = model.Items.OrderByDescending(p => p.LastUpdate).ToList();
            }

            return model;
        }
       
    }
}