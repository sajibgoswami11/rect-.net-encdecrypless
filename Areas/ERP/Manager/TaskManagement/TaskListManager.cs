using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using System.Text.RegularExpressions;
using BizWebAPI.Common;
using BizWebAPI.Areas.ERP.AutoMapper.TaskManagement;
using System.IO;
using System.Linq;

namespace BizWebAPI.Areas.ERP.Manager
{
    public class TaskListManager : ITaskList
    {
        private TaskImageListRepository _taskImageListRepository;
        private TaskListRepository _taskTaskListRepository;
        private CommonRepository _commonRepository;
        private readonly IMapper _mapper;
        public ServiceHandler objServiceHandler = new ServiceHandler();

        public TaskListManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<IEnumerable<TaskListDto>> GetTaskList(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskImageListRepository = new TaskImageListRepository();
                _taskTaskListRepository = new TaskListRepository();
                var taskList = await _taskTaskListRepository.GetTaskList();
                foreach (var item in taskList)
                {
                                      
                    var taskListsImage = await _taskImageListRepository.GetTaskImageById(item.TASKLIST_ID);
                    if(taskListsImage.Count() > 0)
                    {
                        item.IMAGE = "1";
                    }
                    else
                    {
                        item.IMAGE = "0";
                    }
                }
                var taskListMap = _mapper.Map<IEnumerable<TaskListDto>>(taskList);
                strSysText = requestDetaildto.Username + " has successfully fetched TaskList";
                return taskListMap;
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

        public async Task<TaskListDto> GetTaskById(string taskId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptask_id = await _commonRepository.Decrypt(taskId, requestDetaildto.SecurityKey);

                _taskTaskListRepository = new TaskListRepository();
                var taskLists = await _taskTaskListRepository.GeTaskById(decryptask_id);

                var taskListsMap = _mapper.Map<IEnumerable<TaskListDto>>(taskLists);
                List<string> imageUrls = new List<string>();
                foreach (var item in taskListsMap)
                {
                    imageUrls.Add(item.ImagePathTofolder);
                }
                var taskListDetailsById = taskListsMap.FirstOrDefault();
                taskListDetailsById.ImagePathUrls = imageUrls;
                strSysText = requestDetaildto.Username + " has successfully retrive a Task ";
                return taskListDetailsById;
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

        public async Task<string> CreateTask(IEnumerable<TaskCreateAssignDto> taskListDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string returnFilePath = "";
            string byteDataCheck = "";
            byte[] fileSource = null;
            int imageName = 0;
            string decryptassignLink = "";
            try
            {
                foreach (var item in taskListDto)
                {
                    string decryptModuleId = string.Empty;
                    _commonRepository = new CommonRepository();
                    string decryptProjectId = await _commonRepository.Decrypt(item.ProjectId, requestDetaildto.SecurityKey);
                    if (!string.IsNullOrEmpty(item.ModuleId))
                    {
                        decryptModuleId = await _commonRepository.Decrypt(item.ModuleId, requestDetaildto.SecurityKey);
                    }
                    string decryptTaskDetails = await _commonRepository.Decrypt(item.TaskDetails, requestDetaildto.SecurityKey);
                    // string decryptTaskImage = ""; // await _commonRepository.Decrypt(item.Image, requestDetaildto.SecurityKey);
                    string taskDate = await _commonRepository.Decrypt(item.TaskDate, requestDetaildto.SecurityKey);
                    Regex reg = new Regex("[\"]");
                    DateTime decryptTaskDate = Convert.ToDateTime(reg.Replace(taskDate, ""));
                    string decryptEstimatedTime = await _commonRepository.Decrypt(item.EstimatedTime, requestDetaildto.SecurityKey);
                    string decryptTaskAssignTo = await _commonRepository.Decrypt(item.TaskAssignTo, requestDetaildto.SecurityKey);
                    string decryptassignPriority = await _commonRepository.Decrypt(item.Priority.ToString(), requestDetaildto.SecurityKey);
                    if (!string.IsNullOrEmpty(decryptassignLink))
                    {
                        decryptassignLink = await _commonRepository.Decrypt(item.Link.ToString(), requestDetaildto.SecurityKey);
                    }

                    objServiceHandler = new ServiceHandler();
                    List<string> returnFilePathList = new List<string>();

                    if (item.Image != null)
                    {
                        foreach (var base64String in item.Image)
                        {
                            imageName++;
                            if (!string.IsNullOrEmpty(base64String))
                            {

                                byteDataCheck = base64String.Substring(0, base64String.IndexOf(';'));
                                var fileData = base64String.Remove(0, base64String.IndexOf(',') + 1);
                                if (!string.IsNullOrEmpty(base64String))
                                {
                                    fileSource = Convert.FromBase64String(fileData);
                                    if (byteDataCheck == "data:image/jpeg" || byteDataCheck == "data:image/png" || byteDataCheck == "data:image/gif")
                                    {
                                        returnFilePath = objServiceHandler.SaveImageDirectory(Convert.ToString(imageName) + item.ImageName, fileSource);
                                    }
                                    else if (byteDataCheck == "data:application/pdf")
                                    {
                                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + item.ImageName + ".pdf");
                                        returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + item.ImageName + ".pdf";
                                        File.WriteAllBytes(filePath, fileSource);
                                    }
                                    else if (byteDataCheck == "data:application/octet-stream")
                                    {
                                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + item.ImageName + ".txt");
                                        returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + item.ImageName + ".txt";
                                        File.WriteAllBytes(filePath, fileSource);
                                    }
                                    else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                                    {
                                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + item.ImageName + ".docx");
                                        returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + item.ImageName + ".docx";
                                        File.WriteAllBytes(filePath, fileSource);
                                    }
                                    else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || byteDataCheck == "data:application/vnd.ms-excel")
                                    {
                                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + item.ImageName + ".xls");
                                        returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + item.ImageName + ".xls";
                                        File.WriteAllBytes(filePath, fileSource);
                                    }
                                }
                                returnFilePathList.Add(returnFilePath);
                            }
                        }
                    }


                    _taskTaskListRepository = new TaskListRepository();
                    var taskDetails = await _taskTaskListRepository.CreateTask(
                                                                                decryptProjectId,
                                                                                decryptModuleId,
                                                                                decryptTaskDetails,
                                                                                returnFilePathList,
                                                                                decryptTaskDate,
                                                                                decryptEstimatedTime,
                                                                                decryptTaskAssignTo,
                                                                                decryptassignPriority,
                                                                                decryptassignLink,
                                                                                requestDetaildto.UserId
                                                                                );
                }
                strSysText = requestDetaildto.Username + " has successfully created a Task project ";
                return " Successfully created a task project";
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

        public async Task<string> UpdateTaskByID(TaskCreateAssignDto taskListDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string returnFilePath = "";
            string byteDataCheck = "";
            byte[] fileSource = null;
            string decryptTaskAssignTo = "";
            string decryptModuleId = "";
            int imageName = 0;
            string decryptLink = "";
            try
            {
                TaskVMHelper TaskListAssignInfo = new TaskVMHelper();
                _commonRepository = new CommonRepository();
                string decryptTaskId = await _commonRepository.Decrypt(taskListDto.TaskAssignId, requestDetaildto.SecurityKey);
                string decryptProjectId = await _commonRepository.Decrypt(taskListDto.ProjectId, requestDetaildto.SecurityKey);
                if (taskListDto.ModuleId != null)
                {
                    decryptModuleId = await _commonRepository.Decrypt(taskListDto.ModuleId, requestDetaildto.SecurityKey);
                }

                string decryptTaskDetails = await _commonRepository.Decrypt(taskListDto.TaskDetails, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(taskListDto.Link))
                {
                    decryptLink = await _commonRepository.Decrypt(taskListDto.Link, requestDetaildto.SecurityKey);
                }
                //string decryptTaskImage = ""; // await _commonRepository.Decrypt(taskListDto.Image, requestDetaildto.SecurityKey);
                string taskDate = await _commonRepository.Decrypt(taskListDto.TaskDate, requestDetaildto.SecurityKey);
                Regex reg = new Regex("[\"]");
                DateTime decryptTaskDate = Convert.ToDateTime(reg.Replace(taskDate, ""));
                string decryptEstimatedTime = await _commonRepository.Decrypt(taskListDto.EstimatedTime, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(taskListDto.TaskAssignTo))
                {
                    decryptTaskAssignTo = await _commonRepository.Decrypt(taskListDto.TaskAssignTo, requestDetaildto.SecurityKey);
                }
                string decryptassignPriority = await _commonRepository.Decrypt(taskListDto.Priority.ToString(), requestDetaildto.SecurityKey);

                objServiceHandler = new ServiceHandler();
                List<string> returnFilePathList = new List<string>();

                if (taskListDto.Image != null)
                {
                    foreach (var base64String in taskListDto.Image)
                    {
                        imageName++;
                        if (!string.IsNullOrEmpty(base64String))
                        {
                            byteDataCheck = base64String.Substring(0, base64String.IndexOf(';'));
                            var fileData = base64String.Remove(0, base64String.IndexOf(',') + 1);
                            if (!string.IsNullOrEmpty(base64String))
                            {
                                fileSource = Convert.FromBase64String(fileData);
                                if (byteDataCheck == "data:image/jpeg" || byteDataCheck == "data:image/png" || byteDataCheck == "data:image/gif")
                                {
                                    returnFilePath = objServiceHandler.SaveImageDirectory(Convert.ToString(imageName) + taskListDto.ImageName, fileSource);
                                }
                                else if (byteDataCheck == "data:application/pdf")
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + taskListDto.ImageName + ".pdf");
                                    returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + taskListDto.ImageName + ".pdf";
                                    File.WriteAllBytes(filePath, fileSource);
                                }
                                else if (byteDataCheck == "data:application/octet-stream" || byteDataCheck == "data:text/plain")
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + taskListDto.ImageName + ".txt");
                                    returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + taskListDto.ImageName + ".txt";
                                    File.WriteAllBytes(filePath, fileSource);
                                }
                                else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + taskListDto.ImageName + ".docx");
                                    returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + taskListDto.ImageName + ".docx";
                                    File.WriteAllBytes(filePath, fileSource);
                                }
                                else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || byteDataCheck == "data:application/vnd.ms-excel")
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + taskListDto.ImageName + ".xls");
                                    returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + taskListDto.ImageName + ".xls";
                                    File.WriteAllBytes(filePath, fileSource);
                                }
                            }
                            returnFilePathList.Add(returnFilePath);
                        }
                    }
                }

                _taskTaskListRepository = new TaskListRepository();
                var TaskCreateAssignList = await _taskTaskListRepository.UpdateTaskByID(
                                                                                        decryptTaskId,
                                                                                        decryptProjectId,
                                                                                        decryptModuleId,
                                                                                        decryptTaskDetails,
                                                                                        decryptLink,
                                                                                        returnFilePathList,
                                                                                        decryptTaskDate,
                                                                                        decryptEstimatedTime,
                                                                                        decryptTaskAssignTo,
                                                                                        decryptassignPriority,
                                                                                        requestDetaildto.UserId
                                                                                        );
                //var TaskCreateAssignListMap = _mapper.Map<TaskCreateAssignDto>(TaskCreateAssignList);

                strSysText = requestDetaildto.Username + " has successfully updated a task project";
                return " Successfully updated a task project";
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

        public async Task<string> DeleteTaskById(string taskId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTaskId = await _commonRepository.Decrypt(taskId, requestDetaildto.SecurityKey);

                _taskTaskListRepository = new TaskListRepository();
                string taskDetails = await _taskTaskListRepository.DeleteTaskById(decryptTaskId);

                if (taskDetails == "deleted")
                {
                    strSysText = decryptTaskId + " Task is deleted";
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
