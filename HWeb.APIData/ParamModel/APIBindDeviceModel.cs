using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData.ParamModel
{
    public class APIBindDeviceModel : APIBaseModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 设备 id
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 关系名
        /// </summary>
        public string RelationName { get; set; }
        /// <summary>
        /// 电话号码 此关系人员的电话号码
        /// </summary>
        public string RelationPhone { get; set; }
        /// <summary>
        /// 大分组ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 佩戴者姓名
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}
