using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace MQ.Consumer.Test
{
    public class ProxyDemo
    {
        public Tuple<bool,string> SendDoctorInfo(int doctorId, string doctorName, string introduce, string level, string phoneNumber, string serviceDeptName, string speciality, string headImgUrl)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("doctorId", doctorId);
            paramDic.Add("doctorName", doctorName);
            paramDic.Add("introduce", introduce);
            paramDic.Add("level", level != null ? level : "");
            paramDic.Add("phoneNumber", phoneNumber);
            paramDic.Add("serviceDeptName", serviceDeptName);
            paramDic.Add("speciality", speciality);
            paramDic.Add("headImgUrl", headImgUrl);
            bool success = true;
            //执行某些选项的业务操作
            var strJson = JsonConvert.SerializeObject(paramDic);
            Console.Write("DoctorInfo:{0}", strJson);
            return new Tuple<bool, string>(success,strJson);
        }
    }
}
