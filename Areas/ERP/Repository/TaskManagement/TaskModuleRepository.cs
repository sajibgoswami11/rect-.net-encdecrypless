using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using BizWebAPI.Common;
using Dapper;
using Microsoft.Data.Sqlite;
//using Oracle.ManagedDataAccess.Client;

namespace BizWebAPI.Areas.ERP.Repository.TaskManagement
{
    public class TaskModuleRepository
    {
        private readonly string _conString;

        public TaskModuleRepository()
        {
            _conString = DbContext.ERP_Connection;
        }


        internal async Task<IEnumerable<TaskModule>> GetTaskModule()
        {
            try
            {
                string strSql = " SELECT * from ERP_TASK_MODULE  where ISDELETE='0' order by MODULE_ID desc ";

                using var conn = new SqliteConnection(_conString);
                var taskModule = await conn.QueryAsync<TaskModule>(strSql, null);

                return taskModule.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskModule>> GetModuleById(string moduleId)
        {
            try
            {
                string strSql = " SELECT * from ERP_TASK_MODULE  where MODULE_ID=:MODULE_ID ";

                var parameter = new { MODULE_ID = moduleId };

                using var conn = new SqliteConnection(_conString);
                var taskModule = await conn.QueryAsync<TaskModule>(strSql, parameter);

                return taskModule.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskModule>> GetModuleListByProjectId(string ProjectId)
        {
            try
            {
                string strSql = "  SELECT  TM.* from ERP_TASK_MODULE TM  " + // left join ERP_TASK_EMP_TEAM_MAPPING TMAP on TM.MODULE_ID= TMAP.MODULE_ID " +
                    " where TM.PROJECT_ID = :PROJECT_ID and TM.ISDELETE = '0'  ";

                var parameter = new { PROJECT_ID = ProjectId };

                using var conn = new SqliteConnection(_conString);
                var taskModule = await conn.QueryAsync<TaskModule>(strSql, parameter);

                return taskModule.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TasVMkEmpTeamMapping>> GetModuleWiseTeamMember(string ModuleId)
        {
            try
            {
                string strSql = " SELECT TM.TEAM_ID, EM.EMP_ID, EM.EMP_NAME from ERP_TASK_MODULE TM, ERP_TASK_EMP_TEAM_MAPPING TMM, ERP_PR_EMPLOYEE_LIST EM " +
                                " where TM.TEAM_ID = TMM.TEAM_ID AND tmm.EMP_ID=em.EMP_ID and TM.MODULE_ID=:MODULE_ID and TM.ISDELETE='0' ";

                var parameter = new { MODULE_ID = ModuleId };
                using var conn = new SqliteConnection(_conString);
                var taskModule = await conn.QueryAsync<TasVMkEmpTeamMapping>(strSql, parameter);

                return taskModule;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<TaskModule> CreateModule(string moduleId, string moduleName, string milestone, string progressStatus, string projectId, string createdBy, string teamId, List<string> decryptEmpList)
        {
            try
            {
                // string getEmpMap = "";
                string strChkEmpExist = "";
                using var conn = new SqliteConnection(_conString);
                if (!string.IsNullOrEmpty(moduleId))
                {
                    string strUpdateModule = " UPDATE ERP_TASK_MODULE set MODULE_NAME=:MODULE_NAME, PROGRESS_STATUS=:PROGRESS_STATUS, " +
                                       " MILESTONES=:MILESTONES, PROJECT_ID=:PROJECT_ID, UPDATE_BY=:UPDATE_BY, TEAM_ID=:TEAM_ID  where MODULE_ID=:MODULE_ID ";
                    var parameteUpdateModuler = new
                    {
                        MODULE_NAME = moduleName,
                        PROGRESS_STATUS = progressStatus,
                        MILESTONES = milestone,
                        MODULE_ID = moduleId,
                        PROJECT_ID = projectId,
                        TEAM_ID = teamId,
                        UPDATE_BY = createdBy
                    };

                    var updatetaskModule = await conn.QueryAsync<TaskModule>(strUpdateModule, parameteUpdateModuler);
                    // remove person from module
                    string strChkModuleeExistInMapTable = " select * from ERP_TASK_EMP_TEAM_MAPPING WHERE MODULE_ID=:MODULE_ID and  ISDELETE = '0'  ";
                    var moduleeExistInMapTable = await conn.QueryAsync<TaskEmpTeamMapping>(strChkModuleeExistInMapTable, parameteUpdateModuler);
                    foreach (var item in moduleeExistInMapTable)
                    {
                        if (!string.IsNullOrEmpty(item.EMP_ID))
                        {
                            var match = decryptEmpList.FirstOrDefault(x => x == item.EMP_ID);
                            if (match == null)
                            {

                                string sqlProjectEmployeeMap = "update ERP_TASK_EMP_TEAM_MAPPING set module_id=''  where module_id=:module_id and emp_id=:emp_id ";
                                var paramProjectEmployeeMap = new { emp_id = item.EMP_ID, project_id = projectId, module_id = moduleId, GROUP_MAPPING_ID = item.GROUP_MAPPING_ID };
                                await conn.QueryAsync<TaskEmpTeamMapping>(sqlProjectEmployeeMap, paramProjectEmployeeMap);
                            }
                        }
                    }
                    //add module for person
                    foreach (var emp in decryptEmpList)
                    {
                        strChkEmpExist = " select * from ERP_TASK_EMP_TEAM_MAPPING WHERE project_id=:project_id AND EMP_ID=:EMP_ID  and  ISDELETE = '0'    ";
                        var paramModuleEmployeeExist = new { project_id = projectId, emp_id = emp, module_id = moduleId };

                        var moduleEmployeeExistMap = await conn.QueryAsync<TaskEmpTeamMapping>(strChkEmpExist, paramModuleEmployeeExist);
                        //search if person has project not module then update module for person
                        if (moduleEmployeeExistMap.Count() > 0)
                        {
                            strChkEmpExist = "select module_id from ERP_TASK_EMP_TEAM_MAPPING where  EMP_ID=:EMP_ID and module_id = :module_id   ";
                            //
                            var chkEmpHasModule = await conn.QueryAsync<TaskEmpTeamMapping>(strChkEmpExist, paramModuleEmployeeExist);
                           if(chkEmpHasModule.Count() == 1)
                            {
                                string strChkEmpExistButIsdel = " select EMP_ID from ERP_TASK_EMP_TEAM_MAPPING WHERE MODULE_ID=:MODULE_ID AND EMP_ID=:EMP_ID  and  ISDELETE = '1' AND ROWNUM = '1'   ";
                                var moduleEmployeeExistMapIsDel = await conn.QueryAsync<TaskEmpTeamMapping>(strChkEmpExistButIsdel, paramModuleEmployeeExist);
                                foreach(var i in moduleEmployeeExistMapIsDel)
                                {
                                   var match = decryptEmpList.FirstOrDefault(x => x == i.EMP_ID);
                                    if (match != null)
                                    {
                                        string sqlProjectEmployeeMap = " update ERP_TASK_EMP_TEAM_MAPPING set ISDELETE = '0'  where emp_id=:emp_id and project_id=:project_id and module_id=:module_id ";
                                        await conn.QueryAsync<TaskEmpTeamMapping>(sqlProjectEmployeeMap, paramModuleEmployeeExist);
                                    }
                                }              
                            }
                            if (chkEmpHasModule.Count() == 0 && decryptEmpList.Count() > 0)
                            {
                                string strCheck = " select * from ERP_TASK_EMP_TEAM_MAPPING WHERE  EMP_ID=:emp_id and project_id =:project_id and MODULE_ID is null AND ISDELETE = '0'  ";
                                var chkPreExist = await conn.QueryAsync<TaskEmpTeamMapping>(strCheck, paramModuleEmployeeExist);
                                if (chkPreExist.Count() == 1)
                                {
                                    string sqlModuleEmployeeMap = "update ERP_TASK_EMP_TEAM_MAPPING  set module_id=:module_id where GROUP_MAPPING_ID=:GROUP_MAPPING_ID";
                                    var param = new { GROUP_MAPPING_ID = chkPreExist.FirstOrDefault().GROUP_MAPPING_ID, module_id = moduleId };
                                    var updateModuleEmployeeMap = await conn.QueryAsync<TaskEmpTeamMapping>(sqlModuleEmployeeMap, param);
                                }
                                else if(chkPreExist.Count() == 0)
                                {
                                    string sqlModuleEmployeeMap = "Insert into ERP_TASK_EMP_TEAM_MAPPING  (module_id, emp_id, project_id) values ( :module_id ,:emp_id,:project_id) ";
                                    var updateModuleEmployeeMap = await conn.QueryAsync<TaskEmpTeamMapping>(sqlModuleEmployeeMap, paramModuleEmployeeExist);

                                }

                            }
                        }
                       
                    }
                   
                }
                else
                {
                    string strSql = " INSERT INTO ERP_TASK_MODULE  (MODULE_NAME,PROGRESS_STATUS,MILESTONES,PROJECT_ID,TEAM_ID,CREATE_BY,ISDELETE)" +
                                    " VALUES (:MODULE_NAME,:PROGRESS_STATUS,:MILESTONES,:PROJECT_ID,:TEAM_ID,:CREATE_BY,'0') ";

                    var parameter = new
                    {
                        MODULE_NAME = moduleName,
                        PROGRESS_STATUS = progressStatus,
                        MILESTONES = milestone,
                        PROJECT_ID = projectId,
                        TEAM_ID = teamId,
                        CREATE_BY = createdBy
                    };
                    var taskModule = await conn.QueryAsync<TaskModule>(strSql, parameter);

                    string sqlGetModuleForEmployee = "select MODULE_ID FROM ERP_TASK_MODULE WHERE ISDELETE = '0' AND ROWNUM = '1' AND " +
                                                    "MODULE_NAME=:MODULE_NAME AND PROGRESS_STATUS=:PROGRESS_STATUS AND PROJECT_ID=:PROJECT_ID AND CREATE_BY=:CREATE_BY";
                    var moduleParam = new
                    {
                        MODULE_NAME = moduleName,
                        PROGRESS_STATUS = progressStatus,
                        MILESTONES = milestone,
                        PROJECT_ID = projectId,
                        CREATE_BY = createdBy
                    };
                    // add their module 
                    var getModuleId = await conn.QuerySingleAsync<TaskModule>(sqlGetModuleForEmployee, moduleParam);
                    foreach (var emp in decryptEmpList)
                    {
                        strChkEmpExist = " select * from ERP_TASK_EMP_TEAM_MAPPING WHERE project_id=:project_id AND EMP_ID=:EMP_ID  and  ISDELETE = '0'    ";
                        var paramModuleEmployeeExist = new { project_id = projectId, emp_id = emp, module_id = getModuleId.MODULE_ID };

                        var moduleEmployeeExistMap = await conn.QueryAsync<TaskEmpTeamMapping>(strChkEmpExist, paramModuleEmployeeExist);
                        //search if person has project not module then update module for person
                        if (moduleEmployeeExistMap.Count() > 0)
                        {
                            strChkEmpExist = "select module_id from ERP_TASK_EMP_TEAM_MAPPING where  EMP_ID=:EMP_ID and module_id = :module_id   ";
                            //
                            var chkEmpHasModule = await conn.QueryAsync<TaskEmpTeamMapping>(strChkEmpExist, paramModuleEmployeeExist);
                            if (chkEmpHasModule.Count() == 1)
                            {
                                string strChkEmpExistButIsdel = " select EMP_ID from ERP_TASK_EMP_TEAM_MAPPING WHERE MODULE_ID=:MODULE_ID AND EMP_ID=:EMP_ID  and  ISDELETE = '1' AND ROWNUM = '1'   ";
                                var moduleEmployeeExistMapIsDel = await conn.QueryAsync<TaskEmpTeamMapping>(strChkEmpExistButIsdel, paramModuleEmployeeExist);
                                foreach (var i in moduleEmployeeExistMapIsDel)
                                {
                                    var match = decryptEmpList.FirstOrDefault(x => x == i.EMP_ID);
                                    if (match != null)
                                    {
                                        string sqlProjectEmployeeMap = " update ERP_TASK_EMP_TEAM_MAPPING set ISDELETE = '0'  where emp_id=:emp_id and project_id=:project_id and module_id=:module_id ";
                                        await conn.QueryAsync<TaskEmpTeamMapping>(sqlProjectEmployeeMap, paramModuleEmployeeExist);
                                    }
                                }
                            }
                            if (chkEmpHasModule.Count() == 0 && decryptEmpList.Count() > 0)
                            {
                                string strCheck = " select * from ERP_TASK_EMP_TEAM_MAPPING WHERE  EMP_ID=:emp_id and project_id =:project_id and MODULE_ID is null AND ISDELETE = '0'  ";
                                var chkPreExist = await conn.QueryAsync<TaskEmpTeamMapping>(strCheck, paramModuleEmployeeExist);
                                if (chkPreExist.Count() == 1)
                                {
                                    string sqlModuleEmployeeMap = "update ERP_TASK_EMP_TEAM_MAPPING  set module_id=:module_id where GROUP_MAPPING_ID=:GROUP_MAPPING_ID";
                                    var param = new { GROUP_MAPPING_ID = chkPreExist.FirstOrDefault().GROUP_MAPPING_ID, module_id = getModuleId.MODULE_ID };
                                    var updateModuleEmployeeMap = await conn.QueryAsync<TaskEmpTeamMapping>(sqlModuleEmployeeMap, param);
                                }
                                 if (chkPreExist.Count() == 0)
                                {
                                    string sqlModuleEmployeeMap = "Insert into ERP_TASK_EMP_TEAM_MAPPING  (module_id, emp_id, project_id) values ( :module_id ,:emp_id,:project_id) ";
                                    var updateModuleEmployeeMap = await conn.QueryAsync<TaskEmpTeamMapping>(sqlModuleEmployeeMap, paramModuleEmployeeExist);

                                }
                            }
                        }
                    }

                  
                }

                return null;

            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> UpdateModuleByID(string moduleId, string moduleName, string milestone, string progressStatus, string projectId, string updatedBy, List<string> decryptEmpList, DateTime updatedate)
        {
            try
            {
                string strSql = " UPDATE ERP_TASK_MODULE set MODULE_NAME=:MODULE_NAME, PROGRESS_STATUS=:PROGRESS_STATUS, " +
                    " MILESTONES=:MILESTONES, PROJECT_ID=:PROJECT_ID, UPDATE_BY=:UPDATE_BY  where MODULE_ID=:MODULE_ID ";

                var parameter = new
                {
                    MODULE_NAME = moduleName,
                    PROGRESS_STATUS = progressStatus,
                    MILESTONES = milestone,
                    MODULE_ID = moduleId,
                    PROJECT_ID = projectId,
                    UPDATE_BY = updatedBy
                };

                using var conn = new SqliteConnection(_conString);
                var taskModule = await conn.QueryAsync<TaskModule>(strSql, parameter);

                foreach (var emp in decryptEmpList)
                {
                    string sqlModuleEmployeeMap = "update ERP_TASK_EMP_TEAM_MAPPING set module_id=:module_id where emp_id=:emp_id and project_id=:project_id ";
                    var paramModuleEmployeeMap = new { module_id = moduleId, emp_id = emp, project_id = projectId };
                    var updateModuleEmployeeMap = await conn.QueryAsync<TaskEmpTeamMapping>(sqlModuleEmployeeMap, paramModuleEmployeeMap);
                }


                return "update";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
        internal async Task<string> DeleteModuleById(string moduleId)
        {
            try
            {
                string returnText = string.Empty;
                var parameter = new { MODULE_ID = moduleId };
                string taskListSql = " SELECT MODULE_ID from ERP_TASK_TASK_LIST where MODULE_ID=:MODULE_ID";
                using var conn = new SqliteConnection(_conString);
                var taskList = await conn.QueryAsync(taskListSql, parameter);

                if (taskList.Count() == 0)
                {
                    string strSql = " UPDATE ERP_TASK_MODULE SET ISDELETE='1'  WHERE MODULE_ID =:MODULE_ID ";
                    await conn.QueryAsync(strSql, parameter);
                    returnText = "deleted";
                }
                else
                {
                    returnText = "child record found";
                }

                return returnText;
            }
            catch (Exception e)
            {
                string strResult = e.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<SystemUser>> GetPersonsOfModuleByAssignId(string employeeId)
        {
            string strSql = "";
            try
            {
                using var conn = new SqliteConnection(_conString);
                var parameter = new { TASK_ASSIGNEE_ID = employeeId };

                strSql = "  SELECT distinct EL.EMP_ID,EL.EMP_NAME from ERP_TASK_EMP_TEAM_MAPPING TMAP,ERP_PR_EMPLOYEE_LIST EL " +
                    " where TMAP.EMP_ID=EL.EMP_ID and TMAP.PROJECT_ID in " +
                   " (select  distinct ETP.PROJECT_ID FROM ERP_TASK_TASK_LIST TL, ERP_TASK_TASK_ASSIGNEE TAS,ERP_TASK_PROJECT ETP " +
                    " where TAS.TASKLIST_ID = TL.TASKLIST_ID and TL.PROJECT_ID =ETP.PROJECT_ID AND TAS.TASK_ASSIGNEE_ID=:TASK_ASSIGNEE_ID) ";


                var taskPersonOfModules = await conn.QueryAsync<SystemUser>(strSql, parameter);
                

                 return taskPersonOfModules ; 


            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
    }
}
