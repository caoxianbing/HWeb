using System;
using HWeb.Entity.WebEntity;
using HWeb.Framework;
using HWeb.Logic;
using HWeb.Models.ParamModel;
using HWeb.Models.ResultModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace HWeb.Controllers
{
    
    public class OtherController :BaseController
    {

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [HWebAuth]
        public JsonResult UploadImg(UploadImgParam param)
        {
            UploadImgResult res = new UploadImgResult();
            string name = "";
            try
            {
                if (param.file != null)
                {
                    Img img = new Img();
                    string path = "~/Content/images/";
                    string eName = Path.GetExtension(param.file.FileName);
                    name = param.file.FileName;
                    //登陆背景图片保存
                    if (param.ObjId == 1 && param.Type == ImgType.BackImg)
                    {
                        path += "loginback" + eName;
                    }
                    else if(param.Type==ImgType.HeadImg)
                    {
                        path += "himg/himg_" + param.ObjId + "_" + param.Type + eName;
                    }

                    img.ImgPath = path;
                    img.ObjId = param.ObjId;
                    img.ImgType = (int)param.Type;
                    img.Id = param.Id;
                    param.file.SaveAs(Server.MapPath(path));
                    res.ImgId = SysConfigLogic.SaveImg(img);
                    res.State = State.Success;
                    SaveUserLog(AuthUser.UserName + "上传了照片" + path, LogLevel.Info, AuthUser.LoginName, "UploadImg", "上传照片");
                    //更新用户头像
                    if (param.Type == ImgType.HeadImg)
                    {
                        AuthUser.HeadImgId = img.Id;
                        UserLogic.UpdateUserHeadImg(param.ObjId, img.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                res.State = State.Error;
                res.Message = "服务器错误";
                SaveUserLog(AuthUser.UserName + "上传照片" + name + "失败:" + ex.Message, LogLevel.Error, AuthUser.LoginName, "UploadImg", "上传照片");
            }
            return Json(res);
        }
        /// <summary>
        /// 获取最新的系统公告
        /// </summary>
        /// <returns></returns>
         [HWebAuth]
        public JsonResult GetLastNotice()
        {
            NoticeResult res = new NoticeResult();
            Notice n = NoticeLogic.GetLastNotice();
            if (n != null)
            {
                res.State = State.Success;
                res.Title = n.Title;
                res.Content = n.Content;
                res.CreatedUserName = n.CreateUserName;
                res.Created = n.Created.Value.ToString("yyyy-MM-dd");
            }
            else
                res.State = State.Falid;
            return Json(res);
        }

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <returns></returns>
        [HWebAuth]
        public JsonResult GetWebLog(LayUIPageParam param)
        {
            List<WebLog> logs = WebLogLogic.GetWebLog(AuthUser.Id, AuthUser.UserTypeId.Value, param.page - 1, param.limit, param.date);

            LayuiPageResult<WebLog> ws = new LayuiPageResult<WebLog>();
            ws.data = logs;
            ws.code = 0;
            ws.count = WebLogLogic.GetWebLogCount(param.date);
            ws.msg = "获取成功";
            return Json(ws);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult GetImg(int id)
        {
            Img img = SysConfigLogic.GetImgById(id);
            if (img == null)
            {
                WriteLog("图片id为：" + id + "的图未找到");
                return null;
            }
            else
            {
                var imgPath = Server.MapPath(img.ImgPath);
                //从图片中读取byte
                var imgByte = System.IO.File.ReadAllBytes(imgPath);

                return File(imgByte, "image/jpeg");
            }
        }

        /// <summary>
        /// 下载日志文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HWebAuth]
        public FileResult GetWebLogFile(string date)
        {
            //转换下格式
            date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            string logPath = Server.MapPath("~/logs/" + date + ".log");
            return File(logPath, "application/log", date + ".log");
        }
    }
}