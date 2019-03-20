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
    public class DeviceData
    {
        public static DeviceCheckModel CheckDevice(string imei,int userId)
        {
            DeviceCheckModel dcm = new DeviceCheckModel();
            try
            {
                APICheckDeviceModel model = new APICheckDeviceModel();
                model.UserId = userId;
                model.SerialNumber = imei;
                dcm = HttpApi.GetApiResult<DeviceCheckModel>("Device/CheckDevice", model);
                if (dcm == null)
                {
                    dcm = new DeviceCheckModel();
                    dcm.State = -1;
                    dcm.Message = ReadResource.GetStr("DeviceCheck_" + dcm.State);
                }
                else 
                    dcm.Message = ReadResource.GetStr("DeviceCheck_" + dcm.State);
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return new DeviceCheckModel {
                    State = -1,
                    Message = ex.Message
                };
            }
            return dcm;
        }
        /// <summary>
        /// 解绑设备
        /// </summary>
        /// <param name="deviceId">设备服务器的设备id</param>
        /// <param name="userId">设备服务器的用户id</param>
        /// <returns></returns>
        public static BaseModel UnBind(string imei)
        {
            BaseModel dcm = new BaseModel();
            try
            {
                APIUnBindDeviceModel model = new APIUnBindDeviceModel();
                model.SN = imei;
                dcm = HttpApi.GetApiResult<DeviceCheckModel>("Device/RemoveDevice", model);
                ReadResource.ExecBack(dcm, "RemoveShare");
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return new BaseModel {
                    State=-1,
                    Message=ex.Message
                };
            }
            return dcm;
        }

        /// <summary>
        /// 绑定设备
        /// </summary>
        /// <param name="deviceid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static BaseModel BindDevice(int deviceid, int userid)
        {
            BaseModel bm = new BaseModel();
            try
            {
                APIBindDeviceModel bdm = new APIBindDeviceModel();
                bdm.DeviceId = deviceid;
                bdm.UserId = userid;
                bm = HttpApi.GetApiResult<DeviceCheckModel>("Device/BindDeviceForUser", bdm);
                ReadResource.ExecBack(bm, "BindDevice");
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return new BaseModel
                {
                    State = -1,
                    Message = ex.Message
                };
            }

            return bm;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="deviceIds"></param>
        /// <returns></returns>
        public static DeviceListModel GetDeviceList(string deviceIds,int pi,int pc)
        {
            DeviceListModel dlm = null;
            //dlm.Items = new List<DeviceListInfo>();
            try
            {
                APIDeviceListModel model = new APIDeviceListModel();
                model.LoginType = 0;
                model.Id = 1;
                model.PageNo = pi;
                model.PageCount = pc;
                model.AllIds = deviceIds;
                model.Type = 0;
                model.MapType = "baidu";
                model.LastTime = DateTime.Now.AddDays(-10);
                dlm = HttpApi.GetApiResult<DeviceListModel>("Device/ListDevice", model);
                ReadResource.ExecBack(dlm, "ListDevice");
            }
            catch (Exception ex)
            {
                dlm.State = -1;
                dlm.Message = ex.Message;
                LogHelper.ErrorLog(ex);
            }

            return dlm;
        }
        /// <summary>
        /// 获取设备的历史轨迹
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static HistoryModel GetHistory(int deviceId,DateTime startTime,DateTime endTime)
        {
            HistoryModel hm = new HistoryModel();
            try
            {
                APIHistoryModel model = new APIHistoryModel();
                model.ShowLbs = 1;
                model.PositionType = 6;
                //获取5000条
                model.SelectCount = 5000;
                //转成utc时间
                model.StartTime = startTime.AddHours(-8);
                model.EndTime = endTime.AddHours(-8);
                model.DeviceId = deviceId;
                model.MapType = "baidu";
                hm = HttpApi.GetApiResult<HistoryModel>("Location/History", model);
                ReadResource.ExecBack(hm, "ListDevice");
            }
            catch (Exception ex)
            {
                hm.State = -1;
                hm.Message = ex.Message;
                LogHelper.ErrorLog(ex);
            }
            return hm;
        }

        /// <summary>
        /// 下发指令
        /// </summary>
        /// <param name="cmdCode">指令code 设备平台获取</param>
        /// <param name="deviceId">设备id</param>
        /// <param name="deviceModel">设备型号</param>
        /// <param name="param">参数,可空</param>
        /// <returns></returns>
        public static SendCmdModel SendCmd(string cmdCode, int deviceId, int deviceModel, string param = "")
        {
            SendCmdModel bm = new SendCmdModel();
            try
            {
                APISendCmdModel model = new APISendCmdModel();
                model.CmdCode = cmdCode.ToString();
                model.DeviceId = deviceId;
                model.Params = param;
                model.DeviceModel = deviceModel.ToString();
                bm = HttpApi.GetApiResult<SendCmdModel>("Command/SendCommand", model);
                ReadResource.ExecBack(bm, "SendCommand");
            }
            catch (Exception ex)
            {
                bm.State = -1;
                bm.Message = ex.Message;
                LogHelper.ErrorLog(ex);
            }
            return bm;
        }
    }
}
