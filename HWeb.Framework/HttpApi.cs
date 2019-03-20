using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Framework
{
    public static class HttpApi
    {

        public static string GetApiResult(string uri, string param)
        {
            string newurl = Config.ApiUrl + uri;

            return HttpPost(newurl, param);
        }

        public static T GetApiResult<T>(string uri, dynamic param) where T : class
        {
            try
            {
                var stop = new Stopwatch();
                stop.Start();
                string paramJson = JsonConvert.SerializeObject(param);
                var json = GetApiResult(uri, paramJson);
                stop.Stop();
                LogHelper.WriteLog(string.Format("调用:{0}, 参数：{1} \r\n耗时：{2} ms", uri, paramJson, stop.Elapsed.TotalMilliseconds));

                if (!string.IsNullOrEmpty(json))
                {
                    var res = JsonConvert.DeserializeObject<T>(json);
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
            }

            return null;
        }


        private static string HttpPost(string url, string postDataStr)
        {
            var res = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream myResponseStream = response.GetResponseStream();


                if (myResponseStream != null)
                {
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                    res = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();

                }
                myRequestStream.Close();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex);
            }
            return res;
        }

    }
}
