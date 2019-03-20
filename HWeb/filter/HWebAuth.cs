using HWeb.Entity.WebEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWeb
{
    public class HWebAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //后期添加菜单权限验证
            User u = httpContext.Session[ConstVal.SessionUserStr] as User;
            if (u == null)
                return false;
            else {
                //其他权限后面添加
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect("/Error/NotVerified?action=" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName);
        }
    }
}