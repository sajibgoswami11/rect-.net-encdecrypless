using BizWebAPI.Areas.ERP.Common;
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
    public class TaskAssignRepository
    {
        private readonly string _conString;
        // private TaskHelperRepository _taskManagementHelper;

        public TaskAssignRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TaskAssign>> GetTaskAssignList(string UserId)
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
                    strSql = @"  (select TEMPACTIVITY_TYPE, TIME_EXTEND_NOTE, TASK_ASSIGNEE_ID, TASK_DETAILS, PRIORITY, ASSIGNEE_DATE, EMP_ID, EMP_NAME,PROJECT_ID,PROJECT_NAME,MODULE_NAME,ESTIMATED_TIME, TASKLIST_ID, TASK_IMAGE from
                                    (select ETTEMP.TEMPACTIVITY_TYPE, ETTEMP.TIME_EXTEND_NOTE,  TAC.ACTIVITY_ID, TASN.* from
                                    (select TL.STATUS_LIST_ID, TASK_ASSIGNEE_ID, TASK_DETAILS, PRIORITY, ASSIGNEE_DATE, TA.EMP_ID, EM.EMP_NAME, EM.IMAGE,TP.PROJECT_ID, TP.PROJECT_NAME,TM.MODULE_NAME,TL.ESTIMATED_TIME, TL.TASKLIST_ID, TL.IMAGE as TASK_IMAGE
                                    from ERP_TASK_TASK_ASSIGNEE TA
                                    left join ERP_PR_EMPLOYEE_LIST EM on TA.EMP_ID = EM.EMP_ID
                                    left join ERP_TASK_TASK_LIST TL on   TL.TASKLIST_ID = TA.TASKLIST_ID  
                                    left join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TL.PROJECT_ID
                                    left join ERP_TASK_MODULE TM on TM.MODULE_ID = TL.MODULE_ID
                                    where TA.isdelete = '0'   ) TASN
                                    left join ERP_TASK_ACTIVITY TAC on TAC.TASK_ASSIGNEE_ID = TASN.TASK_ASSIGNEE_ID
                                    left join ERP_TASK_TEMPACTIVITY ETTEMP ON TASN.TASK_ASSIGNEE_ID = ETTEMP.TASK_ASSIGNEE_ID) ETA
                                    left join ERP_TASK_STATUS_LIST etsl on etsl.TASK_STATUS_LIST_ID = ETA.STATUS_LIST_ID
                                    where etsl.STATUS_NAME <> 'Close' and ETA.STATUS_LIST_ID is null )
                                                                    union
                                                                    (
                                    select TEMPACTIVITY_TYPE, TIME_EXTEND_NOTE, TASK_ASSIGNEE_ID, TASK_DETAILS, PRIORITY, ASSIGNEE_DATE, EMP_ID, EMP_NAME,PROJECT_ID, PROJECT_NAME,MODULE_NAME,ESTIMATED_TIME, TASKLIST_ID, TASK_IMAGE from
                                    (select ETTEMP.TEMPACTIVITY_TYPE, ETTEMP.TIME_EXTEND_NOTE, TAC.ACTIVITY_ID, TASN.* from
                                    (select TL.STATUS_LIST_ID,TASK_ASSIGNEE_ID, TASK_DETAILS, PRIORITY, ASSIGNEE_DATE, TA.EMP_ID, EM.EMP_NAME, EM.IMAGE,TP.PROJECT_ID, TP.PROJECT_NAME,TM.MODULE_NAME,TL.ESTIMATED_TIME, TL.TASKLIST_ID, TL.IMAGE as TASK_IMAGE
                                    from ERP_TASK_TASK_ASSIGNEE TA
                                    left join ERP_PR_EMPLOYEE_LIST EM on TA.EMP_ID = EM.EMP_ID
                                    left join ERP_TASK_TASK_LIST TL on   TL.TASKLIST_ID = TA.TASKLIST_ID  
                                    left join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TL.PROJECT_ID
                                    left join ERP_TASK_MODULE TM on TM.MODULE_ID = TL.MODULE_ID
                                    where TA.isdelete = '0'  ) TASN
                                    left join ERP_TASK_ACTIVITY TAC on TAC.TASK_ASSIGNEE_ID = TASN.TASK_ASSIGNEE_ID
                                    left join ERP_TASK_TEMPACTIVITY ETTEMP ON TASN.TASK_ASSIGNEE_ID = ETTEMP.TASK_ASSIGNEE_ID) ETA
                                    left join ERP_TASK_STATUS_LIST etsl on etsl.TASK_STATUS_LIST_ID = ETA.STATUS_LIST_ID  where etsl.STATUS_NAME <> 'Close' ) order by ASSIGNEE_DATE desc ";

                }
                else
                {
                    strSql = @"  (select TEMPACTIVITY_TYPE, TIME_EXTEND_NOTE, TASK_ASSIGNEE_ID, TASK_DETAILS, PRIORITY, ASSIGNEE_DATE, EMP_ID, EMP_NAME,PROJECT_ID,PROJECT_NAME,MODULE_NAME,ESTIMATED_TIME, TASKLIST_ID, TASK_IMAGE from
                                    (select ETTEMP.TEMPACTIVITY_TYPE, ETTEMP.TIME_EXTEND_NOTE,  TAC.ACTIVITY_ID, TASN.* from
                                    (select TL.STATUS_LIST_ID, TASK_ASSIGNEE_ID, TASK_DETAILS, PRIORITY, ASSIGNEE_DATE, TA.EMP_ID, EM.EMP_NAME, EM.IMAGE,TP.PROJECT_ID, TP.PROJECT_NAME,TM.MODULE_NAME,TL.ESTIMATED_TIME, TL.TASKLIST_ID, TL.IMAGE as TASK_IMAGE
                                    from ERP_TASK_TASK_ASSIGNEE TA
                                    left join ERP_PR_EMPLOYEE_LIST EM on TA.EMP_ID = EM.EMP_ID
                                    left join ERP_TASK_TASK_LIST TL on   TL.TASKLIST_ID = TA.TASKLIST_ID  
                                    left join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TL.PROJECT_ID
                                    left join ERP_TASK_MODULE TM on TM.MODULE_ID = TL.MODULE_ID
                                    where TA.isdelete = '0'  AND EM.SYS_USR_ID =:SYS_USR_ID ) TASN
                                    left join ERP_TASK_ACTIVITY TAC on TAC.TASK_ASSIGNEE_ID = TASN.TASK_ASSIGNEE_ID
                                    left join ERP_TASK_TEMPACTIVITY ETTEMP ON TASN.TASK_ASSIGNEE_ID = ETTEMP.TASK_ASSIGNEE_ID) ETA
                                    left join ERP_TASK_STATUS_LIST etsl on etsl.TASK_STATUS_LIST_ID = ETA.STATUS_LIST_ID
                                    where etsl.STATUS_NAME <> 'Close' and ETA.STATUS_LIST_ID is null )
                                                                    union
                                                                    (
                                    select TEMPACTIVITY_TYPE, TIME_EXTEND_NOTE, TASK_ASSIGNEE_ID, TASK_DETAILS, PRIORITY, ASSIGNEE_DATE, EMP_ID, EMP_NAME,PROJECT_ID, PROJECT_NAME,MODULE_NAME,ESTIMATED_TIME, TASKLIST_ID, TASK_IMAGE from
                                    (select ETTEMP.TEMPACTIVITY_TYPE, ETTEMP.TIME_EXTEND_NOTE, TAC.ACTIVITY_ID, TASN.* from
                                    (select TL.STATUS_LIST_ID,TASK_ASSIGNEE_ID, TASK_DETAILS, PRIORITY, ASSIGNEE_DATE, TA.EMP_ID, EM.EMP_NAME, EM.IMAGE,TP.PROJECT_ID, TP.PROJECT_NAME,TM.MODULE_NAME,TL.ESTIMATED_TIME, TL.TASKLIST_ID, TL.IMAGE as TASK_IMAGE
                                    from ERP_TASK_TASK_ASSIGNEE TA
                                    left join ERP_PR_EMPLOYEE_LIST EM on TA.EMP_ID = EM.EMP_ID
                                    left join ERP_TASK_TASK_LIST TL on   TL.TASKLIST_ID = TA.TASKLIST_ID  
                                    left join ERP_TASK_PROJECT TP on TP.PROJECT_ID = TL.PROJECT_ID
                                    left join ERP_TASK_MODULE TM on TM.MODULE_ID = TL.MODULE_ID
                                    where TA.isdelete = '0'  AND EM.SYS_USR_ID =:SYS_USR_ID ) TASN
                                    left join ERP_TASK_ACTIVITY TAC on TAC.TASK_ASSIGNEE_ID = TASN.TASK_ASSIGNEE_ID
                                    left join ERP_TASK_TEMPACTIVITY ETTEMP ON TASN.TASK_ASSIGNEE_ID = ETTEMP.TASK_ASSIGNEE_ID) ETA
                                    left join ERP_TASK_STATUS_LIST etsl on etsl.TASK_STATUS_LIST_ID = ETA.STATUS_LIST_ID  where etsl.STATUS_NAME <> 'Close' ) order by ASSIGNEE_DATE desc  ";

                }



                var taskAssignList = await conn.QueryAsync<TaskAssign>(strSql, param);

                return taskAssignList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskAssign>> GetTaskAssignWiseName(string UserId)
        {
            string strSql = "";
            try
            {
                using var conn = new SqliteConnection(_conString);

                //string qryUser = "select EMP_ID,SYS_USR_GRP_TITLE from erp_pr_employee_list el ,ERP_CM_SYSTEM_USER_GROUP sug, erp_cm_system_users su " +
                //"  where sug.SYS_USR_GRP_ID=su.SYS_USR_GRP_ID and su.SYS_USR_ID=el.SYS_USR_ID and  el.SYS_USR_ID = :SYS_USR_ID  ";
                //var param = new { SYS_USR_ID = UserId };
                //var userGroup = await conn.QueryAsync<SystemUser>(qryUser, param);


                //if (userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Admin(TaskManagement)" || userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Cloud Administrator")
                //{
                    strSql = " select distinct  TL.TASKLIST_ID,TL.TASK_DETAILS,ATL.TASK_ASSIGNEE_ID FROM ERP_TASK_TASK_ASSIGNEE ATL,ERP_TASK_TASK_LIST TL where TL.TASKLIST_ID=ATL.TASKLIST_ID   ";
               // }
                //if(userGroup.FirstOrDefault().SYS_USR_GRP_TITLE == "Developer")
                //{
                //    strSql = " select distinct  TL.TASKLIST_ID,TL.TASK_DETAILS,ATL.TASK_ASSIGNEE_ID FROM ERP_TASK_TASK_ASSIGNEE ATL,ERP_TASK_TASK_LIST TL where TL.TASKLIST_ID=ATL.TASKLIST_ID and   ";
                //}
                 var taskAssignList = await conn.QueryAsync<TaskAssign>(strSql);

                return taskAssignList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<TaskAssign> CreateTaskAssign(string taskId, string priority, DateTime AssignDate, string createBy, string empId)
        {
            try
            {
                string strSql = " INSERT INTO ERP_TASK_TASK_ASSIGNEE  (TASKLIST_ID, IPRIORITY, ASSIGNEE_DATE, CREATE_BY, EMP_ID) " +
                                "VALUES (:TASKLIST_ID, :PRIORITY, :ASSIGNEE_DATE, :CREATE_BY, :EMP_ID) ";

                var parameter = new
                {
                    TASKLIST_ID = taskId,
                    PRIORITY = priority,
                    ASSIGNEE_DATE = AssignDate,
                    CREATE_BY = createBy,
                    EMP_ID = empId
                };

                using var conn = new SqliteConnection(_conString);
                var taskAssign = await conn.QueryAsync<TaskAssign>(strSql, parameter);
                // var 
                return null;// user.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskAssign>> GeTaskAssignListById(string taskAssignId)
        {
            try
            {
                string strSql = " SELECT * FROM ERP_TASK_TASK_ASSIGNEE  WHERE TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID ";

                var parameter = new { TASK_ASSIGNEE_ID = taskAssignId };

                using var conn = new SqliteConnection(_conString);
                var taskAssign = await conn.QueryAsync<TaskAssign>(strSql, parameter);

                return taskAssign.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskAssign>> UpdateTaskAssignListByID(string taskAssignId, string taskId, string priority, string updateBy, string empId)
        {
            try
            {
                string strSql = " UPDATE ERP_TASK_TASK_ASSIGNEE set TASKLIST_ID=:TASKLIST_ID, ASSIGNEE_DATE=:ASSIGNEE_DATE, " +
                                " PRIORITY=:PRIORITY, EMP_ID=:EMP_ID, UPDATE_BY=:UPDATE_BY where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID ";

                var parameter = new
                {
                    TASK_ASSIGNEE_ID = taskAssignId,
                    TASKLIST_ID = taskId,
                    ASSIGNEE_DATE = DateTime.Today,
                    PRIORITY = priority,
                    EMP_ID = empId,
                    UPDATE_BY = updateBy
                };

                using var conn = new SqliteConnection(_conString);
                var taskAssign = await conn.QueryAsync<TaskAssign>(strSql, parameter);
                var taskAssignList = await GeTaskAssignListById(taskAssignId);
                return taskAssignList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> IsDeleteTaskAssignById(string taskAssignId)
        {
            try
            {
                string strSql = " UPDATE ERP_TASK_TASK_ASSIGNEE set ISDELETE=1  where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID ";

                var parameter = new { TASK_ASSIGNEE_ID = taskAssignId };

                using var conn = new SqliteConnection(_conString);
                var taskAssign = await conn.QueryAsync<TaskAssign>(strSql, parameter);

                return "Task" + taskAssignId + " is deleted";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> TaskSatarPause(string taskAssignId, string TaskActivityType, string UserId, string empId)
        {
            try
            {
                string returnString = string.Empty;
                using var conn = new SqliteConnection(_conString);
                string getAssignId = "SELECT * FROM ERP_TASK_TEMPACTIVITY  WHERE TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID";
                var param = new { TASK_ASSIGNEE_ID = taskAssignId };
                var AssignId = await conn.QueryAsync<TaskTempActivitry>(getAssignId, param);
                switch (TaskActivityType)
                {
                    case "Start":
                        if (AssignId.Count() > 0)
                        {
                            string strStartSql = " UPDATE ERP_TASK_TEMPACTIVITY set TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID, TEMPACTIVITY_TYPE =:TEMPACTIVITY_TYPE, " +
                                                " PAUSE_TIME=:PAUSE_TIME, START_TIME=:START_TIME, UPDATE_BY=:UPDATE_BY where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID ";
                            var parameterStart = new
                            {
                                TASK_ASSIGNEE_ID = taskAssignId,
                                TEMPACTIVITY_TYPE = TaskActivityType,
                                START_TIME = DateTime.Now,
                                PAUSE_TIME = DateTime.Now,
                                UPDATE_BY = UserId
                            };
                            await conn.QueryAsync(strStartSql, parameterStart);
                            returnString = "Task Pause";
                        }
                        else
                        {
                            string strSql = " INSERT INTO ERP_TASK_TEMPACTIVITY  (TASK_ASSIGNEE_ID, TEMPACTIVITY_TYPE, START_TIME, PAUSE_TIME, CREATE_BY) " +
                                            "VALUES (:TASK_ASSIGNEE_ID, :TEMPACTIVITY_TYPE, :START_TIME, :PAUSE_TIME, :CREATE_BY) ";
                            var parameter = new
                            {
                                TASK_ASSIGNEE_ID = taskAssignId,
                                TEMPACTIVITY_TYPE = TaskActivityType,
                                START_TIME = DateTime.Now,
                                PAUSE_TIME = "",
                                CREATE_BY = UserId
                            };
                            await conn.QueryAsync(strSql, parameter);
                            returnString = "Task Start";
                        }
                        string sql1 = " UPDATE ERP_TASK_TASK_ASSIGNEE SET PRIORITY ='200607010015000001'  where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID  ";
                        var paramAssignStatusUpdate = new { TASK_ASSIGNEE_ID= taskAssignId };
                        await conn.QueryAsync(sql1, paramAssignStatusUpdate);

                        string activityStartSql = " INSERT INTO ERP_TASK_ACTIVITY  (START_TIME,TASK_ACTIVITY_DATE,TASK_STATUS_LIST_ID,EMP_ID,TASK_ASSIGNEE_ID,CREATE_BY) " +
                                            "VALUES (:START_TIME,:TASK_ACTIVITY_DATE,:TASK_STATUS_LIST_ID,:EMP_ID,:TASK_ASSIGNEE_ID, :CREATE_BY) ";
                        var activityStartParameter = new
                        {
                            START_TIME = DateTime.Now,
                            TASK_ACTIVITY_DATE = DateTime.Now,
                            TASK_STATUS_LIST_ID = "200607010015000001",
                            EMP_ID = empId,
                            TASK_ASSIGNEE_ID = taskAssignId,
                            CREATE_BY = UserId
                        };
                        await conn.QueryAsync(activityStartSql, activityStartParameter);
                        returnString = "Task Start";

                        break;
                    case "Pause":
                        if (AssignId.Count() > 0)
                        {
                            string strPauseSql = " UPDATE ERP_TASK_TEMPACTIVITY set TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID, TEMPACTIVITY_TYPE =:TEMPACTIVITY_TYPE, " +
                                                 " PAUSE_TIME=:PAUSE_TIME, UPDATE_BY=:UPDATE_BY where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID ";
                            var parameterPause = new
                            {
                                TASK_ASSIGNEE_ID = taskAssignId,
                                TEMPACTIVITY_TYPE = TaskActivityType,
                                PAUSE_TIME = DateTime.Now,
                                UPDATE_BY = UserId
                            };
                            await conn.QueryAsync(strPauseSql, parameterPause);
                            returnString = "Task Pause";
                        }

                        string activitySql = " SELECT ACTIVITY_ID FROM ERP_TASK_ACTIVITY  WHERE TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID AND EMP_ID=:EMP_ID AND END_TIME IS NULL ";
                        var activityParameter = new
                        {
                            TASK_ASSIGNEE_ID = taskAssignId,
                            EMP_ID = empId
                        };
                        var activity = await conn.QueryAsync<TaskActivity>(activitySql, activityParameter);
                        var activityId = activity.FirstOrDefault().ACTIVITY_ID;

                        string activityPauseSql = " UPDATE ERP_TASK_ACTIVITY set END_TIME=:END_TIME, TASK_ACTIVITY_DATE =:TASK_ACTIVITY_DATE, TASK_STATUS_LIST_ID=:TASK_STATUS_LIST_ID, " +
                                                    "EMP_ID=:EMP_ID, TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID, UPDATE_BY=:UPDATE_BY WHERE ACTIVITY_ID=:ACTIVITY_ID ";
                        var activityPauseParameter = new
                        {
                            ACTIVITY_ID = activityId,
                            END_TIME = DateTime.Now,
                            TASK_ACTIVITY_DATE = DateTime.Now,
                            TASK_STATUS_LIST_ID = "200623010015000003",
                            EMP_ID = empId,
                            TASK_ASSIGNEE_ID = taskAssignId,
                            UPDATE_BY = UserId
                        };
                        await conn.QueryAsync(activityPauseSql, activityPauseParameter);

                        break;
                }

                return returnString;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> AssignTimeExtend(string employeeId, string taskAssignId, string timeExtendNote, string decryptExtendTime)
        {
            try
            {
                string returnString = string.Empty;
                string title = "Time Extend"; string strNotification = "";
                using var conn = new SqliteConnection(_conString);
                string strPauseSql = " UPDATE ERP_TASK_TEMPACTIVITY set TIME_EXTEND_NOTE=:TIME_EXTEND_NOTE,EXTEND_TIME=:EXTEND_TIME where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID ";
                var parameterPause = new
                {
                    TIME_EXTEND_NOTE = timeExtendNote,
                    TASK_ASSIGNEE_ID = taskAssignId,
                    EXTEND_TIME = decryptExtendTime
                };
                await conn.QueryAsync(strPauseSql, parameterPause);
                string strTaskSeenBy = "select EL.EMP_ID from ERP_TASK_TASK_ASSIGNEE TAS,ERP_PR_EMPLOYEE_LIST EL where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID AND TAS.CREATE_BY= EL.SYS_USR_ID ";
                var taskAssignByPerson = await conn.QueryAsync<TaskActivity>(strTaskSeenBy, parameterPause);
                string strTaskCreateBy = "select TAS.NOTIFICATION_ID from ERP_TASK_NOTIFICATION TAS where LINK=:TASK_ASSIGNEE_ID  ";
                var taskAssignPerson = await conn.QueryAsync<TaskNotification>(strTaskCreateBy, parameterPause);
                if (taskAssignPerson.Count() > 0)
                {
                    strNotification = " update erp_task_notification set SEEN_STATUS='0' where NOTIFICATION_ID= '" +taskAssignPerson.FirstOrDefault().NOTIFICATION_ID +"'  ";
                }
                else
                {

                    strNotification = " INSERT INTO ERP_TASK_NOTIFICATION (TITLE,MESSAGE,SEEN_STATUS,SEEN_BY,LINK,CREATE_BY ) VALUES (:TITLE,:MESSAGE,'0',:SEEN_BY,:LINK,:CREATE_BY ) ";
                }
                var paramNotification = new
                {
                    SEEN_BY = taskAssignByPerson.FirstOrDefault().EMP_ID,
                    LINK = taskAssignId,
                    MESSAGE = timeExtendNote,
                    TITLE = title,
                    CREATE_BY = employeeId
                };
                await conn.QueryAsync(strNotification, paramNotification);
                returnString = "Time Extended";

                return returnString;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> DeleteTaskAssignById(string taskAssignId)
        {
            try
            {
                string strSql = " SELECT * from ERP_TASK_TASK_ASSIGNEE  where TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID ";

                var parameter = new { TASK_ASSIGNEE_ID = taskAssignId };

                using var conn = new SqliteConnection(_conString);
                var taskAssign = await conn.QueryAsync(strSql, parameter);

                return "Task" + taskAssignId + " is deleted";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

    }
}
