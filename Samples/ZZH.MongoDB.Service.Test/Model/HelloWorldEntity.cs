using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZZH.MongoDB.Service.Test.Model
{
    /// <summary>
    /// 咨询实体类（示例） 
    /// </summary>
    public class HelloWorldEntity : AggregateBase
    {
        public int DctId { set; get; }
        /// <summary>
        /// 客户ID   
        /// Description:      
        /// </summary>
        public int CustId
        {
            get;
            set;
        }  

        /// <summary>
        /// 客户昵称  
        /// Description: app客户昵称     
        /// </summary>
        public string Nickname
        {
            get;
            set;
        }

        /// <summary>
        /// 头像地址  
        /// Description: 头像地址     
        /// </summary>
        public string PhotoUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 最新问题标题 
        /// Description:      
        /// </summary>
        public string CTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 问题提交时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CommitOn
        {
            get;
            set;
        }
        /// <summary>
        /// 问题排序时间（仅用排序）
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime OrderByTime { set; get; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }      
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }  
    
    }
}
