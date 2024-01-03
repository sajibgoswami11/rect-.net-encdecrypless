using BizWebAPI.Areas.xEmptyArea.Interfaces;
using BizWebAPI.Areas.xEmptyArea.Dtos;
using BizWebAPI.Areas.xEmptyArea.Repository;
using AutoMapper;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace BizWebAPI.Areas.xEmptyArea.Manager
{
    public class AuthManager : IAuth
    {
        private AuthRepository _authRepository;
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

        public async Task<xEmptyAuthDto> Login(string strUsername, string strPassword, HttpContext httpContext)
        {
            string strSysText = "";
            string UserId = "";
            string ClientIpAddress = httpContext.Connection.RemoteIpAddress.ToString();

            try
            {
                _authRepository = new AuthRepository();
                var userDetails = await _authRepository.Login(strUsername, strPassword);
                var user = _mapper.Map<xEmptyAuthDto>(userDetails);
                
                UserId = user.Id;
                strSysText = "Login Successful";

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
                // #################################### System Audit ####################################
                string className = this.GetType().FullName;
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(UserId, ClientIpAddress, methodName, className, strSysText);
                // #################################### System Audit ####################################
            }
        }
    }
}
