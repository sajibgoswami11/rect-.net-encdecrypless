using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class TaskForwardManager : ITaskForward
    {
        private TaskForwardRepository _taskForwardRepository;
        private CommonRepository _commonRepository;
        private readonly IMapper _mapper;

        public TaskForwardManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<string> CreateTaskForward(TaskForwardDto taskForwarDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptAssigneeId = await _commonRepository.Decrypt(taskForwarDto.TaskAssigneeId, requestDetaildto.SecurityKey);
                string decryptForwardEmpId = await _commonRepository.Decrypt(taskForwarDto.EmpId, requestDetaildto.SecurityKey);
                string decryptForwardPriority = await _commonRepository.Decrypt(taskForwarDto.Priority, requestDetaildto.SecurityKey);

                _taskForwardRepository = new TaskForwardRepository();
                var taskForwardCreate = await _taskForwardRepository.CreateTaskForward(decryptAssigneeId, decryptForwardEmpId, decryptForwardPriority);

                strSysText = $"Task forwarded by { requestDetaildto.Username } Task assignee id { taskForwarDto.TaskAssigneeId } and employee id is { taskForwarDto.EmpId } ";
                return "Task Forward successfull";
            }
            catch (Exception ex)
            {
                strSysText= ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<string> DeleteTaskForwardById(string taskForwardId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTaskForwardId = await _commonRepository.Decrypt(taskForwardId, requestDetaildto.SecurityKey);

                _taskForwardRepository = new TaskForwardRepository();
                string taskForwardDelete = await _taskForwardRepository.DeleteTaskForwardById(decryptTaskForwardId);
                
                strSysText = $"Forward task delete by { requestDetaildto.Username }, task forward id { taskForwardId }";
                return taskForwardDelete;
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

        public async Task<IEnumerable<TaskForwardDto>> GetTaskForwardList(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskForwardRepository = new TaskForwardRepository();
                var taskForwardList = await _taskForwardRepository.GetTaskForwardList(requestDetaildto.UserId);
                var taskForwardListMap = _mapper.Map<IEnumerable<TaskForwardDto>>(taskForwardList);

                strSysText = requestDetaildto.Username + " has successfully forwarded a Task ";
                return taskForwardListMap;
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

        public async Task<IEnumerable<TaskForwardDto>> GetTaskForwardById(string taskForwardId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTaskForwardId = await _commonRepository.Decrypt(taskForwardId, requestDetaildto.SecurityKey);

                _taskForwardRepository = new TaskForwardRepository();
                var taskForward = await _taskForwardRepository.GetTaskForwardById(decryptTaskForwardId);
                var taskForwardMap = _mapper.Map<IEnumerable<TaskForwardDto>>(taskForward);

                strSysText = $"Task forward by { requestDetaildto.Username } and task forward id { taskForwardId }";
                return taskForwardMap;
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

        public async Task<string> UpdateTaskForwardByID(TaskForwardDto dto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTaskForwardId = await _commonRepository.Decrypt(dto.TaskForwardId, requestDetaildto.SecurityKey);
                string decryptTaskAssigneeId = await _commonRepository.Decrypt(dto.TaskAssigneeId, requestDetaildto.SecurityKey);
                string decryptTaskForwardPriority = await _commonRepository.Decrypt(dto.Priority, requestDetaildto.SecurityKey);
                string decryptTaskForwardEmployId = await _commonRepository.Decrypt(dto.EmpId, requestDetaildto.SecurityKey);

                _taskForwardRepository = new TaskForwardRepository();
                var updateTaskForward = await _taskForwardRepository.UpdateTaskForward(decryptTaskForwardId, decryptTaskAssigneeId, decryptTaskForwardPriority, decryptTaskForwardEmployId);
                //var updateTaskForwardMap = _mapper.Map<IEnumerable<TaskForwardDto>>(updateTaskForward);

                strSysText = $"Update task forward by { requestDetaildto.Username }, forward id { decryptTaskForwardId }, employee id { decryptTaskForwardEmployId }";
                return " Task forward update successful ";
            }
            catch(Exception ex)
            {
                strSysText= ex.Message.ToString();
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
