using BizWebAPI.Areas.ERP.Models.TaskManagement;
using BizWebAPI.Common;
using Dapper;
using Microsoft.Data.Sqlite;
//using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Repository.TaskManagement
{
    public class EmployeeListRepository
    {
        private readonly string _conString;

        public EmployeeListRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

         internal async Task<IEnumerable<PrEmployeeList>> GetEmployees(string userId)
        {
            try
            {

                string strSql = @"select  EMP_NAME  AS ENAME, el.*  
                                from erp_pr_employee_list el  where el.CMP_BRANCH_ID = (select CMP_BRANCH_ID from ERP_CM_SYSTEM_USERS where sys_usr_id =  :sys_usr_id) ";
                using var conn = new SqliteConnection(_conString);
                var param = new { sys_usr_id = userId };
                var employee = await conn.QueryAsync<PrEmployeeList>(strSql, param);

                return employee;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<PrEmployeeList> GetEmployeeById(string empId,string userId)
        {
            try
            {
                string strSql = "";
                if (!string.IsNullOrEmpty(empId))
                {
                    strSql = "select EMP_NAME  AS ENAME, el.* " +
                               "  from erp_pr_employee_list el where el.EMP_ID = :EMP_ID  ";
                }
                if (!string.IsNullOrEmpty(userId))
                {

                    strSql = "select  EMP_NAME  AS ENAME, el.* " +
                                "  from erp_pr_employee_list el where el.SYS_USR_ID = :SYS_USR_ID  ";

                }
                var parameter = new { EMP_ID = empId, SYS_USR_ID = userId };
                using var conn = new SqliteConnection(_conString);
                var employee = await conn.QuerySingleAsync<PrEmployeeList>(strSql, parameter);

                return employee;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }  
        
        internal async Task<PrEmployeeList> GetEmploywiseProjectModuleTaskById(string empId,string userId)
        {
            try
            {
                string strSql = "";
                if (!string.IsNullOrEmpty(empId))
                {
                    strSql = "select  module_id,taskList_id " +
                               "  from  el where el.EMP_ID = :EMP_ID  ";
                }
                if (!string.IsNullOrEmpty(userId))
                {

                    strSql = "select  CAST(nvl(EMP_TITLE, '') AS VARCHAR(100)) || ' ' || CAST(EMP_NAME AS VARCHAR(500))|| '' ||EMP_CODE AS ENAME, el.* " +
                                "  from erp_pr_employee_list el where el.SYS_USR_ID = :SYS_USR_ID  ";

                }
                var parameter = new { EMP_ID = empId, SYS_USR_ID = userId };
                using var conn = new SqliteConnection(_conString);
                var employee = await conn.QuerySingleAsync<PrEmployeeList>(strSql, parameter);

                return employee;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<PrEmployeeList>> GetEmployeeByProjectModule(string decrypProjectId, string decrypModuleId)
        {
            string strSql = "";
            try
            {
                if (!string.IsNullOrEmpty(decrypProjectId) && string.IsNullOrEmpty(decrypModuleId))
                {
                    strSql = @" SELECT DISTINCT ETEM.EMP_ID, EL.EMP_NAME FROM ERP_TASK_EMP_TEAM_MAPPING ETEM left join ERP_TASK_PROJECT TP on ETEM.PROJECT_ID = TP.PROJECT_ID 
                           left join ERP_PR_EMPLOYEE_LIST EL ON EL.EMP_ID=  ETEM.EMP_ID where TP.PROJECT_ID = :PROJECT_ID   and TP.ISDELETE = '0' and ETEM.ISDELETE ='0' ";

                }
                else if (!string.IsNullOrEmpty(decrypProjectId) && !string.IsNullOrEmpty(decrypModuleId))
                {
                    strSql = @"SELECT ETEM.EMP_ID, EL.EMP_NAME FROM ERP_TASK_EMP_TEAM_MAPPING ETEM left join ERP_TASK_PROJECT TP on ETEM.PROJECT_ID = TP.PROJECT_ID 
                           left join ERP_TASK_MODULE ETM ON ETM.MODULE_ID = ETEM.MODULE_ID left join ERP_PR_EMPLOYEE_LIST EL ON EL.EMP_ID=  ETEM.EMP_ID
                           where TP.PROJECT_ID = :PROJECT_ID  and ETM.MODULE_ID = :MODULE_ID  and TP.ISDELETE = '0'  and ETEM.ISDELETE ='0' ";
                } 
                else if (string.IsNullOrEmpty(decrypProjectId) && !string.IsNullOrEmpty(decrypModuleId))
                {
                    strSql = @"SELECT ETEM.EMP_ID, EL.EMP_NAME FROM ERP_TASK_EMP_TEAM_MAPPING ETEM 
                            left join ERP_TASK_MODULE ETM ON ETM.MODULE_ID = ETEM.MODULE_ID left join ERP_PR_EMPLOYEE_LIST EL ON EL.EMP_ID=  ETEM.EMP_ID
                           where ETM.MODULE_ID = :MODULE_ID AND etm.ISDELETE='0' and ETEM.ISDELETE ='0' ";
                }

                var parameter = new { PROJECT_ID = decrypProjectId, MODULE_ID = decrypModuleId };
                using var conn = new SqliteConnection(_conString);
                var getEmployee = await conn.QueryAsync<PrEmployeeList>(strSql, parameter);

                return getEmployee;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<PrEmployeeList>> CreateEmployee(string registerName,string empCode,string emailAddress, string sysUserId,string branchId, 
                    string permanentAddress,string presentAddress,string contactNumber,string nidCard,string imagePath,string dptId,string dsgId, string accountNo,string joiningDate,string birthDay)
        {
            string strSql = "";
            //branchId = "200511239";
            try
            {
                strSql = "INSERT INTO ERP_PR_EMPLOYEE_LIST (EMP_NAME, SYS_USR_ID,EMP_CODE,EMP_EMAIL, CMP_BRANCH_ID,EMP_JOINING_DATE,EMP_PRE_ADDRES,EMP_PER_ADDRESS,EMP_CONTACT_NUM,DPT_ID,DSG_ID,EMP_NATIONAL_ID,EMP_BIRTHDAY,EMP_BANK_ACC_NO,EMP_PIC ) " +
                    " VALUES (:EMP_NAME,:SYS_USR_ID,:EMP_CODE,:EMP_EMAIL, :CMP_BRANCH_ID,:EMP_JOINING_DATE,:EMP_PRE_ADDRES,:EMP_PER_ADDRESS,:EMP_CONTACT_NUM,:DPT_ID,:DSG_ID,:EMP_NATIONAL_ID,:EMP_BIRTHDAY,:EMP_BANK_ACC_NO,:EMP_PIC ) ";

                var parameter = new { EMP_NAME = registerName, SYS_USR_ID = sysUserId ,EMP_CODE = empCode, EMP_EMAIL = emailAddress, CMP_BRANCH_ID = branchId, EMP_PER_ADDRESS= permanentAddress, EMP_PRE_ADDRES=presentAddress, EMP_CONTACT_NUM=contactNumber, EMP_NATIONAL_ID=nidCard, EMP_PIC=imagePath, DPT_ID=dptId,DSG_ID=dsgId, EMP_BANK_ACC_NO= accountNo, EMP_JOINING_DATE= joiningDate, EMP_BIRTHDAY= birthDay };
                using var conn = new SqliteConnection(_conString);
                var employeeInsert = await conn.QueryAsync<PrEmployeeList>(strSql, parameter);

                return employeeInsert;
            }
            catch(Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
        
        internal async Task<IEnumerable<PrEmployeeList>> UpdateEmployee(string registerName, string empCode, string emailAddress, string sysUserId, string branchId,
                    string permanentAddress, string presentAddress, string contactNumber, string nidCard, string imagePath, string dptId, string dsgId, string accountNo, string joiningDate, DateTime birthDay,string empId )
        {
            string strSql = "";
            try
            {
                if (!string.IsNullOrEmpty(imagePath)){
                    strSql = "Update ERP_PR_EMPLOYEE_LIST set EMP_NAME=:EMP_NAME, SYS_USR_ID=:SYS_USR_ID ,EMP_CODE=:EMP_CODE ,EMP_PIC=:EMP_PIC , " +
                                       " EMP_EMAIL= :EMP_EMAIL, CMP_BRANCH_ID= :CMP_BRANCH_ID, EMP_JOINING_DATE=:EMP_JOINING_DATE,EMP_PRE_ADDRES=:EMP_PRE_ADDRES," +
                                       "EMP_PER_ADDRESS=:EMP_PER_ADDRESS,EMP_CONTACT_NUM=:EMP_CONTACT_NUM,DPT_ID=:DPT_ID,DSG_ID=:DSG_ID,EMP_NATIONAL_ID=:EMP_NATIONAL_ID," +
                                       "EMP_BIRTHDAY=:EMP_BIRTHDAY,EMP_BANK_ACC_NO=:EMP_BANK_ACC_NO where EMP_ID=:EMP_ID ";

                }
                else
                {
                    strSql = "Update ERP_PR_EMPLOYEE_LIST set EMP_NAME=:EMP_NAME, SYS_USR_ID=:SYS_USR_ID ,EMP_CODE=:EMP_CODE, " +
                   " EMP_EMAIL= :EMP_EMAIL, CMP_BRANCH_ID= :CMP_BRANCH_ID, EMP_JOINING_DATE=:EMP_JOINING_DATE,EMP_PRE_ADDRES=:EMP_PRE_ADDRES," +
                   "EMP_PER_ADDRESS=:EMP_PER_ADDRESS,EMP_CONTACT_NUM=:EMP_CONTACT_NUM,DPT_ID=:DPT_ID,DSG_ID=:DSG_ID,EMP_NATIONAL_ID=:EMP_NATIONAL_ID," +
                   "EMP_BIRTHDAY=:EMP_BIRTHDAY,EMP_BANK_ACC_NO=:EMP_BANK_ACC_NO where EMP_ID=:EMP_ID ";

                }
                var parameter = new { EMP_NAME = registerName, SYS_USR_ID = sysUserId ,EMP_CODE = empCode, EMP_ID = empId, EMP_EMAIL = emailAddress,
                    EMP_JOINING_DATE= joiningDate,
                    CMP_BRANCH_ID = branchId,
                    EMP_PRE_ADDRES = presentAddress,
                    EMP_PER_ADDRESS = permanentAddress,
                    EMP_CONTACT_NUM = contactNumber,
                    DPT_ID = dptId,
                    DSG_ID = dsgId,
                    EMP_NATIONAL_ID = nidCard,
                    EMP_BIRTHDAY =birthDay,
                    EMP_BANK_ACC_NO=accountNo,
                    EMP_PIC = imagePath
                };
                using var conn = new SqliteConnection(_conString);
                var employeeUpdate = await conn.QueryAsync<PrEmployeeList>(strSql, parameter);

                return employeeUpdate;
            }
            catch(Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
    }
}
