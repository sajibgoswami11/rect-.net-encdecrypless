//using AllCodeLibrary;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Common;
using Dapper;
using Microsoft.Data.Sqlite;
//using Oracle.ManagedDataAccess.Client;
using System;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.xEmptyArea.Repository
{
    public class CommonRepository
    {
        private readonly string _conString;
        private ServiceHandler _serviceHandler;
        private CryptoLibrary _cryptoLibrary;

        public CommonRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<string> CommonMessage()
        {
            _serviceHandler = new ServiceHandler();
            return await _serviceHandler.CommonMessage();
        }

        internal async Task<string> Encrypt(string value, string key)
        {
            _cryptoLibrary = new CryptoLibrary();
            return await Task.FromResult(_cryptoLibrary.Encrypt(value, key));
        }

        internal async Task<string> Decrypt(string value, string key)
        {
            _cryptoLibrary = new CryptoLibrary();
            return await Task.FromResult(_cryptoLibrary.Decrypt(value, key));
        }

        internal async Task SystemAudit(string strUserId, string strClientIpAddress, string strMethodName, string strClassName, string strRemarks)
        {
            try
            {
                string strSql = "INSERT INTO ERP_CM_SYSTEM_AUDIT (SYS_USR_ID, HOST, OPERATION_TYPE, MODULE, REMARKS) VALUES (:SYS_USR_ID, :HOST, :OPERATION_TYPE, :MODULE, :REMARKS)";

                var parameter = new { SYS_USR_ID = strUserId, HOST = strClientIpAddress, OPERATION_TYPE = strMethodName, MODULE = strClassName, REMARKS = strRemarks };

                using var conn = new SqliteConnection(_conString);
                int executeRow = await conn.ExecuteAsync(strSql, parameter);

                //return executeRow.ToString();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
    }
}
