using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using AutoMapper;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class StatusListManager : IStatusList
    {
        private CommonRepository _commonRepository;
        private StatusListRepository _statusListRepository;
        private readonly IMapper _mapper;

        public StatusListManager(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        #region task status category

        public async Task<IEnumerable<TaskStatusListDto>> GetStatusList(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _statusListRepository = new StatusListRepository();
                var statusDetails = await _statusListRepository.GetStatusList();
                var StatusDetailsMap = _mapper.Map<IEnumerable<TaskStatusListDto>>(statusDetails);

                strSysText = $"Get status list by { requestDetaildto.Username }";
                return StatusDetailsMap;
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
        public async Task<IEnumerable<TaskStatusListDto>> GetStatusByID(string statusId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptstatusId = await _commonRepository.Decrypt(statusId, requestDetaildto.SecurityKey);

                _statusListRepository = new StatusListRepository();
                var statusDetails = await _statusListRepository.GetStatusById(decryptstatusId);
                var statusDetailsMap = _mapper.Map<IEnumerable<TaskStatusListDto>>(statusDetails);

                strSysText = $"Get status list by Id by { requestDetaildto.Username }";
                return statusDetailsMap;
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

        public async Task<IEnumerable<TaskStatusListDto>> GetCategoryWiseStatusList(string CategoryId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptCategoryId = await _commonRepository.Decrypt(CategoryId, requestDetaildto.SecurityKey);

                _statusListRepository = new StatusListRepository();
                var statusDetails = await _statusListRepository.GetCategoryWiseStatusList(decryptCategoryId);
                var statusDetailsMap = _mapper.Map<IEnumerable<TaskStatusListDto>>(statusDetails);

                strSysText = $"Get status list by Id by { requestDetaildto.Username }";
                return statusDetailsMap;
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

        public async Task<string> CreateStatus(TaskStatusListDto dto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptsTATUS_NAME = await _commonRepository.Decrypt(dto.StatusName, requestDetaildto.SecurityKey);
                string decryptstatus_cat_id = await _commonRepository.Decrypt(dto.StatusCategoryId, requestDetaildto.SecurityKey);
                string decryptProgressStat = await _commonRepository.Decrypt(dto.ProgressStatus, requestDetaildto.SecurityKey);

                _statusListRepository = new StatusListRepository();
                var statusDetails = await _statusListRepository.CreateStatus(decryptsTATUS_NAME, decryptstatus_cat_id, decryptProgressStat, requestDetaildto.Username, DateTime.Today);

                strSysText = requestDetaildto.Username + " has successfully created a TaskModule";
                return statusDetails + " by " + requestDetaildto.Username;
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
        public async Task<TaskStatusListDto> UpdteStatusById(TaskStatusListDto dto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptsTATUS_NAME = await _commonRepository.Decrypt(dto.StatusName, requestDetaildto.SecurityKey);
                string decryptstatus_cat_id = await _commonRepository.Decrypt(dto.StatusCategoryId, requestDetaildto.SecurityKey);
                string decryptProgressStat = await _commonRepository.Decrypt(dto.ProgressStatus, requestDetaildto.SecurityKey);
                string decryptstatusId = await _commonRepository.Decrypt(dto.TaskStatusListId, requestDetaildto.SecurityKey);

                _statusListRepository = new StatusListRepository();
                var statusDetails = await _statusListRepository.UpdteStatusById(decryptstatusId, decryptsTATUS_NAME, decryptProgressStat, decryptstatus_cat_id, requestDetaildto.Username, DateTime.Today);
                var statusDetailsMap = _mapper.Map<TaskStatusListDto>(statusDetails);
                strSysText = requestDetaildto.Username + " has  created a status";
                return statusDetailsMap;
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
        public async Task<string> DeleteStatusById(string statusId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptstatusId = await _commonRepository.Decrypt(statusId, requestDetaildto.SecurityKey);

                _statusListRepository = new StatusListRepository();
                string statusDetails = await _statusListRepository.DeleteStatusById(decryptstatusId);

                strSysText = statusDetails;
                return statusDetails;
            }
            catch (Exception e)
            {
                strSysText = e.Message.ToString();
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
