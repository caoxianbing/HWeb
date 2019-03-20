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
using System.Web;
using System.Web.Mvc;


namespace HWeb.Controllers
{
    [HWebAuth]
    public class DeviceController : BaseController
    {
        /// <summary>
        /// 分页获取设备列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult AjaxGetDeviceList(LayUIPageParam param)
        {
            LayuiPageResult<DeviceAndUser> res = new LayuiPageResult<DeviceAndUser>();
            res.data = DeviceLogic.GetDeviceList(AuthUser.UserTypeId.Value, AuthUser.Id, param.page, param.limit);
            res.count = DeviceLogic.GetDeviceCount(AuthUser.UserTypeId.Value, AuthUser.Id);
            return Json(res);
        }
        /// <summary>
        /// 提交用户设备绑定，解绑
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="type"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public JsonResult AjaxSubUD(string imei, int type, string loginName)
        {
            BaseResult res = new BaseResult();

            //解绑
            if (type == 1)
            {
                Device d = DeviceLogic.GetDeviceByImei(imei);
                if (d == null)
                {
                    res.State = State.NotFind;
                    res.Message = "该设备不存在！";
                    return Json(res);
                }
                User user = UserLogic.GetUserById(d.UserId);
                if (user != null)
                {
                    BaseModel bm = DeviceData.UnBind(d.Imei);
                    if (bm.State == 0)
                    {
                        d.UserId = 0;
                        DeviceLogic.SaveDevice(d);
                        SaveUserLog(AuthUser.LoginName + "将设备：" + d.Imei + ",从用户" + user.LoginName + "中解绑了。", LogLevel.Sensitive, AuthUser.LoginName, "AjaxSubUD", "解绑设备");
                        res.State = State.Success;
                        res.Message = "解绑成功！";
                    }
                    else
                    {
                        res.State = State.Falid;
                        res.Message = "解绑失败：" + bm.Message;
                    }
                }
            }
            //绑定
            else if (type == 2)
            {

                User user = UserLogic.GetUser(loginName);

                if (user == null)
                {
                    res.State = State.NotFind;
                    res.Message = "用户不存在！";
                    return Json(res);
                }
                user.IMEI = imei;
                //准备绑定设备
                Device d = preBindDevice(user);
                if (d != null)
                {
                    BaseModel bm = DeviceData.BindDevice(d.APIDeviceId.Value, user.APIUserId.Value);
                    if (bm.State != 0)
                    {
                        res.State = State.Falid;
                        res.Message = "绑定失败：" + bm.Message;
                        return Json(res);
                    }
                    d.UserId = user.Id;
                    DeviceLogic.SaveDevice(d);
                    SaveUserLog(AuthUser.LoginName + "将设备：" + d.Imei + "绑定到了用户" + loginName, LogLevel.Sensitive, AuthUser.LoginName, "AjaxSubUD", "删除设备");
                    res.State = 0;
                    res.Message = "绑定成功！";
                }
                else
                {
                    res.State = State.Falid;
                    res.Message = "设备验证失败";
                }
            }
            else if (type == 3)
            {
                if (AuthUser.UserTypeId == 2 || AuthUser.UserTypeId == 1)
                {

                    Device _d = DeviceLogic.GetDeviceByImei(imei);
                    DeviceLogic.DelDevice(_d.Id);
                    res.State = State.Success;
                    res.Message = "删除设备成功！";
                    SaveUserLog(AuthUser.LoginName + "删除了设备：" + imei, LogLevel.Sensitive, AuthUser.LoginName, "AjaxSubUD", "删除设备");
                }
                else
                {
                    res.State = State.Falid;
                    res.Message = "暂无删除设备的权限";
                    SaveUserLog(AuthUser.LoginName + "删除设备失败：" + imei, LogLevel.Sensitive, AuthUser.LoginName, "AjaxSubUD", "删除设备");
                }
            }
            return Json(res);
        }
        /// <summary>
        /// 绑定设备之前先验证设备是符合绑定的条件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="imei"></param>
        /// <returns></returns>
        Device preBindDevice(User user)
        {

            Device _d = DeviceLogic.GetDeviceByUserId(user.Id);
            //验证设备是否存在
            if (!string.IsNullOrWhiteSpace(user.IMEI) && _d == null)
            {
                //需先调用设备接口验证
                DeviceCheckModel dcm = DeviceData.CheckDevice(user.IMEI, user.APIUserId ?? 0);
                if (dcm.State != 0 && dcm.State != 1105)
                {
                    //  res.State = State.Falid;
                    string msg = dcm.Message;
                    //res.Message = msg;
                    SaveUserLog(AuthUser.LoginName + "绑定设备失败：" + msg, LogLevel.Error, AuthUser.LoginName, "preBindDevice", "绑定该设备");
                    // return Json(res);
                    return null;
                }
                else
                {
                    //添加设备表
                    Device d = new Device
                    {
                        UserId = 0,
                        IconId = 3,
                        Created = DateTime.Now,
                        Imei = user.IMEI,
                        Status = (int)Status.Normal,
                        APIDeviceId = dcm.DeviceId,
                        APIDeviceModel = dcm.Model
                    };
                    DeviceLogic.SaveDevice(d);
                    // SaveUserLog(AuthUser.LoginName + "为" + user.LoginName + "绑定设备：" + user.IMEI, LogLevel.Error, AuthUser.LoginName, "AjaxEditUser", "绑定设备");
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        /// 设备监控
        /// </summary>
        /// <returns></returns>
        public ActionResult Monitor()
        {
            ViewBag.user = AuthUser;
            return View();
        }
        /// <summary>
        /// 历史轨迹
        /// </summary>
        /// <returns></returns>
        public ActionResult History() {
            return View();
        }
        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public JsonResult AjaxAddDevice(string imei)
        {
            BaseResult res = new BaseResult();

            Device _d = DeviceLogic.GetDeviceByImei(imei);
            //验证设备是否存在
            if (_d == null)
            {
                //需先调用设备接口验证
                DeviceCheckModel dcm = DeviceData.CheckDevice(imei, 1);
                if (dcm.State != 0 && dcm.State != 1105)
                {
                    //  res.State = State.Falid;
                    string msg = dcm.Message;
                    res.Message = msg;
                    res.State = State.Falid;
                    //res.Message = msg;
                    SaveUserLog(AuthUser.LoginName + "添加设备失败：" + msg, LogLevel.Error, AuthUser.LoginName, "AjaxAddDevice", "添加设备");
                    // return Json(res);;
                }
                else
                {
                    //添加设备表
                    Device d = new Device
                    {
                        UserId = 0,
                        IconId = 3,
                        Created = DateTime.Now,
                        Imei = imei,
                        Status = (int)Status.Normal,
                        APIDeviceId = dcm.DeviceId,
                        APIDeviceModel = dcm.Model
                    };
                    DeviceLogic.SaveDevice(d);
                    SaveUserLog(AuthUser.LoginName + "添加了设备：" + imei, LogLevel.Info, AuthUser.LoginName, "AjaxAddDevice", "添加设备");
                    res.State = State.Success;
                    res.Message = "添加成功";
                    //return Json(res);
                }
            }
            else
            {
                res.State = State.DeviceIn;
                res.Message = "设备已存在";
            }

            return Json(res);
        }
        /// <summary>
        /// 获取设备指令列表
        /// </summary>
        /// <param name="type">1用户id，2设备id</param>
        /// <param name="objId"></param>
        /// <returns></returns>
        public JsonResult AjaxGetCmdListAndLog(int type, int objId)
        {
            Device d = null;
            if (type == 1)
                d = DeviceLogic.GetDeviceByUserId(objId);
            else if (type == 2)
                d = DeviceLogic.GetDeviceById(objId);
            if (d == null)
            {
                return Json(new BaseResult
                {
                    State = State.NotFind,
                    Message = "未找到绑定设备"
                });
            }

            List<CmdConfig> ccList = CmdLogic.GetCmdList(d.APIDeviceModel ?? 0);
            List<CmdLog> clList = CmdLogic.GetCmdLogs(d.Imei);

            List<CmdQueue> qList = CmdLogic.GetQueueByDeviceId(d.APIDeviceId ?? 0);
            foreach (var q in qList)
            {
                CmdConfig _c = ccList.FirstOrDefault(p => p.CmdCode == q.CmdCode);
                if (_c != null)
                    _c.CmdValue = (q.ExceTime ?? 0).ToString();
            }

            return Json(new { State = State.Success, UserName = AuthUser.LoginName, CmdList = ccList, LogList = clList, Imei = d.Imei, DeviceId = d.Id });
        }

        /// <summary>
        /// 下发指令
        /// </summary>
        /// <param name="cmdCode">指令code</param>
        /// <param name="deviceId">设备id</param>
        /// <returns></returns>
        public JsonResult AjaxSendCmd(string cmdCode, int deviceId, string param,int min)
        {
            BaseResult res = new BaseResult();
            Device d = DeviceLogic.GetDeviceById(deviceId);
            CmdConfig cmd = CmdLogic.GetCmdConfigByCode(cmdCode,d.APIDeviceModel.Value);

            if ((d.APIDeviceModel ?? 0) <= 0)
            {
                res.Message = "指令下发失败,设备型号为空！";
                res.State = State.Falid;
                return Json(res);
            }
            CmdLog log = new CmdLog();
            log.CmdCode = cmdCode;
            log.CmdName = cmd.CmdName;
            log.Imei = d.Imei;
            log.Param = param;
            log.IsSucess = false;
            log.SendLoginName = AuthUser.LoginName;
            //是否立即下发，不是的加入队列
            if ((cmd.IsNowSend ?? true) == false)//设置主动上传间隔
            {
                log.SendDate = DateTime.Now;
                if (min > 0)
                {
                    CmdQueue queue = new CmdQueue();
                    queue.APIDeviceId = d.APIDeviceId;
                    queue.Model = d.APIDeviceModel.Value;
                    queue.CmdCode = cmdCode;
                    queue.Created = DateTime.Now;
                    queue.LastExecTime = DateTime.Now;
                    queue.Param = param;
                    queue.ExceTime = min;
                    queue.IMEI = d.Imei;
                    queue.Notice = AuthUser.LoginName;
                    CmdLogic.SaveCmdQueue(queue);
                    SaveUserLog(AuthUser.LoginName + "设置了设备" + d.Imei + cmd.CmdName + ":" + min + "分钟", LogLevel.Info, AuthUser.LoginName, "AjaxSendCmd", "下发指令");
                }
                else
                {
                    CmdLogic.DelCmdQueue(cmdCode, d.APIDeviceId.Value);
                }
                log.CmdName = cmd.CmdName + "：" + min + "分钟";

                res.State = State.Success;
                res.Message = "设置成功，" + min + "分钟";
                //return Json(res);
            }
            else if (d != null)
            {
                log.SendDate = DateTime.Now;
                SendCmdModel model = DeviceData.SendCmd(cmdCode, d.APIDeviceId ?? 0, d.APIDeviceModel ?? 0, param);
                if (model.State != 0)
                {
                    res.State = State.Falid;
                    res.Message = model.Content + ":" + model.Message;
                    SaveUserLog(AuthUser.LoginName + "下发设备"+d.Imei+"的" + cmd.CmdName + "指令失败：" + res.Message, LogLevel.Sensitive, AuthUser.LoginName, "AjaxSendCmd", "下发指令");
                }
                else
                {
                    log.IsSucess = true;
                    res.State = State.Success;
                    res.Message = model.Content;
                    SaveUserLog(AuthUser.LoginName + "下发设备" + d.Imei + "的" + cmd.CmdName + "指令：" + res.Message, LogLevel.Info, AuthUser.LoginName, "AjaxSendCmd", "下发指令");
                    //WriteLog( + res.Message);
                }
                log.Response = model.State + "|" + res.Message;
                log.ResponseTime = DateTime.Now;
            }
            else
            {
                res.State = State.NotFind;
                res.Message = "设备不存在";
                WriteLog(AuthUser.LoginName + "下发设备" + d.Imei + "的" + cmd + "指令失败：" + res.Message);
            }
            //保存指令下发记录
            CmdLogic.SaveCmdLog(log);
            if (res.Message.StartsWith("Off"))
                res.Message = "设备不线，指令离线下发";
            return Json(res);
        }

        /// <summary>
        /// 监控页面的获取设备列表
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public JsonResult AjaxGetMonitorList(int pi, int pc)
        {
            List<DeviceAndUser> dus = DeviceLogic.GetDeviceList(AuthUser.UserTypeId.Value, AuthUser.Id, pi, pc);
            string deviceIds = "";
            foreach (var du in dus)
                deviceIds += du.APIDeviceId + ",";
            deviceIds=deviceIds.TrimEnd(',');
            DeviceListModel list = DeviceData.GetDeviceList(deviceIds, pi, pc);
            DeviceListResult res = new DeviceListResult();
            res.Items = new List<DeviceInfo>();
            if (list.Items.Count > 0)
            {
                list.Items.ForEach(p =>
                {
                    DeviceAndUser du = dus.FirstOrDefault(t => t.APIDeviceId == p.Id);
                    if (du != null)
                    {
                        DeviceInfo di = new DeviceInfo();
                        di.Id = du.DeviceId;
                        di.APIId = du.APIDeviceId ?? 0;
                        di.Battery = p.Battery;
                        di.Icon = du.IconId ?? 0;
                        di.Model = p.Model;
                        di.IMEI = du.Imei;
                        di.Latitude = Convert.ToDecimal(WebHelper.GetLatLngString(p.Latitude));
                        di.Longitude = Convert.ToDecimal(WebHelper.GetLatLngString(p.Longitude));
                        di.ServerUtcDate = p.ServerUtcDate.AddHours(8);
                        di.DeviceUtcDate = p.DeviceUtcDate.AddHours(8);
                        di.Type = p.Type;
                        di.Sim = p.Sim;
                        di.Status = p.Status;
                        if (DateTime.Now.AddMinutes(-15) < di.ServerUtcDate)
                            di.Status = 2;
                        
                        di.UserId = du.UserId;
                        di.UserName = string.IsNullOrWhiteSpace(du.UserName) ? du.LoginName : du.UserName;
                        di.Sex = du.Sex ?? 2;
                        res.Items.Add(di);
                    }
                });
            }
            else
            {
                res.State = State.Falid;
                res.Message = list.Message;
            }
            return Json(res);
        }
        /// <summary>
        /// 请求用户的健康数据
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public JsonResult AjaxGetHealthInfo(int deviceId)
        {
            Device d = DeviceLogic.GetDeviceById(deviceId);
            if (d == null)
            {
                return Json(new BaseResult { State = State.NotFind, Message = "未找到该设备" });
            }
            HealthInfoModel model = HealthData.GetHealthInfo(d.APIDeviceId ?? 0);

            return Json(new { State = State.Success, Item = model });
        }

        /// <summary>
        /// ajax请求设备的历史轨迹
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public JsonResult AjaxGetHistory(int deviceId, DateTime start, DateTime end)
        {
            Device d = DeviceLogic.GetDeviceById(deviceId);
            if (d == null)
            {
                return Json(new BaseResult { State = State.NotFind, Message = "未找到该设备" });
            }
            HistoryModel model = DeviceData.GetHistory(d.APIDeviceId ?? 0, start, end);
            if (model.State == 0||model.State==10)
            {
                model.Items.ForEach(p => {
                    p.Time = Convert.ToDateTime(p.Time).AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
                    });
                return Json(new { State = State.Success, data = model.Items });
            }
            else
                return Json(new BaseResult { State = State.Falid, Message = model.Message });
        }
    }
}