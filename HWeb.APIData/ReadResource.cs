using HWeb.Entity.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.APIData
{
    public class ReadResource
    {
        public static string GetStr(string name)
        {
            try
            {
                string str = ApiStateToMessage.ResourceManager.GetString(name);
                if (string.IsNullOrWhiteSpace(str))
                    str = "未知错误";
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void ExecBack(BaseModel model, string str)
        {
            if (model == null)
            {
                model = new BaseModel();
                model.State = 1001;
                model.Message = ReadResource.GetStr(str + "_" + model.State);
            }
            else
                model.Message = ReadResource.GetStr(str + "_" + model.State);
            // dcm.State = 0;
        }
    }
}
