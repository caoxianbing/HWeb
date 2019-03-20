using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Framework
{
    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum Status {
        /// <summary>
        /// 正常
        /// </summary>
        Normal=0,
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted=1

    }
    /// <summary>
    /// 操作记录
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 正常记录
        /// </summary>
        Info = 1,
        /// <summary>
        /// 敏感记录
        /// </summary>
        Sensitive = 2,
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 3,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 4
    }
    /// <summary>
    /// 返回状态
    /// </summary>
    public enum State
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 失败
        /// </summary>
        Falid = 1,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 2,
        /// <summary>
        /// 用户名已存在
        /// </summary>
        ExistsLoginName = 3,
        /// <summary>
        /// 图片未找到
        /// </summary>
        NoImg = 4,
        /// <summary>
        /// 没有权限
        /// </summary>
        NoPower = 5,
        /// <summary>
        /// 没有找到
        /// </summary>
        NotFind = 6,
        /// <summary>
        /// 旧密码不正确
        /// </summary>
        PwdFaild = 7,
        /// <summary>
        /// 手机号已被注册
        /// </summary>
        NumIsRegister = 8,
        /// <summary>
        /// 设备已存在
        /// </summary>
        DeviceIn = 9
    }

    /// <summary>
    /// 图片类型
    /// </summary>
    public enum ImgType
    {
        /// <summary>
        /// 头像
        /// </summary>
        HeadImg = 1,
        /// <summary>
        /// 背景图片
        /// </summary>
        BackImg = 2
    }
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserTypeEnum
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperAdmin = 1,
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 2,
        /// <summary>
        /// 医生
        /// </summary>
        Doctor = 3,
        /// <summary>
        /// 居民
        /// </summary>
        Person = 4
    }
}
