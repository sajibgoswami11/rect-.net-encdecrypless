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
    public class TaskActivityRepository
    {
        private readonly string _conString;

        public TaskActivityRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TaskActivity>> GetTaskActivityList(string UserId)
        {

            string strSql = "";
            try
            {
                using var conn = new SqliteConnection(_conString);

                string qryUser = "select EMP_ID,SYS_USR_GRP_TITLE from erp_pr_employee_list el ,ERP_CM_SYSTEM_USER_GROUP sug, erp_cm_system_users su " +
                "  where sug.SYS_USR_GRP_ID=su.SYS_USR_GRP_ID and su.SYS_USR_ID=el.SYS_USR_ID and  el.SYS_USR_ID = :SYS_USR_ID  ";
                var param = new { SYS_USR_ID = UserId };
                var userGroup = await conn.QueryAsync<SystemUser>(qryUser, param);
                if (userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Admin(TaskManagement)" || userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Cloud Administrator")
                {
                    strSql = @"  SELECT etp.PROJECT_NAME,etm.MODULE_NAME,etsl.STATUS_NAME, ETA.* FROM ERP_TASK_ACTIVITY ETA
                                    left join ERP_TASK_TASK_ASSIGNEE TTA on ETA.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID
                                    left join ERP_TASK_TASK_LIST ETL ON ETL.TASKLIST_ID = TTA.TASKLIST_ID
                                    left join erp_task_project etp on etp.PROJECT_ID = ETL.PROJECT_ID
                                    left join erp_task_module etm on etm.MODULE_ID = ETL.MODULE_ID
                                    left join ERP_TASK_STATUS_LIST etsl on etsl.TASK_STATUS_LIST_ID = ETA.TASK_STATUS_LIST_ID 
                                    left join ERP_PR_EMPLOYEE_LIST EMP on EMP.EMP_ID = ETA.EMP_ID
                                    WHERE ETA.ISDELETE='0'  order by ETA.ACTIVITY_ID desc ";
                }
                else
                {
                    strSql = @"  SELECT etp.PROJECT_NAME,etm.MODULE_NAME,etsl.STATUS_NAME, ETA.* FROM ERP_TASK_ACTIVITY ETA
                                    left join ERP_TASK_TASK_ASSIGNEE TTA on ETA.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID
                                    left join ERP_TASK_TASK_LIST ETL ON ETL.TASKLIST_ID = TTA.TASKLIST_ID
                                    left join erp_task_project etp on etp.PROJECT_ID = ETL.PROJECT_ID
                                    left join erp_task_module etm on etm.MODULE_ID = ETL.MODULE_ID
                                    left join ERP_TASK_STATUS_LIST etsl on etsl.TASK_STATUS_LIST_ID = ETA.TASK_STATUS_LIST_ID 
                                    left join ERP_PR_EMPLOYEE_LIST EMP on EMP.EMP_ID = ETA.EMP_ID
                                    WHERE ETA.ISDELETE='0' and EMP.SYS_USR_ID =:SYS_USR_ID order by ETA.ACTIVITY_ID desc ";
                }

                var taskAssignList = await conn.QueryAsync<TaskActivity>(strSql, param);

                return taskAssignList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> CreateTaskActivity(string remarks, string empId, string updateBy, string taskAssigneId, List<string> image,string stampAdmin)
        {
            try
            {
                string taskListIdForActivity = "";
                string activetySql = " SELECT MAX(ACTIVITY_ID) ACTIVITY_ID FROM ERP_TASK_ACTIVITY WHERE TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID AND EMP_ID=:EMP_ID AND " +
                                    " TASK_STATUS_LIST_ID IN ('200607010015000001','200623010015000001','200623010015000003') AND ISDELETE = '0' ORDER BY ACTIVITY_ID DESC ";
                var activityParam = new
                {
                    TASK_ASSIGNEE_ID = taskAssigneId,
                    EMP_ID = empId,
                    TASK_STATUS_LIST_ID = "200607010015000001",
                };
                using var conn = new SqliteConnection(_conString);
                var activity = await conn.QueryAsync<TaskActivity>(activetySql, activityParam);
                var activityId = activity.FirstOrDefault().ACTIVITY_ID;

                string strSql = " UPDATE ERP_TASK_ACTIVITY set TASK_ACTIVITY_DATE=:TASK_ACTIVITY_DATE,REMARKS=:REMARKS, " +
                                "TASK_STATUS_LIST_ID=:TASK_STATUS_LIST_ID, EMP_ID=:EMP_ID, TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID, " +
                                "UPDATE_BY=:UPDATE_BY, END_TIME=:END_TIME where ACTIVITY_ID=:ACTIVITY_ID ";

                var parameter = new
                {
                    ACTIVITY_ID = activityId,
                    END_TIME = DateTime.Now,
                    TASK_ACTIVITY_DATE = DateTime.Now,
                    REMARKS = remarks,
                    TASK_STATUS_LIST_ID = "200502010015000001",
                    EMP_ID = empId,
                    TASK_ASSIGNEE_ID = taskAssigneId,
                    UPDATE_BY = updateBy
                };

                var taskAssign = await conn.QueryAsync<TaskActivity>(strSql, parameter);

                string taskListIdSql = " select TASKLIST_ID from ERP_TASK_TASK_ASSIGNEE TA where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID ";
                var paramListId = new { TASK_ASSIGNEE_ID = taskAssigneId };
                var taskListId = await conn.QueryAsync<TaskAssign>(taskListIdSql, paramListId);
                taskListIdForActivity = taskListId.FirstOrDefault().TASKLIST_ID;
                string updateTaskLIst = " UPDATE ERP_TASK_TASK_LIST SET STATUS_LIST_ID=:STATUS_LIST_ID, COMPLETE_BY_ADMIN =:COMPLETE_BY_ADMIN  where TASKLIST_ID =: TASKLIST_ID   ";

                var paramTaskLIst = new
                {
                    TASKLIST_ID = taskListId.FirstOrDefault().TASKLIST_ID,
                    STATUS_LIST_ID = "200623010015000002",
                    COMPLETE_BY_ADMIN = stampAdmin
                };
                await conn.QueryAsync<TaskList>(updateTaskLIst, paramTaskLIst);

                string getTaskActivityIdSql = "SELECT ACTIVITY_ID FROM ERP_TASK_ACTIVITY WHERE isdelete = '0'AND ROWNUM = '1' AND " +
                                                   "REMARKS=:REMARKS AND TASK_STATUS_LIST_ID=:TASK_STATUS_LIST_ID AND EMP_ID=:EMP_ID AND " +
                                                   "TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID order by CREATE_DATE desc";

                var parameterTaskActivityId = new
                {
                    REMARKS = remarks,
                    TASK_STATUS_LIST_ID = "200502010015000001",
                    EMP_ID = empId,
                    TASK_ASSIGNEE_ID = taskAssigneId,
                };

                var getTaskActivity = await conn.QueryAsync<TaskActivity>(getTaskActivityIdSql, parameterTaskActivityId);

                if (image.Count() > 0)
                {
                    foreach (var itemImage in image)
                    {
                        string sqlImagePath = " insert into erp_task_image (REFERENCE_ID,IMAGE_PATH) values (:REFERENCE_ID,:IMAGE_PATH) ";

                        var paramImagePath = new
                        {
                            REFERENCE_ID = getTaskActivity.FirstOrDefault().ACTIVITY_ID,
                            IMAGE_PATH = itemImage
                        };
                        await conn.QueryAsync<TaskImage>(sqlImagePath, paramImagePath);
                    }
                }

                // var 
                return "done";// user.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskActivity>> GeTaskActivityListById(string taskActivityId)
        {
            try
            {
                string strSql = " SELECT * FROM ERP_TASK_ACTIVITY  WHERE ACTIVITY_ID=:ACTIVITY_ID AND ISDELETE='0'";
                var parameter = new { ACTIVITY_ID = taskActivityId };

                using var conn = new SqliteConnection(_conString);
                var taskAssign = await conn.QueryAsync<TaskActivity>(strSql, parameter);

                return taskAssign.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskActivity>> TaskActivityByProjectModule(string decrypProjectId, string decrypModuleId)
        {
            string strSql = "";
            try
            {
                if (!string.IsNullOrEmpty(decrypProjectId) && string.IsNullOrEmpty(decrypModuleId))
                {
                    strSql = " SELECT * FROM ERP_TASK_ACTIVITY ETA left join ERP_TASK_TASK_ASSIGNEE TTA on ETA.TASK_ASSIGNEE_ID=TTA.TASK_ASSIGNEE_ID " +
                            "left join ERP_TASK_TASK_LIST ETL ON ETL.TASKLIST_ID = TTA.TASKLIST_ID where ETL.PROJECT_ID = :PROJECT_ID and ETA.ISDELETE='0'  ";

                }
                else if (string.IsNullOrEmpty(decrypProjectId) && !string.IsNullOrEmpty(decrypModuleId))
                {
                    strSql = " SELECT * FROM ERP_TASK_ACTIVITY ETA left join ERP_TASK_TASK_ASSIGNEE TTA on ETA.TASK_ASSIGNEE_ID=TTA.TASK_ASSIGNEE_ID " +
                            "left join ERP_TASK_TASK_LIST ETL ON ETL.TASKLIST_ID = TTA.TASKLIST_ID where ETL.MODULE_ID = :MODULE_ID and ETA.ISDELETE='0'  ";

                }
                else if (!string.IsNullOrEmpty(decrypProjectId) && !string.IsNullOrEmpty(decrypModuleId))
                {
                    strSql = " SELECT * FROM ERP_TASK_ACTIVITY ETA left join ERP_TASK_TASK_ASSIGNEE TTA on ETA.TASK_ASSIGNEE_ID=TTA.TASK_ASSIGNEE_ID " +
                        "left join ERP_TASK_TASK_LIST ETL ON ETL.TASKLIST_ID = TTA.TASKLIST_ID where ETL.PROJECT_ID = :PROJECT_ID  and " +
                        "ETL.MODULE_ID = :MODULE_ID and ETA.ISDELETE='0' ";
                }

                var parameter = new { PROJECT_ID = decrypProjectId, MODULE_ID = decrypModuleId };

                using var conn = new SqliteConnection(_conString);
                var taskAssign = await conn.QueryAsync<TaskActivity>(strSql, parameter);

                return taskAssign;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskActivity>> UpdateTaskActivityListByID(string taskActivityId, DateTime stratTime, DateTime endTime, string remarks, string taskStatusListId, string empId, string updatedBy, string taskAssigneId, DateTime decryptActivityDate, List<string> image)
        {
            try
            {
                string strSql = " UPDATE ERP_TASK_ACTIVITY set START_TIME=:START_TIME,END_TIME=:END_TIME,TASK_ACTIVITY_DATE=:TASK_ACTIVITY_DATE,REMARKS=:REMARKS, TASK_STATUS_LIST_ID=:TASK_STATUS_LIST_ID, EMP_ID=:EMP_ID, TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID, UPDATE_DATE=:UPDATE_DATE,UPDATE_BY=:UPDATE_BY  where ACTIVITY_ID=:ACTIVITY_ID";

                var parameter = new
                {
                    ACTIVITY_ID = taskActivityId,
                    START_TIME = stratTime,
                    END_TIME = endTime,
                    TASK_ACTIVITY_DATE = decryptActivityDate,
                    REMARKS = remarks,
                    TASK_STATUS_LIST_ID = taskStatusListId,
                    EMP_ID = empId,
                    TASK_ASSIGNEE_ID = taskAssigneId,
                    UPDATE_DATE = DateTime.Today,
                    UPDATE_BY = updatedBy
                };

                using var conn = new SqliteConnection(_conString);
                var taskActivities = await conn.QueryAsync<TaskActivity>(strSql, parameter);
                if (taskStatusListId == "200502010015000001")
                {
                    string taskListIdSql = " select TASKLIST_ID from ERP_TASK_TASK_ASSIGNEE TA where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID ";
                    var paramListId = new { TASK_ASSIGNEE_ID = taskAssigneId };
                    var taskListId = await conn.QueryAsync<TaskAssign>(taskListIdSql, paramListId);

                    string updateTaskLIst = " UPDATE ERP_TASK_TASK_LIST SET STATUS_LIST_ID=:STATUS_LIST_ID  where TASKLIST_ID =: TASKLIST_ID   ";

                    var paramTaskLIst = new
                    {
                        TASKLIST_ID = taskListId.FirstOrDefault().TASKLIST_ID,
                        STATUS_LIST_ID = "200623010015000002"
                    };
                    await conn.QueryAsync<TaskList>(updateTaskLIst, paramTaskLIst);
                }
                if (image.Count() > 0)
                {
                    foreach (var itemImage in image)
                    {
                        string sqlImagePath = " insert into erp_task_image (REFERENCE_ID,IMAGE_PATH) values (:REFERENCE_ID,:IMAGE_PATH) ";

                        var paramImagePath = new
                        {
                            REFERENCE_ID = taskActivityId,
                            IMAGE_PATH = itemImage
                        };
                        await conn.QueryAsync<TaskImage>(sqlImagePath, paramImagePath);
                    }
                }
                var taskActivityList = await GeTaskActivityListById(taskActivityId);
                return taskActivityList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> IsDeleteTaskActivityById(string taskActivityId)
        {
            try
            {
                string strSql = " UPDATE ERP_TASK_ACTIVITY set ISDELETE='1' where ACTIVITY_ID=:ACTIVITY_ID ";

                var parameter = new { ACTIVITY_ID = taskActivityId };

                using var conn = new SqliteConnection(_conString);
                var taskAssign = await conn.QueryAsync<TaskActivity>(strSql, parameter);

                return "Task" + taskActivityId + " is deleted";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

    }
}
