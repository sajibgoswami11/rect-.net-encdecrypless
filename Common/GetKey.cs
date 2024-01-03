using BizWebAPI.Areas.ERP.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Common
{
    public static class GetKey
    {
        internal static async Task<RequestDetailDto> GetSecurityKey(HttpContext httpContext)
        {
            string strUserId = "";
            string username = "";
            string strSecurityKey = "";
            bool isAuthenticated = false;
            string strClientIpAddress = "";

            if (httpContext.User.Identity.Name != null)
            {
                strClientIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                strUserId = httpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                username = httpContext.User.Identities.FirstOrDefault().Name;
                isAuthenticated = httpContext.User.Identities.FirstOrDefault().IsAuthenticated;
                string strKey = (httpContext.GetTokenAsync("access_token").Result) + username;
                strSecurityKey = strKey.Substring(strKey.Length - 32, 32);
            }

            string actionName = httpContext.Request.RouteValues.Values.ToList()[1].ToString();
            string controllerName = httpContext.Request.RouteValues.Values.ToList()[2].ToString();

            if (controllerName.Equals("Auth"))
            {
                strSecurityKey = "HALTech^%$#@!Trn";
            }

            RequestDetailDto userDetails = new RequestDetailDto 
            { 
                UserId = strUserId, 
                Username = username, 
                SecurityKey = strSecurityKey, 
                IsAuthenticated = isAuthenticated ,
                ClientIPAddress = strClientIpAddress,
                ControllerName = controllerName,
                ActionName = actionName
            };

            return await Task.FromResult(userDetails);
        }
    }
}
