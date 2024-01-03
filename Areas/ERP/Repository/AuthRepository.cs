using BizWebAPI.Areas.ERP.Models;
using BizWebAPI.Common;
using Dapper;
//using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace BizWebAPI.Areas.ERP.Repository
{
    public class AuthRepository
    {
        private readonly string _conString;

        public AuthRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<SystemUser> Login(string strUsername, string strPassword)
        {
            try
            {
                //string strSql = "SELECT * FROM IB_ACCOUNT_LIST AL, IB_CLIENT_LIST CL WHERE AL.CLINT_ID = CL.CLINT_ID AND USER_ID = :USER_ID AND IB_GET_PIN(AL.ACCNT_ID,'IBANK000D2E170001') = :GET_PIN";

                //string strSql = " SELECT SU.*,UG.SYS_USR_GRP_TITLE, EL.* FROM ERP_CM_SYSTEM_USERS SU, ERP_CM_CMP_BRANCH CB, ERP_CM_COMPANY CO, ERP_CM_SYSTEM_USER_GROUP UG, ERP_PR_EMPLOYEE_LIST EL WHERE SU.CMP_BRANCH_ID = CB.CMP_BRANCH_ID AND CB.CMP_COMPANY_ID = CO.COMPANY_ID AND SU.SYS_USR_LOGIN_NAME = :USER_ID AND SU.SYS_USR_GRP_ID = UG.SYS_USR_GRP_ID AND SU.SYS_USR_ID =  EL.SYS_USR_ID  AND ERP_GET_CM_USER_PASS(SU.SYS_USR_ID,'admi0086A2C80001') = :GET_PIN ";
                string strSql = @"  SELECT SU.SYS_USR_LOGIN_NAME,SU.SYS_USR_GRP_ID,SU.SYS_USR_ID,SU.CMP_BRANCH_ID,SU.USER_ACTIVE, UG.SYS_USR_GRP_TITLE, EL.EMP_ID,EL.EMP_TITLE, EL.EMP_NAME,EL.EMP_EMAIL,EL.EMP_PIC  FROM ERP_CM_SYSTEM_USERS SU  left join ERP_CM_SYSTEM_USER_GROUP UG on SU.SYS_USR_GRP_ID = UG.SYS_USR_GRP_ID
                            left join ERP_PR_EMPLOYEE_LIST EL on SU.SYS_USR_ID = EL.SYS_USR_ID   WHERE SU.SYS_USR_LOGIN_NAME  = '" + strUsername + "' and SYS_USR_PASS ='" + strPassword + "' "; 
              
                var parameter = new { USER_ID = strUsername, GET_PIN = strPassword };
                using var conn = new SqliteConnection(_conString);
                var user = await conn.QueryAsync<SystemUser>(strSql, null);

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
