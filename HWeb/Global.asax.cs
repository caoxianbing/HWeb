
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //加载系统配置
            WebHelper.LoadConfig();
            //启动日志队列
            HWebQueue.StartLogQueue();
            //启动指令队列
            HWebQueue.StartCmdQueue(600);
        }        
    }
}
