using HWeb.Entity.WebEntity;
using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Logic
{
    public class WebLogLogic: BaseLogic
    {
        /// <summary>
        /// 保存操作记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static int SaveWebLog(WebLog log)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                Entity.WebLogs.Add(log);
                int res = Entity.SaveChanges();
                return res;
            }
            catch (Exception ex) {
                LogHelper.ErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        /// 分页获取操作记录
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userType">用户类型</param>
        /// <param name="pi">页码</param>
        /// <param name="pc">每页数量</param>
        /// <returns></returns>
        public static List<WebLog> GetWebLog(int userId,int userType,int pi,int pc,string date="")
        {
            try
            {
                IQueryable<WebLog> logs;
                //管理员和超级管理员可以获取所有日志.其他角色这可以获取自己的日志
                //根据日期过滤
                WebDBEntities2 Entity = new WebDBEntities2();
                if (string.IsNullOrWhiteSpace(date))
                    logs = Entity.WebLogs;
                else
                {
                    DateTime todayMin = Convert.ToDateTime(date);
                    DateTime todayMax = Convert.ToDateTime(date+" 23:59:59");

                    logs = Entity.WebLogs.Where(p => p.Created.Value > todayMin && p.Created.Value < todayMax);
                }

                if (userType == 1 || userType == 2)
                {
                    logs = logs.OrderByDescending(p => p.Created).Skip(pc * pi).Take(pc);
                }
                else
                {
                    logs = logs.Where(p => p.UserId == userId).OrderByDescending(p => p.Created).Skip(pc * pi).Take(pc);
                }
                return logs.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取所有日志的数量
        /// </summary>
        /// <returns></returns>
        public static int GetWebLogCount(string date = "")
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            if (string.IsNullOrWhiteSpace(date))
                return Entity.WebLogs.Count();
            else
            {
                DateTime todayMin = Convert.ToDateTime(date);
                DateTime todayMax = Convert.ToDateTime(date + " 23:59:59");
                return Entity.WebLogs.Count(p => p.Created.Value > todayMin && p.Created.Value < todayMax);
            }
        }
    }
}
