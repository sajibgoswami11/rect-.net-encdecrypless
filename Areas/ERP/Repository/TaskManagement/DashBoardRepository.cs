using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
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
    public class DashBoardRepository
    {
        private readonly string _conString;
        public DashBoardRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TaskCreateAssign>> GetTaskList(string CreateBy)
        {
            try
            {
                string strSql = "SELECT TL.*, ATL.EMP_ID FROM ERP_TASK_TASK_LIST TL Left join  ERP_TASK_TASK_ASSIGNEE ATL on " +
                                " TL.TASKLIST_ID = ATL.TASKLIST_ID where TL.CREATE_BY=:CREATE_BY AND TL.isdelete = '0' ";
                var Param = new { CREATE_BY = CreateBy };

                using var conn = new SqliteConnection(_conString);
                var task_list = await conn.QueryAsync<TaskCreateAssign>(strSql, Param);

                return task_list.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskForward>> GetTaskForwardList()
        {
            try
            {
                string strSql = " SELECT * FROM ERP_TASK_TASK_FORWARD where isdelete = '0'  ";

                using var conn = new SqliteConnection(_conString);
                var taskForwardList = await conn.QueryAsync<TaskForward>(strSql, null);

                return taskForwardList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskProject>> GetProjectByEmployee(string empId)
        {
            try
            {
                string strSql = "";
                using var conn = new SqliteConnection(_conString);

                string qryUser = "select EMP_ID,SYS_USR_GRP_TITLE from erp_pr_employee_list el ,ERP_CM_SYSTEM_USER_GROUP sug, erp_cm_system_users su " +
                   "  where sug.SYS_USR_GRP_ID=su.SYS_USR_GRP_ID and su.SYS_USR_ID=el.SYS_USR_ID and  el.EMP_ID = :EMP_ID  ";
                var param = new { EMP_ID = empId };
                var userGroup = await conn.QueryAsync<SystemUser>(qryUser, param);


                if (userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Admin(TaskManagement)" || userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Cloud Administrator")
                {
                    strSql = " SELECT DISTINCT PROJECT_ID FROM ERP_TASK_EMP_TEAM_MAPPING where EMP_ID is not null and ISDELETE='0' order by PROJECT_ID desc  ";
                }
                else
                {
                    strSql = " SELECT DISTINCT PROJECT_ID FROM ERP_TASK_EMP_TEAM_MAPPING where EMP_ID = :EMP_ID and ISDELETE='0' order by PROJECT_ID desc  ";
                }
                var paramProject = new { EMP_ID = empId };
                var taskProjectList = await conn.QueryAsync<TaskProject>(strSql, paramProject);

                return taskProjectList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskModule>> GetModuleByEmployee(string empId)
        {
            try
            {
                string strSql = "";
                using var conn = new SqliteConnection(_conString);

                string qryUser = "select EMP_ID,SYS_USR_GRP_TITLE from erp_pr_employee_list el ,ERP_CM_SYSTEM_USER_GROUP sug, erp_cm_system_users su " +
                 "  where sug.SYS_USR_GRP_ID=su.SYS_USR_GRP_ID and su.SYS_USR_ID=el.SYS_USR_ID and  el.EMP_ID = :EMP_ID  ";
                var param = new { EMP_ID = empId };
                var userGroup = await conn.QueryAsync<SystemUser>(qryUser, param);


                if (userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Admin(TaskManagement)" || userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Cloud Administrator")
                {
                    strSql = "  SELECT DISTINCT MODULE_ID FROM ERP_TASK_EMP_TEAM_MAPPING where  MODULE_ID is not null order by MODULE_ID desc  ";
                }
                else
                {
                    strSql = "  SELECT DISTINCT MODULE_ID FROM ERP_TASK_EMP_TEAM_MAPPING where EMP_ID = :EMP_ID and MODULE_ID is not null order by MODULE_ID desc ";
                }
                var paramModule = new { EMP_ID = empId };
                var taskModuleList = await conn.QueryAsync<TaskModule>(strSql, paramModule);

                return taskModuleList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskAssign>> GetListByEmployee(string empId)
        {
            try
            {
                string strSql = "";
                using var conn = new SqliteConnection(_conString);

                string qryUser = "select EMP_ID,SYS_USR_GRP_TITLE from erp_pr_employee_list el ,ERP_CM_SYSTEM_USER_GROUP sug, erp_cm_system_users su " +
                 "  where sug.SYS_USR_GRP_ID=su.SYS_USR_GRP_ID and su.SYS_USR_ID=el.SYS_USR_ID and  el.EMP_ID = :EMP_ID  ";
                var param = new { EMP_ID = empId };
                var userGroup = await conn.QueryAsync<SystemUser>(qryUser, param);


                if (userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Admin(TaskManagement)" || userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Cloud Administrator")
                {
                    strSql = "  SELECT DISTINCT TASKLIST_ID,TASK_ASSIGNEE_ID FROM ERP_TASK_TASK_ASSIGNEE where  TASKLIST_ID is not null order by TASK_ASSIGNEE_ID desc ";
                }
                else
                {
                    strSql = "  SELECT DISTINCT TASKLIST_ID,TASK_ASSIGNEE_ID FROM ERP_TASK_TASK_ASSIGNEE where EMP_ID = :EMP_ID and TASKLIST_ID is not null order by TASK_ASSIGNEE_ID desc ";
                }
                var paramTask = new { EMP_ID = empId };
                var taskAssignList = await conn.QueryAsync<TaskAssign>(strSql, paramTask);

                return taskAssignList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskAssign>> GetAssigneeByEmployee(string empId)
        {
            try
            {
                string strSql = "  SELECT DISTINCT TASKLIST_ID FROM ERP_TASK_TASK_ASSIGNEE where EMP_ID = :EMP_ID and TASKLIST_ID is not null  ";
                var param = new { EMP_ID = empId };
                using var conn = new SqliteConnection(_conString);
                var taskAssignList = await conn.QueryAsync<TaskAssign>(strSql, param);

                return taskAssignList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<TaskVMDashboard> GetProjectProgress(string decryptProjectId, string empId)
        {
            try
            {
                string strSql = "";
                using var conn = new SqliteConnection(_conString);

                string qryUser = "select EMP_ID,SYS_USR_GRP_TITLE from erp_pr_employee_list el ,ERP_CM_SYSTEM_USER_GROUP sug, erp_cm_system_users su " +
                  "  where sug.SYS_USR_GRP_ID=su.SYS_USR_GRP_ID and su.SYS_USR_ID=el.SYS_USR_ID and  el.EMP_ID = :EMP_ID  ";
                var param = new { EMP_ID = empId, PROJECT_ID = decryptProjectId };
                var userGroup = await conn.QueryAsync<SystemUser>(qryUser, param);


                if (userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Admin(TaskManagement)" || userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Cloud Administrator")
                {

                    strSql = @" select (select  count( TTL.TASKLIST_ID)  from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE tas   on TTL.TASKLIST_ID = TAS.TASKLIST_ID
                              left join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TTL.PROJECT_ID left join ERP_TASK_STATUS_LIST TSL on TSL.TASK_STATUS_LIST_ID = TTL.STATUS_LIST_ID
                              where TSL.STATUS_NAME in ('Complete', 'Close', 'In Progress', 'Open', 'High')  and TP.PROJECT_ID =:PROJECT_ID and TP.ISDELETE='0' and TTL.ISDELETE='0' ) Total_Task ,
  
                              (select  count(DISTINCT TTL.TASKLIST_ID)    from ERP_TASK_TASK_LIST TTL left
                                             join ERP_TASK_TASK_ASSIGNEE tas   on TTL.TASKLIST_ID = TAS.TASKLIST_ID left
                                             join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TTL.PROJECT_ID left
                                             join ERP_TASK_STATUS_LIST TSL on TSL.TASK_STATUS_LIST_ID = TTL.STATUS_LIST_ID
                              where TSL.STATUS_NAME in ('In Progress', 'Open', 'High')  and TP.PROJECT_ID =:PROJECT_ID and TP.ISDELETE='0' and TTL.ISDELETE='0' ) Remaining_Task ,
   
                             (select count(DISTINCT TTL.TASKLIST_ID) from ERP_TASK_TASK_LIST TTL left
                                  join ERP_TASK_TASK_ASSIGNEE tas   on TTL.TASKLIST_ID = TAS.TASKLIST_ID left
                                  join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TTL.PROJECT_ID left
                                  join ERP_TASK_STATUS_LIST TSL on TSL.TASK_STATUS_LIST_ID = TTL.STATUS_LIST_ID
                             where TSL.STATUS_NAME in('Complete', 'Close')  and TP.PROJECT_ID =:PROJECT_ID and TP.ISDELETE='0' and TTL.ISDELETE='0' ) Complete_Task ,
     
                            (select TP.PROJECT_NAME from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE tas   on TTL.TASKLIST_ID = TAS.TASKLIST_ID
                            left join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TTL.PROJECT_ID left join ERP_TASK_STATUS_LIST TSL on TSL.TASK_STATUS_LIST_ID = TTL.STATUS_LIST_ID
                            where TSL.STATUS_NAME in ('Complete', 'Close', 'In Progress', 'Open', 'High') and TP.PROJECT_ID =:PROJECT_ID group by TP.PROJECT_NAME  ) Project_Name
                            from dual ";
                }
                else
                {
                    strSql = @" select (select   count(DISTINCT TTL.TASKLIST_ID)   from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE tas   on TTL.TASKLIST_ID = TAS.TASKLIST_ID
                              left join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TTL.PROJECT_ID left join ERP_TASK_STATUS_LIST TSL on TSL.TASK_STATUS_LIST_ID = TTL.STATUS_LIST_ID
                              where TSL.STATUS_NAME in ('Complete', 'Close', 'In Progress', 'Open', 'High')  and TP.PROJECT_ID =:PROJECT_ID and TAS.EMP_ID=:EMP_ID and TP.ISDELETE='0' and TTL.ISDELETE='0' ) Total_Task ,
  
                              (select  count(DISTINCT TTL.TASKLIST_ID)   from ERP_TASK_TASK_LIST TTL left
                                             join ERP_TASK_TASK_ASSIGNEE tas   on TTL.TASKLIST_ID = TAS.TASKLIST_ID left
                                             join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TTL.PROJECT_ID left
                                             join ERP_TASK_STATUS_LIST TSL on TSL.TASK_STATUS_LIST_ID = TTL.STATUS_LIST_ID
                              where TSL.STATUS_NAME in ('In Progress', 'Open', 'High')  and TP.PROJECT_ID =:PROJECT_ID and TAS.EMP_ID=:EMP_ID and TP.ISDELETE='0' and TTL.ISDELETE='0' ) Remaining_Task ,
   
                             (select count(DISTINCT TTL.TASKLIST_ID) from ERP_TASK_TASK_LIST TTL left
                                  join ERP_TASK_TASK_ASSIGNEE tas   on TTL.TASKLIST_ID = TAS.TASKLIST_ID left
                                  join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TTL.PROJECT_ID left
                                  join ERP_TASK_STATUS_LIST TSL on TSL.TASK_STATUS_LIST_ID = TTL.STATUS_LIST_ID
                             where TSL.STATUS_NAME in('Complete', 'Close')  and TP.PROJECT_ID =:PROJECT_ID and TAS.EMP_ID=:EMP_ID and TP.ISDELETE='0' and TTL.ISDELETE='0' ) Complete_Task ,
     
                            (select TP.PROJECT_NAME from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE tas   on TTL.TASKLIST_ID = TAS.TASKLIST_ID
                            left join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TTL.PROJECT_ID left join ERP_TASK_STATUS_LIST TSL on TSL.TASK_STATUS_LIST_ID = TTL.STATUS_LIST_ID
                            where TSL.STATUS_NAME in ('Complete', 'Close', 'In Progress', 'Open', 'High') and TP.PROJECT_ID =:PROJECT_ID and TAS.EMP_ID=:EMP_ID  group by TP.PROJECT_NAME  ) Project_Name
                            from dual ";
                }


                var taskForwardList = await conn.QuerySingleAsync<TaskVMDashboard>(strSql, param);

                return taskForwardList;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskVMDashboard>> GetTaskDtailsByProjectId(string projectId, string userId)
        {
            string strSql = "";
            try
            {
                using var conn = new SqliteConnection(_conString);

                string qryUser = "select EMP_ID,SYS_USR_GRP_TITLE from erp_pr_employee_list el ,ERP_CM_SYSTEM_USER_GROUP sug, erp_cm_system_users su " +
                    "  where sug.SYS_USR_GRP_ID=su.SYS_USR_GRP_ID and su.SYS_USR_ID=el.SYS_USR_ID and  el.sys_usr_id = :sys_usr_id  ";
                var param = new { sys_usr_id = userId };
                var userGroup = await conn.QueryAsync<SystemUser>(qryUser, param);


                if (userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Admin(TaskManagement)" || userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Cloud Administrator")
                {
                    strSql = " select distinct ttas.TASK_ASSIGNEE_ID, ETEMP.TEMPACTIVITY_TYPE,TTAC.ACTIVITY_ID , etl.PROJECT_ID,etl.MODULE_ID,etl.ESTIMATED_TIME,etl.TASK_DETAILS,ETL.IMAGE,ttas.ASSIGNEE_DATE,TTAC.TASK_STATUS_LIST_ID,TTAC.ACTIVITY_IMAGE,TTAC.CREATE_DATE,TTAC.UPDATE_DATE " +
                                      " from erp_task_task_list ETL left join erp_task_task_assignee ttas on ttas.TASKLIST_ID = ETL.TASKLIST_ID" +
                                      "  left join ERP_TASK_TEMPACTIVITY ETEMP on ETEMP.TASK_ASSIGNEE_ID = ttas.TASK_ASSIGNEE_ID LEFT JOIN ERP_TASK_ACTIVITY TTAC" +
                                      " ON TTAC.TASK_ASSIGNEE_ID = ttas.TASK_ASSIGNEE_ID where ETL.PROJECT_ID = :PROJECT_ID and ETL.ISDELETE='0' order by ttas.ASSIGNEE_DATE desc ";

                }
                else
                {
                    strSql = " select distinct ttas.TASK_ASSIGNEE_ID, ETEMP.TEMPACTIVITY_TYPE,TTAC.ACTIVITY_ID , etl.PROJECT_ID,etl.MODULE_ID,etl.ESTIMATED_TIME,etl.TASK_DETAILS,ETL.IMAGE,ttas.ASSIGNEE_DATE,TTAC.TASK_STATUS_LIST_ID,TTAC.ACTIVITY_IMAGE,TTAC.CREATE_DATE,TTAC.UPDATE_DATE " +
                  " from erp_task_task_list ETL left join erp_task_task_assignee ttas on ttas.TASKLIST_ID = ETL.TASKLIST_ID" +
                  "  left join ERP_TASK_TEMPACTIVITY ETEMP on ETEMP.TASK_ASSIGNEE_ID = ttas.TASK_ASSIGNEE_ID LEFT JOIN ERP_TASK_ACTIVITY TTAC" +
                  " ON TTAC.TASK_ASSIGNEE_ID = ttas.TASK_ASSIGNEE_ID where ETL.PROJECT_ID = :PROJECT_ID and ETL.ISDELETE='0'  and ttas.emp_id =:emp_id  order by ttas.ASSIGNEE_DATE desc ";
                }


                var taskParam = new { PROJECT_ID = projectId, emp_id = userGroup.FirstOrDefault().EMP_ID };
                var dashBoardPercentageInfo = await conn.QueryAsync<TaskVMDashboard>(strSql, taskParam);


                return dashBoardPercentageInfo.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskVMDashboard>> GetAssignIdByStartEndDuration(string assigneeId, string activityId)
        {
            try
            {
                string strSql = "";
                if (!string.IsNullOrEmpty(assigneeId)&& string.IsNullOrEmpty(activityId))
                {
                    strSql = " select END_TIME ,START_TIME from ERP_TASK_ACTIVITY  where TASK_ASSIGNEE_ID = :TASK_ASSIGNEE_ID ";
                }
                else
                {
                    strSql = "select END_TIME ,START_TIME from ERP_TASK_ACTIVITY  where ACTIVITY_ID = :ACTIVITY_ID ";
                }

                var taskParam = new { TASK_ASSIGNEE_ID = assigneeId, ACTIVITY_ID = activityId };
                using var conn = new SqliteConnection(_conString);
                var dashBoardPercentageInfo = await conn.QueryAsync<TaskVMDashboard>(strSql, taskParam);

                return dashBoardPercentageInfo.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskVMDashboard>> GetEmployeeByAssign(string assigneeId)
        {
            try
            {

                string strSql = " select EL.EMP_ID,EL.EMP_NAME from ERP_TASK_TASK_ASSIGNEE TAS,ERP_PR_EMPLOYEE_LIST EL  where  tas.emp_id=el.emp_id and tas.TASK_ASSIGNEE_ID = :TASK_ASSIGNEE_ID ";

                var taskParam = new { TASK_ASSIGNEE_ID = assigneeId };
                using var conn = new SqliteConnection(_conString);
                var dashBoardAssignPerson = await conn.QueryAsync<TaskVMDashboard>(strSql, taskParam);

                return dashBoardAssignPerson.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskVMDashboard>> GetActivityIdByStartEndDuration(string assigneeId)
        {
            try
            {

                string strSql = " select END_TIME ,START_TIME from ERP_TASK_ACTIVITY  where ACTIVITY_ID = :TASK_ASSIGNEE_ID ";

                var taskParam = new { TASK_ASSIGNEE_ID = assigneeId };
                using var conn = new SqliteConnection(_conString);
                var dashBoardPercentageInfo = await conn.QueryAsync<TaskVMDashboard>(strSql, taskParam);

                return dashBoardPercentageInfo.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<TaskVMDashboard> GetDateRangeProjectProgressSummary(string startDate, string EndDate, string usrId)
        {
            try
            {

                string strSql = @"select (select count(*) from ERP_TASK_ACTIVITY TAC left join ERP_TASK_STATUS_LIST STATL on STATL.TASK_STATUS_LIST_ID = TAC.TASK_STATUS_LIST_ID
                where TAC.TASK_ASSIGNEE_ID in (select TTAS.TASK_ASSIGNEE_ID from (select * from(select etmap.PROJECT_ID from ERP_TASK_EMP_TEAM_MAPPING ETMAP  where EMP_ID IN (:EMP_ID) and project_id is not null group by PROJECT_ID) TP, "
              + @"  ERP_TASK_TASK_LIST ETL where ETL.PROJECT_ID = TP.PROJECT_ID and ETL.TASK_DATE BETWEEN TO_DATE('" + startDate + "', 'MM/DD/YYYY HH:MI:SS AM')" +
              "  AND TO_DATE('" + EndDate + "', 'MM/DD/YYYY HH:MI:SS AM')) TSL, ERP_TASK_TASK_ASSIGNEE TTAS" +
              "   where TSL.TASKLIST_ID = TTAS.TASKLIST_ID) ) Total_Task ," +

              "  (select count(STATL.STATUS_NAME) from ERP_TASK_ACTIVITY TAC left join ERP_TASK_STATUS_LIST STATL on STATL.TASK_STATUS_LIST_ID = TAC.TASK_STATUS_LIST_ID " +
              @"   where STATL.TASK_STATUS_LIST_ID = '200502010015000001' and TAC.TASK_ASSIGNEE_ID in (select TTAS.TASK_ASSIGNEE_ID from
                (select* from (select etmap.PROJECT_ID from ERP_TASK_EMP_TEAM_MAPPING ETMAP  where EMP_ID IN (:EMP_ID) and project_id is not null group by PROJECT_ID ) TP, 
               ERP_TASK_TASK_LIST ETL where ETL.PROJECT_ID = TP.PROJECT_ID and ETL.TASK_DATE BETWEEN TO_DATE('" + startDate + "', 'MM/DD/YYYY HH:MI:SS AM')" +
               " AND TO_DATE('" + EndDate + "', 'MM/DD/YYYY HH:MI:SS AM' ) ) TSL,  ERP_TASK_TASK_ASSIGNEE TTAS where TSL.TASKLIST_ID = TTAS.TASKLIST_ID ) ) Complete_Task , " +

               " (select count(TAC.ACTIVITY_ID) from ERP_TASK_ACTIVITY TAC left join ERP_TASK_STATUS_LIST STATL on STATL.TASK_STATUS_LIST_ID = TAC.TASK_STATUS_LIST_ID" +
               " where STATL.STATUS_NAME is null  and TAC.TASK_ASSIGNEE_ID in (select TTAS.TASK_ASSIGNEE_ID from (select* from (select etmap.PROJECT_ID from" +
               "  ERP_TASK_EMP_TEAM_MAPPING ETMAP  where EMP_ID IN ( :EMP_ID) and project_id is not null group by PROJECT_ID ) TP," +
               "  ERP_TASK_TASK_LIST ETL where ETL.PROJECT_ID = TP.PROJECT_ID and  ETL.TASK_DATE BETWEEN TO_DATE('" + startDate + "', 'MM/DD/YYYY HH:MI:SS AM')" +
               " AND TO_DATE('" + EndDate + "', 'MM/DD/YYYY HH:MI:SS AM' )  ) TSL,  ERP_TASK_TASK_ASSIGNEE TTAS  where TSL.TASKLIST_ID = TTAS.TASKLIST_ID ) ) Remaining_Task, " +

               " (select count(TAC.ACTIVITY_ID) from ERP_TASK_ACTIVITY TAC left join ERP_TASK_STATUS_LIST STATL on STATL.TASK_STATUS_LIST_ID = TAC.TASK_STATUS_LIST_ID  where TAC.TASK_STATUS_LIST_ID in ('200623010015000001') and TAC.TASK_ASSIGNEE_ID in  (select TTAS.TASK_ASSIGNEE_ID from ( select* from (select etmap.PROJECT_ID from ERP_TASK_EMP_TEAM_MAPPING ETMAP  where EMP_ID IN (:EMP_ID) and project_id is not null group by PROJECT_ID ) TP,  " +
              "  ERP_TASK_TASK_LIST ETL where ETL.PROJECT_ID = TP.PROJECT_ID AND ETL.TASK_DATE BETWEEN TO_DATE('" + startDate + "', 'MM/DD/YYYY HH:MI:SS AM')" +
               " AND TO_DATE('" + EndDate + "', 'MM/DD/YYYY HH:MI:SS AM' ) ) TSL,  ERP_TASK_TASK_ASSIGNEE TTAS  where TSL.TASKLIST_ID = TTAS.TASKLIST_ID ) )  Active" +
               "  from dual ";

                //sdate = startDate, edate = EndDate, 
                var taskParam = new { sdate = startDate, edate = EndDate, EMP_ID = usrId };
                using var conn = new SqliteConnection(_conString);
                var dashBoardPercentageInfo = await conn.QuerySingleAsync<TaskVMDashboard>(strSql, taskParam);

                return dashBoardPercentageInfo;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

    }
}
