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
    public class NoticeController : BaseController
    {

        /// <summary>
        /// 时间线显示所有系统公告
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Notice = NoticeLogic.GetNotices(0);
            return View();
        }

        /// <summary>
        /// 返回公共列表
        /// </summary>
        /// <returns></returns>
        public JsonResult AjaxGetAllNotice()
        {
            List<Notice> notices = NoticeLogic.GetNotices();
            return Json(new
            {
                code = 0,
                data = notices
            });
        }
        /// <summary>
        /// 保存公共
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public JsonResult AjaxSaveNotice(Notice notice)
        {
            BaseResult res = new BaseResult();
            notice.Created = DateTime.Now;
            if (notice.Id > 0)
            {
                SaveUserLog(AuthUser.LoginName + "修改了公告：" + notice.Title, LogLevel.Sensitive, AuthUser.LoginName, "AjaxSaveNotice", "修改公告");
            }
            else
            {
                SaveUserLog(AuthUser.LoginName + "发布了公告：" + notice.Title, LogLevel.Sensitive, AuthUser.LoginName, "AjaxSaveNotice", "发布公告");
            }
            NoticeLogic.SaveNotice(notice);
            return Json(res);
        }
        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult AjaxDelNotice(int id)
        {
            BaseResult res = new BaseResult();

            Notice n = NoticeLogic.GetNoticeById(id);
            if (n != null)
            {
                NoticeLogic.DelNotice(n);
                res.Message = "删除成功！";
                res.State = State.Success;
                SaveUserLog(AuthUser.LoginName + "删除了公告：" + n.Title, LogLevel.Sensitive, AuthUser.LoginName, "AjaxDelNotice", "删除公告");
            }
            else
            {
                res.Message = "删除失败！";
                res.State = State.Falid;
                SaveUserLog(AuthUser.LoginName + "删除公告：" + n.Title + "失败", LogLevel.Error, AuthUser.LoginName, "AjaxDelNotice", "删除公告");
            }
            return Json(res);
        }
    }
}