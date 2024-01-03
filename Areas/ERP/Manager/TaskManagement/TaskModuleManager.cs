using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Common;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class TaskModuleManager : ITaskModule
    {
        private TaskModuleRepository _taskModuleRepository;
        private CommonRepository _commonRepository;
        private readonly IMapper _mapper;

        public TaskModuleManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<string> CreateModule(IEnumerable<TaskModuleDto> taskModuleDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                string decryptTeam = "";
                string decryptMilestone = "";
                string decryptProgressStat = "";
                string decryptEmpId = "";
                string decryptmoduleId = "";
                _commonRepository = new CommonRepository();
                foreach (var dto in taskModuleDto)
                {
                    if (!string.IsNullOrEmpty(dto.ModuleId))
                    {
                        decryptmoduleId = await _commonRepository.Decrypt(dto.ModuleId, requestDetaildto.SecurityKey);
                    }

                    string decryptModuleName = await _commonRepository.Decrypt(dto.ModuleName, requestDetaildto.SecurityKey);
                    if (!string.IsNullOrEmpty(dto.Milestones))
                    {
                        decryptMilestone = await _commonRepository.Decrypt(dto.Milestones, requestDetaildto.SecurityKey);
                    }
                    if (!string.IsNullOrEmpty(dto.ProgressStatus))
                    {
                        decryptProgressStat = await _commonRepository.Decrypt(dto.ProgressStatus, requestDetaildto.SecurityKey);
                    }
                    string decryptProjectId = await _commonRepository.Decrypt(dto.ProjectId, requestDetaildto.SecurityKey);
                    if (!string.IsNullOrEmpty(dto.TeamId))
                    {
                        decryptTeam = await _commonRepository.Decrypt(dto.TeamId, requestDetaildto.SecurityKey);
                    }
                    List<string> decryptEmpList = new List<string>();
                    _taskModuleRepository = new TaskModuleRepository();
                    foreach (var item in dto.EmpList)
                    {
                        if (item.emp != "")
                        {
                            decryptEmpId = await _commonRepository.Decrypt(item.emp, requestDetaildto.SecurityKey);
                            decryptEmpList.Add(decryptEmpId);
                        }
                    }
                    var moduleDetails = await _taskModuleRepository.CreateModule(
                                                                            decryptmoduleId,
                                                                            decryptModuleName,
                                                                            decryptMilestone,
                                                                            decryptProgressStat,
                                                                            decryptProjectId,
                                                                            requestDetaildto.UserId,
                                                                            decryptTeam,
                                                                            decryptEmpList
                                                                            );
                }

                strSysText = requestDetaildto.Username + " has successfully updated task module";
                return "task module updated successfully";
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

        public async Task<string> UpdateModuleByID(TaskModuleDto taskModuleDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string decryptEmpId = "";
            try
            {
                List<string> decryptEmpList = new List<string>();
                _commonRepository = new CommonRepository();
                string decryptmoduleId = await _commonRepository.Decrypt(taskModuleDto.ModuleId, requestDetaildto.SecurityKey);
                string decryptModuleName = await _commonRepository.Decrypt(taskModuleDto.ModuleName, requestDetaildto.SecurityKey);
                string decryptMilestone = await _commonRepository.Decrypt(taskModuleDto.Milestones, requestDetaildto.SecurityKey);
                string decryptProgressStat = await _commonRepository.Decrypt(taskModuleDto.ProgressStatus, requestDetaildto.SecurityKey);
                string decryptProject_id = await _commonRepository.Decrypt(taskModuleDto.ProjectId, requestDetaildto.SecurityKey);
                foreach (var item in taskModuleDto.EmpList)
                {
                    if (item.emp != "")
                    {
                        decryptEmpId = await _commonRepository.Decrypt(item.emp, requestDetaildto.SecurityKey);
                        decryptEmpList.Add(decryptEmpId);
                    }
                }

                _taskModuleRepository = new TaskModuleRepository();
                var moduleDetails = await _taskModuleRepository.UpdateModuleByID(
                                                                                decryptmoduleId,
                                                                                decryptModuleName,
                                                                                decryptMilestone,
                                                                                decryptProgressStat,
                                                                                decryptProject_id,
                                                                                requestDetaildto.UserId,
                                                                                decryptEmpList,
                                                                                DateTime.Today
                                                                                );
                if (moduleDetails != null)
                {
                    strSysText = requestDetaildto.Username + " has successfully updated a TaskModule " + decryptModuleName;
                }

                return "task module updated successfully";
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

        public async Task<IEnumerable<TaskModuleDto>> GetTaskModule(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskModuleRepository = new TaskModuleRepository();
                var moduleDetails = await _taskModuleRepository.GetTaskModule();
                var moduleDetailsMap = _mapper.Map<IEnumerable<TaskModuleDto>>(moduleDetails);

                strSysText = requestDetaildto.Username + " has successfully fetched TaskModules";
                return moduleDetailsMap;
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

        public async Task<IEnumerable<TaskModuleDto>> GetModuleById(string moduleId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptmoduleId = await _commonRepository.Decrypt(moduleId, requestDetaildto.SecurityKey);

                _taskModuleRepository = new TaskModuleRepository();
                var moduleDetails = await _taskModuleRepository.GetModuleById(decryptmoduleId);
                var moduleDetailsMap = _mapper.Map<IEnumerable<TaskModuleDto>>(moduleDetails);

                strSysText = requestDetaildto.Username + " has successfully updated a TaskModule " + decryptmoduleId;
                return moduleDetailsMap;
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

        public async Task<IEnumerable<TaskModuleDto>> GetModuleListByProjectId(string ProjectId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptProjectId = await _commonRepository.Decrypt(ProjectId, requestDetaildto.SecurityKey);

                _taskModuleRepository = new TaskModuleRepository();
                var moduleDetails = await _taskModuleRepository.GetModuleListByProjectId(decryptProjectId);
                var moduleDetailsMap = _mapper.Map<IEnumerable<TaskModuleDto>>(moduleDetails);

                strSysText = requestDetaildto.Username + " has successfully updated a TaskModule " + decryptProjectId;
                return moduleDetailsMap;
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

        public async Task<IEnumerable<TaskEmpTeamMappingDto>> GetModuleWiseTeamMember(string ModuleId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptModuleId = await _commonRepository.Decrypt(ModuleId, requestDetaildto.SecurityKey);

                _taskModuleRepository = new TaskModuleRepository();
                var TeamMember = await _taskModuleRepository.GetModuleWiseTeamMember(decryptModuleId);
                var TeamMemberList = _mapper.Map<IEnumerable<TaskEmpTeamMappingDto>>(TeamMember);

                strSysText = requestDetaildto.Username + " has successfully updated a TaskModule " + decryptModuleId;
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



        public async Task<string> DeleteModuleById(string moduleId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptmoduleId = await _commonRepository.Decrypt(moduleId, requestDetaildto.SecurityKey);

                _taskModuleRepository = new TaskModuleRepository();
                var moduleDetails = await _taskModuleRepository.DeleteModuleById(decryptmoduleId);
                if (moduleDetails == "deleted")
                {
                    strSysText = "Task module with id " + decryptmoduleId + " is deleted ";
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

        public async Task<IEnumerable<ErpAuthLoginDto>> GetPersonsOfModuleByAssignId(string assignId, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptassignId = await _commonRepository.Decrypt(assignId, requestDetailDto.SecurityKey);

                _taskModuleRepository = new TaskModuleRepository();
                var modulePersons = await _taskModuleRepository.GetPersonsOfModuleByAssignId(decryptassignId);

                var moduleDetailsMap = _mapper.Map<IEnumerable<ErpAuthLoginDto>>(modulePersons);
                return moduleDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

    }
}
