using HWeb.Entity.WebEntity;
using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HWeb.Logic
{
    public class SysConfigLogic:BaseLogic
    {
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        public static SysConfig GetSysConfig()
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            SysConfig sc = Entity.SysConfigs.FirstOrDefault();
            return sc;
        }
   

        /// <summary>
        /// 根据图片id获取图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Img GetImgById(int id)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Imgs.FirstOrDefault(P => P.Id == id);
        }
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static int SaveImg(Img img)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            var _i = Entity.Imgs.FirstOrDefault(p => p.ObjId == img.ObjId && p.ImgType == img.ImgType);
            if (_i != null)
            {
                _i.ImgPath = img.ImgPath;
                _i.ImgType = img.ImgType;
                _i.ClickUrl = img.ClickUrl;
                _i.Alt = img.Alt;
                _i.ObjId = img.ObjId;
                _i.Note = img.Note;
                _i.UploadTime = DateTime.Now;
                img.Id = _i.Id;
            }
            else
            {
                Entity.Imgs.Add(img);
            }
            Entity.SaveChanges();
            return img.Id;
        }

        /// <summary>
        /// 保存系统公告
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int SaveSysConfig(SysConfig config)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            try
            {
                SysConfig s = Entity.SysConfigs.FirstOrDefault();
                s.ICP = config.ICP;
                s.JsVersion = config.JsVersion;
                s.SysName = config.SysName;
                return Entity.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return 0;
            }
        }
    }
}
