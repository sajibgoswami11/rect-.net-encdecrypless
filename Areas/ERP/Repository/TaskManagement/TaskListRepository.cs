using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
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
    public class TaskListRepository
    {
        private readonly string _conString;
        public TaskListRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TaskList>> GetTaskList()
        {
            try
            {
                string strSql = " SELECT TL.*, ATL.EMP_ID FROM ERP_TASK_TASK_LIST TL Left join " +
                                " ERP_TASK_TASK_ASSIGNEE ATL on TL.TASKLIST_ID = ATL.TASKLIST_ID " +
                                " where TL.isdelete = '0' order by TL.TASKLIST_ID desc ";

                using var conn = new SqliteConnection(_conString);
                var task_list = await conn.QueryAsync<TaskList>(strSql, null);
                
                return task_list.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskList>> GeTaskById(string task_id)
        {
            try
            {
                string strSql = @" SELECT TL.*,ETP.IMAGE_PATH,ETP.REFERENCE_ID	,ETP.IMAGE_ID, ATL.EMP_ID FROM ERP_TASK_TASK_LIST TL
                               left join ERP_TASK_TASK_ASSIGNEE ATL on TL.TASKLIST_ID = ATL.TASKLIST_ID
                               left join ERP_TASK_IMAGE ETP ON ETP.REFERENCE_ID = TL.TASKLIST_ID
                               where TL.isdelete = '0'AND TL.TASKLIST_ID=:TASKLIST_ID ";

                var parameter = new { TASKLIST_ID = task_id };

                using var conn = new SqliteConnection(_conString);
                var tasks = await conn.QueryAsync<TaskList>(strSql, parameter);

                return tasks;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> CreateTask(string project_id, string moduleId, 
                                            string task_detail,List<string> image,
                                            DateTime decryptTaskDate, string EstimatedTime, 
                                            string TaskAssignTo, string priority, string Link, string create_by)
        {
            try
            {
                using var conn = new SqliteConnection(_conString);

                string taskSql = " INSERT INTO ERP_TASK_TASK_LIST  (TASK_DATE,TASK_DETAILS,PROJECT_ID,CREATE_BY,MODULE_ID,ESTIMATED_TIME,LINK,STATUS_LIST_ID) " +
                                "VALUES (:TASK_DATE,:TASK_DETAILS,:PROJECT_ID,:CREATE_BY,:MODULE_ID,:ESTIMATED_TIME,:LINK, :STATUS_LIST_ID) ";

                var taskParameter = new
                {
                    PROJECT_ID = project_id,
                    MODULE_ID = moduleId,                    
                    TASK_DETAILS = task_detail,
                    TASK_DATE = decryptTaskDate,
                    LINK = Link,
                    //IMAGE = image,
                    ESTIMATED_TIME = EstimatedTime,
                    CREATE_BY = create_by,
                    STATUS_LIST_ID = priority,
                };

              
                var task = await conn.QueryAsync<TaskList>(taskSql, taskParameter);

                //string getTaskIdSql = "SELECT max(TASKLIST_ID) FROM ERP_TASK_TASK_LIST  WHERE isdelete = '0'AND ROWNUM = '1' AND " +
                //                    "PROJECT_ID=:PROJECT_ID AND CREATE_BY=:CREATE_BY AND TASK_DATE=:TASK_DATE  " +
                //                    " AND ESTIMATED_TIME=:ESTIMATED_TIME AND TASK_DETAILS=:TASK_DETAILS order by CREATE_DATE desc";
                string getTaskIdSql = @"SELECT max(TASKLIST_ID) TASKLIST_ID FROM ERP_TASK_TASK_LIST  WHERE isdelete = '0' AND CREATE_BY =:CREATE_BY   AND TASK_DETAILS =:TASK_DETAILS
      order by TASKLIST_ID desc ";

                var parameterTask = new
                {
                    PROJECT_ID = project_id,
                    CREATE_BY = create_by,
                    TASK_DATE = decryptTaskDate,
                    ESTIMATED_TIME = EstimatedTime,
                    TASK_DETAILS = task_detail
                };                

                var getTaskId = await conn.QueryAsync<TaskList>(getTaskIdSql, parameterTask);

                if (image.Count() > 0)
                {
                    foreach (var itemImage in image)
                    {
                        string sqlImagePath = " insert into erp_task_image (REFERENCE_ID,IMAGE_PATH) values (:REFERENCE_ID,:IMAGE_PATH) ";

                        var paramImagePath = new
                        {
                            REFERENCE_ID = getTaskId.FirstOrDefault().TASKLIST_ID,
                            IMAGE_PATH = itemImage
                        };
                        await conn.QueryAsync<TaskImage>(sqlImagePath, paramImagePath);
                    }
                }

                if (TaskAssignTo != "")
                {
                    string AssignstrSql = " INSERT INTO ERP_TASK_TASK_ASSIGNEE  (TASKLIST_ID,ASSIGNEE_DATE,PRIORITY,CREATE_BY,EMP_ID) " +
                                    "VALUES (:TASKLIST_ID,:ASSIGNEE_DATE,:PRIORITY,:CREATE_BY,:EMP_ID) ";
                    var AssignParameter = new
                    {
                        TASKLIST_ID = getTaskId.FirstOrDefault().TASKLIST_ID,
                        ASSIGNEE_DATE = DateTime.Now,
                        EMP_ID = TaskAssignTo,
                        PRIORITY = priority,
                        CREATE_BY = create_by                        
                    };

                    var assigned = await conn.QueryAsync(AssignstrSql, AssignParameter);
                }
                // var 
                return "records saved";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> UpdateTaskByID(string taskListId, string project_id, string moduleId,
                                            string task_detail, string decryptLink, List<string> image,
                                            DateTime decryptTaskDate, string EstimatedTime,
                                            string TaskAssignTo, string priority, string update_by)
        {
            try
            {
                using var conn = new SqliteConnection(_conString);

                string taskUpdateSql = " UPDATE ERP_TASK_TASK_LIST set PROJECT_ID =:PROJECT_ID, MODULE_ID=:MODULE_ID, TASK_DETAILS=:TASK_DETAILS, " +
                    " TASK_DATE=:TASK_DATE, ESTIMATED_TIME=:ESTIMATED_TIME, LINK=:LINK, UPDATE_BY=:UPDATE_BY,STATUS_LIST_ID=:STATUS_LIST_ID where TASKLIST_ID=:TASKLIST_ID ";

                var taskParameter = new
                {
                    TASKLIST_ID = taskListId,
                    PROJECT_ID = project_id,
                    MODULE_ID = moduleId,
                    TASK_DETAILS = task_detail,
                    TASK_DATE = decryptTaskDate,
                    STATUS_LIST_ID = priority,
                    LINK = decryptLink,
                    //IMAGE = image,
                    ESTIMATED_TIME = EstimatedTime,
                    UPDATE_BY = update_by
                };
                if (image.Count() > 0)
                {
                  foreach(var itemImage in image)
                    {
                        string sqlImagePath = " insert into erp_task_image (REFERENCE_ID,IMAGE_PATH) values (:REFERENCE_ID,:IMAGE_PATH) ";

                        var paramImagePath = new
                        {
                            REFERENCE_ID = taskListId,
                            IMAGE_PATH = itemImage
                        };
                        await conn.QueryAsync<TaskImage>(sqlImagePath, paramImagePath);

                    }
                }


                if(TaskAssignTo != "")
                {
                    string getAssign = "SELECT * from ERP_TASK_TASK_ASSIGNEE WHERE TASKLIST_ID=:TASKLIST_ID";

                    var paramAssign = new { TASKLIST_ID = taskListId };
                    var assign = await conn.QueryAsync<TaskAssign>(getAssign, paramAssign);

                    if(assign.Count() > 0 )
                    {
                        string AssignSql = " UPDATE ERP_TASK_TASK_ASSIGNEE SET TASKLIST_ID=:TASKLIST_ID, " +
                                           " PRIORITY=:PRIORITY, EMP_ID=:EMP_ID, UPDATE_BY=:UPDATE_BY where TASKLIST_ID=:TASKLIST_ID ";

                        var assignParameter = new
                        {
                            TASKLIST_ID = taskListId,
                            //ASSIGNEE_DATE = DateTime.Now,
                            EMP_ID = TaskAssignTo,
                            PRIORITY = priority,
                            UPDATE_BY = update_by
                        };
                        await conn.QueryAsync<TaskCreateAssign>(AssignSql, assignParameter);
                    }
                    else
                    {
                        string AssignstrSql = " INSERT INTO ERP_TASK_TASK_ASSIGNEE  (TASKLIST_ID,PRIORITY,CREATE_BY,EMP_ID) " +
                                    "VALUES (:TASKLIST_ID,:PRIORITY,:CREATE_BY,:EMP_ID) ";
                        var AssignParameter = new
                        {
                            TASKLIST_ID = taskListId,
                            //ASSIGNEE_DATE = DateTime.Now,
                            EMP_ID = TaskAssignTo,
                            PRIORITY = priority,
                            CREATE_BY = update_by
                        };

                        await conn.QueryAsync(AssignstrSql, AssignParameter);
                    }                    
                }
                
                return "records updated"; 
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> DeleteTaskById(string task_id)
        {
            try
            {
                string returnText = string.Empty;
                var parameter = new { TASKLIST_ID = task_id };
                string taskListSql = " SELECT TAC.TASK_ASSIGNEE_ID from ERP_TASK_TASK_ASSIGNEE TAS,ERP_TASK_ACTIVITY TAC where TAS.TASK_ASSIGNEE_ID=TAC.TASK_ASSIGNEE_ID and  TAS.TASKLIST_ID=:TASKLIST_ID";
                using var conn = new SqliteConnection(_conString);
                var taskList = await conn.QueryAsync(taskListSql, parameter);
                if (taskList.Count() == 0)
                {
                    string strSql = " UPDATE ERP_TASK_TASK_LIST set ISDELETE=1  where TASKLIST_ID=:TASKLIST_ID ";
                    var tasks = await conn.QueryAsync(strSql, parameter);
                    returnText = "deleted";
                }
                else
                {
                    returnText = "child record found";
                }

                return returnText;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }


    }
}
