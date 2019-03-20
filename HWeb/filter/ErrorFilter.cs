using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWeb
{
    public class ErrorFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //base.OnException(filterContext);

           var _path = filterContext.HttpContext.Request.Url.LocalPath;
            // string msg = _path + "请求出现错误：" + HttpUtility.UrlEncode(filterContext.Exception.Message);

            // Logic.BaseLogic.DisEntity();
            //LogHelper.ErrorLog(msg);
            //添加错误队列
            HWebQueue.InLogQueue_Error(filterContext.Exception, WebHelper.GetIP(filterContext.HttpContext.Request));
            //ajax请求不用跳转
            if (_path.IndexOf("Ajax") >= 0 || _path.IndexOf("Error") > 0)
            {
                filterContext.RequestContext.HttpContext.Response.Write("{State:2,Message:'服务器内部错误'}");
                filterContext.RequestContext.HttpContext.Response.End();
                return;
            }

            if (!filterContext.ExceptionHandled)
            {
                
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Redirect("/Error/Index?error=" + filterContext.Exception.Message);
            }
        }
    }
}