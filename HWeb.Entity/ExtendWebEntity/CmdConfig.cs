using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace HWeb.Entity.WebEntity
{
    public partial class CmdConfig
    {
        /// <summary>
        /// 医生账号
        /// </summary>
        [NotMapped]
        public string CmdValue { get; set; }
    }
}
