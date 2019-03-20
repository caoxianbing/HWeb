using HWeb.Entity.WebEntity;
using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Logic
{
    public class NoticeLogic : BaseLogic
    {
        /// <summary>
        /// 获取所有系统公告
        /// </summary>
        /// <returns></returns>
        public static List<Notice> GetNotices()
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            try
            {
                return Entity.Notices.OrderByDescending(p => p.Created).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据状态获取所有系统公告
        /// </summary>
        /// <returns></returns>
        public static List<Notice> GetNotices(int status)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            try
            {
                return Entity.Notices.Where(p => p.Status == status).OrderByDescending(p => p.Created).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 保存系统公告 修改和新增
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int SaveNotice(Notice n)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                Notice notice = Entity.Notices.FirstOrDefault(p => p.Id == n.Id);
                if (notice != null)//修改
                {
                    notice.Content = n.Content;
                    notice.CreateUser = n.CreateUser;
                    notice.CreateUserName = n.CreateUserName;
                    notice.Status = n.Status;
                    notice.Title = n.Title;
                }
                else
                {//新增
                    Entity.Notices.Add(n);
                }

                return Entity.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取最新的系统公告
        /// </summary>
        /// <returns></returns>
        public static Notice GetLastNotice()
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                return Entity.Notices.Where(p => p.Status == 0).OrderByDescending(p => p.Created).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int DelNotice(Notice n)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                Entity.Notices.Remove(Entity.Notices.First(p => p.Id == n.Id));
                Entity.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 根据id获取公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Notice GetNoticeById(int id)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                return Entity.Notices.FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
                return null;
            }
        }
    }
}
