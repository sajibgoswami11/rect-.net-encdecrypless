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
    public class TaskNotificationRepoitory
    {
        private readonly string _conString;
        public TaskNotificationRepoitory()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TaskNotification>> GetNotification()
        {
            string sqlTaskNotification = "";
            try
            {
                sqlTaskNotification = "select tn.NOTIFICATION_ID,tn.TITLE,tn.MESSAGE,tn.SEEN_STATUS,tn.SEEN_BY,tn.LINK," +
                    " tn.CREATE_BY ,tmpactivity.EXTEND_TIME from erp_task_notification tn,ERP_TASK_TEMPACTIVITY tmpactivity" +
                          " where tn.link=tmpactivity.TASK_ASSIGNEE_ID and tn.isdelete ='0' AND tn.SEEN_STATUS = '0'";

                using var conn = new SqliteConnection(_conString);
                var getNotification = await conn.QueryAsync<TaskNotification>(sqlTaskNotification, null);
                return getNotification;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> PostNotifications(string notificationId, string replacedTime)
        {
            string sqlTaskNotification = "";
            try
            {
                sqlTaskNotification = " update erp_task_notification set SEEN_STATUS ='1',EXTEND_TIME=:EXTEND_TIME where notification_id = :notification_id";
                using var conn = new SqliteConnection(_conString);
                var param = new { notification_id = notificationId,EXTEND_TIME = replacedTime };
                var postNotification = await conn.QueryAsync<TaskNotification>(sqlTaskNotification, param);

                return "Notification update success ";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskList>> TaskListIdEstimateTime(string notificationId, string assignId,string extendTimeAfterApvById)
        {
            try
            {
                string sqlTaskIdEstim = "";
                using var conn = new SqliteConnection(_conString);
                if (!string.IsNullOrEmpty(notificationId))
                {
                    sqlTaskIdEstim = "select link from erp_task_notification where notification_id =:notification_id ";
                    var param = new { notification_id = notificationId };
                    var linkData = await conn.QueryAsync<TaskNotification>(sqlTaskIdEstim, param);
                    string sql1 = "select EXTEND_TIME from ERP_TASK_TEMPACTIVITY tas where tas.TASK_ASSIGNEE_ID =:TASK_ASSIGNEE_ID ";
                    var param1 = new { TASK_ASSIGNEE_ID = linkData.FirstOrDefault().LINK };
                    var extendData = await conn.QueryAsync<TaskList>(sql1, param1);
                    if (extendData.Count() > 0)
                    {
                        return extendData;
                    }
                    else
                    { return null; }
                }
               else if(!string.IsNullOrEmpty(assignId))
                {
                    string sql1 = "select EXTEND_TIME from ERP_TASK_TEMPACTIVITY tas where tas.TASK_ASSIGNEE_ID =:TASK_ASSIGNEE_ID ";
                    var param1 = new { TASK_ASSIGNEE_ID = assignId };
                    var taskIdData = await conn.QueryAsync<TaskList>(sql1, param1);
                    if (taskIdData.Count() > 0)
                    {
                        return taskIdData;
                    }
                    else
                    { return null; }
                }
               else if (!string.IsNullOrEmpty(extendTimeAfterApvById))
                {
                    string sql1 = "select EXTEND_TIME from erp_task_notification tas where tas.link =:TASK_ASSIGNEE_ID ";
                    var param1 = new { TASK_ASSIGNEE_ID = extendTimeAfterApvById };
                    var taskIdData = await conn.QueryAsync<TaskList>(sql1, param1);
                    if (taskIdData.Count() > 0 )
                    {
                        return taskIdData; }
                    else
                    { return null; }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> AddExtendedTimetoEstimatedOne(string replacedTime, string taskId)
        {
            try
            {
                using var conn = new SqliteConnection(_conString);
                string sql2 = "update erp_task_task_list set ESTIMATED_TIME=:ESTIMATED_TIME where TASKLIST_ID=:TASKLIST_ID ";
                var param2 = new { ESTIMATED_TIME = replacedTime, TASKLIST_ID = taskId };
                await conn.QueryAsync<TaskList>(sql2, param2);

                return "success";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
        internal async Task<IEnumerable<TaskAssign>> TaskIdByAssignIdwise(string decryptAssignId)
        {
            string sqlTaskNotification = "";
            try
            {
                sqlTaskNotification = " select TASKLIST_ID from ERP_TASK_TASK_ASSIGNEE where TASK_ASSIGNEE_ID = :TASK_ASSIGNEE_ID";
                using var conn = new SqliteConnection(_conString);
                var param = new { TASK_ASSIGNEE_ID = decryptAssignId };
                var taskIdNotification = await conn.QueryAsync<TaskAssign>(sqlTaskNotification, param);
                return taskIdNotification;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
    }
}
