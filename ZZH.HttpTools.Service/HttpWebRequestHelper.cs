using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ZZH.HttpTools.Service
{
    /// <summary>
    /// 最为普通的HttWebRequest的请求（Get/Post）
    /// </summary>
    public class HttpWebRequestHelper
    {
        /// <summary>
        /// 普通的HttpGet请求
        /// </summary>
        /// <param name="strURL">请求地址</param>
        /// <param name="headers">Header信息</param>
        /// <param name="TimeOut">超时：60秒（默认）</param>
        /// <returns></returns>
        public static string HttpGets(string strURL, Dictionary<string, string> headers = null, int TimeOut = 60)
        {
            #region Get
            StringBuilder rStr = new StringBuilder();
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strURL);
                request.Timeout = TimeOut * 1000;
                if (headers != null && headers.Count > 0)
                {
                    foreach (var each in headers)
                    {
                        request.Headers.Add(each.Key, each.Value);
                    }
                }
                //header中添加Basic验证
                //request.Headers.Add("Authorization", "Basic " + strAuthionSigns);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                rStr.Append(streamReader.ReadToEnd());
                streamReader.Close();
                response.Close();
            }
            catch (Exception Exp)
            {
                string errs = Exp.Message;
            }
            return rStr.ToString();
            #endregion
        }

        /// <summary>
        /// 普通的HttpPost请求
        /// </summary>
        public static string HttpPost(string strURL, string strRequestBody = "", Dictionary<string, string> headers = null, int TimeOut = 60)
        {
            string res_data = "";
            try
            {
                var postBytes = Encoding.UTF8.GetBytes(strRequestBody);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strURL);
                if (headers != null && headers.Count > 0)
                {
                    foreach (var each in headers)
                    {
                        request.Headers.Add(each.Key, each.Value);
                    }
                }
                //header中添加Basic验证
                //request.Headers.Add("Authorization", "Basic " + strAuthionSigns);
                request.Timeout = TimeOut * 1000;
                request.Method = "POST";
                request.ContentType = "Application/json";
                request.ContentLength = postBytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                res_data = streamReader.ReadToEnd();
                streamReader.Close();
                response.Close();

            }
            catch (Exception Exp)
            {
                string errs = Exp.Message;
            }
            return res_data;
        }
    }
}
