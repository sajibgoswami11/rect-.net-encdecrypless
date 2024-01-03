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
    public class TaskProjectRepository
    {
        private readonly string _conString;
        public TaskProjectRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<string> CreateProject(string project_name, string milestones, string progress_status, string TeamId, string createdBy, List<string> decryptEmpList)
        {
            try
            {
                string strSql = " INSERT INTO ERP_TASK_PROJECT  (PROJECT_NAME,PROGRESS_STATUS,MILESTONES,TEAM_ID,CREATE_BY,ISDELETE)" +
                    " VALUES (:PROJECT_NAME,:PROGRESS_STATUS,:MILESTONES,:TEAM_ID,:CREATE_BY,'0') ";

                var parameter = new
                {
                    PROJECT_NAME = project_name,
                    PROGRESS_STATUS = progress_status,
                    MILESTONES = milestones,
                    TEAM_ID = TeamId,
                    CREATE_BY = createdBy

                };

                using var conn = new SqliteConnection(_conString);

                var project = await conn.QueryAsync<TaskProject>(strSql, parameter);

                // select project
                string getprojectId = "SELECT PROJECT_ID FROM ERP_TASK_PROJECT WHERE isdelete = '0' AND ROWNUM = '1' AND " +
                                    " PROJECT_NAME=:PROJECT_NAME AND PROGRESS_STATUS=:PROGRESS_STATUS AND " +
                                    " MILESTONES=:MILESTONES AND CREATE_BY=:CREATE_BY";
                var projectIdParameter = new
                {
                    PROJECT_NAME = project_name,
                    PROGRESS_STATUS = progress_status,
                    MILESTONES = milestones,
                    CREATE_BY = createdBy
                };
                var projectId = await conn.QuerySingleAsync<TaskProject>(getprojectId, projectIdParameter);

                if (!string.IsNullOrEmpty(TeamId))
                {
                    string taskSql = " UPDATE ERP_TASK_TEAM SET PROJECT_ID=:PROJECT_ID where TEAM_ID=:TEAM_ID ";

                    var TaskParam = new
                    {
                        TEAM_ID = TeamId,
                        PROJECT_ID = projectId.PROJECT_ID
                    };
                    var team = await conn.QueryAsync<TaskTeam>(taskSql, TaskParam);
                }

                foreach (var emp in decryptEmpList)
                {
                    string sqlProjectEmployeeMap = " insert into ERP_TASK_EMP_TEAM_MAPPING ( project_id , emp_id) values (:project_id, :emp_id )  ";
                    var paramProjectEmployeeMap = new { emp_id = emp, project_id = projectId.PROJECT_ID };
                    var updateModuleEmployeeMap = await conn.QueryAsync<TaskTeam>(sqlProjectEmployeeMap, paramProjectEmployeeMap);
                }



                return projectId.PROJECT_ID;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> UpdateProjectByID(string project_id, string project_name, string milestones, string progress_status, string TeamId, string updatedBy, List<string> decryptEmpList)
        
        
        {
            try
            {
                string getempmap = "";
                string strSql = " UPDATE ERP_TASK_PROJECT set PROJECT_NAME=:PROJECT_NAME,PROGRESS_STATUS=:PROGRESS_STATUS, " +
                    " MILESTONES=:MILESTONES,UPDATE_BY=:UPDATE_BY where PROJECT_ID=:PROJECT_ID ";

                var parameter = new
                {
                    PROJECT_NAME = project_name,
                    PROGRESS_STATUS = progress_status,
                    MILESTONES = milestones,
                    PROJECT_ID = project_id,
                    UPDATE_BY = updatedBy
                };

                using var conn = new SqliteConnection(_conString);
                var project = await conn.QueryAsync<TaskProject>(strSql, parameter);
                //// select project
                string getprojectId = "SELECT * FROM ERP_TASK_EMP_TEAM_MAPPING WHERE   PROJECT_ID=:PROJECT_ID";
                var projectIdParameter = new
                {
                    PROJECT_ID = project_id
                };

                var empMapping = await conn.QueryAsync<TaskEmpTeamMapping>(getprojectId, projectIdParameter);
                // remove person from project
                foreach (var map in empMapping)
                {
                    getempmap = " select * from ERP_TASK_EMP_TEAM_MAPPING where EMP_ID =:EMP_ID and PROJECT_ID =:PROJECT_ID ";
                    var paramProjectEmpMap = new
                    {
                        EMP_ID = map.EMP_ID,
                        PROJECT_ID = project_id
                    };
                    var getEmpmap = await conn.QueryAsync<TaskEmpTeamMapping>(getempmap, paramProjectEmpMap);

                    var match = decryptEmpList.FirstOrDefault(x => x == map.EMP_ID);

                    if (match == null)
                    {
                        foreach (var i in getEmpmap)
                        {
                            string sqlProjectEmployeeMap = " UPDATE ERP_TASK_EMP_TEAM_MAPPING set isdelete = '1' where GROUP_MAPPING_ID=:GROUP_MAPPING_ID ";
                            var paramProjectEmployeeMap = new { GROUP_MAPPING_ID = i.GROUP_MAPPING_ID };
                            await conn.QueryAsync<TaskEmpTeamMapping>(sqlProjectEmployeeMap, paramProjectEmployeeMap);
                        }
                    }

                }
                // add and update person to project

                foreach (var item in decryptEmpList)
                 {
                    getempmap = " select * from ERP_TASK_EMP_TEAM_MAPPING where EMP_ID =:EMP_ID and PROJECT_ID =:PROJECT_ID ";

                    var paramProjectEmpChkExist = new
                            {
                                EMP_ID = item,
                                PROJECT_ID = project_id
                            };
                            var chkProjectEmpmap = await conn.QueryAsync<TaskEmpTeamMapping>(getempmap, paramProjectEmpChkExist);
                            // if person not in project not even in delete=1
                            if (chkProjectEmpmap.Count() == 0)
                            {

                                string insertEmpForPrjMap = "insert into ERP_TASK_EMP_TEAM_MAPPING (EMP_ID,PROJECT_ID,ISDELETE) values (:EMP_ID,:PROJECT_ID,'0') ";
                                await conn.QueryAsync<TaskEmpTeamMapping>(insertEmpForPrjMap, paramProjectEmpChkExist);

                            }
                            else
                            {
                                foreach (var itemchkProjectEmpMap in chkProjectEmpmap)
                                {
                                    if (itemchkProjectEmpMap.ISDELETE == "1")
                                    {
                                        string sqlProjectEmployeeMap = " UPDATE ERP_TASK_EMP_TEAM_MAPPING set isdelete = '0' where GROUP_MAPPING_ID=:GROUP_MAPPING_ID ";
                                        var paramProjectEmployeeMap = new { GROUP_MAPPING_ID = itemchkProjectEmpMap.GROUP_MAPPING_ID };
                                        await conn.QueryAsync<TaskEmpTeamMapping>(sqlProjectEmployeeMap, paramProjectEmployeeMap);

                                    }
                                }
                            }
                  }
                return project_id;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskProject>> GetTaskProject()
        {
            try
            {
                string strSql = " SELECT * from ERP_TASK_PROJECT where ISDELETE='0' order by PROJECT_ID desc ";
                //string strSql = " SELECT * from ERP_TASK_PROJECT  where PROJECT_ID=:PROJECT_ID ";

                // var parameter = new { PROJECT_ID = project_id };

                using var conn = new SqliteConnection(_conString);
                var project = await conn.QueryAsync<TaskProject>(strSql, null);

                return project.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<TaskProject> GetProjectById(
                string project_id)
        {
            try
            {
                string strSql = " SELECT * from ERP_TASK_PROJECT  where PROJECT_ID=:PROJECT_ID ";

                var parameter = new { PROJECT_ID = project_id };

                using var conn = new SqliteConnection(_conString);
                var project = await conn.QuerySingleAsync<TaskProject>(strSql, parameter);

                return project;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<PrEmployeeList>> GetProjectWiseTeamMember(string ProjectId)
        {
            try
            {
                string strSql = " SELECT * from ERP_TASK_MODULE  where PROJECT_ID=:PROJECT_ID and ISDELETE='0' ";
                var parameter = new { PROJECT_ID = ProjectId };
                using var conn = new SqliteConnection(_conString);
                var taskModule = await conn.QueryAsync<TaskModule>(strSql, parameter);

                string teamListSql = "SELECT em.EMP_ID, em.emp_name from ERP_TASK_EMP_TEAM_MAPPING tm, ERP_PR_EMPLOYEE_LIST em where tm.EMP_ID=em.EMP_ID AND TEAM_ID=:TEAM_ID";
                var parameterTeamList = new { TEAM_ID = taskModule.FirstOrDefault().MODULE_ID };
                var teamList = await conn.QueryAsync<PrEmployeeList>(teamListSql, parameter);

                return teamList.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }



        internal async Task<string> DeleteProjectById(string project_id)
        {
            try
            {
                string returnText = string.Empty;
                string projectSql = " SELECT PROJECT_ID from ERP_TASK_MODULE where PROJECT_ID=:PROJECT_ID";
                var parameter = new { PROJECT_ID = project_id };
                using var conn = new SqliteConnection(_conString);
                var project = await conn.QueryAsync(projectSql, parameter);

                string taskListSql = " SELECT PROJECT_ID from ERP_TASK_TASK_LIST where PROJECT_ID=:PROJECT_ID";
                var taskList = await conn.QueryAsync(taskListSql, parameter);

                if (project.Count() == 0 && taskList.Count() == 0)
                {
                    string strSql = " UPDATE ERP_TASK_PROJECT SET ISDELETE='1' where PROJECT_ID=:PROJECT_ID ";
                    await conn.QueryAsync(strSql, parameter);
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
