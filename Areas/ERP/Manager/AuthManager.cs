using BizWebAPI.Areas.ERP.Interfaces;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Repository;
using AutoMapper;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using BizWebAPI.Common;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;

namespace BizWebAPI.Areas.ERP.Manager
{
    public class AuthManager : IAuth
    {
        private AuthRepository _authRepository;
        private TaskHelperRepository _taskHelperRepository;
        private CommonRepository _commonRepository;
        private readonly IMapper _mapper;

        public AuthManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<ErpAuthLoginDto> Login(string strUsername, string strPassword, HttpContext httpContext)
        {
            string strSysText = "";
            string UserId = "";
            string ClientIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            try
            {
                var userDetails = await GetKey.GetSecurityKey(httpContext);
                _commonRepository = new CommonRepository();
                string decryptUsername = await _commonRepository.Decrypt(strUsername, userDetails.SecurityKey);
                string decryptPassword = await _commonRepository.Decrypt(strPassword, userDetails.SecurityKey);

                _authRepository = new AuthRepository();
                var loginDetails = await _authRepository.Login(decryptUsername, decryptPassword);
                var user = _mapper.Map<ErpAuthLoginDto>(loginDetails);

                if (user != null)
                {
                    UserId = user.UserId;
                    strSysText = "Login Successful";
                }                
                return user;
            }
            catch (Exception ex)
            {
                UserId = strUsername;
                strSysText = ex.Message.ToString();
                return null;
            }
            finally
            {
                RequestDetailDto requestDetailDto = new RequestDetailDto();
                requestDetailDto.UserId = UserId;
                requestDetailDto.ClientIPAddress = ClientIpAddress;
                requestDetailDto.ActionName = "Login";
                requestDetailDto.ControllerName = "Auth";

                _commonRepository = new CommonRepository();
                //await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }

        public async Task<ErpAuthLoginReturnDto> GetEmployeeByUserId(string SysUserId)
        {
            try
            {
                _taskHelperRepository = new TaskHelperRepository();
                var loginEmployeeDetails = await _taskHelperRepository.GetEmployeeByUserId(SysUserId);
                var employeeDetails = _mapper.Map<ErpAuthLoginReturnDto>(loginEmployeeDetails);

                return employeeDetails;
            }
            catch(Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
    }
}
