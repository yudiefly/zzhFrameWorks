using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZZH.DapperExpression.Service.Data
{
    public class ActiveTransactionProvider : IActiveTransactionProvider
    {
        private bool isOpen;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;

        public ActiveTransactionProvider(IDbConnection connection)
        {
            _dbConnection = connection;
        }

        public IDbConnection GetActiveConnection()
        {
            return _dbConnection;
        }

        public IDbTransaction GetActiveTransaction()
        {
            if (!isOpen)
            {
                _dbConnection.Open();
                isOpen = true;
            }

            if (_dbTransaction != null)
            {
                _dbTransaction = _dbConnection.BeginTransaction();
            }

            return _dbTransaction;
        }

        public void Dispose()
        {
            if (_dbTransaction != null)
            {
                _dbTransaction.Dispose();
                _dbTransaction = null;
            }

            if (_dbConnection != null)
            {
                _dbConnection.Dispose();
                _dbConnection = null;
            }
        }
    }
}
