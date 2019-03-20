using HWeb.Entity.WebEntity;
using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Logic
{
    public class MenuLogic:BaseLogic
    {
        /// <summary>
        /// 根据用户类型,和父级获取用户菜单
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static List<UserMenu> GetMenusByUType(int userType, int parentId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            var list = from u in Entity.UserMenus
                       join t in Entity.UserTypeToMenus on u.Id equals t.MenuId
                       where t.UserTypeId == userType && u.ParentId == parentId
                       select u;

            return list.ToList();
        }

        /// <summary>
        /// 根据用户类型获取所有用户菜单
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static List<SortMenu> GetMenusByUType(int userType)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.SortMenus.Where(p => p.UserTypeId == userType).OrderBy(p => p.Path).ToList();
        }


        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        public static List<SortMenu> GetAllMenus()
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            var list = Entity.SortMenus.Where(p => p.UserTypeId == 1).OrderBy(p => p.Path).ToList();
            return list;
        }
        /// <summary>
        /// 保存用户菜单
        /// </summary>
        /// <param name="um"></param>
        /// <returns></returns>
        public static int SaveMenu(UserMenu um)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            int res = 0;
            try
            {
                if (um.Id <= 0)
                {
                    Entity.UserMenus.Add(um);

                }
                else
                {
                    UserMenu _um = Entity.UserMenus.FirstOrDefault(p => p.Id == um.Id);
                    _um.MenuName = um.MenuName;
                    _um.MenuLevel = um.MenuLevel;
                    _um.MenuCode = um.MenuCode;
                    _um.Note = um.Note;
                    _um.ParentId = um.ParentId;
                    _um.Url = um.Url;
                    _um.Icon = um.Icon;
                    //Entity.UserMenus.Attach(um);
                    //  context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                }
                Entity.SaveChanges();
                res = um.Id;
                //超级管理员有所有菜单的权限
                PowerLogic.AddPower(1, new int[] { um.Id }, "");
            }
            catch (Exception ex)
            {
                res = 0;
                LogHelper.ErrorLog(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 根据id删除用户菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteUserMenu(UserMenu um)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                Entity.UserMenus.Remove(Entity.UserMenus.First(p => p.Id == um.Id));
                Entity.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return 0;
            }
        }

        public static UserMenu GetUserMenuById(int id)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.UserMenus.FirstOrDefault(p => p.Id == id);
        }

    }
}
