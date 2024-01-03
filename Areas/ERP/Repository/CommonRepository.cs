//using AllCodeLibrary;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Common;
using Dapper;
using Microsoft.Data.Sqlite;
////using Oracle.ManagedDataAccess.Client;
using System;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Repository
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

        internal async Task SystemAudit(RequestDetailDto rd, string strRemarks)
        {
            try
            {
                string strSql = "INSERT INTO ERP_CM_SYSTEM_AUDIT (SYS_USR_ID, HOST, OPERATION_TYPE, MODULE, REMARKS) VALUES (:SYS_USR_ID, :HOST, :OPERATION_TYPE, :MODULE, :REMARKS)";

                var parameter = new { SYS_USR_ID = rd.UserId, HOST = rd.ClientIPAddress, OPERATION_TYPE = rd.ActionName, MODULE = rd.ControllerName, REMARKS = strRemarks };

                using var conn = new SqliteConnection(_conString);
                //int executeRow = await conn.ExecuteAsync(strSql, parameter);
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> Upload_Images(string fileName, byte[] imagedata, string path)
        {
            _serviceHandler = new ServiceHandler();
            return await _serviceHandler.Upload_Images(fileName, imagedata, path);
        }

        internal async Task<byte[]> Get_Images(string fileName, string path)
        {
            _serviceHandler = new ServiceHandler();
            return await _serviceHandler.Get_Images(fileName, path);
        }

        internal async Task<string> GetDateStringFromUTC(string strDate, string strFormat)
        {
            _serviceHandler = new ServiceHandler();
            return await _serviceHandler.GetDateStringFromUTC(strDate, strFormat);
        }
    }
}
