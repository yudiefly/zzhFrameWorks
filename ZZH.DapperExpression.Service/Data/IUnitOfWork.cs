using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZZH.DapperExpression.Service.Data
{
    /// <summary>
    /// 表示工作单元(事务相关）
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 提交事务
        /// </summary>
        Task CommitAsync();
    }
}
