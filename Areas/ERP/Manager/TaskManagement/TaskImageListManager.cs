using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class TaskImageListManager : ITaskImageList
    {
        private TaskImageListRepository _taskImageListRepository;
        private CommonRepository _commonRepository;
        private readonly IMapper _mapper;

        public TaskImageListManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<IEnumerable<TaskImageDto>> GetTaskImageById(string taskId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypTaskId = await _commonRepository.Decrypt(taskId, requestDetaildto.SecurityKey);

                _taskImageListRepository = new TaskImageListRepository();
                var taskLists = await _taskImageListRepository.GetTaskImageById(decrypTaskId);
                var taskListsMap = _mapper.Map<IEnumerable<TaskImageDto>>(taskLists);

                return taskListsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<string> DeleteTaskImageById(string imageId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptImageid = await _commonRepository.Decrypt(imageId, requestDetaildto.SecurityKey);

                _taskImageListRepository = new TaskImageListRepository();
                var taskLists = await _taskImageListRepository.DeleteTaskImageById(decryptImageid);
                strSysText = "Image delete successfully";

                return strSysText;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }
    }
}
