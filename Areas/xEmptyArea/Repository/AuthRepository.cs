using BizWebAPI.Areas.xEmptyArea.Models;
using BizWebAPI.Common;
using Dapper;
using Microsoft.Data.Sqlite;
//using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.xEmptyArea.Repository
{
    public class AuthRepository
    {
        private readonly string _conString;

        public AuthRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<AccountList> Login(string strUsername, string strPassword)
        {
            try
            {
                string strSql = "SELECT * FROM IB_ACCOUNT_LIST AL, IB_CLIENT_LIST CL WHERE AL.CLINT_ID = CL.CLINT_ID AND USER_ID = :USER_ID AND IB_GET_PIN(AL.ACCNT_ID,'IBANK000D2E170001') = :GET_PIN";

                var parameter = new { USER_ID = strUsername, GET_PIN = strPassword };

                using var conn = new SqliteConnection(_conString);
                var user = await conn.QueryAsync<AccountList>(strSql, parameter);

                return user.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
    }
}
