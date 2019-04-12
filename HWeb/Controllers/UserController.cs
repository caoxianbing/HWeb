using HWeb.APIData;
using HWeb.Entity.APIModel;
using HWeb.Entity.WebEntity;
using HWeb.Framework;
using HWeb.Logic;
using HWeb.Models.ParamModel;
using HWeb.Models.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HWeb.Controllers
{
    public class UserController : BaseController
    {
        /// <summary>
        /// 登陆界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// ajax登陆提交
        /// </summary>
        /// <param name="loginName">账号</param>
        /// <param name="pwd">密码</param>
        /// <param name="curIp">当前ip</param>
        /// <returns></returns>
        public JsonResult AjaxLogin(string loginName, string pwd,string curIp)
        {
            Session[ConstVal.SessionIpStr] = curIp;
            string aesPwd = Encryption.AESEncrypt(pwd);
            BaseResult res = new BaseResult();
            bool isLogin = false;//是否登陆成功
            User _u = UserLogic.GetUserByLogin(loginName, aesPwd);
            if (_u == null)
            {
                res.State = State.Falid;
                res.Message = "账号或密码错误!";
                return Json(res);
            }
            isLogin = _u != null;
            if (_u.UserTypeId > 3)
            {
                //登陆设备系统
                LoginModel model = UserData.UserLogin(loginName, pwd);
                //登陆成功
                if (model.State == 0)
                {
                    //授权
                    ///FormsAuthentication.SetAuthCookie(loginName, false);
                //    AddAuth(_u.LoginName, _u.UserTypeId.ToString());
                    res.State = (int)State.Success;
                    _u.APIToken = model.AccessToken;
                    _u.APIUserId = model.Item.UserId;
                    Device d = DeviceLogic.GetDeviceByUserId(_u.Id);
                    if (d != null)
                        _u.DeviceId = d.Id;
                    isLogin = true;
                }
                else
                {
                    res.State = State.Falid;
                    res.Message = model.Message;
                    WriteLog("用户" + loginName + "登陆设备系统失败，state:" + model.State + "！");
                    return Json(res);
                }
            }

            //医生，管理员，超级管理员不用登陆设备系统
          //  isLogin = _u != null && _u.UserTypeId < 4;

            if (!isLogin)
            {
                res.State = State.Falid;
                res.Message = "账号或密码错误!";
            }
            else
            {
                //更新用户登录记录
                _u.LastLoginTime = DateTime.Now;
                _u.LastLoginIp = GetIP();
                UserLogic.UpdateUser(_u);
                WriteLog("用户" + loginName + "登陆设备系统成功！");
                AuthUser = _u;//存session
            }

            SaveUserLog(loginName + "登陆" + (isLogin ? "成功" : "失败"), isLogin ? LogLevel.Info : LogLevel.Sensitive, loginName, "AjaxLogin", res.Message);
            return Json(res);
        }

        /// <summary>
        /// 暂时未用
        /// </summary>
        /// <param name="uname"></param>
        /// <param name="userRole"></param>
        void AddAuth(string uname, string userRole)
        {
            //FormsAuthentication.SetAuthCookie(uname,true);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                (1,
                    uname,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(60),
                    true,
                    userRole,
                    "/"
                );
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            cookie.HttpOnly = true;
            HttpContext.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 注册提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult AjaxRegister(RegisterParam param)
        {
            BaseResult res = new BaseResult();
            bool isRegister = true;//是否注册成功
            //检查用户是否存在
            if (UserLogic.ExistsLoginName(param.LoginName))
            {
                res.State = State.ExistsLoginName;
                res.Message = "用户名已存在！";
                isRegister = false;
            }
            else if (UserLogic.ExistsPhoneNum(param.Phone))
            {
                res.State = State.NumIsRegister;
                res.Message = "手机号码已被注册！";
                isRegister = false;
            }

            if (isRegister)
            {
                //先去设备平台注册
                RegisterModel rm = UserData.Register(param.LoginName, param.Pwd, param.Phone, param.IMEI);
                if (rm.User == null)
                {//注册失败直接返回
                    res.State = State.Falid;
                    res.Message = rm.Message;
                    isRegister = false;
                    return Json(res);
                }

                User _u = new User();
                _u.UserName = param.LoginName;
                _u.LoginName = param.LoginName;
                _u.Pwd = Encryption.AESEncrypt(param.Pwd);
                _u.RegisterTime = DateTime.Now;
                _u.PhoneNum = param.Phone;
                _u.Status = (int)Status.Normal;
                _u.HeadImgId = ConstVal.DefaultHeadImgId;
                _u.UserTypeId = 4;//居民
                _u.APIUserId = rm.User.UserId;
                _u.UserInfo = "";
                _u.Sex = 1;
                int i = UserLogic.SaveUser(_u);

                if (i > 0)
                {
                    res.State = State.Success;
                    res.Message = "注册成功！";
                    if (!string.IsNullOrWhiteSpace(param.IMEI))
                    {

                        //检查设备
                        DeviceCheckModel dcm = DeviceData.CheckDevice(param.IMEI, _u.APIUserId ?? 0);

                        //添加设备表
                        Device d = new Device
                        {
                            UserId = _u.Id,
                            IconId = 3,
                            Created = DateTime.Now,
                            Imei = param.IMEI,
                            Status = (int)Status.Normal,
                            APIDeviceId = dcm.DeviceId,
                            APIDeviceModel = dcm.Model
                        };
                        DeviceLogic.SaveDevice(d);
                    }
                }
                else
                {
                    res.State = State.Falid;
                    res.Message = "注册失败！";
                    isRegister = false;
                }
            }
            //记录操作日志
            SaveUserLog("注册账号" + (isRegister ? "成功" : "失败") + ":" + res.Message, LogLevel.Info, param.LoginName, "AjaxRegister", "注册账号");
            return Json(res);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
       // [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            AuthUser = null;
            return Redirect("/User/Login");
        }

        /// <summary>
        ///根据用户类型获取用户列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult AjaxGetUsers(LayUIPageParam param)
        {

            List<User> users = new List<User>();
            LayuiPageResult<User> ls = new LayuiPageResult<User>();
            //医生单独取
            if (param.type == 3)
            {
                List<UserAndDoctor> uad = DoctorLogic.GetUserAndDoctors(param.page, param.limit);
                LayuiPageResult<UserAndDoctor> lsd = new LayuiPageResult<UserAndDoctor>();
                lsd.data = uad;
                lsd.code = 0;
                lsd.count = UserLogic.GetUserCountByType(param.type);
                return Json(lsd);
            }
            else
            {
                //医生登录只取绑定了自己的居民
                if (AuthUser.UserTypeId == 3 && param.type == 4)
                {
                    users = UserLogic.GetUsersByDoctorId(AuthUser.Id, param.page, param.limit);
                    ls.count = UserLogic.GetUsersByDoctorIdCount(AuthUser.Id);
                }
                else
                {
                    users = UserLogic.GetUserByType(param.type, param.page
               , param.limit);
                    ls.count = UserLogic.GetUserCountByType(param.type);
                }
            }
           
            ls.data = users;
            ls.code = 0;

            return Json(ls);
        }


        /// <summary>
        /// 医生首页取居民信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult AjaxGetUsersByDoctor(LayUIPageParam param)
        {
            List<User> users = new List<User>();
            users = UserLogic.GetUsersByDoctorId(AuthUser.Id, param.page, param.limit);

            users.ForEach(p =>
            {
                Device d = DeviceLogic.GetDeviceByUserId(p.Id);
                if (d != null)
                {
                    HealthInfoModel model = HealthData.GetHealthInfo(d.APIDeviceId ?? 0);
                    p.Step = model.Step;
                    p.Heart = model.HeartRate;
                    p.Shrink = model.BloodMax;
                    p.Diastolic = model.BloodMin;
                    p.LastCheckTime = model.LastUpdateTime;
                }
                p.HeartValue = p.HeartMin + "-" + p.HeartMax;
                p.BloodValue = p.BloodMin + "-" + p.BloodMax;
                p.BloodValue2 = p.BloodMin2 + "-" + p.BloodMax2;

            });

            LayuiPageResult<User> ls = new LayuiPageResult<User>();
            ls.count = UserLogic.GetUsersByDoctorIdCount(AuthUser.Id);
            ls.data = users;
            ls.code = 0;
            return Json(ls);

        }


        /// <summary>
        /// 管理员修改用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult EditUserByManage(int userId)
        {
            User u = UserLogic.GetUserById(userId);
            //居民
            if (u.UserTypeId == 4)
            {
                if ((u.BingDoctorId ?? 0) > 0) {
                    u.BindDoctorName = UserLogic.GetUserById(u.BingDoctorId.Value).LoginName;
                }
                Device d = DeviceLogic.GetDeviceByUserId(userId);
                if (d != null)
                    u.IMEI = d.Imei;
            }
            ViewBag.User = u;
            ViewBag.CurUser = AuthUser;
            //如果是医生，则在查询医生信息
            if (u.UserTypeId == 3)
                ViewBag.Doctor = DoctorLogic.GetDoctorByUserId(userId);
            return View();
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>
        public JsonResult AjaxEditUser(User user)
        {
            BaseResult res = new BaseResult();
            //验证医生是否存在
            if (!string.IsNullOrWhiteSpace(user.BindDoctorName))
            {
                User dU = DoctorLogic.GetDoctorByLoginName(user.BindDoctorName);
                if (dU == null)
                {
                    res.State = State.Falid;
                    string msg = "医生不存在";
                    res.Message = msg;
                    SaveUserLog(AuthUser.LoginName + "修改" + user.LoginName + "的信息失败：" + msg, LogLevel.Error, AuthUser.LoginName, "AjaxEditUser", "修改用户信息");
                    return Json(res);

                }
                else
                {
                    user.BingDoctorId = dU.Id;
                    SaveUserLog(AuthUser.LoginName + "为" + user.LoginName + "的绑定了医生：" + dU.LoginName, LogLevel.Error, AuthUser.LoginName, "AjaxEditUser", "绑定医生");

                }
            }

           
            //保存用户信息
            UserLogic.SaveUser(user);
            res.State = State.Success;
            res.Message = "保存成功";
            SaveUserLog(AuthUser.LoginName + "修改" + user.LoginName + "信息成功", LogLevel.Sensitive, AuthUser.LoginName, "AjaxEditUser", "修改用户信息");
            return Json(res);
        }
        /// <summary>
        /// 医生绑定居民
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public JsonResult AjaxBindUserToDoctor(string loginName)
        {
            BaseResult res = new BaseResult();
            //验证医生是否存在
            if (!string.IsNullOrWhiteSpace(loginName))
            {
                if (AuthUser.UserTypeId != 3)
                {
                    res.State = State.Falid;
                    res.Message = "请用医生账号绑定居民";
                    return Json(res);
                }
                User u = UserLogic.GetUserByLoginName(loginName);

                if (u == null||u.UserTypeId!=4)
                {
                    res.State = State.Falid;
                    string msg = "用户不存在或不是居民！";
                    res.Message = msg;
                    SaveUserLog(AuthUser.LoginName + "绑定居民" + loginName + "失败：" + msg, LogLevel.Error, AuthUser.LoginName, "AjaxBindUserToDoctor", "绑定居民");
                    return Json(res);
                }
                else
                {
                    if (u.BingDoctorId > 0)
                    {
                        res.State = State.Falid;
                        string msg = "该居民已绑定医生！";
                        res.Message = msg;
                        SaveUserLog(AuthUser.LoginName + "绑定居民" + loginName + "失败：" + msg, LogLevel.Error, AuthUser.LoginName, "AjaxBindUserToDoctor", "绑定居民");
                        return Json(res);
                    }

                    res.State = State.Success;
                    res.Message = "绑定成功！";
                    u.BingDoctorId = AuthUser.Id;
                    SaveUserLog(AuthUser.LoginName + "为" + u.LoginName + "的绑定了医生：" + AuthUser.LoginName, LogLevel.Error, AuthUser.LoginName, "AjaxBindUserToDoctor", "绑定居民");
                    UserLogic.SaveUser(u);

                }
            }
            return Json(res);
        }

        /// <summary>
        /// 修改医生信息
        /// </summary>
        /// <returns></returns>
        public JsonResult AjaxEditDoctor(User user)
        {
            BaseResult res = new BaseResult();
            Doctor d = new Doctor
            {
                Hospital = user.Hospital,
                Subject = user.Subject,
                DoctorInfo = user.DoctorInfo,
                UserId = user.Id,
                Position = user.Position
            };
            //保存医生信息
            DoctorLogic.SaveDoctor(d);
            UserLogic.SaveUser(user);
            res.State = State.Success;
            res.Message = "保存成功";
            SaveUserLog(AuthUser.LoginName + "修改医生(" + user.LoginName + ")信息成功", LogLevel.Sensitive, AuthUser.LoginName, "AjaxEditDoctor", "修改医生信息");
            return Json(res);
        }
        /// <summary>
        /// 添加医生
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult AjaxSubmitDoctor(User user)
        {
            BaseResult res = new BaseResult();

            //检查账号是否存在
            if (UserLogic.ExistsLoginName(user.LoginName))
            {
                res.State = State.ExistsLoginName;
                res.Message = "该账号已存在！";
                return Json(res);
            }

            Doctor d = new Doctor
            {
                Hospital = user.Hospital,
                Subject = user.Subject,
                DoctorInfo = user.DoctorInfo,
                Position = user.Position
            };
            user.UserTypeId = 3;
            user.Status = 0;
            user.Pwd = Encryption.AESEncrypt("123456");
            user.RegisterTime = DateTime.Now;
            user.HeadImgId = 1;
            //保存医生信息

            UserLogic.SaveUser(user);
            d.UserId = user.Id;
            DoctorLogic.SaveDoctor(d);
            res.State = State.Success;
            res.Message = "保存成功";
            SaveUserLog(AuthUser.LoginName + "添加医生(" + user.LoginName + ")成功", LogLevel.Sensitive, AuthUser.LoginName, "AjaxSubmitDoctor", "添加医生");
            return Json(res);
        }
        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult AJaxAddAdmin(User user)
        {
            BaseResult res = new BaseResult();
            //检查账号是否存在
            if (UserLogic.ExistsLoginName(user.LoginName))
            {
                res.State = State.ExistsLoginName;
                res.Message = "该账号已存在！";
                return Json(res);
            }
            user.UserTypeId = 2;
            user.Status = 0;
            user.Pwd = Encryption.AESEncrypt(user.Pwd);
            user.RegisterTime = DateTime.Now;
            user.HeadImgId = 1;
            //保存医生信息

            UserLogic.SaveUser(user);
            res.State = State.Success;
            res.Message = "保存成功";
            SaveUserLog(AuthUser.LoginName + "添加管理员(" + user.LoginName + ")成功", LogLevel.Sensitive, AuthUser.LoginName, "AjaxSubmitDoctor", "添加管理员");
            return Json(res);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult DeleteUser(int userId)
        {
            BaseResult res = new BaseResult();
            User user = UserLogic.GetUserById(userId);
            //用户不存在
            if (user == null) {
                res.State = State.NotFind;
                res.Message = "该用户已经不存在了";
                return Json(res);
            }
            //超管不能被删除
            if (user.UserTypeId == 1)
            {
                res.State = State.NoPower;
                res.Message = "该账号不能被删除";
                return Json(res);
            }
            //删除医生
            if (user.UserTypeId == 3)
            {
                int count = DoctorLogic.GetUserCountByDoctorId(user.Id);
                if (count > 0)
                {
                    res.State = State.Falid;
                    res.Message = "医生还有绑定的居民，不能被删除！";
                    return Json(res);
                }
                else
                    DoctorLogic.DeleteDoctorByUserId(user.Id);
            }
            //删除用户
            if (user.UserTypeId == 4)
            {
                //医生登录删除用户为解绑
                if (AuthUser.UserTypeId == 3)
                {
                    UserLogic.UnBindUser(userId);
                    res.State = State.Success;
                    res.Message = "成功解绑用户 " + user.LoginName;
                }
                else
                {
                    Device d = DeviceLogic.GetDeviceByUserId(userId);
                    //删除用户要先去设备平台解绑用户绑定的设备
                    if (d != null)
                    {
                        //调用接口
                    }
                }
            }

            UserLogic.DeleteUser(userId);
            SaveUserLog(AuthUser.LoginName + "删除" + WebHelper.GetUTName(user.UserTypeId.Value) + "(" + user.LoginName + ")成功", LogLevel.Sensitive, AuthUser.LoginName, "DeleteUser", "删除用户");
            return Json(res);
        }
        /// <summary>
        ///根据医生id获取所以绑定的用户
        /// </summary>
        /// <param name="dId"></param>
        /// <returns></returns>
        public JsonResult AjaxGetUsersByDoctorId(int dId)
        {
            LayuiPageResult<User> res = new LayuiPageResult<User>();
            res.data = UserLogic.GetUsersByDoctorId(dId);
            res.code = 0;
            return Json(res);
        }

        /// <summary>
        /// 将用户从居民中解绑
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult UnBindUser(int userId)
        {
            BaseResult res = new BaseResult();
            string name=UserLogic.UnBindUser(userId);
            res.State = State.Success;
            SaveUserLog(AuthUser.LoginName + "解绑用户(" + name + ")成功", LogLevel.Sensitive, AuthUser.LoginName, "UnBindUser", "解绑用户");
            return Json(res);
        }
        /// <summary>
        /// 修改当前用户密码
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public JsonResult EditPwd(string oldPwd, string newPwd)
        {
            BaseResult res = new BaseResult();
            if (AuthUser.Pwd == Encryption.AESEncrypt(oldPwd))
            {
                UserLogic.EditPwd(AuthUser.Id, Encryption.AESEncrypt(newPwd));
                res.State = State.Success;
                SaveUserLog(AuthUser.LoginName + "修改密码成功", LogLevel.Sensitive, AuthUser.LoginName, "EditPwd", "修改密码");
            }
            else
            {
                res.State = State.PwdFaild;
                res.Message = "原密码不正确！";
                SaveUserLog(AuthUser.LoginName + "修改密码失败：" + res.Message, LogLevel.Sensitive, AuthUser.LoginName, "EditPwd", "修改密码");
            }

            return Json(res);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult ResetPwd(int userId)
        {
            BaseResult res = new BaseResult();
            if (AuthUser.UserTypeId <= 2)
            {
                string pwd = Encryption.AESEncrypt("123456");
                User user = UserLogic.EditPwd(userId, pwd);
                res.State = State.Success;
                SaveUserLog(AuthUser.LoginName + "重置" + user.LoginName + "的密码成功", LogLevel.Sensitive, AuthUser.LoginName, "ResetPwd", "重置密码");
            }
            else
            {
                res.State = State.NoPower;
                res.Message = "你暂无权限重置用户密码！";
                SaveUserLog(AuthUser.LoginName + "重置用户id为" + userId + "的密码失败：" + res.Message, LogLevel.Error, AuthUser.LoginName, "ResetPwd", "重置密码");
            }
            return Json(res);
        }

        /// <summary>
        /// 用户个人资料
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {
            User user = UserLogic.GetUserById(AuthUser.Id);
            Device _d = DeviceLogic.GetDeviceByUserId(user.Id);
            //根据用户查设备
            if (_d != null)
                user.IMEI = _d.Imei;
            //根据用户查绑定医生
            if ((user.BingDoctorId ?? 0) > 0)
            {
                User dU = UserLogic.GetUserById(user.BingDoctorId.Value);
                if (dU != null)
                    user.BindDoctorName = dU.LoginName;
            }
            ViewBag.User = user;
            if (AuthUser.UserTypeId == 3)
                ViewBag.doctor = DoctorLogic.GetDoctorByUserId(AuthUser.Id);
            return View();
        }
        /// <summary>
        /// 根据设备获取绑定用户
        /// </summary>
        /// <param name="deviceId">设备id</param>
        /// <returns></returns>
        public JsonResult AJaxGetUserInfo(int deviceId)
        {
            User u = UserLogic.GetUserByDeviceId(deviceId);
            if (u != null)
                return Json(new { State = State.Success, Data = u });
            else
                return Json(new { State = State.NotFind, Message = "该设备未绑定居民！" });
        }
    }
}