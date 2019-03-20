using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HWeb.Entity.WebEntity;
using HWeb.Framework;

namespace HWeb.Logic
{
    public class DoctorLogic : BaseLogic
    {
        /// <summary>
        /// 根据用户id获取医生信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Doctor GetDoctorByUserId(int userId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Doctors.FirstOrDefault(p => p.UserId == userId);
        }

        /// <summary>
        /// 根据医生账号验证该医生是否存在
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static User GetDoctorByLoginName(string loginName)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.FirstOrDefault(p => p.LoginName == loginName && p.UserTypeId == 3);
        }
        /// <summary>
        /// 保存医生信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int SaveDoctor(Doctor d)
        {
            try
            {
                WebDBEntities2 Entity = new WebDBEntities2();
                Doctor doctor = Entity.Doctors.FirstOrDefault(p => p.UserId == d.UserId);
                if (doctor != null)
                {
                    doctor.Hospital = d.Hospital;
                    doctor.DoctorInfo = d.DoctorInfo;
                    doctor.Subject = d.Subject;
                    doctor.Position = d.Position;
                }
                else
                {
                    Entity.Doctors.Add(d);
                }

                return Entity.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        /// 根据用户id删除医生
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int DeleteDoctorByUserId(int userId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            Doctor d = Entity.Doctors.FirstOrDefault(p => p.UserId == userId);
            Entity.Doctors.Remove(d);

            return Entity.SaveChanges();
        }

        /// <summary>
        /// 获取医生绑定的居民数量
        /// </summary>
        /// <param name="dId"></param>
        /// <returns></returns>
        public static int GetUserCountByDoctorId(int dId)
        {
            WebDBEntities2 Entity = new WebDBEntities2();
            return Entity.Users.Count(p => p.BingDoctorId == dId && p.Status == (int)Status.Normal);
        }

        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <returns></returns>
        public static List<UserAndDoctor> GetUserAndDoctors(int pi,int pc)
        {
            pi--;
            List<UserAndDoctor> users = new List<UserAndDoctor>();
            WebDBEntities2 Entity = new WebDBEntities2();
            var list = from u in Entity.Users
                       join d in Entity.Doctors
                       on u.Id equals d.UserId
                       where u.UserTypeId == 3
                       select new UserAndDoctor
                       {
                           Id = u.Id,
                           LoginName = u.LoginName,
                           UserName = u.UserName,
                           HeadImgId = u.HeadImgId.Value,
                           Position = d.Position,
                           Hospital = d.Hospital,
                           Subject = d.Subject,
                           PhoneNum = u.PhoneNum,
                           UserTypeId = u.UserTypeId.Value,
                           Sex = u.Sex.Value
                       };
            users = list.OrderBy(p => p.Id).Skip(pi * pc).Take(pc).ToList();
            return users;
        }

    }
}
