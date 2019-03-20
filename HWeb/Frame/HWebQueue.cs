using HWeb.APIData;
using HWeb.Entity.APIModel;
using HWeb.Entity.WebEntity;
using HWeb.Framework;
using HWeb.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace HWeb
{
    public class HWebQueue
    {
        /// <summary>
        /// 日志记录队列
        /// </summary>
        public static Queue<LogQueueInfo> LogQueue;


        /// <summary>
        /// 开启日志队列
        /// </summary>
       public static void StartLogQueue()
        {
            LogQueue = new Queue<LogQueueInfo>();
            //通过线程池开启线程，不停的的取队列
            ThreadPool.QueueUserWorkItem(p => {
                while (true)
                {
                    if (LogQueue.Count > 0)
                    {
                        LogHelper.WriteLog("剩余队列数量：" + LogQueue.Count() + "-----" + DateTime.Now);
                        try
                        {
                            //出队列
                            LogQueueInfo info = LogQueue.Dequeue();
                            //错误日志
                            if (info.Exception != null)
                            {
                                LogHelper.ErrorLog(info.Exception);
                            }
                            //需要存数据的
                            else if (info.IsSaveDB && info.DbLog != null)
                            {
                                //保存到数据库
                                int id = WebLogLogic.SaveWebLog(info.DbLog);
                                LogHelper.WriteLog(info.DbLog.UserName + "操作记录id=" + id + "内容：" + info.DbLog.Content + ",IP:" + info.DbLog.ClientIp);
                            }
                            else
                            {
                                LogHelper.WriteLog(info.Message + ">>ip:" + info.Ip);
                            }
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    else
                    {
                        //队列中没有内容，线程休眠1s
                        Thread.Sleep(2000);
                    }
                }
            });
           
        }
        /// <summary>
        /// 日志加入队列
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ip"></param>
        public static void InLogQueue(string msg, LogLevel level, string ip = "")
        {
            LogQueueInfo info = new LogQueueInfo();
            info.Message = msg;
            info.HappenTime = DateTime.Now;
            info.Ip = ip;
            info.Level = (int)level;
            info.IsSaveDB = false;
            LogQueue.Enqueue(info);
        }

        /// <summary>
        /// 错误日志加入队列
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ip"></param>
        public static void InLogQueue_Error(Exception ex, string ip = "")
        {
            LogQueueInfo info = new LogQueueInfo();
            info.Exception = ex;
            info.Ip = ip;
            LogQueue.Enqueue(info);
        }
        /// <summary>
        /// 启动指令队列
        /// </summary>
        /// <param name="second">执行时间间隔,默认10分钟</param>
        public static void StartCmdQueue(int second=600)
        {
            //最少一分钟跑一次
            if (second < 60) second = 60;
            ThreadPool.QueueUserWorkItem(p =>
            {
                while (true)
                {
                    try
                    {
                        List<CmdQueue> list = CmdLogic.GetCmdQueues();
                        InLogQueue("获取到待执行指令数量：" + list.Count(), LogLevel.Info, "local");
                        if (list.Count > 0)
                        {
                            foreach (var l in list)
                            {
                                //多个指令以_区分
                                string[] cmdCodes = l.CmdCode.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string code in cmdCodes)
                                {
                                    CmdConfig cmd = CacheHelper.GetCachCmdConfig(code, l.Model);
                                    CmdLog log = new CmdLog();
                                    log.CmdCode = code;
                                    log.CmdName = l.ExceTime + "分钟:" + cmd.CmdName;
                                    log.Imei = l.IMEI;
                                    log.Param = l.Param;
                                    log.IsSucess = false;
                                    log.SendLoginName = l.Notice;
                                    log.SendDate = DateTime.Now;
                                    SendCmdModel model = DeviceData.SendCmd(code, l.APIDeviceId ?? 0, l.Model, l.Param);
                                    log.IsSucess = model.State == 0;
                                    log.Notice = "每隔" + l.ExceTime + "分钟下发主动监测。";
                                    log.Response = model.Content + ":" + model.Message;
                                    log.ResponseTime = DateTime.Now;
                                    CmdLogic.SaveCmdLog(log);//保存日志
                                }
                                CmdLogic.UpdateQueueTime(l);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        InLogQueue_Error(ex, "local");
                    }

                    Thread.Sleep(second * 1000);
                }
            });
        }
    }
    /// <summary>
    /// 日志队列信息
    /// </summary>
    public class LogQueueInfo
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// 日志消息
        /// </summary>
        public string Message { get; set; }

        public WebLog DbLog { get; set; }
        /// <summary>
        /// 日志等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 是否存在数据库
        /// </summary>
        public bool IsSaveDB { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 发生日期
        /// </summary>
        public DateTime HappenTime { get; set; }

    }
}