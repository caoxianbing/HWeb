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
    public class ManageController :BaseController
    {

        /// <summary>
        /// 菜单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Menu()
        {
            ViewBag.Ums = WebHelper.LoadUserMenu(1, 0);
            return View();
        }

        /// <summary>
        /// 系统公告管理
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeManage()
        {
            return View();
        }

        /// <summary>
        /// 权限管理
        /// </summary>
        /// <returns></returns>
        public ActionResult PowerManage()
        {
            ViewBag.Uts = UserLogic.GetAllUserType().Where(p => p.Id > AuthUser.UserTypeId).ToList();
            ViewBag.Ums = MenuLogic.GetAllMenus();
            return View();
        }

        /// <summary>
        /// 站点配置
        /// </summary>
        /// <returns></returns>
        public ActionResult SiteConfig()
        {
            return View();
        }

        /// <summary>
        /// 指令配置你
        /// </summary>
        /// <returns></returns>
        public ActionResult CommandConfig()
        {
            return View();
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <returns></returns>
        public JsonResult AjaxEditPower(EditPowerParam param)
        {
            BaseResult res = new BaseResult();
            if (param.UserType == 1)
            {
                res.State = State.Falid;
                res.Message = "修改失败。";
            }
            else
            {
                if (param.Type == 1)
                {
                    PowerLogic.AddPower(param.UserType, param.PIds, AuthUser.LoginName);
                    res.Message = AuthUser.UserName + "添加了" + param.PowerStr;

                }
                else if (param.Type == 2)
                {
                    PowerLogic.DelPower(param.UserType, param.PIds, AuthUser.LoginName);
                    res.Message = AuthUser.UserName + "移除了" + param.PowerStr;
                }
                res.State = State.Success;
                SaveUserLog(res.Message, LogLevel.Sensitive, AuthUser.LoginName, "AJaxEditPower","权限操作");
            }

            return Json(res);
        }

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <returns></returns>
        public ActionResult WebLog()
        {
            return View();
        }

        /// <summary>
        /// 修改站点配置
        /// </summary>
        /// <returns></returns>
        public JsonResult AjaxSubSysConfig(SysConfigParam param)
        {
            SysConfig s = new SysConfig
            {
                ICP = param.ICP,
                JsVersion = param.JsVersion,
                SysName = param.SysName
            };
            SysConfigLogic.SaveSysConfig(s);
            CacheHelper.RemoveCache(ConstVal.SysConfigCacheStr);
            WebHelper.LoadConfig();
            return Json(new BaseResult
            {
                State = State.Success
            });
        }

        /// <summary>
        /// 用户信息管理
        /// </summary>
        /// <returns></returns>
        public ActionResult UserManage()
        {
            return View();
        }

        /// <summary>
        /// 医生管理
        /// </summary>
        /// <returns></returns>
        public ActionResult DoctorManage()
        {
            return View();
        }
        /// <summary>
        /// 管理员管理
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminManage()
        {
            return View();
        }

        /// <summary>
        /// 设备管理
        /// </summary>
        /// <returns></returns>
        public ActionResult DeviceManage()
        {
            return View();
        }
    
    }
}