using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ZZH.HttpTools.Service
{
    /// <summary>
    /// 可以处理Https的HttpRequest请求
    /// </summary>
    public class SHttpWebRequestHelper
    {
        /// <summary>
        /// 创建HttpWebRequest对象
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateHttpWebRequest(string url, int timeoutSecond = 90)
        {
            HttpWebRequest request;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.DefaultConnectionLimit = 1000;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Timeout = timeoutSecond * 1000;
            request.Proxy = null;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            return request;
        }
        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="dic">参数</param>
        /// <param name="headerDic">请求头参数</param>
        /// <returns></returns>
        public static string DoPost(string url, Dictionary<string, string> dic, Dictionary<string, string> headerDic, int timeoutSecond = 90)
        {
            HttpWebRequest request = CreateHttpWebRequest(url);
            request.Method = "POST";
            request.Accept = "*/*";
            request.ContentType = "application/json";
            if (headerDic != null && headerDic.Count > 0)
            {
                foreach (var item in headerDic)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            if (dic != null && dic.Count > 0)
            {
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dic);
                byte[] buffer = Encoding.UTF8.GetBytes(jsonStr);
                request.ContentLength = buffer.Length;
                try
                {
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }
            else
            {
                request.ContentLength = 0;
            }
            request.Timeout = timeoutSecond * 1000;
            return HttpResponse(request);
        }

        public static string DoPost(string url, Dictionary<string, string> dic, int timeoutSecond = 90)
        {
            return DoPost(url, dic, null,timeoutSecond);
        }

        public static string HttpResponse(HttpWebRequest request)
        {
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    var contentEncodeing = response.ContentEncoding.ToLower();

                    if (!contentEncodeing.Contains("gzip") && !contentEncodeing.Contains("deflate"))
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        #region gzip,deflate 压缩解压
                        if (contentEncodeing.Contains("gzip"))
                        {
                            using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    return reader.ReadToEnd();
                                }
                            }
                        }
                        else //if (contentEncodeing.Contains("deflate"))
                        {
                            using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    return reader.ReadToEnd();
                                }
                            }
                        }
                        #endregion gzip,deflate 压缩解压
                    }
                }               
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public static string DoGet(string url, Dictionary<string, string> dic, int timeoutSecond = 90)
        {
            try
            {
                var argStr = dic == null ? "" : dic.ToSortUrlParamString();
                argStr = string.IsNullOrEmpty(argStr) ? "" : ("?" + argStr);
                HttpWebRequest request = CreateHttpWebRequest(url + argStr);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = timeoutSecond * 1000;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string DoGet(string url, Dictionary<string, string> dic, Dictionary<string, string> headerDic)
        {
            try
            {
                var argStr = dic == null ? "" : dic.ToSortUrlParamString();
                argStr = string.IsNullOrEmpty(argStr) ? "" : ("?" + argStr);
                HttpWebRequest request = CreateHttpWebRequest(url + argStr);
                request.Method = "GET";
                request.ContentType = "application/json";
                foreach (var item in headerDic)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}
