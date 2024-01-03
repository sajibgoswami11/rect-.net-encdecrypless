using BizWebAPI.Areas.ERP.Dtos;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces
{
    public interface IAuth
    {
        //Task<User> Register(User user, string strPassword);
        Task<ErpAuthLoginDto> Login(string strUsername, string strPassword, HttpContext httpContext);
        //Task<bool> IsExists(string strUsername);

        Task<ErpAuthLoginReturnDto> GetEmployeeByUserId(string SysUserId);
        Task<string> CommonMessage();
    }
}
