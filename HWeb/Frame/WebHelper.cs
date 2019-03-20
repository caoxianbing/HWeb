using HWeb.APIData;
using HWeb.Entity.APIModel;
using HWeb.Entity.WebEntity;
using HWeb.Framework;
using HWeb.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HWeb
{
    public class WebHelper
    {
        /// <summary>
        /// 加载系统配置
        /// </summary>
        public static void LoadConfig()
        {
            //加载配置
            ConstVal.WebConfig = CacheHelper.GetCache(ConstVal.SysConfigCacheStr) as SysConfig;
            if (ConstVal.WebConfig == null)
            {//如果缓存没有命中， 就从数据取，并写入缓存中
                ConstVal.WebConfig = SysConfigLogic.GetSysConfig();
                //写入缓存
                CacheHelper.SetCache(ConstVal.SysConfigCacheStr, ConstVal.WebConfig);
            }
        }

        /// <summary>
        /// 根据步数获取距离
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public static decimal GetDistance(int step)
        {
            return (decimal)Math.Round((step * 0.7) / 1000, 2);
        }
        //根据步数获取卡路里
        public static decimal GetCariello(int step)
        {
            return (decimal)Math.Round(((step * 0.7) / 1000) * 65.4, 2);
        }

        /// <summary>
        /// 加载用户菜单
        /// </summary>
        /// <param name="userType">用户类型</param>
        /// <param name="parentId">上级菜单id</param>
        /// <returns></returns>
        public static List<UserMenu> LoadUserMenu(int userType, int parentId = 0)
        {
            List<UserMenu> um = CacheHelper.GetCache(ConstVal.UTMenuCacheStr + userType + parentId) as List<UserMenu>;
            if (um == null)
            {
                um = MenuLogic.GetMenusByUType(userType, parentId);
                //设置缓存
                CacheHelper.SetCache(ConstVal.UTMenuCacheStr + userType, um, ConstVal.MenuCacheTime);
            }
            return um;
        }

        /// <summary>
        /// 获取客户端请求ip
        /// </summary>
        /// <returns></returns>
        public static string GetIP(HttpRequestBase req)
        {
            string userIP = "";

            try
            {
                string ip = string.Empty;

                if (!string.IsNullOrEmpty(req.ServerVariables["HTTP_VIA"]))
                    ip = Convert.ToString(req.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                if (string.IsNullOrEmpty(ip))
                    ip = req.UserHostAddress;

                // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，<br> // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
                foreach (IPAddress ipAddr in Dns.GetHostEntry(ip).AddressList)
                {
                    if (ipAddr.AddressFamily.ToString() == "InterNetwork")
                    {
                        userIP = ipAddr.ToString();
                    }
                }
            }
            catch
            {
                if (userIP == "")
                    userIP = "1.1.1.1";
            }

            return userIP;
        }

        /// <summary>
        /// 根据用户类型id,获取用户类型名
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static string GetUTName(int userType)
        {
            string str = "";
            switch (userType)
            {
                case 1:
                    str = "超级管理员";
                    break;
                case 2:
                    str = "管理员";
                    break;
                case 3:
                    str = "医生";
                    break;
                case 4:
                    str = "居民";
                    break;
                default:
                    str = "未知";
                    break;
            }
            return str;
        }
        /// <summary>
        /// 地图转换
        /// </summary>
        /// <param name="oLatLng"></param>
        /// <returns></returns>
        public static string GetLatLngString(object oLatLng)
        {
            try
            {
                decimal latLng = Convert.ToDecimal(oLatLng);
                return latLng.ToString("0.000000");
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 绑定设备之前先验证设备是符合绑定的条件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static Device preBindDevice(User user, string ip = "1.1.1.1")
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
                    //   SaveUserLog(, LogLevel.Error, user.LoginName, "preBindDevice", "绑定该设备");

                    LogQueueInfo queue = new LogQueueInfo();
                    queue.IsSaveDB = true;
                    queue.DbLog = new WebLog
                    {
                        Content = user.LoginName + "绑定设备失败：" + msg,
                        Created = DateTime.Now,
                        UserId = user.Id,
                        UserName = user.LoginName,
                        LogName = msg,
                        LogLevel = (int)LogLevel.Sensitive,
                        ClientIp = ip,
                        Method = "preBindDevice"
                    };
                    HWebQueue.LogQueue.Enqueue(queue);

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
     
    }
}