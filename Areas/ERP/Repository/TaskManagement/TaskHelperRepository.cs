using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using BizWebAPI.Common;
using Dapper;
using Microsoft.Data.Sqlite;
//using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Repository.TaskManagement
{
    internal class TaskHelperRepository
    {
        private readonly string _conString;

        public TaskHelperRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        ///         1st param
        ///         if param is null return total row with list
        ///         if param is not null return only param wise row with list
        ///         2nd param
        ///         needed returan type whate we need         
        /// </returns>
        internal async Task<IEnumerable<TaskProject>> GetProjectInfo(string EmployeeId, string ProjectId)
        {
            string strSql1 = "";
            try
            {
                using var conn = new SqliteConnection(_conString);

                string qryUser = "select EMP_ID,SYS_USR_GRP_TITLE from erp_pr_employee_list el ,ERP_CM_SYSTEM_USER_GROUP sug, erp_cm_system_users su " +
                    "  where sug.SYS_USR_GRP_ID=su.SYS_USR_GRP_ID and su.SYS_USR_ID=el.SYS_USR_ID and  el.EMP_ID = :EMP_ID  ";
                var param = new { EMP_ID = EmployeeId };
                var userGroup = await conn.QueryAsync<SystemUser>(qryUser, param);


                if (userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Admin(TaskManagement)")
                {
                   strSql1 = @" select distinct TP.* from  ERP_TASK_PROJECT TP  left join ERP_TASK_EMP_TEAM_MAPPING TEMPMAP on TEMPMAP.project_id = tp.project_id    where TP.isdelete = '0' AND TEMPMAP.EMP_ID = :EMP_ID ";

                }
                else
                {
                  strSql1 = @" select distinct TP.* from  ERP_TASK_PROJECT TP  left join ERP_TASK_EMP_TEAM_MAPPING TEMPMAP on TEMPMAP.project_id = tp.project_id    where TP.isdelete = '0' AND TEMPMAP.EMP_ID = :EMP_ID ";

                }
                var projectParam = new { EMP_ID = EmployeeId, PROJECT_ID = ProjectId };
                var project = await conn.QueryAsync<TaskProject>(strSql1, projectParam);
                return project;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
        internal async Task<string> GetProjectFieldInfo(string ProjectId)
        {
            string strSql1 = "";
            try
            {
                if (ProjectId != "")
                {
                    strSql1 = "select TP.PROJECT_NAME  FROM  ERP_TASK_PROJECT TP,ERP_TASK_MODULE TMOD " +
                        "where TM.PROJECT_ID = TP.PROJECT_ID AND TM.MODULE_ID = TMOD.MODULE_ID AND TM.isdelete = '0' and TP.PROJECT_ID='" + ProjectId + "'  ";
                }
                using var conn = new SqliteConnection(_conString);
                var project = await conn.QueryAsync<string>(strSql1, null);

                // var sUMMARY = taskTEAM.Concat(project);
                return project.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskModule>> GetModuleInfo(string EmployeeId)
        {
            try
            {
                using var conn = new SqliteConnection(_conString);
                string moduleSql = @" select TMOD.* from erp_task_module tmod left join erp_task_emp_team_mapping tempmap on tempmap.MODULE_ID = tmod.MODULE_ID where tmod.ISDELETE = '0'  and tempmap.EMP_ID =:emp_id  ";
                var moduleParam = new { EMP_ID = EmployeeId };

                var project = await conn.QueryAsync<TaskModule>(moduleSql, moduleParam);

                return project;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<PrEmployeeList> GetEmployeeByUserId(string SysUserId)
        {
            try
            {
                string strSql = "SELECT * FROM ERP_PR_EMPLOYEE_LIST WHERE SYS_USR_ID = :SYS_USR_ID AND ROWNUM = '1' ";
                var parameter = new { SYS_USR_ID = SysUserId };

                using var conn = new SqliteConnection(_conString);
                var employee = await conn.QueryAsync<PrEmployeeList>(strSql, parameter);

                return employee.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskAssign>> GetTaskInfoByEmpId(string employeeId)
        {
            try
            {
                string strSql = " SELECT * FROM ERP_TASK_TASK_LIST TL Left join " +
                                " ERP_TASK_TASK_ASSIGNEE ATL on TL.TASKLIST_ID = ATL.TASKLIST_ID where TL.isdelete = '0' " +
                                " AND ATL.EMP_ID=:EMP_ID ";

                var taskParam = new { EMP_ID = employeeId };
                using var conn = new SqliteConnection(_conString);
                var taskInfo = await conn.QueryAsync<TaskAssign>(strSql, taskParam);

                return taskInfo;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
       
        internal async Task<DashboardProjectDetailsDto> DashboardOnProjectCountClickGetDetails(string projectId)
        {
            try 
            {
                string strSql = "select ( SELECT COUNT(*)  FROM ERP_TASK_TASK_LIST TL , ERP_TASK_STATUS_LIST TSL  where TL.isdelete = '0' and TSL.TASK_STATUS_LIST_ID= TL.STATUS_LIST_ID and TSL.STATUS_NAME in ('Complete') AND TL.PROJECT_ID=:PROJECT_ID) CompleteTask ," +
                    " ( SELECT COUNT(*)  FROM ERP_TASK_TASK_LIST TL where TL.PROJECT_ID=:PROJECT_ID )   TotalTask ," +
                    "   PROJECT_NAME ProjectName, MILESTONES,CREATE_BY CreateBy  from ERP_TASK_PROJECT where PROJECT_ID=:PROJECT_ID  ";

                var taskParam = new { PROJECT_ID = projectId };
                using var conn = new SqliteConnection(_conString);
                var taskInfo = await conn.QueryAsync<DashboardProjectDetailsDto>(strSql, taskParam);
                return taskInfo.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }         
        }             

        internal async Task<DashboardModuleDetailsDto> DashboardOnModuleCountClickGetDetails(string moduleId)
        {
            try 
            {
                string strSql = "select ( SELECT COUNT(*)  FROM ERP_TASK_TASK_LIST TL , ERP_TASK_STATUS_LIST TSL  where TL.isdelete = '0' and TSL.TASK_STATUS_LIST_ID= TL.STATUS_LIST_ID and TSL.STATUS_NAME in ('Complete') AND TL.MODULE_ID=:MODULE_ID) CompleteTask ," +
                    " ( SELECT COUNT(*)  FROM ERP_TASK_TASK_LIST TL where TL.MODULE_ID=:MODULE_ID )   TotalTask ," +
                    "   MODULE_NAME ModuleName, MILESTONES ,CREATE_BY CreateBy from ERP_TASK_MODULE where MODULE_ID=:MODULE_ID  ";

                var taskParam = new { MODULE_ID = moduleId };
                using var conn = new SqliteConnection(_conString);
                var taskInfo = await conn.QueryAsync<DashboardModuleDetailsDto>(strSql, taskParam);
                return taskInfo.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }         
        }

        internal async Task<DashboardAssignDetailsDto> DashboardOnTaskAssignCountClickGetDetails(string listId)
        {
            try
            {
                string strSql = @"select  MODULE_NAME ModuleName, PROJECT_NAME ProjectName, TASK_DETAILS TaskDetails, LINK Tasklink,TASK_DATE Taskdate, TSL.STATUS_NAME  WorkingStatus,
                                ESTIMATED_TIME EstimatedTime, TAS.EMP_ID AssignTo, TAS.TASK_ASSIGNEE_ID TaskAssignId, (select MAX(TTA.TEMPACTIVITY_TYPE) from ERP_TASK_TEMPACTIVITY TTA where TTA.TASK_ASSIGNEE_ID = TAS.TASK_ASSIGNEE_ID and rownum = 1) ActivityType
                                from ERP_TASK_TASK_LIST TL left join ERP_TASK_PROJECT TP on TL.PROJECT_ID = TP.PROJECT_ID left join ERP_TASK_MODULE TM on TL.MODULE_ID = TM.MODULE_ID  
                                left join ERP_TASK_TASK_ASSIGNEE TAS on TAS.TASKLIST_ID = TL.TASKLIST_ID left join ERP_TASK_STATUS_LIST TSL on TSL.TASK_STATUS_LIST_ID= TL.STATUS_LIST_ID where TSL.STATUS_NAME != 'Close' AND TL.TASKLIST_ID =:TASKLIST_ID  ";

                var taskParam = new { TASKLIST_ID = listId };
                using var conn = new SqliteConnection(_conString);
                var taskInfo = await conn.QueryAsync<DashboardAssignDetailsDto>(strSql, taskParam);
                return taskInfo.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<DashboardCompleteDetailsDto> DashboardOnTaskCompleteCountClickGetDetails(string listId)
        {
            try
            {
                string strSql = @"select  MODULE_NAME ModuleName, PROJECT_NAME ProjectName, TASK_DETAILS TaskDetails, LINK Tasklink,TASK_DATE Taskdate, TSL.STATUS_NAME WorkingStatus,
                                ESTIMATED_TIME EstimatedTime, TAS.EMP_ID AssignTo, TAS.TASK_ASSIGNEE_ID TaskAssignId from ERP_TASK_TASK_LIST TL left join ERP_TASK_PROJECT TP on TL.PROJECT_ID = TP.PROJECT_ID 
                                left join ERP_TASK_MODULE TM on TL.MODULE_ID = TM.MODULE_ID  left join ERP_TASK_TASK_ASSIGNEE TAS on TAS.TASKLIST_ID = TL.TASKLIST_ID
                                left join ERP_TASK_STATUS_LIST TSL  on TSL.TASK_STATUS_LIST_ID= TL.STATUS_LIST_ID where  TSL.STATUS_NAME in ('Close')  and TL.TASKLIST_ID =:TASKLIST_ID  ";

                var taskParam = new { TASKLIST_ID = listId };
                using var conn = new SqliteConnection(_conString);
                var taskInfo = await conn.QueryAsync<DashboardCompleteDetailsDto>(strSql, taskParam);
                
                return taskInfo.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }


    }
}
