using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using BizWebAPI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class EmployeeListManager : IEmployeeList
    {
        private CommonRepository _commonRepository;
        private EmployeeListRepository _getEmployeeRepository;
        public ServiceHandler objServiceHandler = new ServiceHandler();
        private readonly IMapper _mapper;

        public EmployeeListManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<IEnumerable<PrEmployeeListDto>> GetEmployees(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _getEmployeeRepository = new EmployeeListRepository();
                var employeeDetails = await _getEmployeeRepository.GetEmployees(requestDetaildto.UserId);
                var employeeDetailsMap = _mapper.Map<IEnumerable<PrEmployeeListDto>>(employeeDetails);

                strSysText = $"Get employees by { requestDetaildto.Username } ";
                return employeeDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<PrEmployeeListDto> GetEmployeeById(string EmployeeId, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            var decryptEmployeeId = "";
            try
            {
                _commonRepository = new CommonRepository();
                decryptEmployeeId = await _commonRepository.Decrypt(EmployeeId, requestDetailDto.SecurityKey);

                _getEmployeeRepository = new EmployeeListRepository();
                var employeeDetails = await _getEmployeeRepository.GetEmployeeById(decryptEmployeeId,"");
                var employeeDetailsMap = _mapper.Map<PrEmployeeListDto>(employeeDetails);
                return employeeDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<IEnumerable<PrEmployeeListDto>> GetEmployeeByProjectModule(TaskActivityProjectModule projectModule, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                string decrypProjectId = string.Empty;
                string decrypModuleId = string.Empty;
                _commonRepository = new CommonRepository();
                if (!string.IsNullOrEmpty(projectModule.ProjectId))
                {
                    decrypProjectId = await _commonRepository.Decrypt(projectModule.ProjectId, requestDetaildto.SecurityKey);
                }

                if (!string.IsNullOrEmpty(projectModule.ModuleId))
                {
                    decrypModuleId = await _commonRepository.Decrypt(projectModule.ModuleId, requestDetaildto.SecurityKey);
                }
                _getEmployeeRepository = new EmployeeListRepository();

                var employeeDetails = await _getEmployeeRepository.GetEmployeeByProjectModule(decrypProjectId, decrypModuleId);
                var employeeDetailsMap = _mapper.Map<IEnumerable<PrEmployeeListDto>>(employeeDetails);
                return employeeDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }

        }

        public async Task<string> CreateEmployee(PrEmployeeListDto employeeListDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string returnFilePath = "";
            string byteDataCheck = "";
            byte[] fileSource = null;
            try
            {
                string decrypEmpName = string.Empty;
                string decrypEmpTitle = string.Empty;
                string decrypSysUserId = string.Empty;
                string decrypEmpEmail = string.Empty;
                string decrypEmployeeCode = string.Empty;
                string decrypCmpBranchId = string.Empty;
                string presentAddress = string.Empty;
                string permanentAddress = string.Empty;
                string contactNumber = string.Empty;
                string imagePath = string.Empty;
                string nidCard = string.Empty;
                string dptId = string.Empty;
                string dsgId = string.Empty;
                string accountNo = string.Empty;
                string joiningDate = string.Empty;
                string birthDay = string.Empty;
                _commonRepository = new CommonRepository();
                if (!string.IsNullOrEmpty(employeeListDto.EmpName))
                {
                    decrypEmpName = await _commonRepository.Decrypt(employeeListDto.EmpName, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.SysUserId))
                {
                    decrypSysUserId = await _commonRepository.Decrypt(employeeListDto.SysUserId, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.EmpCode))
                {
                    decrypEmployeeCode = await _commonRepository.Decrypt(employeeListDto.EmpCode, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.EmpEmail))
                {
                    decrypEmpEmail = await _commonRepository.Decrypt(employeeListDto.EmpEmail, requestDetaildto.SecurityKey);
                }

                if (!string.IsNullOrEmpty(employeeListDto.CmpBranchId))
                {
                    decrypCmpBranchId = await _commonRepository.Decrypt(employeeListDto.CmpBranchId, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.PermanentAddress))
                {
                    permanentAddress = await _commonRepository.Decrypt(employeeListDto.PermanentAddress, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.PresentAddress))
                {
                    presentAddress = await _commonRepository.Decrypt(employeeListDto.PresentAddress, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.ContactNumber))
                {
                    contactNumber = await _commonRepository.Decrypt(employeeListDto.ContactNumber, requestDetaildto.SecurityKey);
                }

                if (!string.IsNullOrEmpty(employeeListDto.NidCard))
                {
                    nidCard = await _commonRepository.Decrypt(employeeListDto.NidCard, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.DepartmentId))
                {
                    dptId = await _commonRepository.Decrypt(employeeListDto.DepartmentId, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.DesignationId))
                {
                    dsgId = await _commonRepository.Decrypt(employeeListDto.DesignationId, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.AccountNo))
                {
                    accountNo = await _commonRepository.Decrypt(employeeListDto.AccountNo, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.JoiningDate))
                {
                    joiningDate = await _commonRepository.Decrypt(employeeListDto.JoiningDate, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.BirthDate))
                {
                    birthDay = await _commonRepository.Decrypt(employeeListDto.BirthDate, requestDetaildto.SecurityKey);
                }
                objServiceHandler = new ServiceHandler();
                if (employeeListDto.ImageSource != null)
                {
                    foreach (var base64String in employeeListDto.ImageSource)
                    {
                        byteDataCheck = base64String.Substring(0, base64String.IndexOf(';'));
                        var fileData = base64String.Remove(0, base64String.IndexOf(',') + 1);

                        if (!string.IsNullOrEmpty(base64String))
                        {
                            fileSource = Convert.FromBase64String(fileData);
                            if (byteDataCheck == "data:image/jpeg" || byteDataCheck == "data:image/png" || byteDataCheck == "data:image/gif")
                            {
                                returnFilePath = objServiceHandler.SaveImageDirectory(employeeListDto.ImagePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/pdf")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", employeeListDto.ImagePath + ".pdf");
                                returnFilePath = @"images\TaskManagement\document\" + employeeListDto.ImagePath + ".pdf";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/octet-stream")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", employeeListDto.ImagePath + ".txt");
                                returnFilePath = @"images\TaskManagement\document\" + employeeListDto.ImagePath + ".txt";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", employeeListDto.ImagePath + ".docx");
                                returnFilePath = @"images\TaskManagement\document\" + employeeListDto.ImagePath + ".docx";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", employeeListDto.ImagePath + ".xls");
                                returnFilePath = @"images\TaskManagement\document\" + employeeListDto.ImagePath + ".xls";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                        }
                        break;
                    }
                }


                _getEmployeeRepository = new EmployeeListRepository();
                var employee = await _getEmployeeRepository.CreateEmployee(decrypEmpName, decrypEmployeeCode, decrypEmpEmail, decrypSysUserId, decrypCmpBranchId,
                    permanentAddress, presentAddress, contactNumber, nidCard, returnFilePath, dptId, dsgId, accountNo, joiningDate, birthDay);
                // var employeeMap =  _mapper.Map<IEnumerable<PrEmployeeListDto>>(employee);
                return " Employee successfully created ";
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

        public async Task<string> UpdateEmployee(PrEmployeeListDto employeeListDto, RequestDetailDto requestDetaildto)
        {
            Regex reg = new Regex("[\"]");
            string strSysText = "";
            string returnFilePath = "";
            string byteDataCheck = "";
            byte[] fileSource = null;
            try
            {
                string decrypEmpName = string.Empty;
                string decrypEmpTitle = string.Empty;
                string decrypSysUserId = string.Empty;
                string decrypEmpEmail = string.Empty;
                string decrypEmployeeCode = string.Empty;
                string decrypCmpBranchId = string.Empty;
                string presentAddress = string.Empty;
                string permanentAddress = string.Empty;
                string contactNumber = string.Empty;
                string imagePath = string.Empty;
                string nidCard = string.Empty;
                string dptId = string.Empty;
                string dsgId = string.Empty;
                string accountNo = string.Empty;
                string joiningDate = string.Empty;
                string birthDay = string.Empty;
                string empId = string.Empty;
                DateTime decryptbirthDay = new DateTime();
                _commonRepository = new CommonRepository();
                if (!string.IsNullOrEmpty(employeeListDto.EmpId))
                {
                    empId = await _commonRepository.Decrypt(employeeListDto.EmpId, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.EmpName))
                {
                    decrypEmpName = await _commonRepository.Decrypt(employeeListDto.EmpName, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.SysUserId))
                {
                    decrypSysUserId = await _commonRepository.Decrypt(employeeListDto.SysUserId, requestDetaildto.SecurityKey);
                }

                if (!string.IsNullOrEmpty(employeeListDto.EmpEmail))
                {
                    decrypEmpEmail = await _commonRepository.Decrypt(employeeListDto.EmpEmail, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.EmpCode))
                {
                    decrypEmployeeCode = await _commonRepository.Decrypt(employeeListDto.EmpCode, requestDetaildto.SecurityKey);
                }

                if (!string.IsNullOrEmpty(employeeListDto.CmpBranchId))
                {
                    decrypCmpBranchId = await _commonRepository.Decrypt(employeeListDto.CmpBranchId, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.PermanentAddress))
                {
                    permanentAddress = await _commonRepository.Decrypt(employeeListDto.PermanentAddress, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.PresentAddress))
                {
                    presentAddress = await _commonRepository.Decrypt(employeeListDto.PresentAddress, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.ContactNumber))
                {
                    contactNumber = await _commonRepository.Decrypt(employeeListDto.ContactNumber, requestDetaildto.SecurityKey);
                }

                if (!string.IsNullOrEmpty(employeeListDto.NidCard))
                {
                    nidCard = await _commonRepository.Decrypt(employeeListDto.NidCard, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.DepartmentId))
                {
                    dptId = await _commonRepository.Decrypt(employeeListDto.DepartmentId, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.DesignationId))
                {
                    dsgId = await _commonRepository.Decrypt(employeeListDto.DesignationId, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.AccountNo))
                {
                    accountNo = await _commonRepository.Decrypt(employeeListDto.AccountNo, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.JoiningDate))
                {
                    joiningDate = await _commonRepository.Decrypt(employeeListDto.JoiningDate, requestDetaildto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(employeeListDto.BirthDate))
                {
                    birthDay = await _commonRepository.Decrypt(employeeListDto.BirthDate, requestDetaildto.SecurityKey);
                    decryptbirthDay = Convert.ToDateTime(reg.Replace(birthDay, ""));
                }
                objServiceHandler = new ServiceHandler();
                if (employeeListDto.ImageSource != null)
                {
                    foreach (var base64String in employeeListDto.ImageSource)
                    {
                        byteDataCheck = base64String.Substring(0, base64String.IndexOf(';'));
                        var fileData = base64String.Remove(0, base64String.IndexOf(',') + 1);
                        if (!string.IsNullOrEmpty(base64String))
                        {
                            fileSource = Convert.FromBase64String(fileData);
                            if (byteDataCheck == "data:image/jpeg" || byteDataCheck == "data:image/png" || byteDataCheck == "data:image/gif")
                            {
                                returnFilePath = objServiceHandler.SaveImageDirectory( employeeListDto.ImagePath, fileSource);
                            }
                        }
                        break;
                    }
                }
                else
                {
                    returnFilePath = "";
                }
                _getEmployeeRepository = new EmployeeListRepository();
                var employeeDetails = await _getEmployeeRepository.GetEmployeeById(empId,"");
                decrypCmpBranchId = employeeDetails.CMP_BRANCH_ID;
                var employee = await _getEmployeeRepository.UpdateEmployee(decrypEmpName, decrypEmployeeCode, decrypEmpEmail, decrypSysUserId, decrypCmpBranchId,
                    permanentAddress, presentAddress, contactNumber, nidCard, returnFilePath, dptId, dsgId, accountNo, joiningDate, decryptbirthDay, empId);
                return " Employee Successfully updated";
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
