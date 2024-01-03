using BizWebAPI.Areas.ERP.Models;
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
    public class TaskForwardRepository
    {
        private readonly string _conString;
        public TaskForwardRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TaskForward>> GetTaskForwardList(string userId)
        {
            string strSql = "";
            try
            {
                using var conn = new SqliteConnection(_conString);

                string qryUser = "select EMP_ID,SYS_USR_GRP_TITLE from erp_pr_employee_list el ,ERP_CM_SYSTEM_USER_GROUP sug, erp_cm_system_users su " +
               "  where sug.SYS_USR_GRP_ID=su.SYS_USR_GRP_ID and su.SYS_USR_ID=el.SYS_USR_ID and  el.SYS_USR_ID = :SYS_USR_ID  ";
                var param = new { SYS_USR_ID = userId };
                var userGroup = await conn.QueryAsync<SystemUser>(qryUser, param);
                if (userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Admin(TaskManagement)" || userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Cloud Administrator")
                {
                    strSql = " SELECT TL.TASK_DETAILS,TF.* FROM ERP_TASK_TASK_FORWARD TF left join ERP_TASK_TASK_ASSIGNEE TAS on TF.TASK_ASSIGNEE_ID = TAS.TASK_ASSIGNEE_ID left join ERP_TASK_TASK_LIST TL on TL.TASKLIST_ID = TAS.TASKLIST_ID  where TF.isdelete = '0'   ";
                }
                else
                {
                    strSql = " SELECT TL.TASK_DETAILS,TF.* FROM ERP_TASK_TASK_FORWARD TF left join ERP_TASK_TASK_ASSIGNEE TAS on TF.TASK_ASSIGNEE_ID = TAS.TASK_ASSIGNEE_ID left join ERP_TASK_TASK_LIST TL on TL.TASKLIST_ID = TAS.TASKLIST_ID" +
                        "  where TF.isdelete = '0' and TAS.EMP_ID = (select EMP_ID from ERP_PR_EMPLOYEE_LIST WHERE SYS_USR_ID = :SYS_USR_ID )  ";
                }

                var taskForwardList = await conn.QueryAsync<TaskForward>(strSql, param );

                return taskForwardList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> CreateTaskForward(string decryptAssigneeId, string decryptForwardEmpId, string decryptForwardPriority)
        {
            try
            {
                string strSql = " INSERT INTO ERP_TASK_TASK_FORWARD  (TASK_FORWARD_DATE,TASK_ASSIGNEE_ID,PRIORITY,EMP_ID) " +
                                "VALUES (:TASK_FORWARD_DATE,:TASK_ASSIGNEE_ID,:PRIORITY,:EMP_ID) ";

                var parameter = new
                {
                    TASK_FORWARD_DATE = DateTime.Now,
                    TASK_ASSIGNEE_ID = decryptAssigneeId,
                    PRIORITY = decryptForwardPriority,
                    EMP_ID = decryptForwardEmpId
                };

                using var conn = new SqliteConnection(_conString);
                var taskForward = await conn.QueryAsync<TaskAssign>(strSql, parameter);

                string queryTaskAssign = " UPDATE ERP_TASK_TASK_ASSIGNEE SET EMP_ID=:EMP_ID  where TASK_ASSIGNEE_ID =: TASK_ASSIGNEE_ID   ";

                var paramAssign = new
                {
                    TASK_ASSIGNEE_ID = decryptAssigneeId,
                    EMP_ID = decryptForwardEmpId
                };
                var taskAssign = await conn.QueryAsync<TaskForward>(queryTaskAssign, paramAssign);
                return "Task is forwarded successfully ";// user.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> DeleteTaskForwardById(string taskForwardId)
        {
            try
            {
                string strSql = " update ERP_TASK_TASK_FORWARD set ISDELETE='1' where TASK_FORWADR_ID=:TASK_FORWADR_ID ";

                var parameter = new { TASK_FORWARD_ID = taskForwardId };

                using var conn = new SqliteConnection(_conString);
                var taskForward = await conn.QueryAsync(strSql, parameter);

                return "TASK_FORWARD_ID" + taskForwardId + " is deleted";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskForward>> UpdateTaskForward(string decryptTaskForwardId, string decryptTaskAssigneeId, string decryptForwardPriority, string decryptTaskForwardEmployId)
        {
            try
            {
                string strSql = " UPDATE  ERP_TASK_TASK_FORWARD  set task_forward_date=:TASK_FORWARD_DATE,task_assignee_id=:TASK_ASSIGNEE_ID,priority=:PRIORITY" +
                    "  ,emp_id=:EMP_ID where TASK_FORWADR_ID=:TASK_FORWADR_ID ";

                var parameter = new
                {
                    TASK_FORWADR_ID = decryptTaskForwardId,
                    TASK_FORWARD_DATE = DateTime.Now,
                    TASK_ASSIGNEE_ID = decryptTaskAssigneeId,
                    PRIORITY = decryptForwardPriority,
                    EMP_ID = decryptTaskForwardEmployId
                };

                using var conn = new SqliteConnection(_conString);
                var taskForward = await conn.QueryAsync<TaskForward>(strSql, parameter);

                var ForwardDetailsById = await GetTaskForwardById(decryptTaskForwardId);
                // var 
                return ForwardDetailsById;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskForward>> GetTaskForwardById(string decryptTaskForwardId)
        {
            try
            {
                string strSql = " select * from ERP_TASK_TASK_FORWARD  where TASK_FORWADR_ID=:TASK_FORWADR_ID";
                var parameter = new
                {
                    TASK_FORWADR_ID = decryptTaskForwardId,
                };
                using var conn = new SqliteConnection(_conString);
                var taskForward = await conn.QueryAsync<TaskForward>(strSql, parameter);

                return taskForward;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }

        }
    }

}
