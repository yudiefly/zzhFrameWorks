using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using zipkin4net;
using zipkin4net.Propagation;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace ZZH.ZipKinClient.Service
{
    public class ZipKinClientHelper
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="zipkinServerUrl"></param>
        public static void Init(string strServiceName,string strZipkinServerUrl)
        {
            ServiceName = strServiceName;
            ZipKinServerUrl = strZipkinServerUrl;
        }
        /// <summary>
        /// 要注册的服务名
        /// </summary>
        public static string ServiceName { set; get; }
        /// <summary>
        /// ZipKin服务器地址（含端口号）
        /// </summary>
        private static string ZipKinServerUrl { set; get; }
        /// <summary>
        /// 向ZipKin服务器注册服务管道
        /// <paramref name="loggerFactory"/>
        /// <paramref name="strZipKinServerUrl">默认本机：http://localhost:9411</paramref>
        /// </summary>
        public static ResponseData<string> RegisterHandle(ILoggerFactory loggerFactory)
        {
            #region RegisterService
            try
            {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender(ZipKinServerUrl, "application/json");
                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer());
                TraceManager.RegisterTracer(tracer);
                TraceManager.Start(logger);
                return new ResponseData<string>
                {
                    Code = "200",
                    Messages = "Success",
                    Data = "注册完成"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    Data = "注册失败",
                    Code = "-100",
                    Messages = ex.Message
                };
            }
            #endregion
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public static void UnRegisterHandle()
        {
            TraceManager.Stop();
        }
        /// <summary>
        /// 注册服务
        /// </summary>
        public static void RegisterService(Func<HttpContext, string> getRpc = null)
        {
            getRpc = getRpc ?? (c => c.Request.Method);
            var extractor = Propagations.B3String.Extractor<IHeaderDictionary>((carrier, key) => carrier[key]);

            var request = ZipkinServiceHttpContext.Current.Request;
            var traceContext = extractor.Extract(request.Headers);

            var trace = traceContext == null ? Trace.Create() : Trace.CreateFromId(traceContext);
            Trace.Current = trace;

            using (var serverTrace = new ServerTrace(ServiceName, getRpc(ZipkinServiceHttpContext.Current)))
            {
                if (request.Host.HasValue)
                {
                    trace.Record(Annotations.Tag("http.host", request.Host.ToString()));
                }
                trace.Record(Annotations.Tag("http.uri", UriHelper.GetDisplayUrl(request)));
                trace.Record(Annotations.Tag("http.path", request.Path));
                trace.Record(Annotations.Tag("Execute.Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")));
                trace.Record(Annotations.Tag("Request.Body", JSONHelper.SerializeObject(request.Body)));
                serverTrace.AddAnnotation(Annotations.ServiceName(ServiceName));
            }
        }

    }
}
