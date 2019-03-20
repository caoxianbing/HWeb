using HWeb.Entity.WebEntity;
using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HWeb.Logic
{
    /// <summary>
    /// 指令logic
    /// </summary>
    public class CmdLogic:BaseLogic
    {
        /// <summary>
        /// 根据设备型号获取指令列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<CmdConfig> GetCmdList(int model)
        {
            WebDBEntities2 entity = new WebDBEntities2();
            return entity.CmdConfigs.Where(p => p.Model == model).ToList();
        }
        /// <summary>
        /// 根据设备id获取设备的指令队列
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static List<CmdQueue> GetQueueByDeviceId(int deviceId)
        {
            WebDBEntities2 entity = new WebDBEntities2();
            return entity.CmdQueues.Where(p => p.APIDeviceId == deviceId).ToList();
        }

        /// <summary>
        /// 根据指令代码获取指令
        /// </summary>
        /// <param name="cmdCode"></param>
        /// <returns></returns>
        public static CmdConfig GetCmdConfigByCode(string cmdCode, int model)
        {
            WebDBEntities2 entity = new WebDBEntities2();
            return entity.CmdConfigs.FirstOrDefault(p => p.CmdCode == cmdCode && p.Model == model);
        }

        /// <summary>
        /// 根据设备imei获取指令下发记录
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static List<CmdLog> GetCmdLogs(string imei)
        {
            WebDBEntities2 entity = new WebDBEntities2();
            //取前100
            return entity.CmdLogs.Where(p => p.Imei == imei).OrderByDescending(p => p.SendDate).Take(100).ToList();
        }
        /// <summary>
        /// 保存指令下发记录
        /// </summary>
        /// <param name="cl"></param>
        public static void SaveCmdLog(CmdLog cl)
        {
            WebDBEntities2 entity = new WebDBEntities2();
            entity.CmdLogs.Add(cl);
            entity.SaveChanges();
        }
        /// <summary>
        /// 添加指令执行队列，定时下发指令
        /// </summary>
        /// <param name="queue"></param>
        public static void SaveCmdQueue(CmdQueue queue)
        {
            WebDBEntities2 entity = new WebDBEntities2();
            CmdQueue q = entity.CmdQueues.FirstOrDefault(p => p.APIDeviceId == queue.APIDeviceId && p.CmdCode == queue.CmdCode);
            if (q != null)
            {
                q.Model = queue.Model;
                q.IMEI = queue.IMEI;
                q.LastExecTime = queue.LastExecTime;
                q.Param = queue.Param;
                q.Notice = queue.Notice;
                q.ExceTime = queue.ExceTime;
            }
            else
            {
                entity.CmdQueues.Add(queue);
            }
            entity.SaveChanges();
        }
        /// <summary>
        /// 获取所有队列
        /// </summary>
        /// <returns></returns>
        public static List<CmdQueue> GetCmdQueues()
        {
            WebDBEntities2 entity = new WebDBEntities2();
            var list = from n in entity.CmdQueues
                       where System.Data.Entity.Core.Objects.EntityFunctions.DiffMinutes(DateTime.Now, n.LastExecTime) < 3
                       select n;
            //select n;
            // int i = list.ToList()[0].Value;
            //entity.CmdQueues.Where(p => p.LastExecTime.Value.AddMinutes(p.ExceTime ?? 0) > DateTime.Now).ToList()
            return list.ToList();
        }
        /// <summary>
        /// 更新队列时间
        /// </summary>
        /// <param name="queue"></param>
        public static void UpdateQueueTime(CmdQueue queue)
        {
            WebDBEntities2 entity = new WebDBEntities2();
            CmdQueue q = entity.CmdQueues.FirstOrDefault(p => p.APIDeviceId == queue.APIDeviceId && p.CmdCode == queue.CmdCode);
            q.LastExecTime = DateTime.Now.AddMinutes(q.ExceTime??0);
            entity.SaveChanges();
        }

        /// <summary>
        /// 移除队列
        /// </summary>
        /// <param name="cmdCode"></param>
        /// <param name="deviceId"></param>
        public static void DelCmdQueue(string cmdCode, int deviceId)
        {
            WebDBEntities2 entity = new WebDBEntities2();
            CmdQueue q = entity.CmdQueues.FirstOrDefault(p => p.APIDeviceId == deviceId && p.CmdCode == cmdCode);
            if (q != null)
            {
                entity.CmdQueues.Remove(q);
                entity.SaveChanges();
            }
        }
    }
}
