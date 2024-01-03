namespace BizWebAPI.Areas.ERP.Dtos
{
    public class RequestDetailDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string SecurityKey { get; set; }
        public bool IsAuthenticated { get; set; }
        public string ClientIPAddress { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
