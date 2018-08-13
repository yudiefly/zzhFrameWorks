using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZZH.MongoDB.Service.Test.Model;

namespace ZZH.MongoDB.Service.Test.Services
{
    /// <summary>
    /// 示例服务
    /// </summary>
    public class HelloWorldService
    {
        /// <summary>
        ///  MongoDB数据库配置
        /// </summary>
        const string MONGODB_CONFIG_SELECTION_KEY = "MongoLogConfig";
        /// <summary>
        /// 存储信息的表名
        /// </summary>
        const string PENDING_DATA_L_COLLECTION_NAME = "PendingData_L_HelloWorld_List";      

        /// <summary>
        /// 待处理列表
        /// </summary>
        private DoMongoRepostory<HelloWorldEntity> RepostoryL;      
     
        /// <summary>
        /// 初始化
        /// </summary>
        public HelloWorldService(MongoConfig mongoConfig)
        {
            RepostoryL = new DoMongoRepostory<HelloWorldEntity>(MONGODB_CONFIG_SELECTION_KEY, PENDING_DATA_L_COLLECTION_NAME, mongoConfig);          

        }
        /// <summary>
        /// 搜索待处理列表（支持简单的分页功能）
        /// </summary> 
        public Tuple<List<HelloWorldEntity>, int> GetPendingListL(int DctId, int pageIndex, int pageSize)
        {
            try
            {
                Expression<Func<HelloWorldEntity, bool>> filter = (p) => (p.DctId == DctId);
                var entity = RepostoryL.FindPagerList(filter, "{\"OrderByTime\" : 1}", pageIndex, pageSize);
                var count = RepostoryL.Count(filter);
                return new Tuple<List<HelloWorldEntity>, int>(entity, count);
            }
            catch (Exception ex)
            {
                return new Tuple<List<HelloWorldEntity>, int>(new List<HelloWorldEntity>(), 0);
            }
        }  


        /// <summary>
        /// 添加列表
        /// </summary>
        /// <param name="Entity"></param>
        public HelloWorldEventArgs AddOrReplacePendingDataL(HelloWorldEntity Entity)
        {
            //先从待处理列表中删除该该客户，然后再添加进去，这样就不会有重复了
            #region 添加待处理
            try
            {
                //先获取一个实体，主要为了保住OrderByTime字段（真是无奈）
                var pde = RepostoryL.FindOne(a => a.CustId == Entity.CustId);
                if (pde != null)
                {
                    Entity.OrderByTime = pde.OrderByTime;
                }
                //删除所有
                Expression<Func<HelloWorldEntity, bool>> filter = (p) => (p.CustId == Entity.CustId);
                RepostoryL.Remove(filter);
               
                RepostoryL.Add(Entity);
                return null;
            }
            catch (Exception ex)
            {
                return new HelloWorldEventArgs
                {
                    BussinessData = Entity,
                    ex = ex
                };
            }
            #endregion            
        }
             
        /// <summary>
        /// 从待处理列表移除客户的咨询列表条目
        /// </summary>
        /// <param name="CustomerID"></param>
        public HelloWorldEventArgs RemovePendingDataL(int CustomerID)
        {
            #region 改成同步的
            try
            {
                //判读该客户的记录是否存在,如果存在则跟新之，否则直接添加
                var pde = RepostoryL.FindOne(a => a.CustId == CustomerID);
                if (pde != null)
                {
                    RepostoryL.RemoveById(pde.Id);
                }
                return null;

            }
            catch (Exception ex)
            {
                return new HelloWorldEventArgs
                {
                    ex = ex,
                    BussinessData = null,
                    others = CustomerID
                };
            }
            #endregion

        }
       
        /// <summary>
        /// Clear
        /// </summary>
        public void ClearPendingDataL(List<int> Doctors = null)
        {
            #region Clear ALL
            try
            {
                if (Doctors != null)
                {
                    foreach (var doctor in Doctors)
                    {
                        Expression<Func<HelloWorldEntity, bool>> filter = (p) => (p.DctId == doctor);
                        RepostoryL.Remove(filter);
                    }
                }
                else
                {
                    Expression<Func<HelloWorldEntity, bool>> filter = (p) => (p.DctId > 0);
                    RepostoryL.Remove(filter);
                }

            }
            catch (Exception ex)
            {

            }
            #endregion
        }   
      
    }

    /// <summary>
    /// 返回事件类
    /// </summary>
    public class HelloWorldEventArgs
    {
        public object others { set; get; }
        public HelloWorldEntity BussinessData { set; get; }
        public Exception ex { set; get; }
    }
}
