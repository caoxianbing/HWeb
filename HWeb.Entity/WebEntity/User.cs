//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HWeb.Entity.WebEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string PhoneNum { get; set; }
        public string Pwd { get; set; }
        public Nullable<System.DateTime> RegisterTime { get; set; }
        public Nullable<int> HeadImgId { get; set; }
        public string Email { get; set; }
        public Nullable<int> UserTypeId { get; set; }
        public Nullable<int> Sex { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string UserInfo { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public string LastLoginIp { get; set; }
        public string PwdKey { get; set; }
        public string APIToken { get; set; }
        public Nullable<int> APIUserId { get; set; }
        public Nullable<int> BingDoctorId { get; set; }
        public Nullable<int> HeartMin { get; set; }
        public Nullable<int> HeartMax { get; set; }
        public Nullable<int> BloodMin { get; set; }
        public Nullable<int> BloodMax { get; set; }
    }
}
