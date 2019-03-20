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
    public class MenuController:BaseController
    {
        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        /// <returns></returns>
        public JsonResult AjaxGetAllMenu()
        {
            List<SortMenu> ums = MenuLogic.GetAllMenus();
            //返回适应layui table格式
            return Json(new
            {
                code = 0,
                data = ums
            });
        }

        /// <summary>
        /// 根据用户类型获取所有菜单
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public JsonResult AjaxGetMenu(int userType)
        {
            List<SortMenu> uts = MenuLogic.GetMenusByUType(userType);

            return Json(new
            {
                menus = uts
            });
        }

        /// <summary>
        /// 提交菜单，保存/修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AjaxSubmitMenu(UserMenu model)
        {
            BaseResult res = new BaseResult();
            //解码
            model.Icon = HttpUtility.UrlDecode(model.Icon);
            model.MenuLevel = model.ParentId == 0 ? 1 : 2;
            bool isEdit = model.Id > 0;
            int r = MenuLogic.SaveMenu(model);
            if (r > 0)
            {
                res.State = (int)State.Success;
                res.Message = "保存成功";

                SaveUserLog(AuthUser.LoginName + (isEdit ? "修改" : "添加") + "了菜单" + model.MenuName, LogLevel.Sensitive, AuthUser.LoginName, "", "菜单操作");
                CacheHelper.ClearMenuCache();
              //  BaseLogic.DisEntity();
            }
            else
            {
                res.State =State.Falid;
                res.Message = "保存失败";
            }

            return Json(res);
        }

        /// <summary>
        /// 删除用户菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult AjaxDelMenu(int id)
        {
            BaseResult res = new BaseResult();
            UserMenu um = MenuLogic.GetUserMenuById(id);
            if (um != null)
            {
                MenuLogic.DeleteUserMenu(um);
                res.Message = "删除成功！";
                res.State =State.Success;
                CacheHelper.ClearMenuCache();
                SaveUserLog(AuthUser.LoginName + "删除了菜单：" + um.MenuName, LogLevel.Sensitive, AuthUser.LoginName, "AjaxDelMenu", "删除菜单");
            }
            else
            {
                res.Message = "删除失败！";
                res.State =State.Falid;
                SaveUserLog(AuthUser.LoginName + "删除菜单：" + um.MenuName + "失败", LogLevel.Error, AuthUser.LoginName, "AjaxDelMenu", "删除菜单");
            }
            return Json(res);
        }
    }
}