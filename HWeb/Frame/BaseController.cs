using HWeb.APIData;
using HWeb.Entity.APIModel;
using HWeb.Entity.WebEntity;
using HWeb.Framework;
using HWeb.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HWeb
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 登陆用户
        /// </summary>
        public User AuthUser {
            get {
                return Session[ConstVal.SessionUserStr] as User;
            }
            set {
                Session[ConstVal.SessionUserStr] = value;
            }
        }

        /// <summary>
        /// 将对象转为json格式字符串
        /// </summary>
        /// <param name="data">要转换的对象</param>
        /// <returns></returns>
        protected new JsonResult Json(object data) {
            return new SelfJsonResult {
                Data = data,
                FormateStr = ConstVal.DefaultDateTimeFormat
            };
        }

        /// <summary>
        /// 保存用户操作记录
        /// </summary>
        /// <param name="content">操作内容</param>
        /// <param name="logLevel">日志等级</param>
        /// <param name="userName">用户名</param>
        /// <param name="method">方法</param>
        protected void SaveUserLog(string content, LogLevel logLevel, string userName, string method, string msg = "")
        {

            int userid = AuthUser == null ? 0 : AuthUser.Id;
            string ip = GetIP();
            LogQueueInfo queue = new LogQueueInfo();
            queue.IsSaveDB = true;
            queue.DbLog = new WebLog
            {
                Content = content,
                Created = DateTime.Now,
                UserId = userid,
                UserName = userName,
                LogName = msg,
                LogLevel = (int)logLevel,
                ClientIp = ip,
                Method = method
            };
            HWebQueue.LogQueue.Enqueue(queue);
            ////保存到数据库
            //int id = WebLogLogic.SaveWebLog(log);
            ////记录日志
            //WriteLog(userName + "操作记录id=" + id + "内容：" + content);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void WriteLog(string msg)
        {
            LogQueueInfo queue = new LogQueueInfo();
            queue.Message = msg;
            queue.Ip = GetIP();
            queue.IsSaveDB = false;
            HWebQueue.LogQueue.Enqueue(queue);
            //  LogHelper.WriteLog(msg + ">>ip:" + GetIP());
        }
       

        /// <summary>
        /// 获取客户端请求ip
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {

            if (Session[ConstVal.SessionIpStr] != null)
                return Session[ConstVal.SessionIpStr].ToString();

                string userIP = "";

            try
            {
                string ip = string.Empty;

                if (!string.IsNullOrEmpty(Request.ServerVariables["HTTP_VIA"]))
                    ip = Convert.ToString(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                if (string.IsNullOrEmpty(ip))
                    ip = Request.UserHostAddress;

                // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，<br> // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
                foreach (IPAddress ipAddr in Dns.GetHostEntry(ip).AddressList)
                {
                    if (ipAddr.AddressFamily.ToString() == "InterNetwork")
                    {
                        userIP = ipAddr.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (userIP == "")
                    userIP = "1.1.1.1";
                HWebQueue.InLogQueue_Error(ex);
            }

            return userIP;
        }

    }
}