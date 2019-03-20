using HWeb.APIData.ParamModel;
using HWeb.Entity.APIModel;
using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HWeb.APIData
{
    /// <summary>
    /// 监控数据
    /// </summary>
    public class HealthData
    {
        /// <summary>
        /// 获取设备当前的健康数据
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static HealthInfoModel GetHealthInfo(int deviceId)
        {
            APIHealthInfoModel model = new APIHealthInfoModel();
            model.DeviceId = deviceId;
            model.TimeOffset = 8;
            HealthInfoModel hm = new HealthInfoModel();
            try
            {
                hm = HttpApi.GetApiResult<HealthInfoModel>("Device/HealthInfo", model);
                ReadResource.ExecBack(hm, "HealthInfo");
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return new HealthInfoModel
                {
                    State = -1,
                    Message = ex.Message
                };
            }
            return hm;
        }

        /// <summary>
        /// 取某段时间内健康数据报表
        /// </summary>
        /// <param name="imei">设备imei</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="type">1:心率 2:血压,3全部</param>
        /// <returns></returns>
        public static HealthReportModel GetHealthReport(string imei, DateTime start, DateTime end, int type=3)
        {
            HealthReportModel rm = new HealthReportModel();
            try
            {
                APIHealthReportModel model = new APIHealthReportModel();
                model.TypeId = type;
                model.Start = start;
                model.End = end;
                model.Imei = imei;
                rm = HttpApi.GetApiResult<HealthReportModel>("Health/GetHealthByType", model);
                ReadResource.ExecBack(rm, "HealthReport");
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return new HealthReportModel
                {
                    State = -1,
                    Message = ex.Message
                };
            }
            return rm;
        }
    }
}
