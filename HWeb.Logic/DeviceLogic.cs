using HWeb.Entity.WebEntity;
using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Logic
{
    public class DeviceLogic:BaseLogic
    {
        /// <summary>
        /// 保存设备
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int SaveDevice(Device d)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                Device device = Entity.Devices.FirstOrDefault(p => p.Imei == d.Imei);
                if (device != null)//如果设备已存在
                {
                    device.Status = d.Status;
                    device.IconId = d.IconId;
                    device.UserId = d.UserId;
                    device.APIDeviceId = d.APIDeviceId > 0 ? d.APIDeviceId : device.APIDeviceId;
                    device.APIDeviceModel = d.APIDeviceModel > 0 ? d.APIDeviceModel : device.APIDeviceModel;
                }
                else
                {
                    Entity.Devices.Add(d);
                }
                Entity.SaveChanges();
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return 1;
            }
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static int DelDevice(int deviceId)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                Device d = Entity.Devices.FirstOrDefault(p => p.Id == deviceId);
                Entity.Devices.Remove(d);
                Entity.SaveChanges();
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return 1;
            }
        }
        /// <summary>
        /// 根据用户id获取用户设备
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Device GetDeviceByUserId(int userId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Devices.FirstOrDefault(p => p.UserId == userId);
        }
        /// <summary>
        /// 根据设备id获取设备
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static Device GetDeviceById(int deviceId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Devices.FirstOrDefault(p => p.Id == deviceId && p.Status == (int)Status.Normal);
        }
        /// <summary>
        /// 根据设备imei获取设备
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static Device GetDeviceByImei(string imei)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Devices.FirstOrDefault(p => p.Imei == imei && p.Status == (int)Status.Normal);
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="userTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<DeviceAndUser> GetDeviceList(int userTypeId, int userId, int pi, int pc)
        {
            List<DeviceAndUser> list = new List<DeviceAndUser>();
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                pi--;
                IQueryable<DeviceAndUser> _l = null;
                //医生只能获取到绑定了自己居民的设备列表
                if (userTypeId == 3)
                {
                    _l = Entity.DeviceAndUsers.Where(p => p.BingDoctorId == userId && p.Status == (int)Status.Normal);
                }
                //用户只取自己的
                else if (userTypeId == 4)
                {
                    _l = Entity.DeviceAndUsers.Where(p => p.UserId == userId && p.Status == (int)Status.Normal);
                }
                else if (userTypeId == 1 || userTypeId == 2)
                {
                    _l = _l = Entity.DeviceAndUsers.Where(p=>p.Status == (int)Status.Normal);
                }
                list = _l.OrderBy(p => p.DeviceId).Skip(pi * pc).Take(pc).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
            }
            return list;
        }
        /// <summary>
        /// 取设备数量
        /// </summary>
        /// <param name="userTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetDeviceCount(int userTypeId, int userId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            if (userTypeId == 3)
            {
                var _l = from n in Entity.Devices
                         join u in Entity.Users
                         on n.UserId equals u.Id
                         where u.BingDoctorId == userId && u.Status == (int)Status.Normal
                         select n;
                return _l.Count();
            }
            //用户只取自己的
            else if (userTypeId == 4)
            {
                return Entity.Devices.Where(p => p.Status == (int)Status.Normal && p.UserId == userId).Count();
            }
            else if (userTypeId == 1 || userTypeId == 2)
                return Entity.Devices.Where(p => p.Status == (int)Status.Normal).Count();
            return 0;
        }
    }
}
