using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZZH.DapperExpression.Service.Data
{
    /// <summary>
    /// 事务提供者
    /// </summary>
    public interface IActiveTransactionProvider : IDisposable
    {
        IDbConnection GetActiveConnection();

        IDbTransaction GetActiveTransaction();
    }
}
