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
    
    public partial class DeviceAndUser
    {
        public int DeviceId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> IconId { get; set; }
        public string Imei { get; set; }
        public string LoginName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> BingDoctorId { get; set; }
        public Nullable<int> APIDeviceId { get; set; }
        public string UserName { get; set; }
        public Nullable<int> Sex { get; set; }
    }
}
