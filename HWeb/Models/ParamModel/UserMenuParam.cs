using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWeb.Models.ParamModel
{
    public class UserMenuParam
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public int ParentId { get; set; }
        public int MenuLevel { get; set; }
        public string Icon { get; set; }
        public string MenuCode { get; set; }

    }
}