using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HWeb.Entity.WebEntity;
using HWeb.Framework;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace HWeb.Logic
{
    public class UserLogic:BaseLogic
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static User GetUserByLogin(string loginName, string pwd)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.FirstOrDefault(p => (p.LoginName == loginName || p.PhoneNum == loginName) && p.Pwd == pwd && p.Status == (int)Status.Normal);
        }

        /// <summary>
        /// 根据账号或手机号码获取用户信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static User GetUserByLoginName(string loginName)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.FirstOrDefault(p =>(p.LoginName == loginName||p.PhoneNum==loginName) && p.Status == (int)Status.Normal);
        }

        /// <summary>
        /// 根据账号或手机号码获取居民信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static User GetUser(string loginName)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.FirstOrDefault(p => (p.LoginName == loginName || p.PhoneNum == loginName) && p.Status == (int)Status.Normal && p.UserTypeId == (int)UserTypeEnum.Person);
        }


        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static int UpdateUser(User u)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            User user = Entity.Users.First(p => p.Id == u.Id);

            user.APIToken = u.APIToken;
            user.APIUserId = u.APIUserId;
            user.Email = u.Email;
            user.LastLoginIp = u.LastLoginIp;
            user.LastLoginTime = u.LastLoginTime;
            return Entity.SaveChanges();
        }

        /// <summary>
        /// 检测该用户名是否存在
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static bool ExistsLoginName(string loginName)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.FirstOrDefault(p => p.LoginName == loginName && p.Status == (int)Status.Normal) != null;
        }

        /// <summary>
        /// 检测该手机号码是否已被注册
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static bool ExistsPhoneNum(string phoneNum)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.FirstOrDefault(p => p.PhoneNum == phoneNum && p.Status == (int)Status.Normal) != null;
        }


        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static int SaveUser(User u)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                if (u.Id > 0)
                {
                    User entity = Entity.Users.FirstOrDefault(p => p.Id == u.Id);
                    entity.UserName = u.UserName;
                    entity.PhoneNum = u.PhoneNum;
                    entity.Email = u.Email;
                    entity.Sex = u.Sex;
                    entity.BirthDate = u.BirthDate;
                    entity.UserInfo = u.UserInfo;
                    entity.Status = u.Status;
                    entity.BingDoctorId = u.BingDoctorId;
                    entity.HeartMin = u.HeartMin;
                    entity.HeartMax = u.HeartMax;
                    entity.BloodMax = u.BloodMax;
                    entity.BloodMin = u.BloodMin;
                    entity.BloodMax2 = u.BloodMax2;
                    entity.BloodMin2 = u.BloodMin2;
                }
                else
                {
                    Entity.Users.Add(u);
                }
                return Entity.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 获取所有用户类型
        /// </summary>
        /// <returns></returns>
        public static List<UserType> GetAllUserType()
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.UserTypes.ToList();
        }
        /// <summary>
        /// 根据用户类型返回用户列表,分页
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<User> GetUserByType(int type, int pi, int pc)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                pi--;
                return Entity.Users.Where(p => p.Status == (int)Status.Normal && p.UserTypeId == type).OrderBy(p => p.Id).Skip(pi * pc).Take(pc).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据设备id获取用户，如果未绑定则返回空
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static User GetUserByDeviceId(int deviceId)
        {
            WebDBEntities2 entity = new WebDBEntities2();
            Device _d = entity.Devices.FirstOrDefault(p => p.Id == deviceId);
            if (_d == null || _d.UserId < 1) return null;
            return entity.Users.FirstOrDefault(p => p.Id == _d.UserId);
        }

        public static int GetUserCountByType(int type)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.Where(p => p.Status == (int)Status.Normal && p.UserTypeId == type).Count();
        }

        /// <summary>
        /// 根据用户id获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static User GetUserById(int userId)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                return Entity.Users.FirstOrDefault(p => p.Status == (int)Status.Normal && p.Id == userId);
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///更新用户头像
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="imgId">图片id</param>
        /// <returns></returns>
        public static int UpdateUserHeadImg(int userId, int imgId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            User _u = Entity.Users.FirstOrDefault(p => p.Id == userId);
            _u.HeadImgId = imgId;
            return Entity.SaveChanges();
        }

        /// <summary>
        /// 逻辑删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int DeleteUser(int userId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            User user = Entity.Users.FirstOrDefault(p => p.Id == userId);
            user.Status = (int)Status.Deleted;

            return Entity.SaveChanges();
        }
        /// <summary>
        /// 根据医生id获取所以绑定的用户
        /// </summary>
        /// <param name="dId"></param>
        /// <returns></returns>
        public static List<User> GetUsersByDoctorId(int dId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.Where(p => p.BingDoctorId == dId&&p.Status == (int)Status.Normal).ToList();
        }

        /// <summary>
        /// 根据医生id获取所以绑定的用户
        /// </summary>
        /// <param name="dId"></param>
        /// <returns></returns>
        public static int GetUsersByDoctorIdCount(int dId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.Where(p => p.BingDoctorId == dId && p.Status == (int)Status.Normal).Count();
        }

        /// <summary>
        /// 根据医生id获取所以绑定的用户,分页
        /// </summary>
        /// <param name="dId"></param>
        /// <returns></returns>
        public static List<User> GetUsersByDoctorId(int dId, int pi, int pc)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            pi--;
            return Entity.Users.Where(p => p.BingDoctorId == dId && p.Status == (int)Status.Normal).OrderBy(p => p.Id).Skip(pi * pc).Take(pc).ToList();
        }
        /// <summary>
        /// 将用户从医生中解绑
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string UnBindUser(int userId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            User user = Entity.Users.FirstOrDefault(p => p.Id == userId);
            if (user != null)
                user.BingDoctorId = null;
            Entity.SaveChanges();
            return user.LoginName;
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static User EditPwd(int userId, string pwd)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            User user = Entity.Users.FirstOrDefault(p => p.Id == userId);
            user.Pwd = pwd;
            Entity.SaveChanges();
            return user;
        }
    }
}
