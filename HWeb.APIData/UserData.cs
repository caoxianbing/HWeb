using HWeb.APIData.ParamModel;
using HWeb.Entity.APIModel;
using HWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData
{
    public class UserData
    {
        /// <summary>
        /// 登陆，成功返回用户信息
        ///  state 0. 表示请求正常，正确返回
        /// 1000. 表示账户不存在
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static LoginModel UserLogin(string loginName, string pwd)
        {
            LoginModel rm = new LoginModel();
            try
            {
                APILoginModel model = new APILoginModel();
                model.Name = loginName;
                model.Pass = pwd;
                model.LoginType = 0;
                rm = HttpApi.GetApiResult<LoginModel>("User/Login", model);
                ReadResource.ExecBack(rm, "Login");
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return new LoginModel {
                    State=-1,
                    Message=ex.Message
                };
            }
            return rm;
        }
        /// <summary>
        /// 获取用户的token
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string GetToken(string loginName, string pwd)
        {
            LoginModel rm = new LoginModel();
            try
            {
                APILoginModel model = new APILoginModel();
                model.Name = loginName;
                model.Pass = pwd;
                model.LoginType = 0;
                rm = HttpApi.GetApiResult<LoginModel>("User/Login", model);
             //   Tokens.Add(rm.Item.UserId, rm.AccessToken);
                return rm.AccessToken;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
                return "";
            }
        }

        /// <summary>
        /// 用户注册 接口返回状态
        /// 0. 注册成功
        /// 1001. 用户名已被注册
        /// 1002. 注册失败
        /// 1003. 用户名，密码不能为空
        /// 1100. 设备Imei码不存在 [仅在参数提供imei才会检测此项]
        /// 1102. 设备已被注册[仅在参数中提供了imei才会检测此项]
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pwd"></param>
        /// <param name="phone"></param>
        /// <param name="IMEI"></param>
        /// <returns></returns>
        public static RegisterModel Register(string loginName, string pwd, string phone, string IMEI = "")
        {
            try
            {
                APIRegisterModel model = new APIRegisterModel();
                model.Contact = loginName;
                model.Username = loginName;
                model.LoginName = loginName;
                model.ContactPhone = phone;
                model.SerialNumber = IMEI;
                model.Password = pwd;
                RegisterModel rm = HttpApi.GetApiResult<RegisterModel>("User/Register", model);
                ReadResource.ExecBack(rm, "Register");
                return rm;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
                return new RegisterModel
                {
                    State = -1,
                    Message = ex.Message
                };
            }
        }
    }
}
