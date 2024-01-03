using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class TeamManager : ITeamEmpMapping
    {

        private CommonRepository _commonRepository;
        private TeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        
        #region task team

        public async Task<IEnumerable<TaskTeamDetailsDto>> GetTaskTeams(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _teamRepository = new TeamRepository();
                var teamDetails = await _teamRepository.GetTaskTeams();
                var teamDetailsMap = _mapper.Map<IEnumerable<TaskTeamDetailsDto>>(teamDetails);

                strSysText = $"Get task team by {  requestDetaildto.Username }";
                return teamDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<TaskTeamDetailsDto>> GetTaskTeamById(string teamId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTeamID = await _commonRepository.Decrypt(teamId, requestDetaildto.SecurityKey);

                _teamRepository = new TeamRepository();
                var teamDetails = await _teamRepository.GetTaskTeamById(decryptTeamID);
                var teamDetailsMap = _mapper.Map<IEnumerable<TaskTeamDetailsDto>>(teamDetails);

                strSysText = requestDetaildto.Username + " has successfully retrive a Task ";
                return teamDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        #region task team emp maping
        public async Task<IEnumerable<TaskEmpTeamMappingDto>> GetTeamWiseEmployeeMapping(string TeamId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string teamId = await _commonRepository.Decrypt(TeamId, requestDetaildto.SecurityKey);

                _teamRepository = new TeamRepository();
                var teamEmpDetails = await _teamRepository.GetTeamWiseEmployeeMapping(teamId);
                var teamEmpDetailsMap = _mapper.Map<IEnumerable<TaskEmpTeamMappingDto>>(teamEmpDetails);

                strSysText = $"Employee Team mapping get by { requestDetaildto.Username }";
                return teamEmpDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        #endregion

        public async Task<string> CreateTeam(TeamCreateDto taskTeamDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTeamName = await _commonRepository.Decrypt(taskTeamDto.TeamName, requestDetaildto.SecurityKey);
                List<string> decryptEmpList = new List<string>();

                foreach (var decryptEmp in taskTeamDto.EmpList)
                {
                    var emp = await _commonRepository.Decrypt(decryptEmp.emp, requestDetaildto.SecurityKey);
                    decryptEmpList.Add(emp);
                }

                _teamRepository = new TeamRepository();
                var teamDetails = await _teamRepository.CreateTeam(decryptTeamName, decryptEmpList, requestDetaildto.UserId);
                
                strSysText = teamDetails + " is created by " + requestDetaildto.Username;
                return teamDetails;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<string> UpdateTeam(TeamCreateDto taskTeamDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTeamID = await _commonRepository.Decrypt(taskTeamDto.TeamId, requestDetaildto.SecurityKey);
                string decryptTeamName = await _commonRepository.Decrypt(taskTeamDto.TeamName, requestDetaildto.SecurityKey);
                List<string> decryptEmpList = new List<string>();

                foreach (var decryptEmp in taskTeamDto.EmpList)
                {
                    var emp = await _commonRepository.Decrypt(decryptEmp.emp, requestDetaildto.SecurityKey);
                    decryptEmpList.Add(emp);
                }

                _teamRepository = new TeamRepository();
                var teamDetails = await _teamRepository.UpdateTaskTeams(decryptTeamID, decryptTeamName, decryptEmpList, requestDetaildto.UserId);
                //var teamDetailsMap = _mapper.Map<IEnumerable<TaskTeamDto>>(teamDetails);

                strSysText = " Team with TEAM_ID " + decryptTeamID + " is updated by " + requestDetaildto.Username;
                return teamDetails;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<string> DeleteTaskTeams(string teamId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTeamID = await _commonRepository.Decrypt(teamId, requestDetaildto.SecurityKey);

                _teamRepository = new TeamRepository();
                string team = await _teamRepository.DeleteTaskTeams(decryptTeamID);

                strSysText = "  TeamId " + decryptTeamID + " is deleted by " + requestDetaildto.Username;
                return strSysText;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }
       
        

        #endregion

        

    }
}
