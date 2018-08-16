using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.HttpTools.Service
{
    public static class ToolExtensions
    {
        public static string ToSortUrlParamString(this Dictionary<string,string> dic)
        {
            if (dic == null)
            {
                return null;
            }
            else
            {
                if(dic.Count==0)
                {
                    return null;
                }
                else
                {
                    StringBuilder str = new StringBuilder();
                    foreach(var each in dic)
                    {
                        str.Append(each.Key + "=" + each.Value + "&");
                    }
                    string result = str.ToString().TrimEnd('&');
                    return result;
                }
            }
        }
    }
}
