using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWeb.Controllers
{
    [ErrorFilter]
    public class ErrorController :BaseController
    {
        // GET: Error
        public ActionResult Index(string error)
        {
            //error = HttpUtility.UrlDecode(error);
            //LogHelper.ErrorLog(error);
            return View();
        }

        public ActionResult NotVerified(string action)
        {
            WriteLog("未通过验证访问" + action + ".....");
            return View();
        }
    }
}