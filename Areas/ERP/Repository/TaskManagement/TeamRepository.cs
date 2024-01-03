using BizWebAPI.Areas.ERP.Dtos;
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
    public class TeamRepository
    {
        //private CommonRepository _commonRepository;

        private readonly string _conString;

        public TeamRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TasVMkEmpTeamMapping>> GetTaskTeams()
        {
            try
            {
                string strSql = "select * from ERP_TASK_TEAM  where isdelete='0' ";
                using var conn = new SqliteConnection(_conString);
                var teamDetails = await conn.QueryAsync<TasVMkEmpTeamMapping>(strSql);
                return teamDetails.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }

        }

        internal async Task<IEnumerable<TasVMkEmpTeamMapping>> GetTaskTeamById(string decryptTeamID)
        {
            try
            {
                string strSql = " select * from ERP_TASK_TEAM  where TEAM_ID=:TEAM_ID ";

                var parameter = new { TEAM_ID = decryptTeamID };

                using var conn = new SqliteConnection(_conString);
                var teamDetails = await conn.QueryAsync<TasVMkEmpTeamMapping>(strSql, parameter);

                return teamDetails;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TasVMkEmpTeamMapping>> GetTeamWiseEmployeeMapping(string TeamId)
        {
            try
            {
                string strSql = "SELECT tm.TEAM_ID, tm.EMP_ID, el.EMP_NAME, el.IMAGE " +
                    "FROM ERP_TASK_EMP_TEAM_MAPPING tm, ERP_PR_EMPLOYEE_LIST el " +
                    "WHERE tm.EMP_ID = el.EMP_ID AND TEAM_ID =:TEAM_ID AND tm.ISDELETE = '0'";

                var parameter = new
                {
                    TEAM_ID = TeamId
                };

                using var conn = new SqliteConnection(_conString);
                var team = await conn.QueryAsync<TasVMkEmpTeamMapping>(strSql, parameter);

                return team.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> CreateTeam(string decryptTeamName, List<string> decryptEmpList, string createdBy)
        {
            try
            {
                ////Team insert
                string teamSql = "insert into ERP_TASK_TEAM (TEAM_NAME, CREATE_BY) " +
                                "values (:TEAM_NAME, :CREATE_BY) ";
                var teamParameter = new
                {
                    TEAM_NAME = decryptTeamName,
                    CREATE_BY = createdBy
                };
                using var conn = new SqliteConnection(_conString);
                var team = await conn.QueryAsync<TaskTeam>(teamSql, teamParameter);

                ////TeamId select
                string geTeamId = "SELECT TEAM_ID FROM ERP_TASK_TEAM WHERE isdelete = '0' AND ROWNUM = '1' AND " +
                    "TEAM_NAME=:TEAM_NAME AND CREATE_BY=:CREATE_BY";
                var teamIdParameter = new
                {
                    TEAM_NAME = decryptTeamName,
                    CREATE_BY = createdBy
                };
                var teamId = await conn.QueryAsync<TaskTeam>(geTeamId, teamIdParameter);

                ////Team Member insert
                foreach (var emp in decryptEmpList)
                {
                    string teamMemberSql = "insert into ERP_TASK_EMP_TEAM_MAPPING (TEAM_ID, EMP_ID) " +
                                            "values (:TEAM_ID, :EMP_ID) ";
                    var teamMemberParam = new
                    {
                        TEAM_ID = teamId.FirstOrDefault().TEAM_ID,
                        EMP_ID = emp
                    };
                    await conn.QueryAsync<TaskTeam>(teamMemberSql, teamMemberParam);
                }

                return decryptTeamName + " team is created";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> UpdateTaskTeams(string decryptTeamID, string decryptTeamName, List<string> decryptEmpList, string updatedBy)
        {
            try
            {
                ////Team Update
                string taskSql = " UPDATE ERP_TASK_TEAM SET TEAM_NAME=:TEAM_NAME " +
                                " UPDATE_BY=:UPDATE_BY  where TEAM_ID=:TEAM_ID ";

                var TaskParam = new
                {
                    TEAM_ID = decryptTeamID,
                    TEAM_NAME = decryptTeamName,
                    UPDATE_BY = updatedBy
                };

                using var conn = new SqliteConnection(_conString);
                var team = await conn.QueryAsync<TaskTeam>(taskSql, TaskParam);

                ////Team member Update
                await conn.QueryAsync("DELETE FROM ERP_TASK_EMP_TEAM_MAPPING WHERE TEAM_ID='"+decryptTeamID+"' ", null);
                foreach (var emp in decryptEmpList)
                {
                    string teamMemberSql = "insert into ERP_TASK_EMP_TEAM_MAPPING (TEAM_ID, EMP_ID) " +
                                            "values (:TEAM_ID, :EMP_ID) ";
                    var teamMemberParam = new
                    {
                        TEAM_ID = decryptTeamID,
                        EMP_ID = emp
                    };
                    await conn.QueryAsync<TaskTeam>(teamMemberSql, teamMemberParam);
                }

                return decryptTeamName + " team is Updated";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> DeleteTaskTeams(string decryptTeamID)
        {
            try
            {
                //// Delete Team
                string teamSql = " UPDATE ERP_TASK_TEAM SET ISDELETE='1' where TEAM_ID=:TEAM_ID ";
                var teamParam = new { TEAM_ID = decryptTeamID };

                //// Delete Team Member
                string teamMemberSql = " UPDATE ERP_TASK_EMP_TEAM_MAPPING SET ISDELETE='1' where TEAM_ID=:TEAM_ID ";
                var teamMemberParam = new { TEAM_ID = decryptTeamID };

                using var conn = new SqliteConnection(_conString);
                await conn.QueryAsync(teamSql, teamParam);
                await conn.QueryAsync(teamMemberSql, teamMemberParam);

                return "  TeamId " + decryptTeamID + " is deleted";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        
    }
}
