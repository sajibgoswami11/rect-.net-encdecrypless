using BizWebAPI.Areas.xEmptyArea.Dtos;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.xEmptyArea.Interfaces
{
    public interface IAuth
    {
        //Task<User> Register(User user, string strPassword);
        Task<xEmptyAuthDto> Login(string strUsername, string strPassword, HttpContext httpContext);
        //Task<bool> IsExists(string strUsername);
        Task<string> CommonMessage();
    }
}
