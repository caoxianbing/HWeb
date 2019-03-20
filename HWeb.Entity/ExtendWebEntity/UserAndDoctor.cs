using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Entity.WebEntity
{
    /// <summary>
    /// 医生用户信息类
    /// </summary>
    public class UserAndDoctor
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public int HeadImgId { get; set; }
        public string Position { get; set; }
        public string Hospital { get; set; }
        public string Subject { get; set; }
        public string PhoneNum { get; set; }
        public int UserTypeId { get; set; }
        public int Sex { get; set; }

     
    }
}
