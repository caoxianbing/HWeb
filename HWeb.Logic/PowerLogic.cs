using HWeb.Entity.WebEntity;
using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Logic
{
    public class PowerLogic : BaseLogic
    {
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="userType">用户类型</param>
        /// <param name="pId">菜单id 数组</param>
        /// <param name="userName">操作的管理员登陆账号</param>
        /// <returns></returns>
        public static int AddPower(int userType, int[] pId, string userName = "")
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            foreach (int i in pId)
            {
                if (Entity.UserTypeToMenus.FirstOrDefault(p => p.UserTypeId == userType && p.MenuId == i) == null)
                    Entity.UserTypeToMenus.Add(new UserTypeToMenu
                    {
                        UserTypeId = userType,
                        MenuId = i,
                        Created = DateTime.Now,
                        Note = userName + "操作",
                        Status = (int)Status.Normal
                    });
            }
            Entity.SaveChanges();
            return Entity.SaveChanges();
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="userType">用户类型</param>
        /// <param name="pId">菜单id 数组</param>
        /// <param name="userName">操作的管理员登陆账号</param>
        /// <returns></returns>
        public static int DelPower(int userType, int[] pId, string userName = "")
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            foreach (int i in pId)
            {
                var _power = Entity.UserTypeToMenus.FirstOrDefault(p => p.UserTypeId == userType && p.MenuId == i);
                if (_power != null)
                    Entity.UserTypeToMenus.Remove(_power);
            }

            return Entity.SaveChanges();
        }
    }
}
