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
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Um = WebHelper.LoadUserMenu(AuthUser.UserTypeId.Value, 0);
            ViewBag.User = AuthUser;
            return View();
        }

        /// <summary>
        /// 居民登录首页界面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserShow()
        {
            return View();
        }
        /// <summary>
        /// 医生登录首页界面
        /// </summary>
        /// <returns></returns>
        public ActionResult DoctorShow()
        {
            return View();
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public JsonResult AjaxChildrenMenu()
        {
            //CacheHelper.RemoveAllCache();//测试移除缓存
            List<UserMenu> ums = WebHelper.LoadUserMenu(AuthUser.UserTypeId.Value, 0);
            Dictionary<string, List<Object>> menus = new Dictionary<string, List<object>>();
            foreach (UserMenu um in ums)
            {
                List<UserMenu> cums = WebHelper.LoadUserMenu(AuthUser.UserTypeId.Value, um.Id);
                List<Object> objs = new List<object>();
                foreach (UserMenu cum in cums)
                    objs.Add(new
                    {
                        title = cum.MenuName,
                        icon = cum.Icon,
                        href = cum.Url,
                        spread = false
                    });
                menus.Add(um.MenuCode, objs);
            }

            return Json(menus);
        }


        /// <summary>
        /// 登陆首页默认加载的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Default()
        {
            List<WebLog> logs = WebLogLogic.GetWebLog(AuthUser.Id, AuthUser.UserTypeId.Value, 0, 10);
            ViewBag.Logs = logs;
            ViewBag.User = AuthUser;
            ViewBag.DeviceCount = DeviceLogic.GetDeviceCount(AuthUser.UserTypeId.Value, AuthUser.Id);
            return View();
        }
    }
}