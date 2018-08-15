using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace ZZH.ZipKinClient.Service
{
    public class JSONHelper
    {
        /// <summary>
        /// 序列化对象成JSON字符串
        /// </summary>
        /// <param name="value">序列化的对象</param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            string ret = string.Empty;
            try
            {
                ret = JsonConvert.SerializeObject(value);
            }
            catch (Exception exception)
            {
                return ret;
            }
            return ret;
        }

        /// <summary>
        /// 将JSON字符串序列化成对象
        /// </summary>
        /// <param name="value">JSON字符串</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// List转成json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToJson<T>(IList<T> list, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = list[0].GetType().Name;
            Json.Append("{\"" + jsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type = pi[j].GetValue(list[i], null).GetType();
                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));
                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            Json.Remove(0, 1);
            Json.Remove(Json.Length - 1, 1);
            return Json.ToString();
        }
        /// <summary>
        /// List转成json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string String2Json(String s)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];

                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + Convert.ToDateTime(str).ToShortDateString() + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }

            if (str.Length == 0)
                str = "\"\"";

            return str;
        }
    }
}
