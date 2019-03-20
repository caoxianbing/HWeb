using System;
using System.Web;
using System.Collections;
using System.Web.Caching;
using HWeb.Entity.WebEntity;
using HWeb.Logic;

namespace HWeb
{
    public class CacheHelper
    {
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static object GetCache(string cacheKey)
        {
            var objCache = HttpRuntime.Cache.Get(cacheKey);
            return objCache;
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object objObject)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject);
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object objObject, int timeout = 7200)
        {
            try
            {
                if (objObject == null) return;
                var objCache = HttpRuntime.Cache;
                //相对过期
                //objCache.Insert(cacheKey, objObject, null, DateTime.MaxValue, timeout, CacheItemPriority.NotRemovable, null);
                //绝对过期时间
                objCache.Insert(cacheKey, objObject, null, DateTime.Now.AddSeconds(timeout), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            catch (Exception)
            {
                //throw;
            }
        }
        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveCache(string cacheKey)
        {
            var cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            var cache = HttpRuntime.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }
        /// <summary>
        /// 设置指令的缓存
        /// </summary>
        /// <param name="cmdCode"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static CmdConfig GetCachCmdConfig(string cmdCode, int model)
        {
            string key = "cmd_" + cmdCode + model;
            CmdConfig cmd = GetCache(key) as CmdConfig;
            if (cmd == null)
                cmd = CmdLogic.GetCmdConfigByCode(cmdCode, model);
            SetCache(key, cmd, 3600);//一个小时过期
            return cmd;
        }

        /// <summary>
        /// 清除菜单缓存
        /// </summary>
        public static void ClearMenuCache()
        {
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();

            while (CacheEnum.MoveNext())
            {
                string key = CacheEnum.Key.ToString();
                if (key.Contains(ConstVal.UTMenuCacheStr))
                    RemoveCache(key);
            }
        }
    }
}

