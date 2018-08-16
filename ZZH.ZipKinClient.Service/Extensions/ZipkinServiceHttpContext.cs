using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.ZipKinClient.Service
{
    public static class ZipkinServiceHttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        /// <summary>
        /// 获取HttpContent
        /// </summary>
        public static HttpContext Current
        {
            get
            {
                return _contextAccessor.HttpContext;
            }
        }
    }
}
