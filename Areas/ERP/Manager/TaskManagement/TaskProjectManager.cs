using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class TaskProjectManager : ITaskProject
    {
        private TaskProjectRepository _taskProjectRepository;
        private CommonRepository _commonRepository;
        private readonly IMapper _mapper;

        public TaskProjectManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<string> CreateProject(TaskProjectDto projectDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                string decryptTeam = "";
                string decryptEmpId = "";
                string decryptProgressStat = "";
                _commonRepository = new CommonRepository();
                string decryptMilestone = await _commonRepository.Decrypt(projectDto.Milestone, requestDetaildto.SecurityKey);
                string decryptProjectName = await _commonRepository.Decrypt(projectDto.ProjectName, requestDetaildto.SecurityKey);
                if (projectDto.ProgressStatus != "" && projectDto.ProgressStatus != null)
                { 
                     decryptProgressStat = await _commonRepository.Decrypt(projectDto.ProgressStatus, requestDetaildto.SecurityKey);
                }
                if (projectDto.TeamId != "" && projectDto.TeamId != null)
                {
                    decryptTeam = await _commonRepository.Decrypt(projectDto.TeamId, requestDetaildto.SecurityKey);
                }
                List<string> decryptEmpList = new List<string>();
                
                foreach (var item in projectDto.EmpList)
                {
                   if( item.emp != ""  )
                    {
                        decryptEmpId = await _commonRepository.Decrypt(item.emp, requestDetaildto.SecurityKey);
                        decryptEmpList.Add(decryptEmpId);
                    }
                 }
                
                _taskProjectRepository = new TaskProjectRepository();

                var projectDetails = await _taskProjectRepository.CreateProject(
                                                                                decryptProjectName,
                                                                                decryptMilestone,
                                                                                decryptProgressStat,
                                                                                decryptTeam,
                                                                                requestDetaildto.UserId,
                                                                               decryptEmpList
                                                                                );

                strSysText = requestDetaildto.Username + " has successfully created a TaskProject";
                return projectDetails;
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

        public async Task<string> UpdateProjectByID(TaskProjectDto projectDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                string decryptTeam = "";
                string decryptEmpId = "";
                string decryptProgressStat = "";
                _commonRepository = new CommonRepository();
                string decryptProjectId = await _commonRepository.Decrypt(projectDto.ProjectId, requestDetaildto.SecurityKey);
                string decryptMilestone = await _commonRepository.Decrypt(projectDto.Milestone, requestDetaildto.SecurityKey);
                string decryptProjectName = await _commonRepository.Decrypt(projectDto.ProjectName, requestDetaildto.SecurityKey);
                if (projectDto.ProgressStatus != "" && projectDto.ProgressStatus != null)
                {
                    decryptProgressStat = await _commonRepository.Decrypt(projectDto.ProgressStatus, requestDetaildto.SecurityKey);
                }
                if (projectDto.TeamId != "" && projectDto.TeamId != null)
                {
                    decryptTeam = await _commonRepository.Decrypt(projectDto.TeamId, requestDetaildto.SecurityKey);
                }
                List<string> decryptEmpList = new List<string>();

                foreach (var item in projectDto.EmpList)
                {
                    if (item.emp != "")
                    {
                        decryptEmpId = await _commonRepository.Decrypt(item.emp, requestDetaildto.SecurityKey);
                        decryptEmpList.Add(decryptEmpId);
                    }
                }

                _taskProjectRepository = new TaskProjectRepository();

                var projectDetails = await _taskProjectRepository.UpdateProjectByID(
                                                                                    decryptProjectId,
                                                                                    decryptProjectName,
                                                                                    decryptMilestone,
                                                                                    decryptProgressStat,
                                                                                    decryptTeam,
                                                                                    requestDetaildto.UserId,
                                                                                    decryptEmpList
                                                                                );



                //string decryptTeamId = "";
                //_commonRepository = new CommonRepository();
                //string decryptProjectId = await _commonRepository.Decrypt(dto.ProjectId, requestDetaildto.SecurityKey);
                //string decryptProjectName = await _commonRepository.Decrypt(dto.ProjectName, requestDetaildto.SecurityKey);
                //string decryptMilestone = await _commonRepository.Decrypt(dto.Milestone, requestDetaildto.SecurityKey);
                //string decryptProgressStat = await _commonRepository.Decrypt(dto.ProgressStatus, requestDetaildto.SecurityKey);
                //if (dto.TeamId != "")
                //{
                //    decryptTeamId = await _commonRepository.Decrypt(dto.TeamId, requestDetaildto.SecurityKey);
                //}
                //_taskProjectRepository = new TaskProjectRepository();
                //var projectDetails = await _taskProjectRepository.UpdateProjectByID(
                //                                                                    decryptProjectId,
                //                                                                    decryptProjectName,
                //                                                                    decryptMilestone,
                //                                                                    decryptProgressStat,
                //                                                                    decryptTeamId,
                //                                                                    requestDetaildto.UserId,
                //                                                                    DateTime.Today
                //                                                                    );
                //var projectDetailsMap = _mapper.Map<IEnumerable<TaskProjectDto>>(projectDetails);

                strSysText = requestDetaildto.Username + " has successfully updated a task project " + decryptProjectName;
                return " TaskProject updated successfully ";
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

        public async Task<IEnumerable<TaskProjectDto>> GetTaskProject(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskProjectRepository = new TaskProjectRepository();
                var projectDetails = await _taskProjectRepository.GetTaskProject();
                var projectDetailsMap = _mapper.Map<IEnumerable<TaskProjectDto>>(projectDetails);

                strSysText = requestDetaildto.Username + " has successfully fetched TaskProjects";
                return projectDetailsMap;
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

        public async Task<TaskProjectDto> GetProjectById(string projectId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptProjectId = await _commonRepository.Decrypt(projectId, requestDetaildto.SecurityKey);

                _taskProjectRepository = new TaskProjectRepository();
                var projectDetails = await _taskProjectRepository.GetProjectById(decryptProjectId);
                var projectDetailsMap = _mapper.Map<TaskProjectDto>(projectDetails);

                strSysText = requestDetaildto.Username + " has successfully updated a TaskProject " + decryptProjectId;
                return projectDetailsMap;
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

        public async Task<IEnumerable<TaskTeamMemberListDto>> GetProjectWiseTeamMember(string projectId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptProjectId = await _commonRepository.Decrypt(projectId, requestDetaildto.SecurityKey);

                _taskProjectRepository = new TaskProjectRepository();
                var TeamMember = await _taskProjectRepository.GetProjectWiseTeamMember(decryptProjectId);
                var TeamMemberList = _mapper.Map<IEnumerable<TaskTeamMemberListDto>>(TeamMember);

                strSysText = requestDetaildto.Username + " has successfully updated a TaskModule " + decryptProjectId;
                return TeamMemberList;
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

        public async Task<string> DeleteProjectById(string projectId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptProject_id = await _commonRepository.Decrypt(projectId, requestDetaildto.SecurityKey);

                _taskProjectRepository = new TaskProjectRepository();
                string projectDetails = await _taskProjectRepository.DeleteProjectById(decryptProject_id);
                if(projectDetails == "deleted")
                {
                    strSysText = "Task project with id " + decryptProject_id + " is deleted ";
                }
                else
                {
                    strSysText = "Record can't delete for child record found";
                }                
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
    }
}
